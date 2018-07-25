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
	}
}
