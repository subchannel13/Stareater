using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;
using System.Collections.Concurrent;

namespace Stareater.Controllers
{
	public class BombardmentController
	{
		private readonly BombardBattleGame battleGame;
		private readonly MainGame mainGame;
		private readonly GameController gameController;
		private readonly Dictionary<Player, IBombardEventListener> playerListeners;

		private BlockingCollection<Action> messageQueue = new BlockingCollection<Action>(1);
		private BombardmentProcessor processor = null;
		
		internal BombardmentController(BombardBattleGame battleGame, MainGame mainGame, GameController gameController)
		{
			this.playerListeners = new Dictionary<Player, IBombardEventListener>();
			
			this.battleGame = battleGame;
			this.mainGame = mainGame;
			this.gameController = gameController;
			
			this.Star = mainGame.States.Stars.At[battleGame.Location];
			this.processor = new BombardmentProcessor(battleGame, mainGame);
		}
		
		internal void Register(PlayerController player, IBombardEventListener eventListener)
		{
			playerListeners.Add(player.PlayerInstance(this.mainGame), eventListener);
		}
		
		internal void Start()
		{
			this.checkNextPlayer();

			while (!this.processor.IsOver)
			{
				var message = this.messageQueue.Take();
				message();
				this.checkNextPlayer();
			}
		}

		internal IEnumerable<Player> Participants
		{
			get
			{
				return this.processor.Participants;
			}
		}

		private void checkNextPlayer()
		{
			if (!this.processor.IsOver)
				playerListeners[this.battleGame.PlayOrder.Peek()].BombardTurn();
			else
				gameController.BombardmentResolved(this.battleGame);
		}

		public void Bombard(int planetPosition)
		{
			this.messageQueue.Add(() => this.processor.Bombard(this.battleGame.Planets.First(x => x.PlanetData.Position == planetPosition)));
		}
		
		public void Leave()
		{
			//TODO(check) remove current player instead of ending whole phase
			gameController.BombardmentResolved(this.battleGame);
		}

		public StarData Star { get; private set; }
		
		public IEnumerable<CombatPlanetInfo> Planets 
		{
			get
			{
				return this.battleGame.Planets.
					Where(x => x.Colony != null).
					Select(x => new CombatPlanetInfo(x));
			}
		}

		public IEnumerable<CombatPlanetInfo> Targets
		{
			get
			{
				var player = this.battleGame.PlayOrder.Peek();

				return this.battleGame.Planets.
					Where(x => x.Colony != null && this.mainGame.Processor.IsAtWar(player, x.Colony.Owner)).
					Select(x => new CombatPlanetInfo(x));
			}
		}
	}
}
