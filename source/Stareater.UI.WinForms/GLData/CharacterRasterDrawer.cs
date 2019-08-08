using OpenTK.Graphics;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Stareater.GLData
{
	class CharacterRasterDrawer : ICharacterDrawer
	{
		private readonly AtlasBuilder atlas;
		private readonly Font font;
		private readonly ColorMap texture;
		private readonly Brush brush;

		public CharacterRasterDrawer(AtlasBuilder atlas, ColorMap texture, Font font)
		{
			this.atlas = atlas;
			this.font = font;
			this.texture = texture;
			this.brush = new SolidBrush(Color.White);
		}

		public void Dispose()
		{ }

		public Rectangle Draw(char c)
		{
			var text = c.ToString();
			Rectangle rect;

			using (var textBmp = new Bitmap(this.font.Height * 2, this.font.Height * 2))
			{
				using (var g = Graphics.FromImage(textBmp))
				{
					rect = this.atlas.Add(new SizeF(
						g.MeasureString(text, this.font, int.MaxValue, StringFormat.GenericTypographic).Width,
						TextRenderer.MeasureText(g, text, this.font, new Size(int.MaxValue, int.MaxValue)).Height
					));

					g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
					g.DrawString(text, this.font, this.brush, 0, 0, StringFormat.GenericTypographic);
				}
				this.texture.Paste(textBmp, rect.X, rect.Y, rect.Width, rect.Height);

				for (int y = 0; y < textBmp.Height; y++)
					for (int x = 0; x < textBmp.Width; x++)
						this.texture[rect.X + x, rect.Y + y] = new Color4(1, 1, 1, this.texture[rect.X + x, rect.Y + y].A);
			}

			return rect;
		}
	}
}
