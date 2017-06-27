using Stareater.Players;
using System.Collections.Generic;

namespace Stareater
{
	class BombardBattleGame : ABattleGame
	{
		public Queue<Player> PlayOrder { get; private set; }

		public BombardBattleGame(ABattleGame battleGame) : 
			base(
				battleGame.Rng, battleGame.Location, battleGame.TurnLimit,
				battleGame.Combatants, battleGame.Planets, battleGame.Retreated,
				battleGame.Turn
			)
		{
			this.PlayOrder = new Queue<Player>();
		}
	}
}
