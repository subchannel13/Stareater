using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Ships;
using Stareater.Ships.Missions;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.Controllers
{
	public class FleetController
	{
		private readonly MainGame game;
		private readonly Player player;
		
		public FleetInfo Fleet { get; private set; }

		private readonly Dictionary<Design, long> selection = new Dictionary<Design, long>();
		private readonly Dictionary<Design, double> selectionPopulation = new Dictionary<Design, double>(); //TODO(v0.8) Make new type and unify with selection quantity
		private readonly List<WaypointInfo> simulationWaypoints = new List<WaypointInfo>();
		public double SimulationEta { get; private set; }
		public double SimulationFuel { get; private set; }

		internal FleetController(FleetInfo fleet, MainGame game, Player player)
		{
			this.Fleet = fleet;
			this.game = game;
			this.player = player;
			this.SimulationEta = 0;
			this.SimulationFuel = 0;
			
			if (this.Fleet.IsMoving) {
				this.simulationWaypoints = new List<WaypointInfo>(this.Fleet.Missions.Waypoints);
				this.calcSimulation();
			}
		}
		
		public bool Valid
		{
			get { return this.game.States.Fleets.Contains(this.Fleet.FleetData); }
		}
		
		public IEnumerable<ShipGroupInfo> ShipGroups
		{
			get
			{
				return this.Fleet.FleetData.Ships.Select(x => new ShipGroupInfo(x, this.game.Derivates[this.Fleet.Owner.Data].DesignStats[x.Design], this.game.Statics));
			}
		}
		
		public bool CanMove
		{
			get 
			{ 
				foreach(var design in this.selection.Keys)
					if (design.IsDrive == null)
						return false;
				
				return true;
			}
		}

		public IList<Vector2D> SimulationWaypoints()
		{
			return this.simulationWaypoints.Select(x => x.Destionation).ToList();
		}
		
		public void DeselectGroup(ShipGroupInfo group)
		{
			selection.Remove(group.Data.Design);
			selectionPopulation.Remove(group.Data.Design);

			if (!this.CanMove)
				this.simulationWaypoints.Clear();
		}
		
		public void SelectGroup(ShipGroupInfo group, long quantity)
		{
			this.SelectGroup(group, quantity, group.Population * quantity / (double)group.Quantity);
		}

		public void SelectGroup(ShipGroupInfo group, long quantity, double population)
		{
			quantity = Methods.Clamp(quantity, 0, this.Fleet.FleetData.Ships.WithDesign[group.Data.Design].Quantity);
			selection[group.Data.Design] = quantity;
			selectionPopulation[group.Data.Design] = population;

			if (selection[group.Data.Design] <= 0)
				this.DeselectGroup(group);

			if (!this.CanMove)
				this.simulationWaypoints.Clear();

			this.calcSimulation();
		}

		public FleetController Send(IEnumerable<Vector2D> waypoints)
		{
			if (!this.game.States.Stars.At.Contains(this.Fleet.Position))
				return this;
			
			if (this.CanMove && waypoints != null && waypoints.LastOrDefault() != this.Fleet.FleetData.Position)
			{
				var missions = new List<AMission>();
				var lastPoint = this.Fleet.FleetData.Position;
				foreach(var point in waypoints)
				{
					var lastStar = this.game.States.Stars.At[lastPoint];
					var nextStar = this.game.States.Stars.At[point];
					var wormhole = this.game.States.Wormholes.At[lastStar].FirstOrDefault(x => x.Endpoints.Any(nextStar));
					missions.Add(new MoveMission(nextStar, wormhole));
				}
				
				return this.giveOrder(missions);
			}
			else if (this.game.States.Stars.At.Contains(this.Fleet.FleetData.Position))
				return this.giveOrder(new AMission[0]);
			
			return this;
		}

		public FleetController Disembark()
		{
			return this.giveOrder(this.Fleet.FleetData.Missions.Concat(new[] { new DisembarkMission() }));
		}

		public FleetController LoadPopulation()
		{
			return this.giveOrder(this.Fleet.FleetData.Missions.Concat(new[] { new LoadMission() }));
		}

		public void SimulateTravel(StarInfo destination)
		{
			if (!this.game.States.Stars.At.Contains(this.Fleet.Position))
				return;
			
			this.simulationWaypoints.Clear();
			//TODO(later): find shortest path
			//TODO(later) prevent changing destination midfilght
			this.simulationWaypoints.Add(new WaypointInfo(
				destination.Data.Position,
				this.game.States.Wormholes.At[this.game.States.Stars.At[this.Fleet.Position], destination.Data].Any()
			));
			
			this.calcSimulation();
		}

		
		private FleetInfo addFleet(ICollection<Fleet> shipOrders, Fleet newFleet)
		{
			var similarFleet = shipOrders.FirstOrDefault(x => x.Missions.SequenceEqual(newFleet.Missions));
			var playerProc = this.game.Derivates[this.player];
			
			if (similarFleet != null) {
				foreach(var shipGroup in newFleet.Ships)
					if (similarFleet.Ships.WithDesign.Contains(shipGroup.Design))
						similarFleet.Ships.WithDesign[shipGroup.Design].Quantity += shipGroup.Quantity;
					else
						similarFleet.Ships.Add(shipGroup);
				
				return new FleetInfo(similarFleet, playerProc, this.game.Statics);
			}
			else {
				if (newFleet.Ships.Count > 0)
					shipOrders.Add(newFleet);
				var fleetInfo = new FleetInfo(newFleet, playerProc, this.game.Statics);

				return fleetInfo;
			}
		}
		
		private void calcSimulation()
		{
			var player = this.Fleet.Owner.Data;
			var designStats = this.game.Derivates.Players.Of[player].DesignStats;
			double baseSpeed = this.selection.Keys.
				Aggregate(double.MaxValue, (s, x) => Math.Min(designStats[x].GalaxySpeed, s));
			
			var lastPosition = this.Fleet.FleetData.Position;
			var wormholeSpeed = this.game.Statics.ShipFormulas.WormholeSpeed;
			var fleetSize = this.selection.Sum(x => designStats[x.Key].Size * x.Value);
			this.SimulationEta = 0;
			this.SimulationFuel = 0;

			foreach (var waypoint in simulationWaypoints)
			{
				var speed = waypoint.UsingWormhole ? 
					wormholeSpeed.Evaluate(new Var("speed", baseSpeed).Get) : 
					baseSpeed;
				
				var distance = (waypoint.Destionation - lastPosition).Length;
				this.SimulationEta += distance / speed;

				if (this.game.States.Stellarises.OwnedBy[player].Any())
				{
					var supplyDistance = this.game.States.Stellarises.
						OwnedBy[player].
						Min(x => (waypoint.Destionation - x.Location.Star.Position).Length);
					this.SimulationFuel = Math.Max(
						fleetSize * this.game.Statics.ShipFormulas.FuelUsage.Evaluate(new Var("dist", supplyDistance).Get), 
						this.SimulationFuel);
				}
				lastPosition = waypoint.Destionation;
			}
		}
		
		private FleetController giveOrder(IEnumerable<AMission> newMissions)
		{
			if (this.selection.Count == 0 || this.Fleet.FleetData.Missions.SequenceEqual(newMissions))
				return this;
			
			//create regroup order if there is none
			HashSet<Fleet> shipOrders;
			if (!this.game.Orders[this.Fleet.FleetData.Owner].ShipOrders.ContainsKey(this.Fleet.FleetData.Position)) {
				shipOrders = new HashSet<Fleet>();
				this.game.Orders[this.Fleet.FleetData.Owner].ShipOrders.Add(this.Fleet.FleetData.Position, shipOrders);
			}
			else
				shipOrders = this.game.Orders[this.Fleet.FleetData.Owner].ShipOrders[this.Fleet.FleetData.Position];
			
			//remove current fleet from regroup
			shipOrders.Remove(this.Fleet.FleetData);
			
			//add new fleet
			var newFleet = new Fleet(this.Fleet.FleetData.Owner, this.Fleet.FleetData.Position, new LinkedList<AMission>(newMissions));
			foreach (var group in this.Fleet.FleetData.Ships.Where(x => this.selection.ContainsKey(x.Design)))
				newFleet.Ships.Add(new ShipGroup(group.Design, this.selection[group.Design], 0, 0, this.selectionPopulation[group.Design]));

			var newFleetInfo = this.addFleet(shipOrders, newFleet);
			
			//add old fleet remains
			var oldFleet = new Fleet(this.Fleet.FleetData.Owner, this.Fleet.FleetData.Position, this.Fleet.FleetData.Missions);
			foreach(var group in this.Fleet.FleetData.Ships) 
				if (this.selection.ContainsKey(group.Design) && group.Quantity - this.selection[group.Design] > 0)
					oldFleet.Ships.Add(new ShipGroup(
						group.Design, 
						group.Quantity - this.selection[group.Design], 
						0, 0, 
						group.PopulationTransport - this.selectionPopulation[group.Design]));
			this.Fleet = this.addFleet(shipOrders, oldFleet);

			return new FleetController(
				newFleetInfo, 
				this.game,
				this.player
			);
		}
	}
}
