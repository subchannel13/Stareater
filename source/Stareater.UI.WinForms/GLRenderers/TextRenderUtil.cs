using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;

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
		Dictionary<char, CharTextureInfo> characterPositions = new Dictionary<char, CharTextureInfo>();
		Vector2 nextCharOffset;
		
		private TextRenderUtil()
		{ }

		public void Prepare(IEnumerable<string> texts)
		{
			HashSet<char> missinCharacters = new HashSet<char>();
			foreach (string text in texts)
				foreach (char c in text)
					if (!characterPositions.ContainsKey(c))
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

					characterPositions.Add(c, new CharTextureInfo(nextCharOffset, size));
					g.DrawString(c.ToString(), font, textBrush, nextCharOffset.X, nextCharOffset.Y, StringFormat.GenericTypographic);

					nextCharOffset += new Vector2(size.Width, 0);
				}
			}
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
	}
}
