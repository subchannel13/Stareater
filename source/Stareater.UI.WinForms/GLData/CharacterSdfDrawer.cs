using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Stareater.GLData
{
	class CharacterSdfDrawer : ICharacterDrawer
	{
		private const int Padding = 2;

		private readonly AtlasBuilder atlas;
		private readonly Font font;
		private readonly FastPixel pixelator;
		private readonly Bitmap fakeBitmap;
		private readonly Graphics fakeCanvas;

		public CharacterSdfDrawer(AtlasBuilder atlas, Bitmap texture, Font font)
		{
			this.atlas = atlas;
			this.font = font;
			this.pixelator = new FastPixel(texture);
			this.fakeBitmap = new Bitmap(1, 1);
			this.fakeCanvas = Graphics.FromImage(this.fakeBitmap);

			pixelator.Lock();
		}

		public void Dispose()
		{
			pixelator.Unlock(true);
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
			var measuredSize = this.fakeCanvas.MeasureString(text, font, int.MaxValue, StringFormat.GenericTypographic);
			var rect = this.atlas.Add(measuredSize);
			
			int width = rect.Size.Width + Padding * 2;
			int height = rect.Size.Height + Padding * 2;
			var distField = genSdf(contures, width, height);

			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
					pixelator.SetPixel(rect.X + x, rect.Y + y, Color.FromArgb((int)(distField[y, x] * 255), 255, 255, 255));

			return rect;
		}

		private static List<Contour> getContures(GraphicsPath path)
		{
			var contoures = new List<Contour>();
			var pathIter = new GraphicsPathIterator(path);
			var pathPoints = path.PathPoints;

			for (int subPathI = 0; subPathI < pathIter.SubpathCount; subPathI++)
			{
				var mySubPaths = pathIter.NextSubpath(out int subPathStartIndex, out int subPathEndIndex, out bool isClosed);

				var strokes = new List<Stroke>();
				while (true)
				{
					var count = pathIter.NextPathType(out byte pType, out int startI, out int endI);

					if (count == 0)
						break;

					if (pType != (byte)PathPointType.Line)
						throw new Exception("Invalid stroke type " + pType);

					for (int i = 1; i < count; i++)
						strokes.Add(new Stroke(pathPoints[startI + i - 1], pathPoints[startI + i]));
				}

				if (isClosed)
					strokes.Add(new Stroke(path.PathPoints[subPathEndIndex], path.PathPoints[subPathStartIndex]));

				contoures.Add(new Contour(strokes));
			}

			return contoures;
		}

		private double[,] genSdf(List<Contour> contures, int width, int height)
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

		class Contour
		{
			private readonly IEnumerable<Stroke> strokes;

			public Contour(IEnumerable<Stroke> strokes)
			{
				this.strokes = strokes;
			}

			public double Distance(Vector2 fromP)
			{
				return this.strokes.Min(stroke => stroke.Distance(fromP));
			}

			public int RayHits(Vector2 fromP)
			{
				return this.strokes.
					Where(stroke => stroke.RayHitTest(fromP)).
					Count();
			}
		}

		class Stroke
		{
			private readonly Vector2 p0, p1;

			public Stroke(PointF p0, PointF p1)
			{
				this.p0 = new Vector2(p0.X, p0.Y);
				this.p1 = new Vector2(p1.X, p1.Y);
			}

			public float Distance(Vector2 fromP)
			{
				var p = fromP - this.p0;
				var dir = this.p1 - this.p0;
				if (dir.X == 0 && dir.Y == 0)
					return (fromP - this.p0).Length;

				var t = Vector2.Dot(p, dir) / dir.LengthSquared;

				if (float.IsNaN(t))
					throw new ArithmeticException();

				if (t <= 0)
					return (fromP - this.p0).Length;
				if (t >= 1)
					return (fromP - this.p1).Length;

				return (fromP - this.p0 * t).Length;
			}

			public bool RayHitTest(Vector2 fromP)
			{
				var dir = this.p1 - this.p0;

				if (dir.Y == 0)
					return false;

				var yShift = this.p0.Y == fromP.Y || this.p1.Y == fromP.Y ? 
					Math.Min(0.5f, Math.Abs(dir.Y / 2)) : 
					0;

				var p = this.p0 - fromP - new Vector2(0, yShift);

				var t = -p.Y / dir.Y;

				if (float.IsNaN(t))
					throw new ArithmeticException();

				if (t < 0 || t > 1)
					return false;

				if (t == 0 || t == 1)
					return this.p0.Y < 0 || this.p1.Y < 0;

				return p.X + t * dir.X > 0;
			}
		}
	}
}
