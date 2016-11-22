using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.GLData;

namespace Stareater.GLRenderers
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
		const float FontSize = 30;	
		const string FontFamily = "Arial";

		const float SpaceUnitWidth = 0.25f;
		
		private Bitmap textureBitmap;
		private Font font;
		private Vector2 nextCharOffset;

		private Dictionary<char, CharTextureInfo> characterInfos = new Dictionary<char, CharTextureInfo>();
		private readonly Vector2[] unitQuad;
		private readonly Vector2[] unitQuadTriangles;
		
		public int TextureId { get; private set; }
		
		private TextRenderUtil()
		{
			this.unitQuad = new Vector2[] {
				new Vector2(0, 0),
				new Vector2(0, -1),
				new Vector2(1, -1),
				new Vector2(1, 0),
			};
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
		
		private float measureWidth(string text)
		{
			float textWidth = 0;
			
			foreach (char c in text) {
				if (!this.characterInfos.ContainsKey(c))
					Prepare(new string[] { text });
				        
				if (!char.IsWhiteSpace(c))
					textWidth += this.characterInfos[c].Aspect;
				else if (c == ' ')
					textWidth += SpaceUnitWidth;
				else
					throw new ArgumentException("Unsupported whitespace character, character code: " + (int)c);
			}
			
			return textWidth;
		}
		
		public void BufferText(string text, float adjustment, Matrix4 transform, VertexArrayBuilder vaoBuilder)
		{
			float textWidth = measureWidth(text);
			float charOffset = textWidth * adjustment;
			
			foreach (char c in text)
				if (!char.IsWhiteSpace(c)) 
				{
					var charInfo = this.characterInfos[c];
					
					for (int v = 0; v < 6; v++) 
					{
						var charPos = Vector4.Transform(
							new Vector4(unitQuadTriangles[v].X * charInfo.Aspect + charOffset, unitQuadTriangles[v].Y, 0, 1), 
							transform
						);
						vaoBuilder.AddTexturedVertex(charPos.X, charPos.Y, charInfo.TextureCoords[v].X, charInfo.TextureCoords[v].Y);
					}
					charOffset += charInfo.Aspect;
				}
				else if (c == ' ')
					charOffset += SpaceUnitWidth;
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

			lazyInitialization();

			using (Graphics g = Graphics.FromImage(this.textureBitmap)) {
				if (this.nextCharOffset == Vector2.Zero)
					g.Clear(Color.Transparent);

				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				Brush textBrush = new SolidBrush(Color.White);

				foreach (char c in missinCharacters) {
					SizeF size = g.MeasureString(c.ToString(), font, int.MaxValue, StringFormat.GenericTypographic);
					size = new SizeF(
						(float)Math.Ceiling(size.Width) / Width,
						(float)Math.Ceiling(size.Height) / Height
					);

					if (this.nextCharOffset.X + size.Width >= 1)
						this.nextCharOffset = new Vector2(0, this.nextCharOffset.Y + size.Height);

					this.characterInfos.Add(c, new CharTextureInfo(nextCharOffset, size));
					g.DrawString(c.ToString(), font, textBrush, nextCharOffset.X * Width, nextCharOffset.Y * Height, StringFormat.GenericTypographic);

					this.nextCharOffset += new Vector2(size.Width, 0);
				}
			}

			TextureUtils.UpdateTexture(this.TextureId, this.textureBitmap);
		}
		
		public void RenderText(string text, float adjustment)
		{
			float textWidth = measureWidth(text);
			float charOffset = textWidth * adjustment;
			
			GL.BindTexture(TextureTarget.Texture2D, this.TextureId);
			GL.Begin(BeginMode.Quads);

			foreach (char c in text)
				if (!char.IsWhiteSpace(c)) {
					var charInfo = this.characterInfos[c];

					for (int v = 0; v < 4; v++) {
						GL.TexCoord2(charInfo.TextureCoords[v]);
						GL.Vertex2(
							unitQuad[v].X * charInfo.Aspect + charOffset,
							unitQuad[v].Y
						);
					}
					charOffset += charInfo.Aspect;
				}
				else if (c == ' ')
					charOffset += SpaceUnitWidth;

			GL.End();
		}
	}
}
