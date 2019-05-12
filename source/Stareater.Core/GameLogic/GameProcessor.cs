using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Ships;
using Stareater.Ships.Missions;
using Stareater.Galaxy;
using Stareater.GameLogic.Combat;
using Stareater.GameLogic.Planning;
using Stareater.Utils;
using Stareater.GameData.Databases.Tables;

namespace Stareater.GameLogic
{
	class GameProcessor
	{
		private readonly MainGame game;
		private readonly List<FleetMovement> fleetMovement = new List<FleetMovement>();
		private readonly Queue<Conflict> conflicts = new Queue<Conflict>();
		private readonly Queue<Pair<Player>> audiences = new Queue<Pair<Player>>();

		public GameProcessor(MainGame game)
		{
			this.game = game;
		}

		public bool IsOver
		{
			get
			{
				 //TODO(v0.8) end game by leaving stareater
				return game.States.Colonies.Select(x => x.Owner).Distinct().Count() <= 1;
			}
		}
		
		#region Turn processing
		public void ProcessPrecombat()
		{
			var processors = this.game.MainPlayers.Select(x => this.game.Derivates[x]).ToList();

			this.CalculateBaseEffects();
			this.CalculateSpendings();
			this.CalculateDerivedEffects();
			this.commitFleetOrders();

			this.game.States.Reports.Clear();
			foreach (var playerProc in processors)
				playerProc.ProcessPrecombat(this.game);
			this.game.Derivates.Natives.ProcessPrecombat(this.game.Statics, this.game.States, this.game.Derivates); 
			//TODO(later) process natives postcombat
			
			this.moveShips();
			this.detectConflicts();
			this.enqueueAudiences();
		}

		public void ProcessPostcombat()
		{
			foreach (var star in this.game.States.Stars)
			{
				foreach (var trait in star.Traits)
					trait.PostcombatApply(this.game.Statics, star, this.game.States.Planets.At[star]);
				star.Traits.ApplyPending();
			}

			this.doStareaterActions();
			this.doColonization();
			this.mergeFleets();
			
			foreach (var playerProc in this.game.MainPlayers.Select(x => this.game.Derivates[x]))
				playerProc.ProcessPostcombat(this.game);

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
			{
				stellaris.ApplyPolicy(this.game, this.game.Orders[stellaris.Owner].Policies[stellaris.Stellaris]);
				stellaris.CalculateBaseEffects(game);
			}
			foreach (var colonyProc in this.game.Derivates.Colonies)
				colonyProc.CalculateBaseEffects(this.game.Statics, this.game.Derivates[colonyProc.Owner]);
			foreach (var playerProc in this.game.Derivates.Players)
				playerProc.CalculateBaseEffects(this.game);
        }

		public void CalculateSpendings()
		{
			foreach (var colonyProc in this.game.Derivates.Colonies)
				colonyProc.CalculateSpending(
					this.game,
					this.game.Derivates[colonyProc.Owner]
				);

			foreach (var stellaris in this.game.Derivates.Stellarises)
				stellaris.CalculateSpending(this.game);

			foreach (var player in this.game.Derivates.Players) {
				player.CalculateDevelopment(
					this.game,
					this.game.Derivates.Colonies.OwnedBy[player.Player]
				);
				player.CalculateResearch(
					this.game,
					this.game.Derivates.Colonies.OwnedBy[player.Player]
				);
			}
		}

		public void CalculateDerivedEffects()
		{
			foreach (var colonyProc in this.game.Derivates.Colonies)
				colonyProc.CalculateDerivedEffects(this.game.Statics, this.game.Derivates[colonyProc.Owner]);
			foreach (var stellaris in this.game.Derivates.Stellarises)
				stellaris.CalculateDerivedEffects(this.game);
		}
		#endregion
		
		#region Conflict cycling
		public bool HasConflict
		{
			get
			{
				return this.conflicts.Count != 0;
			}
		}
		
		public Conflict NextConflict()
		{
			return this.conflicts.Dequeue();
		}

