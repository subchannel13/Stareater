using Stareater.Utils.StateEngine;
using Stareater.Ships;

namespace Stareater.Galaxy
{
	class ShipGroup 
	{
		[StatePropertyAttribute]
		public Design Design { get; private set; }

		[StatePropertyAttribute]
		public long Quantity { get; set; }

		[StatePropertyAttribute]
		public double Damage { get; set; }

		[StatePropertyAttribute]
		public double UpgradePoints { get; set; }

		[StatePropertyAttribute]
		public double PopulationTransport { get; set; }

		public ShipGroup(Design design, long quantity, double damage, double upgradePoints, double population) 
		{
			this.Design = design;
			this.Quantity = quantity;
			this.Damage = damage;
			this.UpgradePoints = upgradePoints;
			this.PopulationTransport = population;
		}

		private ShipGroup() 
		{ } 
	}
}
