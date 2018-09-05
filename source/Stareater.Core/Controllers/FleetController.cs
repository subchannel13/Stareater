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
		private readonly List<WaypointInfo> simulationWaypoints = new List<WaypointInfo>();
		private double eta = 0;

		internal FleetController(FleetInfo fleet, MainGame game, Player player)
		{
			this.Fleet = fleet;
			this.game = game;
			this.player = player;
			
			if (this.Fleet.IsMoving) {
				this.simulationWaypoints = new List<WaypointInfo>(this.Fleet.Missions.Waypoints);
				this.calcEta();
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
				return this.Fleet.FleetData.Ships.Select(x => new ShipGroupInfo(x, this.game.Derivates.Of(this.Fleet.Owner.Data).DesignStats[x.Design], this.game.Statics));
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
		
		public double Eta
		{
			get { return this.eta; }
		}
		
		public IList<Vector2D> SimulationWaypoints()
		{
			return this.simulationWaypoints.Select(x => x.Destionation).ToList();
		}
		
		public void DeselectGroup(ShipGroupInfo group)
		{
			selection.Remove(group.Data.Design);
			
			if (!this.CanMove)
				this.simulationWaypoints.Clear();
		}
		
		public void SelectGroup(ShipGroupInfo group, long quantity)
		{
			quantity = Methods.Clamp(quantity, 0, this.Fleet.FleetData.Ships.WithDesign[group.Data.Design].Quantity);
			selection[group.Data.Design] = quantity;
			
			if (selection[group.Data.Design] <= 0)
				selection.Remove(group.Data.Design);
			
			if (!this.CanMove)
				this.simulationWaypoints.Clear();
			
			this.calcEta();
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
			
			this.calcEta();
		}

		
		private FleetInfo addFleet(ICollection<Fleet> shipOrders, Fleet newFleet)
		{
			var similarFleet = shipOrders.FirstOrDefault(x => x.Missions.SequenceEqual(newFleet.Missions));
			var playerProc = this.game.Derivates.Of(this.player);
			
			if (similarFleet != null) {
				foreach(var shipGroup in newFleet.Ships)
					if (similarFleet.Ships.WithDesign.Contains(shipGroup.Design))
						similarFleet.Ships.WithDesign[shipGroup.Design].Quantity += shipGroup.Quantity;
					else
						similarFleet.Ships.Add(shipGroup);
				
				return new FleetInfo(similarFleet, playerProc, this.game.Statics);
			}
			else {
				shipOrders.Add(newFleet);
				var fleetInfo = new FleetInfo(newFleet, playerProc, this.game.Statics);

				return fleetInfo;
			}
		}
		
		private void calcEta()
		{
			var playerProc = game.Derivates.Players.Of[this.Fleet.Owner.Data];
			double baseSpeed = this.selection.Keys.
				Aggregate(double.MaxValue, (s, x) => Math.Min(playerProc.DesignStats[x].GalaxySpeed, s));
			
			var lastPosition = this.Fleet.FleetData.Position;
			var wormholeSpeed = game.Statics.ShipFormulas.WormholeSpeed;
			this.eta = 0;
			
			foreach(var waypoint in simulationWaypoints)
			{
				var speed = waypoint.UsingWormhole ? 
					wormholeSpeed.Evaluate(new Var("speed", baseSpeed).Get) : 
					baseSpeed;
				
				var distance = (waypoint.Destionation - lastPosition).Length;
				eta += distance / speed;
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
			var movedPopulation = new Dictionary<Design, double>();
			foreach (var group in this.Fleet.FleetData.Ships.Where(x => this.selection.ContainsKey(x.Design)))
			{
				var population = Math.Ceiling(group.PopulationTransport * this.selection[group.Design] / group.Quantity);
				movedPopulation[group.Design] = population;
				newFleet.Ships.Add(new ShipGroup(group.Design, this.selection[group.Design], 0, 0, population));
			}

			var newFleetInfo = this.addFleet(shipOrders, newFleet);
			
			//add old fleet remains
			var oldFleet = new Fleet(this.Fleet.FleetData.Owner, this.Fleet.FleetData.Position, this.Fleet.FleetData.Missions);
			foreach(var group in this.Fleet.FleetData.Ships) 
				if (this.selection.ContainsKey(group.Design) && group.Quantity - this.selection[group.Design] > 0)
					oldFleet.Ships.Add(new ShipGroup(
						group.Design, 
						group.Quantity - this.selection[group.Design], 
						0, 0, 
						group.PopulationTransport - movedPopulation[group.Design]));
			if (oldFleet.Ships.Count > 0)
				this.addFleet(shipOrders, oldFleet);

			return new FleetController(
				newFleetInfo, 
				this.game,
				this.player
			);
		}
	}
}
