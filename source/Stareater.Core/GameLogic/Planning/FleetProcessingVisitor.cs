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
		private readonly double voidSpeed;
		private readonly double laneSpeed;

		public FleetProcessingVisitor(Fleet fleet, MainGame game)
		{
			this.fleet = fleet;
			this.game = game;
			this.newPosition = fleet.Position;
			this.missions = new LinkedList<AMission>(fleet.Missions);

			this.voidSpeed = game.Statics.ShipFormulas.GalaxySpeed.Evaluate(
				FleetProcessor.SpeedVars(
					this.game.Statics,
					game.Derivates[fleet.Owner],
					fleet.Ships.ToDictionary(x => x.Design, x => x.Quantity),
					false
				).Get
			);
			this.laneSpeed = game.Statics.ShipFormulas.GalaxySpeed.Evaluate(
				FleetProcessor.SpeedVars(
					this.game.Statics,
					game.Derivates[fleet.Owner],
					fleet.Ships.ToDictionary(x => x.Design, x => x.Quantity),
					true
				).Get
			);
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
				new LinkedList<AMission>(this.unfinishedMissions.Concat(this.missions)),
				fleet
			);

			foreach(var group in this.fleet.Ships)
				resultFleet.Ships.Add(new ShipGroup(group.Design, group.Quantity, group.Damage, group.UpgradePoints, group.PopulationTransport));
			
			return resultFleet;
		}
		
		#region IMissionVisitor implementation

		void IMissionVisitor.Visit(MoveMission mission)
		{
			var speed = mission.UsedWormhole != null ? this.laneSpeed : this.voidSpeed;

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

		void IMissionVisitor.Visit(DisembarkMission mission)
		{
			if (!this.game.States.Stars.At.Contains(this.fleet.Position))
			{
				this.unfinishedMissions.AddLast(mission);
				this.stay();
				return;
			}

			var newFleet = this.localFleet();
			var destinations = this.game.States.Colonies.
				AtStar[this.game.States.Stars.At[this.fleet.Position], this.fleet.Owner].
				Select(x => this.game.Derivates[x]).
				ToList();

			foreach (var group in newFleet.Ships.Where(x => x.PopulationTransport > 0))
				group.PopulationTransport = Methods.DistributePoints(
					group.PopulationTransport, 
					destinations.Select(site => new PointReceiver(
						site.Desirability,
						site.MaxPopulation - site.Colony.Population,
						x => site.Colony.Population += x
					))
				);

			this.movementSteps.Add(new FleetMovement(
				this.fleet,
				newFleet,
				this.time,
				1,
				new Vector2D()
			));

			this.time = 1;
		}

		//TODO(later) could pick up population in advance if visiting multiple systems and gets intercepted before loading
		//TODO(later) wouldn't move until fully loaded
		void IMissionVisitor.Visit(LoadMission mission)
		{
			var stats = this.game.Derivates[this.fleet.Owner].DesignStats;
			var capacity = this.fleet.Ships.Sum(x => stats[x.Design].ColonizerPopulation * x.Quantity);

			if (!this.game.States.Stars.At.Contains(this.fleet.Position))
			{
				this.stay();
				return;
			}

			var star = this.game.States.Stars.At[this.fleet.Position];
			var stellaris = this.game.Derivates[this.game.States.Stellarises.At[star, this.fleet.Owner]];

			var newFleet = this.localFleet();
			var availableMigrants = stellaris.IsMigrants;
			foreach (var group in newFleet.Ships)
			{
				var embarked = Math.Min(stats[group.Design].ColonizerPopulation * group.Quantity - group.PopulationTransport, availableMigrants);

				group.PopulationTransport += embarked;
				availableMigrants -= embarked;
			}

			var totalEmbarked = stellaris.IsMigrants - availableMigrants;
			Methods.DistributePoints(totalEmbarked,
				this.game.Derivates.Colonies.At[stellaris.Location, stellaris.Owner].
				Select(colonyProc => new PointReceiver(
					1 - colonyProc.Desirability,
					colonyProc.Colony.Population,
					x => colonyProc.Colony.Population -= x
				)));
			stellaris.IsMigrants = availableMigrants;

			var endTime = this.time;
			if (newFleet.Ships.Sum(x => x.PopulationTransport) < capacity)
			{
				newFleet.Missions.AddFirst(new LoadMission());
				endTime = 1;
			}

			this.movementSteps.Add(new FleetMovement(
					this.fleet,
					newFleet,
					this.time,
					endTime,
					new Vector2D()
			));
			this.time = endTime;
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
