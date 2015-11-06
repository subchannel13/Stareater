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
			this.precombatColonization();
			
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

			this.postcombatColonization();
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
				if (fleet.Mission.Type == MissionType.Move) {
					this.game.States.Fleets.PendRemove(fleet);
					var mission = fleet.Mission as MoveMission;
					
					var playerProc = game.Derivates.Players.Of(fleet.Owner);
					double baseSpeed = fleet.Ships.Select(x => x.Design).
						Aggregate(double.MaxValue, (s, x) => Math.Min(playerProc.DesignStats[x].GalaxySpeed, s));
						
					//TODO(v0.5) loop through all waypoints
					var startStar = game.States.Stars.At(mission.Waypoints[0]);
					var endStar = game.States.Stars.At(mission.Waypoints[1]);
					var speed = baseSpeed;
					if (game.States.Wormholes.At(startStar).Intersect(game.States.Wormholes.At(endStar)).Any())
						speed += 0.5; //TODO(later) consider making moddable
					
					var waypoints = mission.Waypoints.Skip(1).ToArray();
					var distance = (waypoints[0] - fleet.Position).Magnitude();

					//TODO(v0.5) detect conflicts
					//TODO(v0.5) merge with existing fleet
					if (distance <= speed) {
						var newFleet = new Fleet(
							fleet.Owner,
							waypoints[0],
							new StationaryMission(this.game.States.Stars.At(waypoints[0]))
						);
						newFleet.Ships.Add(fleet.Ships);
						this.game.States.Fleets.PendAdd(newFleet);
						
						fleet.Owner.Intelligence.StarFullyVisited(endStar, game.Turn);
					}
					else {
						var direction = (waypoints[0] - fleet.Position);
						direction.Normalize();

						var newFleet = new Fleet(
							fleet.Owner,
							fleet.Position + direction * speed,
							fleet.Mission
						);
						newFleet.Ships.Add(fleet.Ships);
						this.game.States.Fleets.PendAdd(newFleet);
					}
				}

			this.game.States.Fleets.ApplyPending();
			
			foreach(var star in game.States.Stars) 
			{
				var playerFleets = this.game.States.Fleets.At(star.Position).GroupBy(x => x.Owner);
				foreach(var playerFleet in playerFleets) 
				{
					var stationary = playerFleet.Where(x => x.Mission.Type == MissionType.Stationary).ToArray();
					if (stationary.Length <= 1)
						continue;
					
					var newFleet = new Fleet(stationary[0].Owner, stationary[0].Position, stationary[0].Mission);
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

		private void precombatColonization()
		{
			foreach(var project in this.game.States.ColonizationProjects)
			{
				var arrived = new HashSet<Fleet>();
				foreach(var fleet in project.Enroute)
					if (fleet.Position != project.Destination.Star.Position)
					{
						//TODO(v0.5) move colonizers
					}
					else
					{
						foreach(var group in fleet.Ships)
							project.Arrived.Add(group);
						
						arrived.Add(fleet);
						//TODO(v0.5) remove arrived fleet
					}
				project.Enroute.RemoveAll(arrived.Contains);
				
				var playerProc = this.game.Derivates.Of(project.Owner);
				var arrivedPopulation = project.Arrived.Sum(x => playerProc.DesignStats[x.Design].ColonizerPopulation * x.Quantity);
				if (arrivedPopulation >= this.game.Statics.ColonyFormulas.ColonizationPopulationThreshold.Evaluate(null))
				{
					var colony = new Colony(arrivedPopulation, project.Destination, project.Owner);
					foreach(var group in project.Arrived)
						foreach(var building in playerProc.DesignStats[group.Design].ColonizerBuildings)
							colony.Buildings.Add(building.Key, building.Value * group.Quantity);
					this.game.States.Colonies.Add(colony);
					
					var colonyProc = new ColonyProcessor(colony);
					colonyProc.CalculateBaseEffects(this.game.Statics, this.game.Derivates.Players.Of(colony.Owner));
					this.game.Derivates.Colonies.Add(colonyProc);
				
					project.Owner.Orders.ColonizationOrders.Remove(project.Destination);
				}
				//TODO(v0.5) what happens to colonizers that arrive after the colony is established?
			}
		}

		private void postcombatColonization()
		{
			foreach(var project in this.game.States.ColonizationProjects)
			{
				foreach(var fleet in project.NewColonizers)
				{
					var newFleet = new Fleet(
						fleet.Owner, 
						fleet.Position, 
						new MoveMission(new Vector2D[] { fleet.Position, project.Destination.Star.Position })
					);
					newFleet.Ships.Add(fleet.Ships);
					project.Enroute.Add(newFleet);
				}
				project.NewColonizers.Clear();
			}
		}
	}
}
