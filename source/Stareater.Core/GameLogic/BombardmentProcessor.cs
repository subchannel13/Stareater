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
				if (this.game.Turn >= this.game.TurnLimit)
					return true;
				
				//TODO(v0.6) check only hostile colonies
				var colonies = this.game.Planets.Where(x => x.Colony != null);
				
				//TODO(v0.6) doesn't check war declarations
				//TODO(v0.6) check if attacker has any bombs left
				return !colonies.Any();
			}
		}
		
		public void Bombard(CombatPlanet planet)
		{
			foreach(var unit in this.game.Combatants)
			{
				//TODO(v0.6) hack, maybe move range check out of UseAbility
				unit.Position = planet.Position;
				
				for(int i = 0; i < unit.AbilityCharges.Length; i++)
					this.UseAbility(i, unit.AbilityCharges[i], planet);
			}
			
			this.nextRound();
		}
	}
}
