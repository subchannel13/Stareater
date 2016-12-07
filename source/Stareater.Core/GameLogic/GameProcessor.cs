using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.GameData;
using Stareater.Ships;
using Stareater.Ships.Missions;
using Stareater.Galaxy;

namespace Stareater.GameLogic
{
	class GameProcessor
	{
		private readonly MainGame game;
		private readonly List<FleetMovement> fleetMovement = new List<FleetMovement>();
		private readonly Queue<SpaceBattleGame> conflicts = new Queue<SpaceBattleGame>();

		public GameProcessor(MainGame game)
		{
			this.game = game;
		}

		public bool IsOver
		{
			get
			{
				 //TODO(later) end game by leaving stareater
				return game.States.Colonies.Select(x => x.Owner).Distinct().Count() <= 1;
			}
		}
		
		#region Turn processing
		public void ProcessPrecombat()
		{
			this.CalculateBaseEffects();
			this.CalculateSpendings();
			this.CalculateDerivedEffects();
			this.commitFleetOrders();

			this.game.States.Reports.Clear();
			foreach (var playerProc in this.game.MainPlayers.Select(x => this.game.Derivates.Of(x)))
				playerProc.ProcessPrecombat(
					this.game.Statics,
					this.game.States,
					this.game.Derivates
				);
			this.game.Derivates.Natives.ProcessPrecombat(this.game.Statics, this.game.States, this.game.Derivates); 
			//TODO(v0.6) process natives postcombat
			
			this.moveShips();
			this.detectConflicts();
		}

		public void ProcessPostcombat()
		{
			this.doColonization();
			this.mergeFleets();
			
			foreach (var playerProc in this.game.MainPlayers.Select(x => this.game.Derivates.Of(x)))
				playerProc.ProcessPostcombat(this.game.Statics, this.game.States, this.game.Derivates);

			this.doRepairs();

			this.CalculateBaseEffects();
			this.CalculateSpendings();
			this.CalculateDerivedEffects();
			
			this.game.Turn++;
		}
		#endregion

		#region Derived stats calculation
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
					this.game.Derivates.Colonies.At[stellaris.Location]
				);

