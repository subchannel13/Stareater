using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Ships.Missions;

namespace Stareater.GameLogic
{
	class FleetProcessingVisitor : IMissionVisitor
	{
		private readonly Fleet fleet;
		private readonly Game game;
		
		public FleetProcessingVisitor(Fleet fleet, Game game)
		{
			this.fleet = fleet;
			this.game = game;
		}

		#region IMissionVisitor implementation

		public void Visit(MoveMission mission)
		{
			this.game.States.Fleets.PendRemove(fleet);
			
			var playerProc = game.Derivates.Players.Of(fleet.Owner);
			double baseSpeed = fleet.Ships.Select(x => x.Design).
				Aggregate(double.MaxValue, (s, x) => Math.Min(playerProc.DesignStats[x].GalaxySpeed, s));
				
			//TODO(v0.5) loop through all waypoints
			var speed = baseSpeed;
			if (mission.UsedWormhole != null)
				speed += 0.5; //TODO(later) consider making moddable
			
			var distance = (mission.Destination.Position - fleet.Position).Magnitude();

			//TODO(v0.5) detect conflicts
			//TODO(v0.5) merge with existing fleet
			if (distance <= speed) {
				var newFleet = new Fleet(
					fleet.Owner,
					mission.Destination.Position,
					new LinkedList<AMission>()
				);
				newFleet.Ships.Add(fleet.Ships);
				this.game.States.Fleets.PendAdd(newFleet);
				
				fleet.Owner.Intelligence.StarFullyVisited(game.States.Stars.At(mission.Destination.Position), game.Turn);
			}
			else {
				var direction = (mission.Destination.Position - fleet.Position);
				direction.Normalize();

				var newFleet = new Fleet(
					fleet.Owner,
					fleet.Position + direction * speed,
					new LinkedList<AMission>(fleet.Missions)
				);
				newFleet.Ships.Add(fleet.Ships);
				this.game.States.Fleets.PendAdd(newFleet);
			}
		}

		public void Visit(ColonizationMission mission)
		{
			this.game.States.Fleets.PendRemove(fleet);
			var project = game.States.ColonizationProjects.Of(mission.Target);
			
			foreach(var group in fleet.Ships)
				project.Arrived.Add(group);
		}

		public void Visit(SkipTurnMission mission)
		{
			//No operation
		}
		
		#endregion
	}
}
