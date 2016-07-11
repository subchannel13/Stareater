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
		private readonly MainGame game;
		private readonly List<FleetMovement> fleetMovement = new List<FleetMovement>();
		private readonly Queue<SpaceBattleGame> conflicts = new Queue<SpaceBattleGame>();

		public GameProcessor(MainGame game)
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
		}

		public void ProcessPostcombat()
		{
			this.doColonization();
			this.mergeFleets();
			
			foreach (var playerProc in this.game.Derivates.Players)
				playerProc.ProcessPostcombat(this.game.Statics, this.game.States, this.game.Derivates);

			// TODO(v0.5): Update ship designs

			// TODO(v0.5): Upgrade and repair ships

			CalculateBaseEffects();
			CalculateSpendings();
			CalculateDerivedEffects();
			
			this.game.Turn++;
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

		public bool HasConflicts 
		{
			get
			{
				return this.conflicts.Count != 0;
			}
		}
		
		public bool IsOver
		{
			get
			{
				 //TODO(later) end game by leaving stareater
				return game.States.Colonies.Select(x => x.Owner).Distinct().Count() <= 1;
			}
		}
		
		public SpaceBattleGame NextConflict()
		{
			return this.conflicts.Dequeue();
		}

		public void ConflictResolved(SpaceBattleGame battleGame)
		{
			//TODO(v0.5) decide what to do with retreated ships, send them to nearest fiendly system?
			foreach(var unit in battleGame.Combatants.Concat(battleGame.Retreated))
			{
				var fleet = new Fleet(unit.Owner, battleGame.Location, new LinkedList<AMission>());
				fleet.Ships.Add(unit.Ships);
				
				//FIXME(v0.5) adds too many ships, should remove participants
				this.game.States.Fleets.Add(fleet);
			}
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
				if (this.game.States.Stars.AtContains(position.Key))
					conflicts.Enqueue(new SpaceBattleGame(position.Key, visits[position.Key], position.Value, this.game));
			//TODO(later) deep space interception
			
			this.game.States.Fleets.Clear();
			foreach(var fleet in visits.Values.SelectMany(x => x))
				this.game.States.Fleets.Add(fleet.LocalFleet);
		}

		private void doColonization()
		{
			foreach(var project in this.game.States.ColonizationProjects)
			{
				var playerProc = this.game.Derivates.Of(project.Owner);
				bool colonyExists = this.game.States.Colonies.AtPlanetContains(project.Destination);
				
				var colonizers = this.game.States.Fleets.At(project.Destination.Star.Position).Where(
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
					colonyProc.CalculateBaseEffects(this.game.Statics, this.game.Derivates.Players.Of(colony.Owner));
					
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
					

					if (!this.game.States.Stellarises.AtContains(project.Destination.Star))
					{
						var stellaris = new StellarisAdmin(project.Destination.Star, project.Owner);
						this.game.States.Stellarises.Add(stellaris);
						this.game.Derivates.Stellarises.Add(new StellarisProcessor(stellaris));
					}
					
					project.Owner.Orders.ColonizationOrders.Remove(project.Destination);
					this.game.States.ColonizationProjects.PendRemove(project);
				}
			}
			
			this.game.States.ColonizationProjects.ApplyPending();
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
				var perPlayerFleets = this.game.States.Fleets.At(star.Position).GroupBy(x => x.Owner);
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
								if (newFleet.Ships.DesignContains(ship.Design))
									newFleet.Ships.Design(ship.Design).Quantity += ship.Quantity;
								else
									newFleet.Ships.Add(new ShipGroup(ship.Design, ship.Quantity));
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
