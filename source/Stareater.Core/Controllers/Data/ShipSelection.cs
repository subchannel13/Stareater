using Stareater.Galaxy;

namespace Stareater.Controllers.Data
{
	class ShipSelection
	{
		public long Quantity { get; private set; }
		public double Population { get; private set; }
		public ShipGroup Ships { get; private set; }

		public ShipSelection(long quantity, ShipGroup ships, double population)
		{
			this.Quantity = quantity;
			this.Population = population;
			this.Ships = ships;
		}
	}
}
