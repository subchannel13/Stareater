using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using Stareater.GLData.SpriteShader;

namespace Stareater.GLData
{
	class TextRenderUtil
	{
		#region Singleton
		static TextRenderUtil instance = null;

		public static TextRenderUtil Get
		{
			get
			{
				if (instance == null)
					instance = new TextRenderUtil();
				return instance;
			}
		}
		#endregion

		public const float SdfSizeThreshold = 16;
		public const float SdfFontSize = 24;
		public const float RasterFontSize = 24; //TODO(later) remove the need for it

		const int Width = 512;
		const int Height = 512;
		const int Spacing = 2;
		const string FontFamily = "Arial";

		const float SpaceUnitWidth = 0.25f;
		
		private ColorMap textureData;

		private readonly Dictionary<float, Dictionary<char, CharTextureInfo>> characterInfos = new Dictionary<float, Dictionary<char, CharTextureInfo>>();
		private readonly Dictionary<float, Font> fonts = new Dictionary<float, Font>();
		private readonly Dictionary<float, float> fontHeights = new Dictionary<float, float>();
		private readonly AtlasBuilder textureBuilder = new AtlasBuilder(Spacing, new Size(Width, Height));
		
		public int TextureId { get; private set; }
		
		public void Initialize()
		{
			if (this.textureData == null)
				this.textureData = new ColorMap(Width, Height, Color.FromArgb(0, 255, 255, 255));
			
			if (this.TextureId == 0)
				this.TextureId = TextureUtils.CreateTexture(this.textureData);
		}

		private void initializeFor(float fontSize)
		{
			if (!this.characterInfos.ContainsKey(fontSize))
				this.characterInfos[fontSize] = new Dictionary<char, CharTextureInfo>();

			if (!this.fonts.ContainsKey(fontSize))
			{
				var font = new Font(FontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
				this.fonts[fontSize] = font;

				using(var fakeCanvas = new Bitmap(1,1))
				using (var g = Graphics.FromImage(fakeCanvas))
				{
					this.fontHeights[fontSize] = TextRenderer.MeasureText(g, " ", font, new Size(int.MaxValue, int.MaxValue)).Height;
				}
			}
		}

		public float FontHeight(float fontSize)
		{
			this.initializeFor(fontSize);

			return this.fontHeights[fontSize];
		}

		public float WidthOf(string text, float fontSize)
		{
			if (string.IsNullOrEmpty(text))
				return 0;

			if (fontSize < SdfSizeThreshold)
			{
				this.prepareRaster(text, fontSize);
				return this.measureWidth(text, fontSize);
			}
			else
			{
				this.prepareSdf(text);
				return this.measureWidth(text, SdfFontSize);
			}
		}

		//TODO(later) try to remove the need transform parameter
		//TODO(later) try to remove whole method
		public Dictionary<float, IEnumerable<float>> BufferRaster(string text, float adjustment, float z0, float zRange)
		{
			return this.BufferRaster(text, RasterFontSize, adjustment, z0, zRange);
		}

		//TODO(later) try to remove the need transform parameter
		public Dictionary<float, IEnumerable<float>> BufferRaster(string text, float fontSize, float adjustment, float z0, float zRange)
		{
			this.prepareRaster(text, fontSize);

			return bufferText(text, this.characterInfos[fontSize], this.measureWidth(text, fontSize), adjustment, z0, zRange);
		}

		//TODO(later) try to remove the need transform parameter
		public Dictionary<float, IEnumerable<float>> BufferSdf(string text, float adjustment, float z0, float zRange)
		{
			this.prepareSdf(text);

			return bufferText(text, this.characterInfos[SdfFontSize], this.measureWidth(text, SdfFontSize), adjustment, z0, zRange);
		}

		//TODO(later) try to remove the need transform parameter
		private Dictionary<float, IEnumerable<float>> bufferText(string text, Dictionary<char, CharTextureInfo> characters, float textWidth, float adjustment, float z0, float zRange)
		{
			float charOffsetX = textWidth * adjustment;
			float charOffsetY = 0;
			var layers = new List<float>[4];
			int row = 0;
			int colunm = 0;
			for (int i = 0; i < layers.Length; i++)
				layers[i] = new List<float>();

			foreach (char c in text)
				if (!char.IsWhiteSpace(c))
				{
					var charInfo = characters[c];
					var layer = 2 * (row % 2) + colunm % 2;

					for (int v = 0; v < 6; v++)
					{
						layers[layer].AddRange(SpriteHelpers.TexturedVertex(
							charInfo.VertexCoords[v].X * charInfo.Aspect + charOffsetX,
							charInfo.VertexCoords[v].Y + charOffsetY,
							charInfo.TextureCoords[v].X, charInfo.TextureCoords[v].Y));
					}
					charOffsetX += charInfo.Aspect;
					colunm++;
				}
				else if (c == ' ')
					charOffsetX += SpaceUnitWidth;
				else if (c == '\n')
				{
					charOffsetX = textWidth * adjustment;
					charOffsetY--;
					row++;
					colunm = 0;
				}

			return Enumerable.Range(0, layers.Length).
				ToDictionary(i => z0 - i * zRange / 4, i => (IEnumerable<float>)layers[i]);
		}

		private float measureWidth(string text, float fontSize)
		{
			this.initializeFor(fontSize);
			var characters = this.characterInfos[fontSize];
			var textWidth = 0f;

			foreach (var line in text.Split('\n'))
			{
				var lineWidth = 0f;

				foreach (char c in line)
				{
					if (!char.IsWhiteSpace(c))
						lineWidth += characters[c].Aspect;
					else if (c == ' ')
						lineWidth += SpaceUnitWidth;
					else if (c != '\r')
						throw new ArgumentException("Unsupported whitespace character, character code: " + (int)c);
				}

				textWidth = Math.Max(textWidth, lineWidth);
			}

			return textWidth;
		}

		private void prepareRaster(string text, float fontSize)
		{
			this.initializeFor(fontSize);
			this.prepare(text, this.characterInfos[fontSize], () => new CharacterRasterDrawer(this.textureBuilder, this.textureData, this.fonts[fontSize]));
		}

		private void prepareSdf(string text)
		{
			this.initializeFor(SdfFontSize);
			this.prepare(text, this.characterInfos[SdfFontSize],() => new CharacterSdfDrawer(this.textureBuilder, this.textureData, this.fonts[SdfFontSize]));
		}

		private void prepare(string text, Dictionary<char, CharTextureInfo> characters, Func<ICharacterDrawer> drawerMaker)
		{
			this.Initialize();
			
			var missinCharacters = new HashSet<char>();
			foreach (char c in text)
				if (!characters.ContainsKey(c) && !char.IsWhiteSpace(c))
					missinCharacters.Add(c);

			if (missinCharacters.Count == 0)
				return;

			using(var drawer = drawerMaker())
			foreach (char c in missinCharacters)
				characters[c] = drawer.Draw(c);

			TextureUtils.UpdateTexture(this.TextureId, this.textureData);
		}
	}
}
