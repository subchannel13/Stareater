using System;
using Stareater.SpaceCombat;

namespace Stareater.GameLogic
{
	class BombardmentProcessor : ACombatProcessor
	{
		public BombardmentProcessor(SpaceBattleGame battleGame, MainGame mainGame) : base(battleGame, mainGame)
		{ }
		
		public void Bombard(CombatPlanet planet)
		{
			foreach(var unit in this.game.Combatants)
				for(int i = 0; i < unit.AbilityCharges.Length; i++)
					this.UseAbility(i, unit.AbilityCharges[i], planet);
			
			//TODO(v0.6) end bombard turn
		}
	}
}
