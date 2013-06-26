using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLRenderers
{
	public class TextRenderUtil
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
		
		private int textureId;
		private Bitmap textureBitmap;
		private Font font;
		private Vector2 nextCharOffset;

		private Dictionary<char, CharTextureInfo> characterInfos = new Dictionary<char, CharTextureInfo>();
		private Vector2[] unitQuad;
		
		private TextRenderUtil()
		{
			this.unitQuad = new Vector2[] {
				new Vector2(0, 0),
				new Vector2(0, -1),
				new Vector2(1, -1),
				new Vector2(1, 0),
			};
		}

		public void Prepare(IEnumerable<string> texts)
		{
			HashSet<char> missinCharacters = new HashSet<char>();
			foreach (string text in texts)
				foreach (char c in text)
					if (!this.characterInfos.ContainsKey(c))
						missinCharacters.Add(c);

			if (missinCharacters.Count == 0 && this.textureId != 0)
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
						this.nextCharOffset += new Vector2(0, size.Height);

					this.characterInfos.Add(c, new CharTextureInfo(nextCharOffset, size));
					g.DrawString(c.ToString(), font, textBrush, nextCharOffset.X * Width, nextCharOffset.Y * Height, StringFormat.GenericTypographic);

					this.nextCharOffset += new Vector2(size.Width, 0);
				}
			}

			TextureUtils.Get.UpdateTexture(this.textureId, this.textureBitmap);
		}
		
		private void lazyInitialization()
		{
			if (this.textureBitmap == null)
				this.textureBitmap = new Bitmap(Width, Height);
			
			if (this.font == null)
				this.font = new Font(FontFamily, FontSize, FontStyle.Bold);
			
			if (this.textureId == 0)
				this.textureId = TextureUtils.Get.CreateTexture(this.textureBitmap);
		}

		public void RenderText(string text, float adjustment)
		{
			float textWidth = 0;
			foreach (char c in text)
				textWidth += this.characterInfos[c].Aspect;

			GL.BindTexture(TextureTarget.Texture2D, this.textureId);

			float charOffset = textWidth * adjustment;
			GL.Begin(BeginMode.Quads);

			foreach (char c in text) {
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
			GL.End();
		}
	}
}
