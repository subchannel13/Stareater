using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Stareater.GLData
{
	class FastPixel
	{
		private byte[] rgbValues;
		private BitmapData bmpData;
		private IntPtr bmpPtr;
		private bool locked = false;

		private readonly bool isAlpha;
		private readonly Bitmap bitmap;
		private readonly int width;
		private readonly int height;
		private readonly int bytesPerPixel;

		public FastPixel(Bitmap bitmap)
		{
			if (bitmap.PixelFormat == (bitmap.PixelFormat | PixelFormat.Indexed))
				throw new Exception("Cannot lock an Indexed image.");

			this.bitmap = bitmap;
			this.isAlpha = this.bitmap.PixelFormat.HasFlag(PixelFormat.Alpha);
			this.bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
			this.width = bitmap.Width;
			this.height = bitmap.Height;
		}

		public void Lock()
		{
			if (this.locked)
				throw new Exception("Bitmap already locked.");

			var rect = new Rectangle(0, 0, this.width, this.height);
			this.bmpData = this.bitmap.LockBits(rect, ImageLockMode.ReadWrite, this.bitmap.PixelFormat);
			this.bmpPtr = this.bmpData.Scan0;

			if (this.isAlpha)
			{
				int bytes = (this.width * this.height) * 4;
				this.rgbValues = new byte[bytes];
				System.Runtime.InteropServices.Marshal.Copy(this.bmpPtr, rgbValues, 0, this.rgbValues.Length);
			}
			else
			{
				int bytes = (this.bmpData.Stride * this.height);
				this.rgbValues = new byte[bytes];
				System.Runtime.InteropServices.Marshal.Copy(this.bmpPtr, rgbValues, 0, this.rgbValues.Length);
			}
			this.locked = true;
		}

		public void Unlock(bool setPixels)
		{
			if (!this.locked)
				throw new Exception("Bitmap not locked.");

			// Copy the RGB values back to the bitmap;
			if (setPixels)
				System.Runtime.InteropServices.Marshal.Copy(this.rgbValues, 0, this.bmpPtr, this.rgbValues.Length);

			// Unlock the bits.;
			this.bitmap.UnlockBits(bmpData);
			this.locked = false;
		}

		public void Clear(Color colour)
		{
			if (!this.locked)
				throw new Exception("Bitmap not locked.");

			if (this.isAlpha)
			{
				for (int index = 0; index < this.rgbValues.Length; index += 4)
				{
					this.rgbValues[index] = colour.B;
					this.rgbValues[index + 1] = colour.G;
					this.rgbValues[index + 2] = colour.R;
					this.rgbValues[index + 3] = colour.A;
				}
			}
			else
			{
				for (int index = 0; index < this.rgbValues.Length; index += bytesPerPixel)
				{
					this.rgbValues[index] = colour.B;
					this.rgbValues[index + 1] = colour.G;
					this.rgbValues[index + 2] = colour.R;
				}
			}
		}

		public void SetPixel(Point location, Color colour)
		{
			this.SetPixel(location.X, location.Y, colour);
		}

		public void SetPixel(int x, int y, Color colour)
		{
			if (!this.locked)
				throw new Exception("Bitmap not locked.");

			if (this.isAlpha)
			{
				int index = (y * bmpData.Stride + x * 4);
				this.rgbValues[index] = colour.B;
				this.rgbValues[index + 1] = colour.G;
				this.rgbValues[index + 2] = colour.R;
				this.rgbValues[index + 3] = colour.A;
			}
			else
			{
				int index = (y * bmpData.Stride + x * bytesPerPixel);
				this.rgbValues[index] = colour.B;
				this.rgbValues[index + 1] = colour.G;
				this.rgbValues[index + 2] = colour.R;
			}
		}

		public Color GetPixel(Point location)
		{
			return this.GetPixel(location.X, location.Y);
		}

		public Color GetPixel(int x, int y)
		{
			if (!this.locked)
				throw new Exception("Bitmap not locked.");

			if (this.isAlpha)
			{
				int index = (y * bmpData.Stride + x * 4);
				int b = this.rgbValues[index];
				int g = this.rgbValues[index + 1];
				int r = this.rgbValues[index + 2];
				int a = this.rgbValues[index + 3];
				return Color.FromArgb(a, r, g, b);
			}
			else
			{
				int index = (y * bmpData.Stride + x * bytesPerPixel);
				int b = this.rgbValues[index];
				int g = this.rgbValues[index + 1];
				int r = this.rgbValues[index + 2];
				return Color.FromArgb(r, g, b);
			}
		}
	}
}
