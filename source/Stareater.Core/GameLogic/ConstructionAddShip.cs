using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.GameData.Databases;
using Stareater.Ships;
using Stareater.Ships.Missions;

namespace Stareater.GameLogic
{
	class ConstructionAddShip : IConstructionEffect
	{
		public Design Design { get; private set; }
		
		public ConstructionAddShip(Design design)
		{
			this.Design = design;
		}

		public void Apply(StatesDB states, TemporaryDB derivates, AConstructionSite site, long quantity)
		{
			//TODO(v0.6) report new ship construction
			var fleet = states.Fleets.At[site.Location.Star.Position].FirstOrDefault(x => x.Owner == site.Owner && x.Missions.Count == 0);

			if (fleet == null) {
				fleet = new Fleet(site.Owner, site.Location.Star.Position, new LinkedList<AMission>());
				states.Fleets.Add(fleet);
			}
			
			if (fleet.Ships.DesignContains(Design))
				fleet.Ships.Design(Design).Quantity += quantity;
			else
				fleet.Ships.Add(new ShipGroup(Design, quantity, 0, 0));
		}
		
		public void Accept(IConstructionVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
