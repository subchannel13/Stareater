using System;
using System.Linq;
using Stareater.Galaxy;
using Stareater.GameData.Databases;
using Stareater.Ships;
using Stareater.Ships.Missions;

namespace Stareater.GameLogic
{
	class ConstructionAddShip : IConstructionEffect
	{
		private readonly Design design;
		
		public ConstructionAddShip(Design design)
		{
			this.design = design;
		}

		public void Apply(StatesDB states, TemporaryDB derivates, AConstructionSite site, long quantity)
		{
			//TODO(v0.5) report new ship construction
			var fleet = states.Fleets.At(site.Location.Star.Position).FirstOrDefault(x => x.Owner == site.Owner && x.Mission == null);

			if (fleet == null) {
				fleet = new Fleet(site.Owner, site.Location.Star.Position, null);
				states.Fleets.Add(fleet);
			}
			
			if (fleet.Ships.DesignContains(design))
				fleet.Ships.Design(design).Quantity += quantity;
			else
				fleet.Ships.Add(new ShipGroup(design, quantity));
		}
	}
}
