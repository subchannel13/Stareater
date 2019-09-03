using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

		public const float SdfFontSize = 24;
		public const int SdfPadding = 4;

		const int Width = 512;
		const int Height = 512;
		const int Spacing = 2;
		const string FontFamily = "Arial";

		const float SpaceUnitWidth = 0.25f;
		
		private ColorMap textureData;

		private readonly Dictionary<char, CharTextureInfo> characterInfos = new Dictionary<char, CharTextureInfo>();
		private readonly Font font = new Font(FontFamily, SdfFontSize, FontStyle.Regular, GraphicsUnit.Pixel);
		private readonly AtlasBuilder textureBuilder = new AtlasBuilder(Spacing, new Size(Width, Height));
		
		public int TextureId { get; private set; }
		
		public void Initialize()
		{
			if (this.textureData == null)
				this.textureData = new ColorMap(Width, Height, Color.FromArgb(0, 255, 255, 255));
			
			if (this.TextureId == 0)
				this.TextureId = TextureUtils.CreateTexture(this.textureData);
		}

		public float WidthOf(string text)
		{
			if (string.IsNullOrEmpty(text))
				return 0;

			this.prepare(text);
			return this.measureWidth(text);
		}

		public Dictionary<float, IEnumerable<float>> BufferText(string text, float adjustment, float z0, float zRange)
		{
			this.prepare(text);
			float textWidth = this.measureWidth(text);

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
					var charInfo = this.characterInfos[c];
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

		private float measureWidth(string text)
		{
			var textWidth = 0f;

			foreach (var line in text.Split('\n'))
			{
				var lineWidth = 0f;

				foreach (char c in line)
				{
					if (!char.IsWhiteSpace(c))
						lineWidth += this.characterInfos[c].Aspect;
					else if (c == ' ')
						lineWidth += SpaceUnitWidth;
					else if (c != '\r')
						throw new ArgumentException("Unsupported whitespace character, character code: " + (int)c);
				}

				textWidth = Math.Max(textWidth, lineWidth);
			}

			return textWidth;
		}

		private void prepare(string text)
		{
			this.Initialize();

			var missinCharacters = new HashSet<char>();
			foreach (char c in text)
				if (!this.characterInfos.ContainsKey(c) && !char.IsWhiteSpace(c))
					missinCharacters.Add(c);

			if (missinCharacters.Count == 0)
				return;

			using (var fakeBitmap = new Bitmap(1, 1))
			using (var fakeCanvas = Graphics.FromImage(fakeBitmap))
				foreach (char c in missinCharacters)
				{
					var path = new GraphicsPath();
					path.AddString(c.ToString(), font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), StringFormat.GenericTypographic);
					path.Flatten();

					var contures = getContures(path).ToList();
					path.Dispose();

					//TODO(later) measure from path points instead
					var measuredSize = new SizeF(
						fakeCanvas.MeasureString(c.ToString(), this.font, int.MaxValue, StringFormat.GenericTypographic).Width + SdfPadding * 2,
						TextRenderer.MeasureText(fakeCanvas, c.ToString(), this.font, new Size(int.MaxValue, int.MaxValue)).Height + SdfPadding * 2
					);
					var rect = this.textureBuilder.Add(measuredSize);

					int width = rect.Size.Width;
					int height = rect.Size.Height;
					var distField = genSdf(contures, width, height);

					for (int y = 0; y < height; y++)
						for (int x = 0; x < width; x++)
							this.textureData[rect.X + x, rect.Y + y] = Color.FromArgb((int)(distField[y, x] * 255), 255, 255, 255);

					this.characterInfos[c] = new CharTextureInfo(
						rect,
						this.textureData.Width, this.textureData.Height,
						width / (float)(width - 2 * SdfPadding), height / (float)(height - 2 * SdfPadding),
						0.5f, -0.5f
					);
				}

			TextureUtils.UpdateTexture(this.TextureId, this.textureData);
		}

		private static IEnumerable<GlyphContour> getContures(GraphicsPath path)
		{
			var pathPoints = path.PathPoints;
			var pathTypes = path.PathTypes.Select(x => x & (byte)PathPointType.PathTypeMask).ToList();
			var contoureRanges = new List<KeyValuePair<int, int>>();
			var start = 0;
			var count = 0;

			for (int i = 0; i < pathPoints.Length; i++)
			{
				if (pathTypes[i] == (byte)PathPointType.Start)
				{
					contoureRanges.Add(new KeyValuePair<int, int>(start, count));
					start = i;
					count = 0;
				}

				count++;
			}
			contoureRanges.Add(new KeyValuePair<int, int>(start, count));

			foreach (var group in contoureRanges.Where(x => x.Value > 1))
			{
				var strokes = new List<PointF[]>();
				for (int i = 1; i < group.Value; i++)
					strokes.Add(new[] { pathPoints[group.Key + i - 1], pathPoints[group.Key + i] });

				strokes.Add(new[] { pathPoints[group.Key + group.Value - 1], pathPoints[group.Key] });
				yield return new GlyphContour(strokes);
			}
		}

		private static double[,] genSdf(List<GlyphContour> contures, int width, int height)
		{
			var distField = new double[height, width];

			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
				{
					var fromP = new Vector2(x - SdfPadding, y - SdfPadding);
					var minDist = Math.Min(contures.Min(shape => shape.Distance(fromP)), SdfPadding);
					if (contures.Sum(shape => shape.RayHits(fromP)) % 2 != 0)
						minDist *= -1;

					distField[y, x] = -minDist / SdfPadding / 2 + 0.5;
				}

			return distField;
		}
	}
}
