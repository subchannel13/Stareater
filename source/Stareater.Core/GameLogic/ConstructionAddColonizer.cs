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
				var wormhole = states.Wormholes.At[lastStar].FirstOrDefault(x => x.FromStar == nextStar || x.ToStar == nextStar);
				missions.AddLast(new MoveMission(Destination.Star, wormhole));
			}
			
			missions.AddLast(new ColonizationMission(Destination));
			
			//TODO(v0.6) report new ship construction
			derivates.Of(site.Owner).SpawnShip(site.Location.Star, this.ColonizerDesign, quantity, missions, states);
		}
		
		public void Accept(IConstructionVisitor visitor)
		{
			visitor.Visit(this);
		}
		#endregion
	}
}
