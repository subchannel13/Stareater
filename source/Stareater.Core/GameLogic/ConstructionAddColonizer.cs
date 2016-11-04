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
		public Design ColonizerDesign { get; private set; }
		public Planet Destination { get; private set; }

		public ConstructionAddColonizer(Design colonizerDesign, Planet destination)
		{
			this.ColonizerDesign = colonizerDesign;
			this.Destination = destination;
		}
			
		#region IConstructionEffect implementation
		public void Apply(StatesDB states, TemporaryDB derivates, AConstructionSite site, long quantity)
		{
			var project = states.ColonizationProjects.Of[Destination].FirstOrDefault(x => x.Owner == this.ColonizerDesign.Owner);
			var missions = new LinkedList<AMission>();
			missions.AddLast(new SkipTurnMission());
			
			//TODO(later) check shortest path
			if (site.Location.Star != Destination.Star)
			{
				var lastStar = site.Location.Star;
				var nextStar = Destination.Star;
				var wormhole = states.Wormholes.At(lastStar).FirstOrDefault(x => x.FromStar == nextStar || x.ToStar == nextStar);
				missions.AddLast(new MoveMission(Destination.Star, wormhole));
			}
			
			missions.AddLast(new ColonizationMission(Destination));
			
			var fleet = states.Fleets.At(site.Location.Star.Position).FirstOrDefault(x => x.Owner == site.Owner && x.Missions.SequenceEqual(missions));
			if (fleet == null)
			{
				fleet = new Fleet(site.Owner, site.Location.Star.Position, missions);
				states.Fleets.Add(fleet);
			}

			if (fleet.Ships.DesignContains(ColonizerDesign))
				fleet.Ships.Design(ColonizerDesign).Quantity += quantity;
			else
				fleet.Ships.Add(new ShipGroup(ColonizerDesign, quantity, 0, 0));
		}
		
		public void Accept(IConstructionVisitor visitor)
		{
			visitor.Visit(this);
		}
		#endregion
	}
}