			foreach (var player in this.game.Derivates.Players) {
				player.CalculateDevelopment(
					this.game.Statics,
					this.game.States,
					this.game.Derivates.Colonies.OwnedBy[player.Player]
				);
				player.CalculateResearch(
					this.game.Statics,
					this.game.States,
					this.game.Derivates.Colonies.OwnedBy[player.Player]
				);
			}
		}

		public void CalculateDerivedEffects()
		{
			foreach (var colonyProc in this.game.Derivates.Colonies)
				colonyProc.CalculateDerivedEffects(this.game.Statics, this.game.Derivates.Of(colonyProc.Owner));
		}
		#endregion
		
		#region Conflict cycling
		public bool HasConflicts
		{
			get
			{
				return this.conflicts.Count != 0;
			}
		}
		
		public SpaceBattleGame NextConflict()
		{
			return this.conflicts.Dequeue();
		}

		public void ConflictResolved(SpaceBattleGame battleGame)
		{
			//TODO(later) decide what to do with retreated ships, send them to nearest fiendly system?
			foreach(var unit in battleGame.Combatants.Concat(battleGame.Retreated))
			{
				var fleet = new Fleet(unit.Owner, battleGame.Location, new LinkedList<AMission>());
				fleet.Ships.Add(unit.Ships);
				
				this.game.States.Fleets.Add(fleet);
			}
		}
		#endregion
		
		private void commitFleetOrders()
		{
			foreach (var player in this.game.AllPlayers)
			{
				foreach (var order in player.Orders.ShipOrders) 
				{
					var totalDamage = new Dictionary<Design, double>();
					var totalUpgrades = new Dictionary<Design, double>();
					var shipCount = new Dictionary<Design, double>();
					foreach (var fleet in this.game.States.Fleets.At[order.Key].Where(x => x.Owner == player))
					{
						foreach(var ship in fleet.Ships)
						{
							if (!shipCount.ContainsKey(ship.Design))
							{
								shipCount.Add(ship.Design, 0);
								totalDamage.Add(ship.Design, 0);
								totalUpgrades.Add(ship.Design, 0);
							}
							
							totalDamage[ship.Design] += ship.Damage;
							totalUpgrades[ship.Design] += ship.UpgradePoints;
							shipCount[ship.Design] += ship.Quantity;
						}
						
						this.game.States.Fleets.PendRemove(fleet);
					}
					this.game.States.Fleets.ApplyPending();
					
					foreach (var fleet in order.Value)
					{
						foreach(var ship in fleet.Ships)
						{
							ship.Damage = totalDamage[ship.Design] * ship.Quantity / shipCount[ship.Design];
							ship.UpgradePoints = totalUpgrades[ship.Design] * ship.Quantity / shipCount[ship.Design]; //TODO(v0.6) test
						}
						this.game.States.Fleets.Add(fleet);
					}
				}

				player.Orders.ShipOrders.Clear();
			}
		}

		private void detectConflicts()
		{
			var visits = new Dictionary<Vector2D, ICollection<FleetMovement>>();
			var conflictPositions = new Dictionary<Vector2D, double>();
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
				{
					if (!conflictPositions.ContainsKey(step.LocalFleet.Position))
						conflictPositions.Add(step.LocalFleet.Position, step.ArrivalTime);
					decidedFleet.UnionWith(fleets.Where(x => x.ArrivalTime < step.ArrivalTime).Select(x => x.OriginalFleet));
				}
	        }
			
			this.conflicts.Clear();
			foreach(var position in conflictPositions.OrderBy(x => x.Value))
				if (this.game.States.Stars.At.Contains(position.Key))
					conflicts.Enqueue(new SpaceBattleGame(position.Key, visits[position.Key], position.Value, this.game));
			//TODO(later) deep space interception
			
			//FIXME(later) could make "fleet trail" if fleet visits multiple stars in the same turn
			this.game.States.Fleets.Clear();
			foreach(var fleet in visits.Where(x => !conflictPositions.ContainsKey(x.Key)).SelectMany(x => x.Value))
				this.game.States.Fleets.Add(fleet.LocalFleet);
		}

		private void doColonization()
		{
			foreach(var project in this.game.States.ColonizationProjects)
			{
				var playerProc = this.game.Derivates.Of(project.Owner);
				bool colonyExists = this.game.States.Colonies.AtPlanet.Contains(project.Destination);
				
				var colonizers = this.game.States.Fleets.At[project.Destination.Star.Position].Where(
					x => 
					{
						if (x.Owner != project.Owner || x.Missions.Count == 0)
							return false;
						
						var mission = x.Missions.First.Value as ColonizationMission;
						return mission != null && mission.Target == project.Destination;
					});
					
				var arrivedPopulation = colonizers.SelectMany(x => x.Ships).Sum(x => playerProc.DesignStats[x.Design].ColonizerPopulation * x.Quantity);
				var colonizationTreshold = this.game.Statics.ColonyFormulas.ColonizationPopulationThreshold.Evaluate(null);
				
				if (!colonyExists && arrivedPopulation >= colonizationTreshold)
				{
					var colony = new Colony(0, project.Destination, project.Owner);
					var colonyProc = new ColonyProcessor(colony);
					colonyProc.CalculateBaseEffects(this.game.Statics, this.game.Derivates.Players.Of[colony.Owner]);
					
					foreach(var fleet in colonizers)
					{
						foreach(var shipGroup in fleet.Ships)
						{
							var shipStats = playerProc.DesignStats[shipGroup.Design];
							var groupPopulation = shipStats.ColonizerPopulation * shipGroup.Quantity;
							var landingLimit = (long)Math.Ceiling((colonizationTreshold - colony.Population) / shipStats.ColonizerPopulation);
							var shipsLanded = Math.Min(shipGroup.Quantity, landingLimit);
							
							colonyProc.AddPopulation(shipsLanded * shipStats.ColonizerPopulation);
							
							foreach(var building in shipStats.ColonizerBuildings)
								if (colony.Buildings.ContainsKey(building.Key))
									colony.Buildings[building.Key] += building.Value * shipGroup.Quantity;
								else
									colony.Buildings.Add(building.Key, building.Value * shipGroup.Quantity);	
							
							shipGroup.Quantity -= shipsLanded;
							if (shipGroup.Quantity < 1)
								fleet.Ships.PendRemove(shipGroup);
						}
						
						fleet.Ships.ApplyPending();
						if (fleet.Ships.Count == 0)
							game.States.Fleets.PendRemove(fleet);
					}
					game.States.Fleets.ApplyPending();
					
					this.game.States.Colonies.Add(colony);
					this.game.Derivates.Colonies.Add(colonyProc);

					if (this.game.States.Stellarises.At[project.Destination.Star].All(x => x.Owner != project.Owner))
					{
						var stellaris = new StellarisAdmin(project.Destination.Star, project.Owner);
						this.game.States.Stellarises.Add(stellaris);
						this.game.Derivates.Stellarises.Add(new StellarisProcessor(stellaris));
					}
				}
				
				if (colonyExists || !colonyExists && arrivedPopulation >= colonizationTreshold)
				{
					project.Owner.Orders.ColonizationOrders.Remove(project.Destination);
					this.game.States.ColonizationProjects.PendRemove(project);
				}
			}
			
			this.game.States.ColonizationProjects.ApplyPending();
		}

		private void doRepairs()
		{
			foreach(var stellaris in this.game.States.Stellarises)
			{
				var player = stellaris.Owner;
				var localFleet = this.game.States.Fleets.
					At[stellaris.Location.Star.Position].
					Where(x => x.Owner == player).ToList();
				var repairPoints = this.game.Derivates.Colonies.
					At[stellaris.Location.Star].
					Where(x => x.Owner == player).
					Aggregate(0.0, (sum, x) => sum + x.RepairPoints);
				
				//TODO(v0.6) do repairs
				
				var groupsFrom = new Dictionary<ShipGroup, Fleet>();
				foreach(var fleet in localFleet)
					foreach(var shipGroup in fleet.Ships)
						groupsFrom.Add(shipGroup, fleet);
				var upgradableShips = localFleet.
					SelectMany(x => x.Ships).
					Where(x => player.Orders.RefitOrders.ContainsKey(x.Design) && player.Orders.RefitOrders[x.Design] != null).ToList();
				var totalNeededUpgradePoints = upgradableShips.
					Select(x => player.Orders.RefitOrders[x.Design].Cost * x.Quantity - x.UpgradePoints).
					Aggregate(0.0, (sum, x) => x > 0 ? sum + x : sum);
				
				foreach(var shipGroup in upgradableShips)
				{
					var refitTo = player.Orders.RefitOrders[shipGroup.Design];
					var fullUpgradeCost = refitTo.Cost * shipGroup.Quantity - shipGroup.UpgradePoints; //TODO(v0.6) make smarter cost function which takes component difference into account
					var investment = repairPoints * fullUpgradeCost / totalNeededUpgradePoints;
					
					if (fullUpgradeCost < investment)
					{
						shipGroup.UpgradePoints = refitTo.Cost * shipGroup.Quantity;
						investment -= investment - fullUpgradeCost;
					}
					else
						shipGroup.UpgradePoints += investment;
					
					repairPoints -= investment;
					totalNeededUpgradePoints -= fullUpgradeCost;
					var upgradedShips = (long)Math.Floor(shipGroup.UpgradePoints / refitTo.Cost);
					
					if (upgradedShips > 0)
					{
						shipGroup.Quantity -= upgradedShips;
						shipGroup.UpgradePoints -= upgradedShips * refitTo.Cost;
						
						var fleet = groupsFrom[shipGroup];
						var existingGroup = fleet.Ships.FirstOrDefault(x => x.Design == refitTo);
						
						if (shipGroup.Quantity <= 0)
							fleet.Ships.Remove(shipGroup);
						
						if (existingGroup != null)
							existingGroup.Quantity += upgradedShips;
						else
							fleet.Ships.Add(new ShipGroup(refitTo, upgradedShips, 0, 0));
					}
				}
			}
		}

		private void mergeFleets()
		{
			var filter = new InvalidMissionVisitor(this.game);
			foreach(var fleet in this.game.States.Fleets)
			{
				var newfleet = filter.Check(fleet);
				
				if (newfleet != null)
				{
					this.game.States.Fleets.PendAdd(newfleet);
					this.game.States.Fleets.PendRemove(fleet);
				}
			}
			this.game.States.Fleets.ApplyPending();
			
			/*
 			 * Aggregate fleets, if there are multiple fleets of the same owner 
			 * at the same star with same missions, merge them to one fleet.
 			 */
			foreach(var star in game.States.Stars) 
			{
				var perPlayerFleets = this.game.States.Fleets.At[star.Position].GroupBy(x => x.Owner);
				foreach(var fleets in perPlayerFleets) 
				{
					var missionGroups = new Dictionary<LinkedList<AMission>, List<Fleet>>();

					foreach (var fleet in fleets)
					{
						var missionKey = missionGroups.Keys.FirstOrDefault(x => x.SequenceEqual(fleet.Missions));
						
						if (missionKey == null)
						{
							missionKey = fleet.Missions;
							missionGroups.Add(missionKey, new List<Fleet>());
						}
						missionGroups[missionKey].Add(fleet);
					}

					foreach (var grouping in missionGroups.Where(x => x.Value.Count > 1))
					{
						var newFleet = new Fleet(grouping.Value[0].Owner, grouping.Value[0].Position, grouping.Key);
						foreach (var fleet in grouping.Value)
						{
							this.game.States.Fleets.PendRemove(fleet);
							foreach (var ship in fleet.Ships)
								if (newFleet.Ships.WithDesign.Contains(ship.Design))
									newFleet.Ships.WithDesign[ship.Design].Quantity += ship.Quantity;
								else
									newFleet.Ships.Add(new ShipGroup(ship.Design, ship.Quantity, ship.Damage, ship.UpgradePoints));
						}
						this.game.States.Fleets.PendAdd(newFleet);
					}
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
