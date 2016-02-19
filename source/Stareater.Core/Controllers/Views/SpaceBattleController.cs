using System;
using Stareater.Galaxy;

namespace Stareater.Controllers.Views
{
	public class SpaceBattleController
	{
		private readonly SpaceBattleGame battleGame;
		
		internal SpaceBattleController(SpaceBattleGame battleGame, MainGame mainGame)
		{
			this.battleGame = battleGame;
			this.Star = mainGame.States.Stars.At(battleGame.Position);
		}
		
		public static readonly int BattlefieldRadius = 4;
		public StarData Star { get; private set; }
	}
}
