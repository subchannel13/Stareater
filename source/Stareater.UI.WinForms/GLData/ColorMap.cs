using OpenTK.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Stareater.GLData
{
	class ColorMap
	{
		private readonly Color4[,] pixels;

		public int Height { get; private set; }
		public int Width { get; private set; }

		public ColorMap(Bitmap image)
		{
			this.pixels = extractPixels(image);
			this.Height = image.Height;
			this.Width = image.Width;
		}

		public ColorMap(int width, int height)
		{
			this.Width = width;
			this.Height = height;
			this.pixels = new Color4[height, width];
		}

		public ColorMap(int width, int height, Color4 fillColor) : this(width, height)
		{
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

		public IEnumerable<Color4> Subregion(int minX, int minY, int maxX, int maxY)
		{
			for (int y = minY; y <= maxY; y++)
				for (int x = minX; x <= maxX; x++)
					if (x >= 0 && x < this.Width && y >= 0 && y < this.Height)
						yield return this[x, y];
		}

		public Color4 this[int x, int y]
		{
			get { return this.pixels[y, x]; }
			set { this.pixels[y, x] = value; }
		}

		public void Save(string filename)
		{
			using(var bitmap = new Bitmap(this.Width, this.Height))
			{
				var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
				var pixels = this.RawPixels();
				Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
				bitmap.Save(filename, ImageFormat.Png);
			}
		}

		private static Color4[,] extractPixels(Bitmap image)
		{
			var bmpData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
			var bgraValues = new int[bmpData.Width * bmpData.Height];
			Marshal.Copy(bmpData.Scan0, bgraValues, 0, bgraValues.Length);

			var pixels = new Color4[image.Height, image.Width];
			for (int y = 0; y < image.Height; y++)
				for (int x = 0; x < image.Width; x++)
				{
					var data = bgraValues[image.Width * y + x];
					pixels[y, x] = new Color4(
						(byte)(data >> 16 & 0xFF), 
						(byte)(data >> 8 & 0xFF), 
						(byte)(data & 0xFF), 
						(byte)(data >> 24 & 0xFF)
					);
				}

			image.UnlockBits(bmpData);
			return pixels;
		}
	}
}
