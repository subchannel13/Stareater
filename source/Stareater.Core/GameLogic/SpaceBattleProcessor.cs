using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.SpaceCombat;
using Stareater.Utils;

namespace Stareater.GameLogic
{
	class SpaceBattleProcessor
	{
		private const double sigmoidBase = 0.90483741803595957316424905944644; //e^-0.1
		
		private readonly SpaceBattleGame game;
		private readonly MainGame mainGame;
		
		public SpaceBattleProcessor(SpaceBattleGame battleGame, MainGame mainGame)
		{
			this.game = battleGame;
			this.mainGame = mainGame;
		}
		
		#region Initialization
		public void Initialize(IEnumerable<FleetMovement> fleets, double startTime)
		{
			this.initUnits(fleets);
			this.initBodies();
			this.makeUnitOrder();
			
			this.game.TurnLimit = (int)Math.Ceiling(50 * (1 - startTime));
		}

		private void initBodies()
		{
			double maxPlanets = this.mainGame.States.Planets.Max(x => x.Position) - 1;
			var star = mainGame.States.Stars.At[game.Location];
			var planets = this.mainGame.States.Planets.At[star];
			var colonies = this.mainGame.States.Colonies.AtStar[star];
			
			var rings = new Dictionary<int, List<Vector2D>>();
			for(int i = 0; i <= SpaceBattleGame.BattlefieldRadius; i++)
				rings[i] = new List<Vector2D>();
			for(int y = -SpaceBattleGame.BattlefieldRadius; y <= SpaceBattleGame.BattlefieldRadius; y++)
				for(int x = -SpaceBattleGame.BattlefieldRadius; x <= SpaceBattleGame.BattlefieldRadius; x++)
				{
					int distance = (int)Methods.HexDistance(new Vector2D(x, y));
					if (distance <= SpaceBattleGame.BattlefieldRadius)
						rings[distance].Add(new Vector2D(x, y));
				}
			    
			var unoccupied = new Dictionary<int, List<Vector2D>>();
			for(int i = 0; i <= SpaceBattleGame.BattlefieldRadius; i++)
				unoccupied[i] = new List<Vector2D>(rings[i]);
			
			for(int i = 0; i < planets.Count; i++)
			{
				var ring = (int)Math.Floor(Methods.Lerp(
					(planets[i].Position - 1) / maxPlanets, 
					1, 
					SpaceBattleGame.BattlefieldRadius + 0.9999
				));
				Vector2D position;
				
				if (unoccupied[ring].Count > 0)
				{
					int slot = this.game.Rng.Next(unoccupied[ring].Count);
					position = unoccupied[ring][slot];
					
					unoccupied[ring].RemoveAt(slot);
				}
				else
					position = rings[ring][this.game.Rng.Next(rings[ring].Count)];
				
				this.game.Planets[i] = new CombatPlanet(
					colonies.FirstOrDefault(x => x.Location.Planet == planets[i]),
					planets[i],
					position,
					this.mainGame.Statics.ColonyFormulas.PopulationHitPoints.Evaluate(null) //TODO(later) pass relevant variables
				);
			}
		}
		
		private void initUnits(IEnumerable<FleetMovement> fleets)
		{
			foreach(var fleet in fleets)
			{
				Vector2D position;
				if (fleet.MovementDirection.Magnitude() > 0)
					position = -fleet.MovementDirection * (SpaceBattleGame.BattlefieldRadius + 0.25);
				else
				{
					var angle = game.Rng.NextDouble() * 2 * Math.PI;
					position = new Vector2D(Math.Cos(angle), Math.Sin(angle));
				}
				position = correctPosition(position);
				
				foreach(var shipGroup in fleet.LocalFleet.Ships)
				{
					var designStats = mainGame.Derivates.Of(shipGroup.Design.Owner).DesignStats[shipGroup.Design];
					var abilities = designStats.Abilities.Select(x => x.Quantity * (double)shipGroup.Quantity).ToArray();
					var simiralCombatant = this.game.Combatants.FirstOrDefault(x => x.Position == position && x.Ships.Design == shipGroup.Design);
					
					if (simiralCombatant == null)
						this.game.Combatants.Add(new Combatant(position, fleet.OriginalFleet.Owner, shipGroup, designStats, abilities));
					else
					{
						simiralCombatant.Ships.Quantity += shipGroup.Quantity;
						for(int i = 0; i < abilities.Length; i++)
							simiralCombatant.AbilityCharges[i] += abilities[i];
					}
				}
			}
			
			var players = this.game.Combatants.Select(x => x.Owner).Distinct();
			foreach(var unit in this.game.Combatants)
				this.rollCloaking(unit, this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design], players);
		}
		
