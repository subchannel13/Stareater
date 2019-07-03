using System;
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
		
		const int Width = 512;
		const int Height = 512;
		const float Spacing = 2 / (float)Width;
		const float FontSize = 30;	
		const string FontFamily = "Arial";

		const float SpaceUnitWidth = 0.25f;
		
		private Bitmap textureBitmap;
		private Font font;
		private Vector2 nextCharOffset;

		private readonly Dictionary<char, CharTextureInfo> characterInfos = new Dictionary<char, CharTextureInfo>();
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

		private void lazyInitialization()
		{
			if (this.textureBitmap == null)
				this.textureBitmap = new Bitmap(Width, Height);
			
			if (this.font == null)
				this.font = new Font(FontFamily, FontSize, FontStyle.Bold);
			
			if (this.TextureId == 0)
				this.TextureId = TextureUtils.CreateTexture(this.textureBitmap);
		}

		public float MeasureWidth(string text)
		{
			if (string.IsNullOrEmpty(text))
				return 0;

			var textWidth = 0f;

			foreach (var line in text.Split('\n'))
			{
				var lineWidth = 0f;

				foreach (char c in line)
				{
					if (!this.characterInfos.ContainsKey(c))
						Prepare(new string[] { text });

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

		public IEnumerable<float> BufferText(string text, float adjustment, Matrix4 transform)
		{
			float textWidth = this.MeasureWidth(text);
			float charOffsetX = textWidth * adjustment;
			float charOffsetY = 0;

			foreach (char c in text)
				if (!char.IsWhiteSpace(c))
				{
					var charInfo = this.characterInfos[c];

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

		public void Prepare()
		{
			this.lazyInitialization();
		}

        public void Prepare(IEnumerable<string> texts)
		{
			var missinCharacters = new HashSet<char>();
			foreach (string text in texts)
				foreach (char c in text)
					if (!this.characterInfos.ContainsKey(c) && !char.IsWhiteSpace(c))
						missinCharacters.Add(c);

			if (missinCharacters.Count == 0 && this.TextureId != 0)
				return;

			this.lazyInitialization();

			using (var g = Graphics.FromImage(this.textureBitmap)) {
				if (this.nextCharOffset == Vector2.Zero)
					g.Clear(Color.Transparent);

				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				var textBrush = new SolidBrush(Color.White);

				foreach (char c in missinCharacters) {
					var size = g.MeasureString(c.ToString(), font, int.MaxValue, StringFormat.GenericTypographic);
					size = new SizeF(
						(float)Math.Ceiling(size.Width) / Width,
						(float)Math.Ceiling(size.Height) / Height
					);

					if (this.nextCharOffset.X + size.Width >= 1)
						this.nextCharOffset = new Vector2(0, this.nextCharOffset.Y + size.Height + Spacing);

					this.characterInfos.Add(c, new CharTextureInfo(nextCharOffset, size));
					g.DrawString(c.ToString(), font, textBrush, nextCharOffset.X * Width, nextCharOffset.Y * Height, StringFormat.GenericTypographic);

					this.nextCharOffset += new Vector2(size.Width + Spacing, 0);
				}
			}

			TextureUtils.UpdateTexture(this.TextureId, this.textureBitmap);
		}
	}
}
