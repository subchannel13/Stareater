using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views.Combat;
using Stareater.Galaxy;

namespace Stareater.Controllers.Views
{
	public class SpaceBattleController
	{
		private readonly SpaceBattleGame battleGame;
		
		internal SpaceBattleController(SpaceBattleGame battleGame, MainGame mainGame)
		{
			this.battleGame = battleGame;
			this.Star = mainGame.States.Stars.At(battleGame.Location);
		}
	
		public static readonly int BattlefieldRadius = SpaceBattleGame.BattlefieldRadius;
		
		public StarData Star { get; private set; }
		
		public IEnumerable<CombatantInfo> Units
		{
			get { return this.battleGame.Combatants.Select(x => new CombatantInfo(x)); }
		}
	}
}
