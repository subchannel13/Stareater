﻿using System;
using System.Collections.Generic;
using System.Drawing;
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

		public const float SdfTextSize = 10;

		const int Width = 512;
		const int Height = 512;
		const int Spacing = 2;
		const float FontSize = 30;	
		const string FontFamily = "Arial";

		const float SpaceUnitWidth = 0.25f;
		
		private Bitmap textureBitmap;

		private readonly Dictionary<float, Dictionary<char, CharTextureInfo>> characterInfos = new Dictionary<float, Dictionary<char, CharTextureInfo>>();
		private readonly Dictionary<float, Font> fonts = new Dictionary<float, Font>();
		private readonly Dictionary<float, float> fontHeights = new Dictionary<float, float>();
		private readonly AtlasBuilder textureBuilder = new AtlasBuilder(Spacing, new Size(Width, Height));
		private readonly Vector2[] unitQuadTriangles;
		
		public int TextureId { get; private set; }
		
		private TextRenderUtil()
		{
			this.unitQuadTriangles = new Vector2[] {
				new Vector2(0, 0),
				new Vector2(1, 0),
				new Vector2(1, -1),
				
				new Vector2(1, -1),
				new Vector2(0, -1),
				new Vector2(0, 0),
			};
		}

		public void Initialize()
		{
			if (this.textureBitmap == null)
			{
				this.textureBitmap = new Bitmap(Width, Height);
				using (var g = Graphics.FromImage(this.textureBitmap))
					g.Clear(Color.Transparent);
			}
			
			if (this.TextureId == 0)
				this.TextureId = TextureUtils.CreateTexture(this.textureBitmap);
		}

		private void initializeFor(float fontSize)
		{
			if (!this.characterInfos.ContainsKey(fontSize))
				this.characterInfos[fontSize] = new Dictionary<char, CharTextureInfo>();

			if (!this.fonts.ContainsKey(fontSize))
				this.fonts[fontSize] = new Font(FontFamily, fontSize, FontStyle.Bold);
		}

		public float FontHeight(float fontSize)
		{
			this.initializeFor(fontSize);
			var font = this.fonts[fontSize];

			if (!this.fontHeights.ContainsKey(fontSize))
			{
				this.Initialize();
				using (var g = Graphics.FromImage(this.textureBitmap))
				{
					var size = g.MeasureString(" ", font, int.MaxValue, StringFormat.GenericTypographic);
					this.fontHeights[fontSize] = (float)Math.Ceiling(size.Height);
				}
			}

			return this.fontHeights[fontSize];
		}

		public float MeasureWidth(string text, float fontSize)
		{
			if (string.IsNullOrEmpty(text))
				return 0;

			this.initializeFor(fontSize);
			var characters = this.characterInfos[fontSize];
			var textWidth = 0f;

			foreach (var line in text.Split('\n'))
			{
				var lineWidth = 0f;

				foreach (char c in line)
				{
					if (!characters.ContainsKey(c))
						this.prepareRaster(text, fontSize); //TODO(v0.8) should prepare before measurement

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

		//TODO(later) try to remove the need transform parameter
		public IEnumerable<float> BufferRaster(string text, float adjustment, Matrix4 transform)
		{
			return this.BufferRaster(text, FontSize, adjustment, transform);
		}

		//TODO(later) try to remove the need transform parameter
		public IEnumerable<float> BufferRaster(string text, float fontSize, float adjustment, Matrix4 transform)
		{
			this.prepareRaster(text, fontSize);

			return bufferText(text, this.characterInfos[fontSize], this.MeasureWidth(text, fontSize), adjustment, transform);
		}

		//TODO(later) try to remove the need transform parameter
		public IEnumerable<float> BufferSdf(string text, float adjustment, Matrix4 transform)
		{
			this.prepareSdf(text);

			return bufferText(text, this.characterInfos[SdfTextSize], this.MeasureWidth(text, SdfTextSize), adjustment, transform);
		}

		//TODO(later) try to remove the need transform parameter
		private IEnumerable<float> bufferText(string text, Dictionary<char, CharTextureInfo> characters, float textWidth, float adjustment, Matrix4 transform)
		{
			float charOffsetX = textWidth * adjustment;
			float charOffsetY = 0;

			foreach (char c in text)
				if (!char.IsWhiteSpace(c))
				{
					var charInfo = characters[c];

					for (int v = 0; v < 6; v++)
					{
						var charPos = Vector4.Transform(
							new Vector4(unitQuadTriangles[v].X * charInfo.Aspect + charOffsetX, unitQuadTriangles[v].Y + charOffsetY, 0, 1),
							transform
						);
						foreach (var dataBit in SpriteHelpers.TexturedVertex(charPos.X, charPos.Y, charInfo.TextureCoords[v].X, charInfo.TextureCoords[v].Y))
							yield return dataBit;
					}
					charOffsetX += charInfo.Aspect;
				}
				else if (c == ' ')
					charOffsetX += SpaceUnitWidth;
				else if (c == '\n')
				{
					charOffsetX = textWidth * adjustment;
					charOffsetY--;
				}
		}

		private void prepareRaster(string text, float fontSize)
		{
			this.initializeFor(fontSize);
			this.prepare(text, this.characterInfos[fontSize], () => new CharacterRasterDrawer(this.textureBuilder, this.textureBitmap, this.fonts[fontSize]));
		}

		private void prepareSdf(string text)
		{
			this.initializeFor(SdfTextSize);
			this.prepare(text, this.characterInfos[SdfTextSize],() => new CharacterSdfDrawer(this.textureBuilder, this.textureBitmap, this.fonts[SdfTextSize]));
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
				characters[c] = new CharTextureInfo(drawer.Draw(c), Width, Height);

			TextureUtils.UpdateTexture(this.TextureId, this.textureBitmap);
		}
	}
}
