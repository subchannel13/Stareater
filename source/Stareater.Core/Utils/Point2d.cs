using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Utils
{
	public struct Point2d
	{
		public double X { get; private set; }
		public double Y { get; private set; }

		public Point2d(double x, double y) : this()
		{
			this.X = x;
			this.Y = y;
		}

		public double Distance(Point2d other)
		{
			double dx = other.X - this.X;
			double dy = other.Y - this.Y;
			return Math.Sqrt(dx * dx + dy * dy);
		}

		public override bool Equals(object obj)
		{
			if (obj is Point2d) {
				Point2d other = (Point2d)obj;
				return this.X == other.X && this.Y == other.Y;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() + Y.GetHashCode() * 101;
		}
	}
}