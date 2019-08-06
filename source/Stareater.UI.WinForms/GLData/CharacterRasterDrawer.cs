using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Stareater.GLData
{
	class CharacterRasterDrawer : ICharacterDrawer
	{
		private readonly AtlasBuilder atlas;
		private readonly Font font;
		private readonly Graphics canvas;
		private readonly Brush brush;

		public CharacterRasterDrawer(AtlasBuilder atlas, Bitmap texture, Font font)
		{
			this.atlas = atlas;
			this.font = font;
			this.canvas = Graphics.FromImage(texture);
			this.canvas.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			this.brush = new SolidBrush(Color.White);
		}

		public void Dispose()
		{
			this.canvas.Dispose();
		}

		public Rectangle Draw(char c)
		{
			var text = c.ToString();

			var rect = this.atlas.Add(new SizeF(
				this.canvas.MeasureString(text, this.font, int.MaxValue, StringFormat.GenericTypographic).Width,
				TextRenderer.MeasureText(this.canvas, text, this.font, new Size(int.MaxValue, int.MaxValue)).Height
			));

			using (var textBmp = new Bitmap(rect.Width, rect.Height))
			{
				using (var g = Graphics.FromImage(textBmp))
				{
					g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
					g.DrawString(text, this.font, this.brush, 0, 0, StringFormat.GenericTypographic);
				}

				var bmpData = textBmp.LockBits(new Rectangle(0, 0, textBmp.Width, textBmp.Height), ImageLockMode.ReadWrite, textBmp.PixelFormat);
				var bgraValues = new byte[4 * bmpData.Width * textBmp.Height];
				System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, bgraValues, 0, bgraValues.Length);

				for (int y = 0; y < textBmp.Height; y++)
					for (int x = 0; x < textBmp.Width; x++)
					{
						var i = 4 * (y * bmpData.Width + x);
						bgraValues[i] = 255;
						bgraValues[i + 1] = 255;
						bgraValues[i + 2] = 255;
					}
				System.Runtime.InteropServices.Marshal.Copy(bgraValues, 0, bmpData.Scan0, bgraValues.Length);
				textBmp.UnlockBits(bmpData);

				this.canvas.DrawImage(textBmp, rect.Location);
			}

			return rect;
		}
	}
}
