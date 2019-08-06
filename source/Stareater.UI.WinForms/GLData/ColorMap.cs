using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Stareater.GLData
{
	class ColorMap
	{
		private readonly Color[,] pixels;

		public int Height { get; private set; }
		public int Width { get; private set; }

		public ColorMap(Bitmap image)
		{
			this.pixels = new Color[image.Height, image.Width];
			this.Height = image.Height;
			this.Width = image.Width;

			var bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
			var bgraValues = new int[bmpData.Width * bmpData.Height];
			Marshal.Copy(bmpData.Scan0, bgraValues, 0, bgraValues.Length);

			for (int y = 0; y < this.Height; y++)
				for (int x = 0; x < this.Width; x++)
					this.pixels[y, x] = Color.FromArgb(bgraValues[image.Width * y + x]);

			image.UnlockBits(bmpData);
		}

		public int[] RawColors()
		{
			var bgraValues = new int[this.Height * this.Width];

			for (int y = 0; y < this.Height; y++)
				for (int x = 0; x < this.Width; x++)
					bgraValues[this.Width * y + x] = this.pixels[y, x].ToArgb(); 

			return bgraValues;
		}
	}
}
