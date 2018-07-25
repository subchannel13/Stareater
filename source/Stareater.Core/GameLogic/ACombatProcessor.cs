using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Players;
using Stareater.SpaceCombat;
using Stareater.Utils;
using Stareater.GameLogic.Combat;

namespace Stareater.GameLogic
{
	abstract class ACombatProcessor
	{
		protected readonly MainGame mainGame;
		
		protected ACombatProcessor(MainGame mainGame)
		{
			this.mainGame = mainGame;
		}

		protected abstract ABattleGame battleGame { get; }
		
		public bool CanBombard
		{
			get
			{
				if (this.battleGame.Turn >= this.battleGame.TurnLimit)
					return false;

				var colonies = this.battleGame.Planets.Where(x => x.Colony != null).ToList();

				return this.battleGame.Combatants.Where(
					ship => colonies.Any(planet => this.mainGame.Processor.IsAtWar(ship.Owner, planet.Colony.Owner))
				).Any(unit => unitCanBombard(unit));
			}
		}

		/*TODO(later) add methods for attacking other kind of targets, 
		 * extrude functionality form UseAbiliy methods in SpaceBattleProcessor */
		protected double attackPlanet(AbilityStats abilityStats, double quantity, CombatPlanet planet)
		{
			var spent = 0.0;

			if (abilityStats.IsInstantDamage && planet.Colony != null)
			{
				var killsPerShot = abilityStats.FirePower / planet.PopulationHitPoints;
				var casualties = Math.Min(quantity * killsPerShot, planet.Colony.Population);
				//TODO(later) factor in shields and armor
				//TODO(later) roll for target, building or population

				planet.Colony.Population -= casualties;
				spent = Math.Ceiling(casualties / killsPerShot);
			}

			return spent;
		}

		protected void calculateInitiative()
		{
			foreach(var unit in this.battleGame.Combatants)
			{
				var stats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design];
				unit.Initiative = this.battleGame.Rng.NextDouble() + stats.CombatSpeed;
			}
		}
		
		protected virtual void nextRound()
		{
			this.battleGame.Turn++;
			this.battleGame.Combatants.RemoveAll(x => x.Ships.Quantity <= 0);
			var players = this.battleGame.Combatants.Select(x => x.Owner).Distinct();
			
			foreach(var unit in this.battleGame.Combatants)
			{
				var stats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design];
				
				unit.MovementPoints += 1;
				unit.MovementPoints = Math.Min(unit.MovementPoints, 1);

				var restCount = unit.Ships.Quantity - 1;
				unit.TopShields = Math.Min(unit.TopShields + stats.ShieldRegeneration, stats.ShieldPoints);
				unit.RestShields = Math.Min(unit.RestShields + stats.ShieldRegeneration * restCount, stats.ShieldPoints * restCount);

				for (int i = 0; i < unit.AbilityCharges.Length; i++)
				{
					unit.AbilityCharges[i] = stats.Abilities[i].Quantity * (double)unit.Ships.Quantity;
					if (!double.IsInfinity(stats.Abilities[i].Ammo))
						unit.AbilityCharges[i] = Math.Min(unit.AbilityCharges[i], unit.AbilityAmmo[i]);
				}
				
				this.rollCloaking(unit, stats, players);
			}
			
			foreach(var planet in this.battleGame.Planets.Where(x => x.Colony != null && x.Colony.Population < 1))
			{
				this.mainGame.States.Colonies.Remove(planet.Colony);
				this.mainGame.Derivates.Colonies.Remove(this.mainGame.Derivates.Of(planet.Colony));
				this.mainGame.Orders[planet.Colony.Owner].ConstructionPlans.Remove(planet.Colony);
				planet.Colony = null;
				//TODO(v0.8) check if stellaris should be removed too
			}
		}
		
		protected double sensorStrength(Vector2D position, Player owner)
		{
			var designStats = this.mainGame.Derivates.Of(owner).DesignStats;
			var rangePenalty = this.mainGame.Statics.ShipFormulas.SensorRangePenalty;
			
			return this.battleGame.Combatants.Where(x => x.Owner == owner).Max(
				x => 
				{
					var distance = Methods.HexDistance(x.Position, position);
					return designStats[x.Ships.Design].Detection + distance * rangePenalty;
				}
			);
		}
		
		protected void rollCloaking(Combatant unit, DesignStats stats, IEnumerable<Player> players)
		{
			unit.CloakedFor.Clear();
			foreach(var player in players.Where(x => x != unit.Owner))
		        if (Probability(stats.Cloaking - sensorStrength(unit.Position, player)) > this.battleGame.Rng.NextDouble())
					unit.CloakedFor.Add(player);
		}
		
		//TODO(v0.8) maybe include colony owner check
		protected bool unitCanBombard(Combatant unit)
		{
			var statList = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities;
			for (int i = 0; i < statList.Count; i++)
				if (statList[i].TargetColony && (double.IsInfinity(statList[i].Ammo) || unit.AbilityAmmo[i] >= 1))
					return true;

			return false;
		}

		#region Math helpers
		protected const double sigmoidBase = 0.90483741803595957316424905944644; //e^-0.1
		
		protected static double Probability(double modifer)
		{
			if (modifer < 0)
				return 0.5 * Math.Pow(sigmoidBase, -modifer);
			else
				return 1 - 0.5 * Math.Pow(sigmoidBase, modifer);
		}
		
		protected static double Reduce(double attack, double defense, double defenseEffect)
		{
			if (attack <= 0 || double.IsPositiveInfinity(defense))
				return 0;

			return attack * Math.Pow(2, -defenseEffect * defense / attack);
		}
		#endregion
	}
}
