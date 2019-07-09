using System.Drawing;

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
			this.canvas.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			this.brush = new SolidBrush(Color.White);
		}

		public void Dispose()
		{
			this.canvas.Dispose();
		}

		public Rectangle Draw(char c)
		{
			var rect = this.atlas.Add(this.canvas.MeasureString(c.ToString(), this.font, int.MaxValue, StringFormat.GenericTypographic));

			this.canvas.DrawString(c.ToString(), this.font, this.brush, rect.Location, StringFormat.GenericTypographic);
			return rect;
		}
	}
}
