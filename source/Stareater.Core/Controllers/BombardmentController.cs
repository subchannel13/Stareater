using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
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
			this.playerListeners.Values.First().BombardTurn();
		}

		public void Leave()
		{
			gameController.BombardmentResolved(this.battleGame);
		}

		public IEnumerable<CombatPlanetInfo> Planets 
		{
			get
			{
				//TODO(v0.6)
				return new CombatPlanetInfo[0];
			}
		}
	}
}
