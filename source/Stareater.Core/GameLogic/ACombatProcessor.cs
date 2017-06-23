using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Players;
using Stareater.SpaceCombat;
using Stareater.Utils;

namespace Stareater.GameLogic
{
	class ACombatProcessor
	{
		protected readonly SpaceBattleGame game;
		protected readonly MainGame mainGame;
		
		public ACombatProcessor(SpaceBattleGame game, MainGame mainGame)
		{
			this.game = game;
			this.mainGame = mainGame;
		}
		
		public bool CanBombard
		{
			get
			{
				if (this.game.Turn >= this.game.TurnLimit)
					return false;

				var colonies = this.game.Planets.Where(x => x.Colony != null).ToList();
				var hostileShips = this.game.Combatants.Where(
					ship => colonies.Any(planet => this.mainGame.Processor.IsAtWar(ship.Owner, planet.Colony.Owner))
				);

				return this.game.Combatants.Where(
					ship => colonies.Any(planet => this.mainGame.Processor.IsAtWar(ship.Owner, planet.Colony.Owner))
				).Any(
					unit =>
					{
						var statList = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities;
						for(int i = 0; i < statList.Count; i++)
							if (statList[i].TargetColony && (double.IsInfinity(statList[i].Ammo) || unit.AbilityAmmo[i] >= 1))
								return true;
						
						return false;
					}
				);
			}
		}
		
		public void UseAbility(int index, double quantity, CombatPlanet planet)
		{
			var unit = this.game.PlayOrder.Peek();
			var abilityStats = this.mainGame.Derivates.Of(unit.Owner).DesignStats[unit.Ships.Design].Abilities[index];
			var chargesLeft = quantity;
			var spent = 0.0;
			
			if (!abilityStats.TargetColony || Methods.HexDistance(planet.Position, unit.Position) > abilityStats.Range)
				return;
			
			if (abilityStats.IsInstantDamage && planet.Colony != null)
			{
				var killsPerShot = abilityStats.FirePower / planet.PopulationHitPoints;
				var casualties = Math.Min(quantity * killsPerShot, planet.Colony.Population);
				//TODO(later) factor in shields and armor
				//TODO(later) roll for target, building or population
				
				planet.Colony.Population -= casualties;
				spent = Math.Ceiling(casualties / killsPerShot);
			}
			
			unit.AbilityCharges[index] -= spent;
			if (!double.IsInfinity(unit.AbilityAmmo[index]))
				unit.AbilityAmmo[index] -= spent;
		}
		
		protected void makeUnitOrder()
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
		
		protected void nextRound()
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
				{
					unit.AbilityCharges[i] = stats.Abilities[i].Quantity * (double)unit.Ships.Quantity;
					if (!double.IsInfinity(stats.Abilities[i].Ammo))
						unit.AbilityCharges[i] = Math.Min(unit.AbilityCharges[i], unit.AbilityAmmo[i]);
				}
				
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
		
		protected double sensorStrength(Vector2D position, Player owner)
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
		
		protected void rollCloaking(Combatant unit, DesignStats stats, IEnumerable<Player> players)
		{
			unit.CloakedFor.Clear();
			foreach(var player in players.Where(x => x != unit.Owner))
		        if (Probability(stats.Cloaking - sensorStrength(unit.Position, player)) > this.game.Rng.NextDouble())
					unit.CloakedFor.Add(player);
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
