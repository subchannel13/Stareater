namespace Stareater.Utils
{
	public class Circle
	{
		public Vector2D Center { get; private set; }
		public double Radius { get; private set; }

		public Circle(Vector2D center, double radius)
		{
			this.Center = center;
			this.Radius = radius;
		}

		public bool Contains(Vector2D point)
		{
			return (this.Center - point).LengthSquared <= this.Radius * this.Radius;
		}
	}
}
