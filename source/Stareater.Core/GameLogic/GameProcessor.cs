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
		private Game game;

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

		private void moveShips()
		{
			foreach (var fleet in this.game.States.Fleets)
			{
				//TODO(v0.5) fleet processor should loop throug collection internally and track how much "action points" fleet has after each mission
				var fleetProcessor = new FleetProcessingVisitor(fleet, game);
				
			}

			this.game.States.Fleets.ApplyPending();
			
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
					}
					else
						this.game.Derivates.Of(colony).AddPopulation(arrivedPopulation);
					
					project.Owner.Orders.ColonizationOrders.Remove(project.Destination);
					project.Arrived.Clear();
				}
				//TODO(v0.5) remove colonization project from states at some point
			}
		}
	}
}
