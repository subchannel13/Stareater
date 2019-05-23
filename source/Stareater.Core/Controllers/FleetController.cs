using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Data;
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
		
		public FleetInfo Fleet { get; private set; }

		private readonly Dictionary<Design, ShipSelection> selection;
		private readonly List<WaypointInfo> simulationWaypoints = new List<WaypointInfo>();
		public double SimulationEta { get; private set; }
		public double SimulationFuel { get; private set; }

		internal FleetController(FleetInfo fleet, MainGame game)
		{
			this.Fleet = fleet;
			this.game = game;
			this.SimulationEta = 0;
			this.SimulationFuel = 0;
			this.selection = fleet.FleetData.Ships.ToDictionary(
				x => x.Design, 
				x => new ShipSelection(x.Quantity, x.Quantity, x.PopulationTransport)
			);
			
			if (this.Fleet.IsMoving) {
				this.simulationWaypoints = new List<WaypointInfo>(this.Fleet.Missions.Waypoints);
				this.calcSimulation();
			}
		}

		private Player player
		{
			get { return this.Fleet.Owner.Data; }
		}

		public bool Valid
		{
			get { return this.game.States.Fleets.Contains(this.Fleet.FleetData); }
		}
		
		public IEnumerable<ShipGroupInfo> ShipGroups
		{
			get
			{
				return this.Fleet.FleetData.Ships.
					Where(x => this.selection[x.Design].MaxQuantity > 0).
					Select(x => new ShipGroupInfo(x, this.game.Derivates[this.Fleet.Owner.Data].DesignStats[x.Design], this.game.Statics));
			}
		}
		
		public bool CanMove
		{
			get 
			{
				return this.selection.All(x => x.Key.IsDrive != null || x.Value.Quantity <= 0);
			}
		}

		public IList<StarInfo> SimulationWaypoints()
		{
			return this.simulationWaypoints.Select(x => new StarInfo(x.DestionationStar)).ToList();
		}
		
		public void DeselectGroup(ShipGroupInfo group)
		{
			this.selection[group.Data.Design] = new ShipSelection(0, this.selection[group.Data.Design].MaxQuantity, 0);

			if (!this.CanMove)
				this.simulationWaypoints.Clear();
		}
		
		public void SelectGroup(ShipGroupInfo group, long quantity)
		{
			this.SelectGroup(group, quantity, group.Population * quantity / (double)group.Quantity);
		}

		public void SelectGroup(ShipGroupInfo group, long quantity, double population)
		{
			quantity = Methods.Clamp(quantity, 0, group.Quantity);
			if (quantity <= 0)
			{
				this.DeselectGroup(group);
				return;
			}

			var minPopulation = group.Population - (group.Quantity - quantity) * group.Design.ColonizerPopulation;
			population = Methods.Clamp(population, minPopulation, group.Population);

			this.selection[group.Data.Design] = new ShipSelection(quantity, this.selection[group.Data.Design].MaxQuantity, population);

			if (!this.CanMove)
				this.simulationWaypoints.Clear();

			this.calcSimulation();
		}

		public FleetController SplitSelection()
		{
			var controller = new FleetController(this.Fleet, this.game);
			foreach (var group in this.selection.Where(x => x.Value.Quantity > 0))
				controller.selection[group.Key] = new ShipSelection(group.Value.Quantity, group.Value.Quantity, group.Value.Population);

			return controller;
		}

		public FleetController Send(StarInfo destination)
		{
			if (!this.game.States.Stars.At.Contains(this.Fleet.Position) || !this.selection.Any(x => x.Value.Quantity > 0))
				return this;

			//TODO(later) prevent changing destination midfilght
			if (this.CanMove && destination.Position != this.Fleet.FleetData.Position)
				return this.giveOrder(
					this.game.Derivates[this.player].
					ShortestPathTo(this.game.States.Stars.At[this.Fleet.Position], destination.Data, this.baseTravelSpeed(), this.game).
					Select(x => new MoveMission(x.ToNode, this.game.States.Wormholes.At.GetOrDefault(x.FromNode, x.ToNode))).
					ToList()
				);
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
			if (!this.selection.Any(x => x.Value.Quantity > 0))
			{
				this.calcSimulation();
				return;
			}

			var playerProc = this.game.Derivates[this.player];
			//TODO(later) prevent changing destination midfilght
			this.simulationWaypoints.AddRange(
				playerProc.ShortestPathTo(this.game.States.Stars.At[this.Fleet.Position], destination.Data, this.baseTravelSpeed(), this.game).
				Select(x => new WaypointInfo(
					x.ToNode,
					playerProc.VisibleWormholeAt(x.FromNode, x.ToNode, this.game)
				))
			);

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

		private double baseTravelSpeed()
		{
			var playerProc = this.game.Derivates.Players.Of[this.Fleet.Owner.Data];

			return this.selection.Where(x => x.Value.Quantity > 0).Min(x => playerProc.DesignStats[x.Key].GalaxySpeed);
		}

		private void calcSimulation()
		{
            this.SimulationEta = 0;
            this.SimulationFuel = 0;

            if (!this.selection.Any(x => x.Value.Quantity > 0))
                return;

            var playerProc = this.game.Derivates.Players.Of[this.Fleet.Owner.Data];
			var fleet = this.selectedFleet(simulationWaypoints.Select(x => new MoveMission(x.DestionationStar, x.UsedWormhole)));
			var baseSpeed = this.baseTravelSpeed();
			
			var lastPosition = this.Fleet.FleetData.Position;
			var wormholeSpeed = this.game.Statics.ShipFormulas.WormholeSpeed;

			foreach (var waypoint in simulationWaypoints)
			{
				var speed = waypoint.UsingWormhole ? 
					wormholeSpeed.Evaluate(new Var("speed", baseSpeed).Get) : 
					baseSpeed;
				
				var distance = (waypoint.Destionation - lastPosition).Length;
				this.SimulationEta += distance / speed;
				this.SimulationFuel = Math.Max(playerProc.FuelUsage(fleet, waypoint.Destionation, game), this.SimulationFuel);
				lastPosition = waypoint.Destionation;
			}
		}
		
		private FleetController giveOrder(IEnumerable<AMission> newMissions)
		{
			var fleet = this.Fleet.FleetData;

			if (!this.selection.Any(x => x.Value.Quantity > 0) || fleet.Missions.SequenceEqual(newMissions))
				return this;

			//create regroup order if there is none
			if (!this.game.Orders[fleet.Owner].ShipOrders.ContainsKey(fleet.Position))
				this.game.Orders[fleet.Owner].ShipOrders.Add(fleet.Position, new HashSet<Fleet>(this.game.States.Fleets.At[fleet.Position, fleet.Owner]));

			var shipOrders = this.game.Orders[fleet.Owner].ShipOrders[fleet.Position];

			//remove current fleet from regroup
			shipOrders.Remove(fleet);
			
			//TODO(v0.8) ensure no ship duplication, check orders for similar starting fleet and compare number of ships
			var newFleetInfo = this.addFleet(shipOrders, this.selectedFleet(newMissions));
			this.Fleet = this.addFleet(shipOrders, this.unselectedFleet());

			return new FleetController(
				newFleetInfo, 
				this.game
			);
		}

		private Fleet selectedFleet(IEnumerable<AMission> newMissions)
		{
			var fleet = new Fleet(this.player, this.Fleet.FleetData.Position, new LinkedList<AMission>(newMissions));
			fleet.Ships.Add(
				this.Fleet.FleetData.Ships.
				Where(x => this.selection[x.Design].Quantity > 0).
				Select(x => new ShipGroup(
					x.Design, 
					this.selection[x.Design].Quantity, 
					x.Damage * selectedPart(x.Design), x.UpgradePoints * selectedPart(x.Design), 
					this.selection[x.Design].Population)
				)
			);

			return fleet;
		}

		private Fleet unselectedFleet()
		{
			var fleet = new Fleet(this.player, this.Fleet.FleetData.Position, this.Fleet.FleetData.Missions);
			fleet.Ships.Add(
				this.Fleet.FleetData.Ships.
				Where(x => x.Quantity - this.selection[x.Design].Quantity > 0).
				Select(x => new ShipGroup(
					x.Design,
					x.Quantity - this.selection[x.Design].Quantity,
					x.Damage * (1 - selectedPart(x.Design)), x.UpgradePoints * (1 - selectedPart(x.Design)),
					x.PopulationTransport - this.selection[x.Design].Population)
				)
			);

			return fleet;
		}

		private double selectedPart(Design design)
		{
			return this.selection[design].Quantity / this.selection[design].MaxQuantity;
		}
	}
}
