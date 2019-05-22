namespace Stareater.Controllers.Data
{
	class ShipSelection
	{
		public long Quantity { get; private set; }
		public long MaxQuantity { get; private set; }
		public double Population { get; private set; }

		public ShipSelection(long quantity, long maxQuantity, double population)
		{
			this.Quantity = quantity;
			this.MaxQuantity = maxQuantity;
			this.Population = population;
		}
	}
}
