using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stareater.GLData
{
	class GlyphContour
	{
		private readonly IEnumerable<Vector2[]> strokes;

		public GlyphContour(IEnumerable<PointF[]> strokes)
		{
			this.strokes = strokes.
				Select(stroke => new Vector2[] {
					new Vector2(stroke[0].X, stroke[0].Y),
					new Vector2(stroke[1].X, stroke[1].Y)
				}).ToList();
		}

		public double Distance(Vector2 fromP)
		{
			return this.strokes.Min(stroke => distance(stroke, fromP));
		}

		public int RayHits(Vector2 fromP)
		{
			return this.strokes.
				Where(stroke => rayHitTest(stroke, fromP)).
				Count();
		}

		private float distance(Vector2[] stroke, Vector2 fromP)
		{
			var p = fromP - stroke[0];
			var dir = stroke[1] - stroke[0];
			if (dir.X == 0 && dir.Y == 0)
				return (fromP - stroke[0]).Length;

			var t = Vector2.Dot(p, dir) / dir.LengthSquared;

			if (float.IsNaN(t))
				throw new ArithmeticException();

			if (t <= 0)
				return (fromP - stroke[0]).Length;
			if (t >= 1)
				return (fromP - stroke[1]).Length;

			return (p - dir * t).Length;
		}

		private bool rayHitTest(Vector2[] stroke, Vector2 fromP)
		{
			var dir = stroke[1] - stroke[0];

			if (dir.Y == 0)
				return false;

			var yShift = stroke[0].Y == fromP.Y || stroke[1].Y == fromP.Y ?
				Math.Min(0.5f, Math.Abs(dir.Y / 2)) :
				0;

			var p = new Vector2(stroke[0].X - fromP.X, stroke[0].Y - fromP.Y - yShift);

			var t = -p.Y / dir.Y;

			if (float.IsNaN(t))
				throw new ArithmeticException();

			if (t < 0 || t > 1)
				return false;

			if (t == 0 || t == 1)
				return stroke[0].Y < 0 || stroke[1].Y < 0;

			return p.X + t * dir.X > 0;
		}
	}
}
