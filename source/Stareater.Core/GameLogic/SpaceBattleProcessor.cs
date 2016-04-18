using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.GameData.Databases.Tables;
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
			
			this.game.TurnLimit = (int)Math.Ceiling(50 * (1 - startTime));
		}

		private void initBodies()
		{
			double maxPlanets = this.mainGame.States.Planets.Max(x => x.Position);
			var star = mainGame.States.Stars.At(game.Location);
			var planets = this.mainGame.States.Planets.At(star);
			var colonies = this.mainGame.States.Colonies.AtStar(star);
			
			for(int i = 0; i < planets.Count; i++)
			{
				var distance = Methods.Lerp(planets[i].Position / maxPlanets, 1, SpaceBattleGame.BattlefieldRadius);
				var angle = game.Rng.NextDouble() * 2 * Math.PI;
				
				this.game.Planets[i] = new CombatPlanet(
					colonies.FirstOrDefault(x => x.Location.Planet == planets[i]),
					snapPosition(correctPosition(new Vector2D(Math.Cos(angle), Math.Sin(angle)) * distance)),
					this.mainGame.Statics.ColonyFormulas.PopulationHitPoints.Evaluate(null) //TODO(later) pass relevant variables
				);
				//TODO(v0.5) try to make unique positions
			}
		}
		
		private void initUnits(IEnumerable<FleetMovement> fleets)
		{
			foreach(var fleet in fleets)
			{
				Vector2D position;
				if (fleet.MovementDirection.Magnitude() > 0)
					position = -fleet.MovementDirection * SpaceBattleGame.BattlefieldRadius;
				else
				{
					var angle = game.Rng.NextDouble() * 2 * Math.PI;
					position = new Vector2D(Math.Cos(angle), Math.Sin(angle));
				}
				position = snapPosition(correctPosition(position));
				
				foreach(var shipGroup in fleet.LocalFleet.Ships)
				{
					var designStats = mainGame.Derivates.Of(shipGroup.Design.Owner).DesignStats[shipGroup.Design];
					var abilities = designStats.Abilities.Select(x => x.Quantity * (double)shipGroup.Quantity);
					this.game.Combatants.Add(new Combatant(position, fleet.OriginalFleet.Owner, shipGroup, designStats, abilities.ToArray()));
				}
			}
		}
		
		private Vector2D correctPosition(Vector2D position)
		{
			var snapped = snapPosition(position);
			double yHeight = (SpaceBattleGame.BattlefieldRadius * 2 - Math.Abs(snapped.X));
			
			if (Math.Abs(snapped.Magnitude()) < 1e-3 && position.Magnitude() > 0)
				return correctPosition(position + position * 0.5 / position.Magnitude());
			else if (!Methods.InsideHexGrid(snapped, SpaceBattleGame.BattlefieldRadius))
				return correctPosition(position - position * 0.5 / position.Magnitude());
				
			return position;
		}
		
		private Vector2D snapPosition(Vector2D position)
		{
			return new Vector2D(
				Math.Round(position.X, MidpointRounding.AwayFromZero),
				Math.Round(position.Y - 0.5, MidpointRounding.AwayFromZero)
			);
		}
		#endregion

		public bool IsOver 
		{
			get { return this.game.Turn >= this.game.TurnLimit; }
		}
		
		public void MakeUnitOrder()
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

		#region Unit actions
		public void MoveTo(Vector2D destination)
		{
			if (!this.ValidMoves(this.game.PlayOrder.Peek()).Contains(destination))
				return;
			
			var unit = this.game.PlayOrder.Peek();
			
			if (Methods.InsideHexGrid(destination, SpaceBattleGame.BattlefieldRadius))
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
			{
				this.game.Turn++;
				this.game.Combatants.RemoveAll(x => x.Ships.Quantity <= 0);
				
				foreach(var unit in this.game.Combatants)
				{
					var stats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design];
					
					unit.MovementPoints += 1;
					unit.MovementPoints = Math.Min(unit.MovementPoints, 1);
					
					unit.ShieldPoints += stats.ShieldRegeneration;
					unit.ShieldPoints = Math.Min(unit.ShieldPoints, stats.ShieldPoints);
					
					for(int i = 0; i < unit.AbilityCharges.Length; i++)
						unit.AbilityCharges[i] = stats.Abilities[i].Quantity * (double)unit.Ships.Quantity;
				}
				
				foreach(var planet in this.game.Planets.Where(x => x.Colony != null && x.Colony.Population < 1))
				{
					this.mainGame.States.Colonies.Remove(planet.Colony); //TODO(v0.5) test and see if more stuff has to be updated
					this.mainGame.Derivates.Colonies.Remove(this.mainGame.Derivates.Of(planet.Colony));
					planet.Colony = null;
				}
				
				this.MakeUnitOrder();
			}
		}

		public void UseAbility(int index, double quantity, Combatant target)
		{
			var unit = this.game.PlayOrder.Peek();
			var abilityStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities[index];
			var chargesLeft = quantity;
			
			if (!Methods.InsideHexGrid(target.Position - unit.Position, abilityStats.Range))
				return;
			
			if (abilityStats.IsInstantDamage)
				chargesLeft = this.doDirectAttack(abilityStats, quantity, target);
			
			unit.AbilityCharges[index] = chargesLeft;
		}
		
		public void UseAbility(int index, double quantity, CombatPlanet planet)
		{
			var unit = this.game.PlayOrder.Peek();
			var abilityStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities[index];
			var chargesLeft = quantity;
			
			if (!Methods.InsideHexGrid(planet.Position - unit.Position, abilityStats.Range))
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
		#endregion
		
		public IEnumerable<Vector2D> ValidMoves(Combatant unit)
		{
			if (unit.MovementPoints <= 0)
				yield break;
			
			var yOffset = (int)Math.Abs(unit.Position.X) % 2 == 0 ? 0 : 1;
				
			yield return unit.Position + new Vector2D(0, 1);
			yield return unit.Position + new Vector2D(1, 0 + yOffset);
			yield return unit.Position + new Vector2D(1, -1 + yOffset);
			yield return unit.Position + new Vector2D(0, -1);
			yield return unit.Position + new Vector2D(-1, -1 + yOffset);
			yield return unit.Position + new Vector2D(-1, 0 + yOffset);
		}

		#region Damage dealing
		double attackTop(AbilityStats abilityStats, double quantity, Combatant target, DesignStats targetStats)
		{
			while (target.HitPoints > 0 && quantity > 0)
			{
				quantity--;
				
				//TODO(v0.5) factor in stealth, sensors and distance
				if (Probability(abilityStats.Accuracy - targetStats.Evasion) < this.game.Rng.NextDouble())
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
		
		private double doDirectAttack(AbilityStats abilityStats, double quantity, Combatant target)
		{
			var targetStats = this.mainGame.Derivates.Of(target.Owner).DesignStats[target.Ships.Design];
			
			while(quantity > 0)
				quantity = attackTop(abilityStats, quantity, target, targetStats);
			
			//TODO(later) do different calculation for multiple ships below top of the stack
			return quantity;
		}
		#endregion
		
		#region Math helpers
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
