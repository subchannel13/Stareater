using System.Drawing;
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

			this.canvas.DrawString(text, this.font, this.brush, rect.Location, StringFormat.GenericTypographic);
			return rect;
		}
	}
}
