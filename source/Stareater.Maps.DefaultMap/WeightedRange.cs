namespace Stareater.Maps.DefaultMap
{
	struct WeightedRange
	{
		public double Min { get; private set; }
		public double Max { get; private set; }
		public double Weight { get; private set; }

		public WeightedRange(double min, double max, double weight)
		{
			this.Min = min;
			this.Max = max;
			this.Weight = weight;
		}
	}
}
