using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGenerics.DataStructures.Mathematical;
using Stareater.Ships.Missions;
using Stareater.Galaxy;

namespace Stareater.GameLogic
{
	class GameProcessor
	{
		private readonly Game game;
		private readonly List<FleetMovement> fleetMovement = new List<FleetMovement>();
		private readonly List<Conflict> conflicts = new List<Conflict>();

		public GameProcessor(Game game)
		{
			this.game = game;
		}

		public void ProcessPrecombat()
		{
			this.CalculateBaseEffects();
			this.CalculateSpendings();
			this.CalculateDerivedEffects();
			this.commitFleetOrders();

			this.game.States.Reports.Clear();
			foreach (var playerProc in this.game.Derivates.Players)
				playerProc.ProcessPrecombat(
					this.game.Statics,
					this.game.States,
					this.game.Derivates
				);

			this.moveShips();
			this.detectConflicts();
			this.doColonization();
			
			/*
			 * TODO(v0.5): Process ships
			 * - Space combat
			 * - Ground combat
			 * - Bombardment
			 * - Colonise planets
			 */
		}

		public void ProcessPostcombat()
		{
			this.mergeStationaryFleets();
			
			foreach (var playerProc in this.game.Derivates.Players)
				playerProc.ProcessPostcombat(this.game.Statics, this.game.States, this.game.Derivates);

			// TODO(v0.5): Update ship designs

			// TODO(v0.5): Upgrade and repair ships

			/*
			 * TODO(v0.5): Colonies, 2nd pass
			 * - Apply normal effect buildings
			 * - Check construction queue
			 * - Recalculate colony effects
			 */

			CalculateBaseEffects();
			CalculateSpendings();
			CalculateDerivedEffects();
		}

		public void CalculateBaseEffects()
		{
			foreach (var stellaris in this.game.Derivates.Stellarises)
				stellaris.CalculateBaseEffects();
			foreach (var colonyProc in this.game.Derivates.Colonies)
				colonyProc.CalculateBaseEffects(this.game.Statics, this.game.Derivates.Of(colonyProc.Owner));
		}

		public void CalculateSpendings()
		{
			foreach (var colonyProc in this.game.Derivates.Colonies)
				colonyProc.CalculateSpending(
					this.game.Statics,
					this.game.Derivates.Of(colonyProc.Owner)
				);

			foreach (var stellaris in this.game.Derivates.Stellarises)
				stellaris.CalculateSpending(
					this.game.Derivates.Of(stellaris.Owner),
					this.game.Derivates.Colonies.At(stellaris.Location)
				);

			foreach (var player in this.game.Derivates.Players) {
				player.CalculateDevelopment(
					this.game.Statics,
					this.game.States,
					this.game.Derivates.Colonies.OwnedBy(player.Player)
				);
				player.CalculateResearch(
					this.game.Statics,
					this.game.States,
					this.game.Derivates.Colonies.OwnedBy(player.Player)
				);
			}
		}

		public void CalculateDerivedEffects()
		{
			foreach (var colonyProc in this.game.Derivates.Colonies)
				colonyProc.CalculateDerivedEffects(this.game.Statics, this.game.Derivates.Of(colonyProc.Owner));
		}

		private void commitFleetOrders()
		{
			foreach (var player in this.game.Players) {
				foreach (var order in player.Orders.ShipOrders) {
					foreach (var fleet in this.game.States.Fleets.At(order.Key).Where(x => x.Owner == player))
						this.game.States.Fleets.PendRemove(fleet);

					this.game.States.Fleets.ApplyPending();
					foreach (var fleet in order.Value)
						this.game.States.Fleets.Add(fleet);
				}

				player.Orders.ShipOrders.Clear();
			}
		}

		public bool HasConflicts {
			get
			{
				return this.conflicts.Count != 0;
			}
		}

