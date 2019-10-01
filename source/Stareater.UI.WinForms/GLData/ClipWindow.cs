using System;
using System.Drawing;
using OpenTK;

namespace Stareater.GLData
{
	class ClipWindow : IEquatable<ClipWindow>
	{
		private readonly Vector2 min;
		private readonly Vector2 max;

		public ClipWindow()
		{
			this.min = new Vector2();
			this.max = new Vector2();
		}

		public ClipWindow(Vector2 center, Vector2 size)
		{
			this.min = center - size / 2;
			this.max = center + size / 2;
		}

		public ClipWindow(Vector2 center, Vector2 size, ClipWindow container)
		{
			var childMin = center - size / 2;
			var childMax = center + size / 2;

			this.min = new Vector2(Math.Max(childMin.X, container.min.X), Math.Max(childMin.Y, container.min.Y));
			this.max = new Vector2(Math.Min(childMax.X, container.max.X), Math.Min(childMax.Y, container.max.Y));
		}

		public bool Contains(Vector2 point)
		{
			return point.X >= this.min.X && point.X < this.max.X &&
				point.Y >= this.min.Y && point.Y < this.max.Y;
		}

		public bool IsEmpty
		{
			get
			{
				return this.min.X >= this.max.X || this.min.Y >= this.max.Y;
			}
		}

		public Rectangle ScissorRectangle(Matrix4 view)
		{
			var screenMin = Vector4.Transform(new Vector4(this.min.X, this.min.Y, 0, 1), view);
			var size = Vector4.Transform(new Vector4(this.max.X, this.max.Y, 0, 1), view) - screenMin;

			return new Rectangle(
				(int)Math.Floor(screenMin.X), (int)Math.Floor(screenMin.Y), 
				(int)Math.Ceiling(size.X), (int)Math.Ceiling(size.Y)
			);
		}

		public bool Equals(ClipWindow other)
		{
			if (other is null)
				return false;

			if (Object.ReferenceEquals(this, other))
				return true;

			if (this.GetType() != other.GetType())
				return false;

			return (this.max == other.max) && (this.min == other.min) || (this.IsEmpty && other.IsEmpty);
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as ClipWindow);
		}

		public override int GetHashCode()
		{
			if (this.IsEmpty)
				return 0;

			return unchecked(this.min.GetHashCode() * 0x00010000 + this.max.GetHashCode());
		}

		public static bool operator ==(ClipWindow lhs, ClipWindow rhs)
		{
			if (lhs is null)
				return rhs is null;
			
			return lhs.Equals(rhs);
		}

		public static bool operator !=(ClipWindow lhs, ClipWindow rhs)
		{
			return !(lhs == rhs);
		}
	}
}
