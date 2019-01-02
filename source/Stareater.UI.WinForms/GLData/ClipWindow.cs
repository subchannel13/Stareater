using System;
using System.Drawing;
using OpenTK;

namespace Stareater.GLData
{
	class ClipWindow
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
	}
}
