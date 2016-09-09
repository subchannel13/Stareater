using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.Ships;
using Stareater.Ships.Missions;

namespace Stareater.GameLogic
{
	class ConstructionAddColonizer : IConstructionEffect
	{
		private readonly Design colonizerDesign;
		private readonly Planet destination;

		public ConstructionAddColonizer(Design colonizerDesign, Planet destination)
		{
			this.colonizerDesign = colonizerDesign;
			this.destination = destination;
		}
			
		#region IConstructionEffect implementation
		public void Apply(StatesDB states, TemporaryDB derivates, AConstructionSite site, long quantity)
		{
			var project = states.ColonizationProjects.Of(destination).FirstOrDefault(x => x.Owner == this.colonizerDesign.Owner);
			var missions = new LinkedList<AMission>();
			missions.AddLast(new SkipTurnMission());
			
			//TODO(later) check shortest path
			if (site.Location.Star != destination.Star)
			{
				var lastStar = site.Location.Star;
				var nextStar = destination.Star;
				var wormhole = states.Wormholes.At(lastStar).FirstOrDefault(x => x.FromStar == nextStar || x.ToStar == nextStar);
				missions.AddLast(new MoveMission(destination.Star, wormhole));
			}
			
			missions.AddLast(new ColonizationMission(destination));
			
			var fleet = states.Fleets.At(site.Location.Star.Position).FirstOrDefault(x => x.Owner == site.Owner && x.Missions.SequenceEqual(missions));
			if (fleet == null)
			{
				fleet = new Fleet(site.Owner, site.Location.Star.Position, missions);
				states.Fleets.Add(fleet);
			}

			if (fleet.Ships.DesignContains(colonizerDesign))
				fleet.Ships.Design(colonizerDesign).Quantity += quantity;
			else
				fleet.Ships.Add(new ShipGroup(colonizerDesign, quantity, 0, 0));
		}
		#endregion
		
	}
}
