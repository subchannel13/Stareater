using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.Ships.Missions;

namespace Stareater.GameLogic
{
	class FleetProcessingVisitor : IMissionVisitor
	{
		private readonly Fleet fleet;
		private readonly Game game;
		
		private double actionPoints = 1;
		private LinkedList<AMission> remainingMissions = new LinkedList<AMission>();
		private Vector2D newPosition;
		private bool removeFleet = false;
		
		public FleetProcessingVisitor(Fleet fleet, Game game)
		{
			this.fleet = fleet;
			this.game = game;
			this.newPosition = fleet.Position;
		}

		public void Run()
		{
			foreach(var mission in fleet.Missions)
				if (actionPoints > 0)
					mission.Accept(this);
				else
					remainingMissions.AddLast(mission);
			
			this.game.States.Fleets.PendRemove(fleet);
			
			if (!removeFleet)
			{
				var newFleet = new Fleet(fleet.Owner, this.newPosition, this.remainingMissions);
				newFleet.Ships.Add(fleet.Ships);
				this.game.States.Fleets.PendAdd(newFleet);
			}
		}
		
		#region IMissionVisitor implementation

		void IMissionVisitor.Visit(MoveMission mission)
		{
			var playerProc = game.Derivates.Players.Of(fleet.Owner);
			double baseSpeed = fleet.Ships.Select(x => x.Design).
				Aggregate(double.MaxValue, (s, x) => Math.Min(playerProc.DesignStats[x].GalaxySpeed, s));
				
			var speed = baseSpeed;
			if (mission.UsedWormhole != null)
				speed += 0.5; //TODO(later) consider making moddable
			
			var distance = (mission.Destination.Position - fleet.Position).Magnitude();

			//TODO(v0.5) detect conflicts
			//TODO(v0.5) merge with existing fleet
			if (distance <= speed * actionPoints) {
				this.newPosition = mission.Destination.Position;
				
				fleet.Owner.Intelligence.StarFullyVisited(game.States.Stars.At(mission.Destination.Position), game.Turn);
				this.actionPoints -= distance / speed;
			}
			else {
				var direction = (mission.Destination.Position - fleet.Position);
				direction.Normalize();

				this.newPosition = fleet.Position + direction * speed;
				remainingMissions.AddLast(mission);
				
				this.actionPoints = 0;
			}
		}

		void IMissionVisitor.Visit(ColonizationMission mission)
		{
			var project = game.States.ColonizationProjects.Of(mission.Target);
			
			foreach(var group in fleet.Ships)
				project.Arrived.Add(group);
			
			//TODO(v0.5) remove fleet at GameProcessor.doColonization
			this.removeFleet = true;
		}

		void IMissionVisitor.Visit(SkipTurnMission mission)
		{
			this.actionPoints = 0;
		}
		
		#endregion
	}
}