		private Vector2D correctPosition(Vector2D position)
		{
			var snapped = snapPosition(position);
			
			if (Math.Abs(snapped.Magnitude()) < 1e-3 && position.Magnitude() > 0)
				return Methods.HexNeighbours(new Vector2D(0, 0)).
					Aggregate((a, b) => (a - position).Magnitude() > (b - position).Magnitude() ? a : b);
			
			var corrected = snapped;
			while(Methods.HexDistance(corrected) > SpaceBattleGame.BattlefieldRadius)
			{
				var distance = Methods.HexDistance(corrected);
				corrected = Methods.HexNeighbours(corrected).
					Where(x => Methods.HexDistance(x) < distance).
					Aggregate((a, b) => (a - snapped).Magnitude() > (b - snapped).Magnitude() ? a : b);
			}
							
			return corrected;
		}
		
		private Vector2D snapPosition(Vector2D position)
		{
			var x = Math.Round(position.X, MidpointRounding.AwayFromZero);
			var yOffset = Math.Abs((int)x) % 2 == 0 ? 0 : -0.5;
			
			return new Vector2D(
				x,
				Math.Round(position.Y + yOffset, MidpointRounding.AwayFromZero)
			);
		}
		#endregion

		public bool IsOver 
		{
			get { return this.game.Turn >= this.game.TurnLimit; }
		}
		
		private void makeUnitOrder()
		{
			foreach(var unit in this.game.Combatants)
			{
				var stats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design];
				unit.Initiative = this.game.Rng.NextDouble() + stats.CombatSpeed;
			}
			
			var units = new List<Combatant>(this.game.Combatants);
			units.Sort((a, b) => -a.Initiative.CompareTo(b.Initiative));
			
