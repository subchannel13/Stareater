using System;
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
	}
}
