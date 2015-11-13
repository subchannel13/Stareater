using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Ships.Missions;

namespace Stareater.GameLogic
{
	class FleetProcessingVisitor : IMissionVisitor
	{
		private Fleet fleet;
		private Game game;
		
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
			var startStar = game.States.Stars.At(mission.Waypoints[0]);
			var endStar = game.States.Stars.At(mission.Waypoints[1]);
			var speed = baseSpeed;
			if (game.States.Wormholes.At(startStar).Intersect(game.States.Wormholes.At(endStar)).Any())
				speed += 0.5; //TODO(later) consider making moddable
			
			var waypoints = mission.Waypoints.Skip(1).ToArray();
			var distance = (waypoints[0] - fleet.Position).Magnitude();

			//TODO(v0.5) detect conflicts
			//TODO(v0.5) merge with existing fleet
			if (distance <= speed) {
				var newFleet = new Fleet(
					fleet.Owner,
					waypoints[0],
					new LinkedList<AMission>()
				);
				newFleet.Ships.Add(fleet.Ships);
				this.game.States.Fleets.PendAdd(newFleet);
				
				fleet.Owner.Intelligence.StarFullyVisited(endStar, game.Turn);
			}
			else {
				var direction = (waypoints[0] - fleet.Position);
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

		#endregion
	}
}
