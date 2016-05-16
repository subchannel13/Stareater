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
		private readonly MainGame game;
		
		private double time = 0;
		private LinkedList<AMission> remainingMissions;
		private Vector2D newPosition;
		private Vector2D movementDirection = new Vector2D();
		private List<FleetMovement> movementSteps = new List<FleetMovement>();
		
		public FleetProcessingVisitor(Fleet fleet, MainGame game)
		{
			this.fleet = fleet;
			this.game = game;
			this.newPosition = fleet.Position;
			this.remainingMissions = new LinkedList<AMission>(fleet.Missions);
		}

		public IEnumerable<FleetMovement> Run()
		{
			while(this.remainingMissions.Count > 0 && this.time < 1)
			{
				var mission = this.remainingMissions.First.Value;
				this.remainingMissions.RemoveFirst();
				mission.Accept(this);
			}
			
			if (time < 1)
				this.movementSteps.Add(new FleetMovement(
					this.fleet,
					localFleet(),
					this.time,
					1,
					movementDirection
				));
			
			return this.movementSteps;
		}
		
		private Fleet localFleet()
		{
			var resultFleet = new Fleet(fleet.Owner, this.newPosition, new LinkedList<AMission>(this.remainingMissions));

			foreach(var group in this.fleet.Ships)
				resultFleet.Ships.Add(new ShipGroup(group.Design, group.Quantity));
			
			return resultFleet;
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
			
			this.movementDirection = mission.Destination.Position - fleet.Position;
			var distance = this.movementDirection.Magnitude();

			if (distance <= speed * (1 - time)) {
				this.newPosition = mission.Destination.Position;
				
				fleet.Owner.Intelligence.StarFullyVisited(game.States.Stars.At(mission.Destination.Position), game.Turn);
				this.time += distance / speed;
				
				this.movementSteps.Add(new FleetMovement(
					this.fleet,
					localFleet(),
					this.time,
					this.time,
					this.movementDirection
				));
			}
			else {
				var direction = (mission.Destination.Position - fleet.Position);
				direction.Normalize();

				this.newPosition = fleet.Position + direction * speed;
				remainingMissions.AddLast(mission);
				
				this.movementSteps.Add(new FleetMovement(
					this.fleet,
					localFleet(),
					this.time,
					1,
					this.movementDirection
				));
				this.time = 1;
			}
		}

		void IMissionVisitor.Visit(ColonizationMission mission)
		{
			if (this.newPosition == mission.Target.Star.Position)
				remainingMissions.AddLast(mission);
			
			this.movementSteps.Add(new FleetMovement(
					this.fleet,
					localFleet(),
					this.time,
					1,
					new Vector2D()
			));
			this.time = 1;
		}

		void IMissionVisitor.Visit(SkipTurnMission mission)
		{
			this.movementSteps.Add(new FleetMovement(
					this.fleet,
					localFleet(),
					this.time,
					1,
					new Vector2D()
			));
			this.time = 1;
		}
		
		#endregion
	}
}
