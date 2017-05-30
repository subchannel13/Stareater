using System;
using System.Collections.Generic;
using Stareater.Controllers.Views;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class BombardmentController
	{
		private readonly SpaceBattleGame battleGame;
		private readonly MainGame mainGame;
		private readonly GameController gameController;
		private readonly Dictionary<Player, IBombardEventListener> playerListeners;
		
		internal BombardmentController(SpaceBattleGame battleGame, MainGame mainGame, GameController gameController)
		{
			this.playerListeners = new Dictionary<Player, IBombardEventListener>();
			
			this.battleGame = battleGame;
			this.mainGame = mainGame;
			this.gameController = gameController;
		}
		
		internal void Register(PlayerController player, IBombardEventListener eventListener)
		{
			playerListeners.Add(player.PlayerInstance(this.mainGame), eventListener);
		}
		
		internal void Start()
		{
			gameController.BombardmentResolved(this.battleGame);
		}

		public void Leave()
		{
			throw new NotImplementedException();
		}
	}
}
