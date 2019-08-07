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
			this.pixels = extractPixels(image);
			this.Height = image.Height;
			this.Width = image.Width;
		}

		public ColorMap(int width, int height, Color fillColor)
		{
			this.Width = width;
			this.Height = height;
			this.pixels = new Color[height, width];

			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
					this.pixels[y, x] = fillColor;
		}

		public int[] RawPixels()
		{
			var bgraValues = new int[this.Height * this.Width];

			for (int y = 0; y < this.Height; y++)
				for (int x = 0; x < this.Width; x++)
					bgraValues[this.Width * y + x] = this.pixels[y, x].ToArgb(); 

			return bgraValues;
		}

		public void Paste(Bitmap image, int offsetX, int offsetY, int width, int height)
		{
			var imagePixels = extractPixels(image);

			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
					this.pixels[offsetY + y, offsetX + x] = imagePixels[y, x];
		}

		public Color this[int x, int y]
		{
			get { return this.pixels[y, x]; }
			set { this.pixels[y, x] = value; }
		}

		private static Color[,] extractPixels(Bitmap image)
		{
			var bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
			var bgraValues = new int[bmpData.Width * bmpData.Height];
			Marshal.Copy(bmpData.Scan0, bgraValues, 0, bgraValues.Length);

			var pixels = new Color[image.Height, image.Width];
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
					pixels[y, x] = Color.FromArgb(bgraValues[image.Width * y + x]);

			image.UnlockBits(bmpData);
			return pixels;
		}
	}
}