		private void doColonization()
		{
			foreach(var project in this.game.States.ColonizationProjects)
			{
				var playerProc = this.game.Derivates.Of(project.Owner);
				bool colonyExists = this.game.States.Colonies.AtPlanetContains(project.Destination);
				//TODO(v0.5) deduce arrived ships from fleets state
				var arrivedPopulation = project.Arrived.Sum(x => playerProc.DesignStats[x.Design].ColonizerPopulation * x.Quantity);
				
				if (colonyExists || arrivedPopulation >= this.game.Statics.ColonyFormulas.ColonizationPopulationThreshold.Evaluate(null))
				{
					var colony = colonyExists ? 
						this.game.States.Colonies.AtPlanet(project.Destination) :
						new Colony(arrivedPopulation, project.Destination, project.Owner);
					
					foreach(var group in project.Arrived)
						foreach(var building in playerProc.DesignStats[group.Design].ColonizerBuildings)
							if (colony.Buildings.ContainsKey(building.Key))
								colony.Buildings[building.Key] += building.Value * group.Quantity;
							else
								colony.Buildings.Add(building.Key, building.Value * group.Quantity);
					
					if (!colonyExists)
					{
						this.game.States.Colonies.Add(colony);
						
						var colonyProc = new ColonyProcessor(colony);
						colonyProc.CalculateBaseEffects(this.game.Statics, this.game.Derivates.Players.Of(colony.Owner));
						this.game.Derivates.Colonies.Add(colonyProc);

						if (!this.game.States.Stellarises.AtContains(project.Destination.Star))
						{
							var stellaris = new StellarisAdmin(project.Destination.Star, project.Owner);
							this.game.States.Stellarises.Add(stellaris);
							this.game.Derivates.Stellarises.Add(new StellarisProcessor(stellaris));
						}
					}
					else
						this.game.Derivates.Of(colony).AddPopulation(arrivedPopulation);
					
					project.Owner.Orders.ColonizationOrders.Remove(project.Destination);
					project.Arrived.Clear();
				}
				//TODO(v0.5) remove colonization project from states at some point
			}
		}
				
		//TODO(v0.5) move above doColonization
		private void detectConflicts()
		{
			var visits = new Dictionary<Vector2D, ICollection<FleetMovement>>();
			var conflictPositions = new HashSet<Vector2D>();
			var decidedFleet = new HashSet<Fleet>();
			
			foreach(var step in this.fleetMovement.OrderBy(x => x.ArrivalTime))
	        {
				if (!visits.ContainsKey(step.LocalFleet.Position))
					visits.Add(step.LocalFleet.Position, new List<FleetMovement>());
				
				if (decidedFleet.Contains(step.OriginalFleet) || visits[step.LocalFleet.Position].Any(x => x.OriginalFleet == step.OriginalFleet))
					continue;
				
				var fleets = visits[step.LocalFleet.Position];
				fleets.Add(step);
				
				bool inConflict = fleets.Aggregate(
					false,
					(isInConflict, fleet) => isInConflict | fleets.Any(
						x => x.OriginalFleet.Owner != fleet.OriginalFleet.Owner && 
							x.ArrivalTime <= fleet.DepartureTime && x.DepartureTime >= fleet.ArrivalTime
					)
				);
				
				if (inConflict)
					decidedFleet.UnionWith(fleets.Where(x => x.ArrivalTime < step.ArrivalTime).Select(x => x.OriginalFleet));
	        }
			
			this.conflicts.Clear();
			foreach(var position in conflictPositions)
				conflicts.Add(new Conflict(position, visits[position]));
			
			this.game.States.Fleets.Clear();
			foreach(var fleet in visits.Values.SelectMany(x => x).Where(x => !x.Remove))
				this.game.States.Fleets.Add(fleet.LocalFleet);
		}
		
		private void mergeStationaryFleets()
		{
			/*
 			 * Aggregate stationary fleets, if there are multiple stationary fleets of 
			 * the same owner at the same star, merge them to one fleet.
 			 */
			foreach(var star in game.States.Stars) 
			{
				var playerFleets = this.game.States.Fleets.At(star.Position).GroupBy(x => x.Owner);
				foreach(var playerFleet in playerFleets) 
				{
					var stationary = playerFleet.Where(x => x.Missions.Count == 0).ToArray();
					if (stationary.Length <= 1)
						continue;
					
					var newFleet = new Fleet(stationary[0].Owner, stationary[0].Position, new LinkedList<AMission>());
					foreach(var fleet in stationary) 
					{
						this.game.States.Fleets.PendRemove(fleet);
						foreach(var ship in fleet.Ships)
							if (newFleet.Ships.DesignContains(ship.Design))
								newFleet.Ships.Design(ship.Design).Quantity += ship.Quantity;
							else
								newFleet.Ships.Add(new ShipGroup(ship.Design, ship.Quantity));
					}
					this.game.States.Fleets.PendAdd(newFleet);
				}
			}
			this.game.States.Fleets.ApplyPending();
		}
		
		private void moveShips()
		{
			this.fleetMovement.Clear();
			
			foreach (var fleet in this.game.States.Fleets)
			{
				var fleetProcessor = new FleetProcessingVisitor(fleet, game);
				this.fleetMovement.AddRange(fleetProcessor.Run());
			}
		}
	}
}