			foreach(var unit in units)
				this.game.PlayOrder.Enqueue(unit);
		}
		
		private void nextRound()
		{
			this.game.Turn++;
			this.game.Combatants.RemoveAll(x => x.Ships.Quantity <= 0);
			var players = this.game.Combatants.Select(x => x.Owner).Distinct();
			
			foreach(var unit in this.game.Combatants)
			{
				var stats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design];
				
				unit.MovementPoints += 1;
				unit.MovementPoints = Math.Min(unit.MovementPoints, 1);
				
				unit.ShieldPoints += stats.ShieldRegeneration;
				unit.ShieldPoints = Math.Min(unit.ShieldPoints, stats.ShieldPoints);
				
				for(int i = 0; i < unit.AbilityCharges.Length; i++)
					unit.AbilityCharges[i] = stats.Abilities[i].Quantity * (double)unit.Ships.Quantity;
				
				this.rollCloaking(unit, stats, players);
			}
			
			foreach(var planet in this.game.Planets.Where(x => x.Colony != null && x.Colony.Population < 1))
			{
				this.mainGame.States.Colonies.Remove(planet.Colony);
				this.mainGame.Derivates.Colonies.Remove(this.mainGame.Derivates.Of(planet.Colony));
				planet.Colony = null;
				 //TODO(later) if stellaris should be removed too
			}
			
			this.makeUnitOrder();
		}
		
		private double sensorStrength(Vector2D position, Player owner)
		{
			var designStats = this.mainGame.Derivates.Of(owner).DesignStats;
			var rangePenalty = this.mainGame.Statics.ShipFormulas.SensorRangePenalty;
			
			return this.game.Combatants.Where(x => x.Owner == owner).Max(
				x => 
				{
					var distance = Methods.HexDistance(x.Position, position);
					return designStats[x.Ships.Design].Detection + distance * rangePenalty;
				}
			);
		}
		
		private void rollCloaking(Combatant unit, DesignStats stats, IEnumerable<Player> players)
		{
			unit.CloakedFor.Clear();
			foreach(var player in players.Where(x => x != unit.Owner))
		        if (Probability(stats.Cloaking - sensorStrength(unit.Position, player)) > this.game.Rng.NextDouble())
					unit.CloakedFor.Add(player);
		}
		
		#region Unit actions
		public void MoveTo(Vector2D destination)
		{
			if (!this.ValidMoves(this.game.PlayOrder.Peek()).Contains(destination))
				return;
			
			var unit = this.game.PlayOrder.Peek();
			
			if (Methods.HexDistance(destination) <= SpaceBattleGame.BattlefieldRadius)
			{
				unit.Position = destination;
				unit.MovementPoints -= 1 / mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].CombatSpeed;
			}
			else
			{
				this.game.Retreated.Add(unit);
				this.game.Combatants.Remove(unit);
				this.UnitDone();
			}
		}
		
		public void UnitDone()
		{
			this.game.PlayOrder.Dequeue();
			
			if (this.game.PlayOrder.Count == 0)
				this.nextRound();
		}

		public void UseAbility(int index, double quantity, Combatant target)
		{
			var unit = this.game.PlayOrder.Peek();
			var abilityStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities[index];
			var chargesLeft = quantity;
			
			if (!abilityStats.TargetShips || Methods.HexDistance(target.Position, unit.Position) > abilityStats.Range)
				return;
			
			if (abilityStats.IsInstantDamage)
				chargesLeft = this.doDirectAttack(unit, abilityStats, quantity, target);
			
			unit.AbilityCharges[index] = chargesLeft;
		}
		
		public void UseAbility(int index, double quantity, CombatPlanet planet)
		{
			var unit = this.game.PlayOrder.Peek();
			var abilityStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities[index];
			var chargesLeft = quantity;
			
			if (!abilityStats.TargetColony || Methods.HexDistance(planet.Position, unit.Position) > abilityStats.Range)
				return;
			
			if (abilityStats.IsInstantDamage && planet.Colony != null)
			{
				var killsPerShot = abilityStats.FirePower / planet.PopulationHitPoints;
				var casualties = Math.Min(quantity * killsPerShot, planet.Colony.Population);
				//TODO(later) factor in shields and armor
				//TODO(later) roll for target, building or population
				
				planet.Colony.Population -= casualties;
				chargesLeft -= Math.Ceiling(casualties / killsPerShot);
			}
			
			unit.AbilityCharges[index] = chargesLeft;
		}
		
		public void UseAbility(int index, double quantity, StarData star)
		{
			var unit = this.game.PlayOrder.Peek();
			var abilityStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities[index];
			var chargesLeft = quantity;
			
			if (!abilityStats.TargetStar || Methods.HexDistance(unit.Position) > abilityStats.Range)
				return;
			
			if (abilityStats.IsInstantDamage)
			{
				if (star.Traits.All(x => x.Type.IdCode != abilityStats.AppliesTrait.IdCode))
					star.Traits.Add(abilityStats.AppliesTrait.Instantiate(star));
				
				chargesLeft -= quantity;
			}
			
			unit.AbilityCharges[index] = chargesLeft;
		}
		#endregion
		
		public IEnumerable<Vector2D> ValidMoves(Combatant unit)
		{
			return unit.MovementPoints <= 0 ? 
				new Vector2D[0] : 
				neighborPositions(unit.Position);
		}
		
		#region Damage dealing
		private double attackTop(Combatant attacker, AbilityStats abilityStats, double quantity, Combatant target, DesignStats targetStats)
		{
			var distance = Methods.HexDistance(attacker.Position, target.Position);
			var detection = sensorStrength(target.Position, attacker.Owner);
			var targetStealth = targetStats.Jamming + (target.CloakedFor.Contains(attacker.Owner) ? this.mainGame.Statics.ShipFormulas.NaturalCloakBonus : 0);
				
			while (target.HitPoints > 0 && quantity > 0)
			{
				quantity--;
				
				
				if (targetStealth > detection && Math.Pow(sigmoidBase, targetStealth - detection) > this.game.Rng.NextDouble())
					continue;
				
				if (Probability(abilityStats.Accuracy - targetStats.Evasion + distance * abilityStats.AccuracyRangePenalty) < this.game.Rng.NextDouble())
					continue;
				
				double firePower = abilityStats.FirePower;
				
				if (target.ShieldPoints > 0)
				{
					double shieldFire = firePower - Reduce(firePower, targetStats.ShieldThickness, abilityStats.ShieldEfficiency);
					double shieldDamage = Reduce(shieldFire, targetStats.ShieldReduction, 1);
					firePower -= shieldFire - shieldDamage; //damage reduction difference
					
					shieldDamage = Math.Min(shieldDamage, target.ShieldPoints);
					target.ShieldPoints -= shieldDamage;
					firePower -= shieldDamage;
				}
				
				double armorDamage = Reduce(firePower, targetStats.ArmorReduction, abilityStats.ArmorEfficiency);
				target.HitPoints -= armorDamage;
			}
			
			if (target.HitPoints <= 0)
			{
				target.Ships.Quantity--;
				target.HitPoints = targetStats.HitPoints;
				target.ShieldPoints = targetStats.ShieldPoints;
			}
			
			return Math.Max(quantity, 0);
		}
		
		private double doDirectAttack(Combatant attacker, AbilityStats abilityStats, double quantity, Combatant target)
		{
			var targetStats = this.mainGame.Derivates.Of(target.Owner).DesignStats[target.Ships.Design];
			
			while(quantity > 0)
				quantity = attackTop(attacker, abilityStats, quantity, target, targetStats);
			
			//TODO(later) do different calculation for multiple ships below top of the stack
			return quantity;
		}
		#endregion
		
		#region Math helpers
		private static IEnumerable<Vector2D> neighborPositions(Vector2D position)
		{
			return Methods.HexNeighbours(position);
		}
		
		private static double Probability(double modifer)
		{
			if (modifer < 0)
				return 0.5 * Math.Pow(sigmoidBase, -modifer);
			else
				return 1 - 0.5 * Math.Pow(sigmoidBase, modifer);
		}
		
		private static double Reduce(double attack, double defense, double defenseEffect)
		{
			if (attack <= 0 || double.IsPositiveInfinity(defense))
				return 0;

			return attack * Math.Pow(2, -defenseEffect * defense / attack);
		}
		#endregion
	}
}
