using System;
using System.Globalization;

namespace Stareater.Utils
{
	public struct Vector2D : IEquatable<Vector2D>
	{
		public double X { get; private set; }
		public double Y { get; private set; }

		public Vector2D(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		public bool IsZero
		{
			get
			{
				return this.X == 0 && this.Y == 0;
			}
		}

		public double Length
		{
			get
			{
				return Math.Sqrt(this.X * this.X + this.Y * this.Y);
			}
		}

		public double LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y;
			}
		}

		public Vector2D PerpendicularLeft
		{
			get
			{
				return new Vector2D(-this.Y, this.X);
			}
		}

		public Vector2D PerpendicularRight
		{
			get
			{
				return new Vector2D(this.Y, -this.X);
			}
		}

		public Vector2D Unit
		{
			get
			{
				if (this.X == 0 && this.Y == 0)
#pragma warning disable CA1303 // Do not pass literals as localized parameters
					throw new InvalidOperationException("Can't normalize zero vector");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

				var length = this.Length;
				
				return new Vector2D(this.X / length, this.Y / length);
			}
		}

		public double Dot(Vector2D right)
		{
			return this.X * right.X + this.Y * right.Y;
		}

		public double Cross(Vector2D right)
		{
			return this.X * right.Y - this.Y * right.X;
		}

		public bool Equals(Vector2D other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Vector2D))
				return false;

			return this.Equals((Vector2D)obj);
		}

		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		public override string ToString()
		{
			return String.Format(CultureInfo.InvariantCulture, "({0}, {1})", this.X, this.Y);
		}

		public static Vector2D Add(Vector2D left, Vector2D right)
		{
			return new Vector2D(left.X + right.X, left.Y + right.Y);
		}

		public static Vector2D Subtract(Vector2D left, Vector2D right)
		{
			return new Vector2D(left.X - right.X, left.Y - right.Y);
		}

		public static Vector2D Negate(Vector2D vector)
		{
			return new Vector2D(-vector.X, -vector.Y);
		}

		public static Vector2D Multiply(Vector2D vector, double scale)
		{
			return new Vector2D(vector.X * scale, vector.Y * scale);
		}

		public static Vector2D Divide(Vector2D vector, double scale)
		{
			return new Vector2D(vector.X / scale, vector.Y / scale);
		}

		public static Vector2D operator +(Vector2D left, Vector2D right)
		{
			return new Vector2D(left.X + right.X, left.Y + right.Y);
		}

		public static Vector2D operator -(Vector2D left, Vector2D right)
		{
			return new Vector2D(left.X - right.X, left.Y - right.Y);
		}

		public static Vector2D operator -(Vector2D v)
		{
			return new Vector2D(-v.X, -v.Y);
		}

		public static Vector2D operator *(Vector2D v, double scale)
		{
			return new Vector2D(v.X * scale, v.Y * scale);
		}

		public static Vector2D operator *(double scale, Vector2D v)
		{
			return new Vector2D(v.X * scale, v.Y * scale);
		}

		public static Vector2D operator /(Vector2D v, double scale)
		{
			return new Vector2D(v.X / scale, v.Y / scale);
		}

		public static bool operator ==(Vector2D left, Vector2D right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector2D left, Vector2D right)
		{
			return !left.Equals(right);
		}
	}
}