		public void ConflictResolved(ABattleGame battleGame)
		{
			//TODO(later) decide what to do with retreated ships, send them to nearest fiendly system?
			foreach(var unit in battleGame.Combatants.Concat(battleGame.Retreated))
			{
                unit.Ships.Damage = this.game.Derivates[unit.Owner].DesignStats[unit.Ships.Design].HitPoints * unit.Ships .Quantity - 
					unit.TopArmor - 
					unit.RestArmor;

				var fleet = new Fleet(unit.Owner, battleGame.Location, new LinkedList<AMission>());
				fleet.Ships.Add(unit.Ships);
				
				this.game.States.Fleets.Add(fleet);
			}
		}
		#endregion

		#region Diplomacy
		public bool HasAudience 
		{
			get
			{
				return this.audiences.Count != 0;
			}
		}

		public Pair<Player> NextAudience()
		{
			return this.audiences.Dequeue();
		}

		public void AudienceConcluded(Pair<Player> participants, HashSet<Treaty> treaties)
		{
			foreach(var oldTreaty in this.game.States.Treaties.Of[participants].ToList())
				this.game.States.Treaties.Remove(oldTreaty);
			this.game.States.Treaties.Add(treaties);
		}

        public bool IsAtWar(Player party1, Player party2)
        {
        	return party1 == this.game.StareaterOrganelles || party2 == this.game.StareaterOrganelles ||
            	this.game.States.Treaties.Of[party1, party2].Any();
        }
		#endregion

		private void commitFleetOrders()
		{
			foreach (var player in this.game.AllPlayers)
			{
				foreach (var order in this.game.Orders[player].ShipOrders) 
				{
					var totalDamage = new Dictionary<Design, double>();
					var totalUpgrades = new Dictionary<Design, double>();
					var shipCount = new Dictionary<Design, double>();
					foreach (var fleet in this.game.States.Fleets.At[order.Key, player])
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
							ship.UpgradePoints = totalUpgrades[ship.Design] * ship.Quantity / shipCount[ship.Design];
						}
						this.game.States.Fleets.Add(fleet);
					}
				}

				this.game.Orders[player].ShipOrders.Clear();
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
				
				if (!game.States.Stars.At.Contains(step.LocalFleet.Position))
					continue; //TODO(later) no deepspace interception for now
				var star = game.States.Stars.At[step.LocalFleet.Position];
				
				var players = new HashSet<Player>(fleets.Where(x => x.ArrivalTime < step.ArrivalTime).Select(x => x.OriginalFleet.Owner));
				players.UnionWith(game.States.Colonies.AtStar[star].Select(x => x.Owner));
				players.Remove(step.LocalFleet.Owner);

				bool inConflict = step.LocalFleet.Owner == game.StareaterOrganelles ?
					players.Any() :
					players.Any(x => this.IsAtWar(step.LocalFleet.Owner, x));

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
					conflicts.Enqueue(new Conflict(position.Key, visits[position.Key], position.Value));
			//TODO(later) deep space interception
			
			//TODO(v0.8) could make "fleet trail" if fleet visits multiple stars in the same turn
			this.game.States.Fleets.Clear();
			foreach(var fleet in visits.Where(x => !conflictPositions.ContainsKey(x.Key)).SelectMany(x => x.Value))
				this.game.States.Fleets.Add(fleet.LocalFleet);
		}

