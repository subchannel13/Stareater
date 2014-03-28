using System;
using Stareater.Ships;

namespace Stareater.Galaxy
{
	class ShipGroup
	{
		public Design Design { get; private set; }
		public long Quantity { get; set; }
		
		public ShipGroup(Design design, long quantity)
		{
			this.Design = design;
			this.Quantity = quantity;
		}
	}
}
