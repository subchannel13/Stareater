using System;

namespace Stareater.Controllers.Views
{
	public class SpaceBattleController
	{
		private readonly SpaceBattleGame battleGame;
		
		internal SpaceBattleController(SpaceBattleGame battleGame)
		{
			this.battleGame = battleGame;
		}
		
		public static readonly int BattlefieldRadius = 4;
	}
}
