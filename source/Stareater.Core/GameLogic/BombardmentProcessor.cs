using System;
using System.Linq;
using Stareater.SpaceCombat;

namespace Stareater.GameLogic
{
	class BombardmentProcessor : ACombatProcessor
	{
		public BombardmentProcessor(SpaceBattleGame battleGame, MainGame mainGame) : base(battleGame, mainGame)
		{ }
		
		public bool IsOver 
		{
			get 
			{ 
				return !this.CanBombard;
			}
		}
		
		public void Bombard(CombatPlanet planet)
		{
			foreach(var unit in this.game.Combatants)
			{
				//TODO(v0.7) hack, maybe move range check out of UseAbility
				unit.Position = planet.Position;
				
				for(int i = 0; i < unit.AbilityCharges.Length; i++)
					this.UseAbility(i, unit.AbilityCharges[i], planet);
			}
			
			this.nextRound();
		}
	}
}
