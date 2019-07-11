using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stareater.GLData
{
	class CharacterSdfDrawer : ICharacterDrawer
	{
		private const int Padding = 4;

		private readonly AtlasBuilder atlas;
		private readonly Font font;
		private readonly Bitmap texture;
		private readonly BitmapData bmpData;
		private readonly IntPtr bmpPtr;
		private readonly Bitmap fakeBitmap;
		private readonly Graphics fakeCanvas;

		public CharacterSdfDrawer(AtlasBuilder atlas, Bitmap texture, Font font)
		{
			this.atlas = atlas;
			this.font = font;
			this.texture = texture;
			this.fakeBitmap = new Bitmap(1, 1);
			this.fakeCanvas = Graphics.FromImage(this.fakeBitmap);

			this.bmpData = texture.LockBits(new Rectangle(0, 0, texture.Width, texture.Height), ImageLockMode.ReadWrite, texture.PixelFormat);
			this.bmpPtr = this.bmpData.Scan0;

		}

		public void Dispose()
		{
			this.texture.UnlockBits(bmpData);
			fakeCanvas.Dispose();
			fakeBitmap.Dispose();
		}

		public Rectangle Draw(char c)
		{
			var text = c.ToString();
			var path = new GraphicsPath();
			path.AddString(text, font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), StringFormat.GenericTypographic);
			path.Flatten();

			var contures = getContures(path);
			var measuredSize = new SizeF(
				this.fakeCanvas.MeasureString(text, this.font, int.MaxValue, StringFormat.GenericTypographic).Width + Padding * 2,
				TextRenderer.MeasureText(this.fakeCanvas, text, this.font, new Size(int.MaxValue, int.MaxValue)).Height + Padding * 2
			);
			var rect = this.atlas.Add(measuredSize);
			
			int width = rect.Size.Width;
			int height = rect.Size.Height;
			var distField = genSdf(contures, width, height);

			for (int y = 0; y < height; y++)
			{
				var rgbValues = new byte[4 * width];
				for (int x = 0; x < width; x++)
				{
					rgbValues[4 * x] = (byte)(distField[y, x] * 255);
					rgbValues[4 * x + 1] = rgbValues[4 * x];
					rgbValues[4 * x + 2] = rgbValues[4 * x];
					rgbValues[4 * x + 3] = rgbValues[4 * x];
				}

				Marshal.Copy(rgbValues, 0, this.bmpPtr + (rect.Y + y) * bmpData.Stride + rect.X * 4, rgbValues.Length);
			}

			return new Rectangle(rect.X + Padding, rect.Y + Padding, width - 2 * Padding, height - 2 * Padding);
		}

		private static List<GlyphContour> getContures(GraphicsPath path)
		{
			var contoures = new List<GlyphContour>();
			var pathIter = new GraphicsPathIterator(path);
			var pathPoints = path.PathPoints;

			for (int subPathI = 0; subPathI < pathIter.SubpathCount; subPathI++)
			{
				var mySubPaths = pathIter.NextSubpath(out int subPathStartIndex, out int subPathEndIndex, out bool isClosed);

				var strokes = new List<PointF[]>();
				while (true)
				{
					var count = pathIter.NextPathType(out byte pType, out int startI, out int endI);

					if (count == 0)
						break;

					if (pType != (byte)PathPointType.Line)
						throw new Exception("Invalid stroke type " + pType);

					for (int i = 1; i < count; i++)
						strokes.Add(new PointF[] { pathPoints[startI + i - 1], pathPoints[startI + i] });
				}

				if (isClosed)
					strokes.Add(new PointF[] { path.PathPoints[subPathEndIndex], path.PathPoints[subPathStartIndex] });

				contoures.Add(new GlyphContour(strokes));
			}

			return contoures;
		}

		private double[,] genSdf(List<GlyphContour> contures, int width, int height)
		{
			var distField = new double[height, width];

			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
				{
					var fromP = new Vector2(x - Padding, y - Padding);
					var minDist = Math.Min(contures.Min(shape => shape.Distance(fromP)), Padding);
					if (contures.Sum(shape => shape.RayHits(fromP)) % 2 != 0)
						minDist *= -1;

					distField[y, x] = -minDist / Padding / 2 + 0.5;
				}

			return distField;
		}

	}
}
