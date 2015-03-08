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
		private Design design;
		
		public ConstructionAddShip(Design design)
		{
			this.design = design;
		}
		
		public void Apply(StatesDB states, AConstructionSite site, double quantity)
		{
			//TODO(v0.5) report new ship construction
			var fleet = states.Fleets.At(site.Location.Star.Position).
				Where(x => x.Owner == site.Owner && x.Mission.Type == MissionType.Stationary).
				FirstOrDefault();

			if (fleet == null) {
				fleet = new Fleet(site.Owner, site.Location.Star.Position, new StationaryMission(site.Location.Star));
				states.Fleets.Add(fleet);
			}
			
			long intQuantity = (long)Math.Floor(quantity);
			
			if (fleet.Ships.DesignContains(design))
				fleet.Ships.Design(design).Quantity += intQuantity;
			else
				fleet.Ships.Add(new ShipGroup(design, intQuantity));
		}
	}
}