		private void doStareaterActions()
		{
			foreach(var player in this.game.MainPlayers.Where(x => this.game.Orders[x].EjectingStar != null))
			{
				if (!this.game.Derivates[player].ControlsStareater)
					continue;

				var star = this.game.Orders[player].EjectingStar;
				this.game.States.Stars.Remove(star);

                foreach (var lane in this.game.States.Wormholes.At[star])
					this.game.States.Wormholes.PendRemove(lane);
				this.game.States.Wormholes.ApplyPending();

				foreach (var planet in this.game.States.Planets.At[star])
					this.game.States.Planets.PendRemove(planet);
				this.game.States.Planets.ApplyPending();

				foreach (var stellaris in this.game.States.Stellarises.At[star])
				{
					this.game.States.Stellarises.PendRemove(stellaris);
					this.game.Derivates.Stellarises.Remove(this.game.Derivates[stellaris]);
					this.game.Orders[stellaris.Owner].Policies.Remove(stellaris);
				}
				this.game.States.Stellarises.ApplyPending();

				foreach (var colony in this.game.States.Colonies.AtStar[star])
				{
					this.game.States.Colonies.PendRemove(colony);
					this.game.Derivates.Colonies.Remove(this.game.Derivates[colony]);
					this.game.Orders[colony.Owner].ConstructionPlans.Remove(colony);
				}
				this.game.States.Colonies.ApplyPending();

				foreach (var vpRewards in this.game.Derivates[player].EjectVictoryPoints)
					vpRewards.Key.VictoryPoints += vpRewards.Value;
			}
		}

		//TODO(v0.8) can unintentionally land non-colony ships like scouts
        private void doColonization()
		{
			foreach(var project in this.game.States.ColonizationProjects)
			{
				var playerProc = this.game.Derivates[project.Owner];
				bool colonyExists = this.game.States.Colonies.AtPlanet.Contains(project.Destination);
				
				var colonizers = this.game.States.Fleets.At[project.Destination.Star.Position].
					Where(x => x.Owner == project.Owner && x.Ships.Any(s => s.PopulationTransport > 0));
					
				var arrivedPopulation = colonizers.SelectMany(x => x.Ships).Sum(x => x.PopulationTransport);
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
							var landingLimit = (long)Math.Ceiling((colonizationTreshold - colony.Population) / shipStats.ColonizerPopulation);
							var shipsLanded = Math.Min(shipGroup.Quantity, landingLimit);
							
							colonyProc.AddPopulation(shipsLanded * shipStats.ColonizerPopulation);
							
							foreach(var building in shipStats.ColonizerBuildings)
								if (colony.Buildings.ContainsKey(building.Key))
									colony.Buildings[building.Key] += building.Value * shipGroup.Quantity;
								else
									colony.Buildings.Add(building.Key, building.Value * shipGroup.Quantity);	
							
							shipGroup.Quantity -= shipsLanded;
							shipGroup.PopulationTransport -= shipsLanded * shipStats.ColonizerPopulation;
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
					this.game.Orders[project.Owner].ConstructionPlans[colony] = new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio);
					this.game.Orders[project.Owner].AutomatedConstruction[colony] = new ConstructionOrders(0);

					if (this.game.States.Stellarises.At[project.Destination.Star].All(x => x.Owner != project.Owner))
					{
						var stellaris = new StellarisAdmin(project.Destination.Star, project.Owner);
						this.game.States.Stellarises.Add(stellaris);
						this.game.Derivates.Stellarises.Add(new StellarisProcessor(stellaris));
						this.game.Orders[project.Owner].Policies[stellaris] = game.Statics.Policies.First();
						this.game.Orders[project.Owner].AutomatedConstruction[stellaris] = new ConstructionOrders(0);
					}
				}
				
