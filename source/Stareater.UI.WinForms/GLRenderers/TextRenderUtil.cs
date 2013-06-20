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
		
		Bitmap textureBitmap;
		Font font;
		Vector2 nextCharOffset;

		Dictionary<char, CharTextureInfo> characterInfos = new Dictionary<char, CharTextureInfo>();
		Vector2[] unitQuad;
		
		private TextRenderUtil()
		{
			unitQuad = new Vector2[] {
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
					if (!characterInfos.ContainsKey(c))
						missinCharacters.Add(c);

			if (missinCharacters.Count == 0 && TextureManager.Get.FontTextureId == 0)
				return;

			lazyInitialization();

			using (Graphics g = Graphics.FromImage(textureBitmap)) {
				if (nextCharOffset == Vector2.Zero)
					g.Clear(Color.Transparent);

				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				Brush textBrush = new SolidBrush(Color.White);

				foreach (char c in missinCharacters) {
					SizeF size = g.MeasureString(c.ToString(), font, int.MaxValue, StringFormat.GenericTypographic);
					size = new SizeF(
						(float)Math.Ceiling(size.Width) / Width,
						(float)Math.Ceiling(size.Height) / Height
					);

					if (nextCharOffset.X + size.Width >= 1)
						nextCharOffset += new Vector2(0, size.Height);

					characterInfos.Add(c, new CharTextureInfo(nextCharOffset, size));
					g.DrawString(c.ToString(), font, textBrush, nextCharOffset.X * Width, nextCharOffset.Y * Height, StringFormat.GenericTypographic);

					nextCharOffset += new Vector2(size.Width, 0);
				}
			}

			TextureManager.Get.UpdateTexture(TextureManager.Get.FontTextureId, textureBitmap);
		}
		
		private void lazyInitialization()
		{
			if (textureBitmap == null)
				textureBitmap = new Bitmap(Width, Height);
			
			if (font == null)
				font = new Font(FontFamily, FontSize, FontStyle.Bold);
			
			if (TextureManager.Get.FontTextureId == 0)
				TextureManager.Get.Load(TextureContext.Font, textureBitmap);
		}

		public void RenderText(string text, float adjustment)
		{
			float textWidth = 0;
			foreach (char c in text)
				textWidth += characterInfos[c].Aspect;

			GL.BindTexture(TextureTarget.Texture2D, TextureManager.Get.FontTextureId);

			float charOffset = textWidth * adjustment;
			GL.Begin(BeginMode.Quads);

			foreach (char c in text) {
				var charInfo = characterInfos[c];
				
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
