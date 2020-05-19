using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Controllers.Data;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Ships;
using Stareater.Ships.Missions;
using Stareater.Utils;

namespace Stareater.Controllers
{
	public class FleetController
	{
		private readonly MainGame game;
		
		public FleetInfo Fleet { get; private set; }

		private Dictionary<Design, ShipSelection> selection;
		private readonly List<WaypointInfo> simulationWaypoints = new List<WaypointInfo>();
		private Dictionary<Design, long> carriedSelection = new Dictionary<Design, long>();
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
				x => new ShipSelection(x.Quantity, x, x.PopulationTransport)
			);
			this.calcCarriers();

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
				var playerProc = this.game.Derivates[this.Fleet.Owner.Data];

				return this.selection.Select(x => new ShipGroupInfo(x.Value.Ships, playerProc.DesignStats[x.Key])).ToList();
			}
		}

		public bool CanMove
		{
			get 
			{
				var designStats = this.game.Derivates.Players.Of[this.Fleet.Owner.Data].DesignStats;
				var totalTows = this.carriedSelection.
					Where(x => x.Key.IsDrive != null && designStats[x.Key].TowCapacity > 0).
					Sum(x => x.Value * designStats[x.Key].TowCapacity);
				var towedSize = this.carriedSelection.
					Where(x => x.Key.IsDrive == null).
					Sum(x => x.Value * designStats[x.Key].Size);

				return this.carriedSelection.Any(x => x.Key.IsDrive != null || x.Value <= 0) && towedSize <= totalTows;
			}
		}

		public IList<StarInfo> SimulationWaypoints()
		{
			return this.simulationWaypoints.Select(x => new StarInfo(x.DestionationStar)).ToList();
		}

		public long SelectionCount(ShipGroupInfo group)
		{
			if (group == null)
				throw new ArgumentNullException(nameof(group));

			return this.selection[group.Data.Design].Quantity;
		}

		public double SelectionPopulation(ShipGroupInfo group)
		{
			if (group == null)
				throw new ArgumentNullException(nameof(group));

			return this.selection[group.Data.Design].Population;
		}

		public void DeselectGroup(ShipGroupInfo group)
		{
			if (group == null)
				throw new ArgumentNullException(nameof(group));

			this.selection[group.Data.Design] = new ShipSelection(0, this.selection[group.Data.Design].Ships, 0);
			this.calcCarriers();

			if (!this.CanMove)
				this.simulationWaypoints.Clear();
		}

		public void SelectGroup(ShipGroupInfo group, long quantity)
		{
			if (group == null)
				throw new ArgumentNullException(nameof(group));

			this.SelectGroup(group, quantity, group.Population * quantity / (double)group.Quantity);
		}

		public void SelectGroup(ShipGroupInfo group, long quantity, double population)
		{
			if (group == null)
				throw new ArgumentNullException(nameof(group));

			quantity = Methods.Clamp(quantity, 0, group.Quantity);
			if (quantity <= 0)
			{
				this.DeselectGroup(group);
				return;
			}

			var minPopulation = group.Population - (group.Quantity - quantity) * group.Design.ColonizerPopulation;
			population = Methods.Clamp(population, minPopulation, group.Population);

			this.selection[group.Data.Design] = new ShipSelection(quantity, this.selection[group.Data.Design].Ships, population);
			this.calcCarriers();

			if (!this.CanMove)
				this.simulationWaypoints.Clear();

			this.calcSimulation();
		}

		public FleetController SplitSelection()
		{
			var controller = new FleetController(this.Fleet, this.game);
			controller.selection.Clear();

			foreach (var group in this.selection.Where(x => x.Value.Quantity > 0))
				controller.selection[group.Key] = new ShipSelection(
					group.Value.Quantity, 
					new ShipGroup(
						group.Key, 
						group.Value.Quantity, 
						group.Value.Ships.Damage * selectedPart(group.Key), 
						group.Value.Ships.UpgradePoints * selectedPart(group.Key),
						group.Value.Population
					),
					group.Value.Population
				);
			controller.calcCarriers();

			//TODO(v0.8) remove split ships from current controller instance
			return controller;
		}

		public bool IsCarried(ShipGroupInfo group)
		{
			if (group == null)
				throw new ArgumentNullException(nameof(group));

			var selected = this.selectedCount(group.Design.Data);
			if (selected == 0)
				return false;

			return !this.carriedSelection.TryGetValue(group.Design.Data, out var uncarried) || uncarried < selected;
		}

		public bool IsTowed(ShipGroupInfo group)
		{
			if (group == null)
				throw new ArgumentNullException(nameof(group));

			var selected = this.selectedCount(group.Design.Data);
			if (selected == 0 || group.Design.Data.IsDrive != null)
				return false;

			return this.carriedSelection.ContainsKey(group.Design.Data) && this.CanMove;
		}

		public FleetController Send(StarInfo destination)
		{
			if (destination == null)
				throw new ArgumentNullException(nameof(destination));

			//TODO(later) prevent changing immediate destination midfilght but allow to change final destination
			if (!this.game.States.Stars.At.Contains(this.Fleet.Position) || !this.selection.Any(x => x.Value.Quantity > 0))
				return this;

			if (this.CanMove && destination.Position != this.Fleet.FleetData.Position)
				return this.giveOrder(
					this.game.Derivates[this.player].
					ShortestPathTo(this.game.States.Stars.At[this.Fleet.Position], destination.Data, this.carriedSelection, this.game).
					Select(x => new MoveMission(x.FromNode, x.ToNode, this.game.States.Wormholes.At.GetOrDefault(x.FromNode, x.ToNode))).
					ToList()
				);
			else if (this.game.States.Stars.At.Contains(this.Fleet.FleetData.Position))
				return this.giveOrder(new AMission[0]);
			
			return this;
		}

		public FleetController SendDirectly(StarInfo destination)
		{
			if (destination == null)
				throw new ArgumentNullException(nameof(destination));

			if (!this.game.States.Stars.At.Contains(this.Fleet.Position) || !this.selection.Any(x => x.Value.Quantity > 0))
				return this;

			//TODO(later) prevent changing destination midfilght
			if (this.CanMove && destination.Position != this.Fleet.FleetData.Position)
				return this.giveOrder(new AMission[] { new MoveMission(
					this.game.States.Stars.At[this.Fleet.Position],
					destination.Data, 
					this.game.States.Wormholes.At.GetOrDefault(this.game.States.Stars.At[this.Fleet.Position], destination.Data)
				) });
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
			if (destination == null)
				throw new ArgumentNullException(nameof(destination));

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
				playerProc.ShortestPathTo(this.game.States.Stars.At[this.Fleet.Position], destination.Data, this.carriedSelection, this.game).
				Select(x => new WaypointInfo(
					x.FromNode,
					x.ToNode,
					playerProc.VisibleWormholeAt(x.FromNode, x.ToNode, this.game)
				))
			);

			this.calcSimulation();
		}

		public void SimulateDirectTravel(StarInfo destination)
		{
			if (destination == null)
				throw new ArgumentNullException(nameof(destination));

			if (!this.game.States.Stars.At.Contains(this.Fleet.Position))
				return;

			this.simulationWaypoints.Clear();
			if (!this.selection.Any(x => x.Value.Quantity > 0))
			{
				this.calcSimulation();
				return;
			}

			var playerProc = this.game.Derivates[this.player];
			var start = this.game.States.Stars.At[this.Fleet.Position];
			//TODO(later) prevent changing destination midfilght
			this.simulationWaypoints.Add(new WaypointInfo(
				start,
				destination.Data,
				playerProc.VisibleWormholeAt(start, destination.Data, this.game)
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
				
				return new FleetInfo(similarFleet, playerProc);
			}
			else {
				if (newFleet.Ships.Count > 0)
					shipOrders.Add(newFleet);
				var fleetInfo = new FleetInfo(newFleet, playerProc);

				return fleetInfo;
			}
		}

		private void calcCarriers()
		{
			this.carriedSelection = FleetProcessor.FillCarriers(
				this.game.Statics,
				this.game.Derivates[this.Fleet.Owner.Data],
				this.selection.ToDictionary(x => x.Key, x => x.Value.Quantity)
			);
		}

		private void calcSimulation()
		{
            this.SimulationEta = 0;
            this.SimulationFuel = 0;

            if (!this.selection.Any(x => x.Value.Quantity > 0))
                return;

            var playerProc = this.game.Derivates.Players.Of[this.Fleet.Owner.Data];
			var fleet = this.selectedFleet(simulationWaypoints.Select(x => new MoveMission(x.StartStar, x.DestionationStar, x.UsedWormhole)));
			
			var lastPosition = this.Fleet.FleetData.Position;
			var speedFormula = game.Statics.ShipFormulas.GalaxySpeed;
			var voidSpeed = speedFormula.Evaluate(FleetProcessor.SpeedVars(this.game.Statics, playerProc, this.carriedSelection, false).Get);
			var laneSpeed = speedFormula.Evaluate(FleetProcessor.SpeedVars(this.game.Statics, playerProc, this.carriedSelection, true).Get);

			foreach (var waypoint in simulationWaypoints)
			{
				var speed = waypoint.UsingWormhole ? laneSpeed : voidSpeed;
				
				var distance = (waypoint.Destionation - lastPosition).Length;
				this.SimulationEta += distance / speed;
				this.SimulationFuel = Math.Max(playerProc.FuelUsage(fleet, waypoint.StartStar.Position, waypoint.Destionation, game), this.SimulationFuel);
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
			this.selection = this.Fleet.FleetData.Ships.ToDictionary(
				x => x.Design,
				x => new ShipSelection(x.Quantity, x, x.PopulationTransport)
			);
			this.calcCarriers();

			return new FleetController(
				newFleetInfo, 
				this.game
			);
		}

		private Fleet selectedFleet(IEnumerable<AMission> newMissions)
		{
			var fleet = new Fleet(this.player, this.Fleet.FleetData.Position, new LinkedList<AMission>(newMissions), this.Fleet.FleetData.PreviousTurn);
			fleet.Ships.Add(
				this.Fleet.FleetData.Ships.
				Where(x => selectedCount(x.Design) > 0).
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
			var fleet = new Fleet(this.player, this.Fleet.FleetData.Position, this.Fleet.FleetData.Missions, this.Fleet.FleetData.PreviousTurn);
			fleet.Ships.Add(
				this.Fleet.FleetData.Ships.
				Where(x => x.Quantity - selectedCount(x.Design) > 0).
				Select(x => new ShipGroup(
					x.Design,
					x.Quantity - selectedCount(x.Design),
					x.Damage * (1 - selectedPart(x.Design)), x.UpgradePoints * (1 - selectedPart(x.Design)),
					x.PopulationTransport - (this.selection.ContainsKey(x.Design) ? this.selection[x.Design].Population : 0))
				)
			);

			return fleet;
		}

		private double selectedPart(Design design)
		{
			if (!this.selection.ContainsKey(design))
				return 0;

			return this.selection[design].Quantity / this.selection[design].Ships.Quantity;
		}

		private long selectedCount(Design design)
		{
			if (!this.selection.ContainsKey(design))
				return 0;

			return this.selection[design].Quantity;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			var star = this.game.States.Stars.At.Contains(this.Fleet.Position) ? game.States.Stars.At[this.Fleet.Position] : null;
			if (star != null)
				sb.Append(star.Name.ToText(Localization.LocalizationManifest.Get.CurrentLanguage) + " ");
			else
				sb.Append(this.Fleet.Position + " ");

			var count = this.selection.Values.Sum(x => x.Quantity);
			var pop = this.selection.Values.Sum(x => x.Population);
			sb.Append($"{count} ships, {pop:0} pop ");

			return sb.ToString();
		}
	}
}