				if (colonyExists || arrivedPopulation >= colonizationTreshold)
				{
					this.game.Orders[project.Owner].ColonizationTargets.Remove(project.Destination);
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
				var localFleet = this.game.States.Fleets.At[stellaris.Location.Star.Position, player];
				var repairPoints = this.game.Derivates.Colonies.
					At[stellaris.Location.Star, player].
					Aggregate(0.0, (sum, x) => sum + x.RepairPoints);
				
				var designStats = this.game.Derivates[player].DesignStats;
				var repairCostFactor = this.game.Statics.ShipFormulas.RepairCostFactor;
				var damagedShips = localFleet.SelectMany(x => x.Ships).Where(x => x.Damage > 0);
				var totalNeededRepairPoints = damagedShips.Sum(x => repairCostFactor * x.Damage * x.Design.Cost / designStats[x.Design].HitPoints);
				
				foreach(var shipGroup in damagedShips)
				{
					var repirPerHp = repairCostFactor * shipGroup.Design.Cost / designStats[shipGroup.Design].HitPoints;
					var fullRepairCost = shipGroup.Damage * repirPerHp;
					var investment = repairPoints * fullRepairCost / totalNeededRepairPoints;
					
					if (fullRepairCost < investment)
					{
						shipGroup.Damage = 0;
						investment -= investment - fullRepairCost;
					}
					else
						shipGroup.Damage += investment / repirPerHp;
					
					repairPoints -= investment;
					totalNeededRepairPoints -= fullRepairCost;
				}

				var refitOrders = this.game.Orders[player].RefitOrders;
				var refitCosts = this.game.Derivates[player].RefitCosts;
				var groupsFrom = new Dictionary<ShipGroup, Fleet>();

				foreach(var fleet in localFleet)
					foreach(var shipGroup in fleet.Ships)
						groupsFrom.Add(shipGroup, fleet);
				var upgradableShips = localFleet.
					SelectMany(x => x.Ships).
					Where(x => refitOrders.ContainsKey(x.Design)).ToList();
				var totalNeededUpgradePoints = upgradableShips.
					Select(x => refitCosts[x.Design][refitOrders[x.Design]] * x.Quantity - x.UpgradePoints).
					Aggregate(0.0, (sum, x) => x > 0 ? sum + x : sum);
				
				foreach(var shipGroup in upgradableShips)
				{
					var refitTo = refitOrders[shipGroup.Design];
					var refitCost = refitCosts[shipGroup.Design][refitOrders[shipGroup.Design]];
					var fullUpgradeCost = refitCost * shipGroup.Quantity - shipGroup.UpgradePoints;
					var investment = repairPoints * fullUpgradeCost / totalNeededUpgradePoints;
					
					if (fullUpgradeCost < investment)
					{
						shipGroup.UpgradePoints = refitCost * shipGroup.Quantity;
						investment -= investment - fullUpgradeCost;
					}
					else
						shipGroup.UpgradePoints += investment;
					
					repairPoints -= investment;
					totalNeededUpgradePoints -= fullUpgradeCost;
					var upgradedShips = refitCost > 0 ? (long)Math.Floor(shipGroup.UpgradePoints / refitCost) : shipGroup.Quantity;
					
					if (upgradedShips > 0)
					{
						var movedPopulation = shipGroup.PopulationTransport * upgradedShips / shipGroup.Quantity;
						shipGroup.Quantity -= upgradedShips;
						shipGroup.UpgradePoints -= upgradedShips * refitCost;

						var fleet = groupsFrom[shipGroup];
						var existingGroup = fleet.Ships.FirstOrDefault(x => x.Design == refitTo);
						
						if (shipGroup.Quantity <= 0)
							fleet.Ships.Remove(shipGroup);

						if (existingGroup != null)
						{
							existingGroup.Quantity += upgradedShips;
							existingGroup.PopulationTransport += movedPopulation;
						}
						else
							fleet.Ships.Add(new ShipGroup(refitTo, upgradedShips, 0, 0, movedPopulation));
					}
				}
			}
		}

		private void enqueueAudiences()
		{
			var requests = this.game.MainPlayers.
				SelectMany(p1 => this.game.Orders[p1].AudienceRequests.Select(p2 => new Pair<Player>(p1, this.game.MainPlayers[p2]))).
				Distinct();

			foreach (var request in requests)
				this.audiences.Enqueue(request);

			foreach (var player in this.game.MainPlayers)
				this.game.Orders[player].AudienceRequests.Clear();
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
								{
									newFleet.Ships.WithDesign[ship.Design].Quantity += ship.Quantity;
									newFleet.Ships.WithDesign[ship.Design].PopulationTransport += ship.PopulationTransport;
								}
								else
									newFleet.Ships.Add(new ShipGroup(ship.Design, ship.Quantity, ship.Damage, ship.UpgradePoints, ship.PopulationTransport));
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
