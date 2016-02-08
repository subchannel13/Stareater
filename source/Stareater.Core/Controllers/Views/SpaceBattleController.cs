using System;

namespace Stareater.Controllers.Views
{
	public class SpaceBattleController
	{
		private SpaceBattleGame battleGame;
		
		internal SpaceBattleController(SpaceBattleGame battleGame)
		{
			this.battleGame = battleGame;
		}
	}
}
