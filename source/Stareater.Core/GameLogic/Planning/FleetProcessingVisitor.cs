using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Ships.Missions;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic.Planning
{
	class FleetProcessingVisitor : IMissionVisitor
	{
		private readonly Fleet fleet;
		private readonly MainGame game;
		
		private double time = 0;
		private readonly LinkedList<AMission> unfinishedMissions = new LinkedList<AMission>();
		private readonly LinkedList<AMission> missions;
		private Vector2D newPosition;
		private Vector2D movementDirection = new Vector2D();
		private readonly List<FleetMovement> movementSteps = new List<FleetMovement>();
		
		public FleetProcessingVisitor(Fleet fleet, MainGame game)
		{
			this.fleet = fleet;
			this.game = game;
			this.newPosition = fleet.Position;
			this.missions = new LinkedList<AMission>(fleet.Missions);
		}

		public IEnumerable<FleetMovement> Run()
		{
			while(this.missions.Count > 0 && this.time < 1)
			{
				var mission = this.missions.First.Value;
				this.missions.RemoveFirst();
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
			var resultFleet = new Fleet(
				fleet.Owner, 
				this.newPosition, 
				new LinkedList<AMission>(this.unfinishedMissions.Concat(this.missions))
			);

			foreach(var group in this.fleet.Ships)
				resultFleet.Ships.Add(new ShipGroup(group.Design, group.Quantity, group.Damage, group.UpgradePoints, group.PopulationTransport));
			
			return resultFleet;
		}
		
		#region IMissionVisitor implementation

		void IMissionVisitor.Visit(MoveMission mission)
		{
			var playerProc = game.Derivates.Players.Of[fleet.Owner];
			double baseSpeed = fleet.Ships.Select(x => x.Design).
				Aggregate(double.MaxValue, (s, x) => Math.Min(playerProc.DesignStats[x].GalaxySpeed, s));
				
			var speed = mission.UsedWormhole != null ? 
				game.Statics.ShipFormulas.WormholeSpeed.Evaluate(new Var("speed", baseSpeed).Get) : 
				baseSpeed;
			
			this.movementDirection = mission.Destination.Position - fleet.Position;
			var distance = this.movementDirection.Length;

			if (distance <= speed * (1 - time)) {
				this.newPosition = mission.Destination.Position;
				
				fleet.Owner.Intelligence.StarFullyVisited(game.States.Stars.At[mission.Destination.Position], game.Turn);
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
				var direction = (mission.Destination.Position - fleet.Position).Unit;

				this.newPosition = fleet.Position + direction * speed;
				unfinishedMissions.AddLast(mission);
				
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
				unfinishedMissions.AddLast(mission);
			
			this.movementSteps.Add(new FleetMovement(
					this.fleet,
					localFleet(),
					this.time,
					1,
					new Vector2D()
			));
			this.time = 1;
		}

		void IMissionVisitor.Visit(LoadMission mission)
		{
			var stats = this.game.Derivates.Of(this.fleet.Owner).DesignStats;
			var capacity = this.fleet.Ships.Sum(x => stats[x.Design].ColonizerPopulation) - this.fleet.Ships.Sum(x => x.PopulationTransport);

			if (!this.game.States.Stars.At.Contains(this.fleet.Position))
			{
				this.stay();
				return;
			}

			var colonies = this.game.States.Colonies.AtStar[this.game.States.Stars.At[this.fleet.Position]].
				Where(x => x.Owner==this.fleet.Owner).
				ToList();

			//TODO(0.8) pick up population
			this.stay();
		}

		void IMissionVisitor.Visit(SkipTurnMission mission)
		{
			this.stay();
		}
		
		#endregion

		private void stay()
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
	}
}
