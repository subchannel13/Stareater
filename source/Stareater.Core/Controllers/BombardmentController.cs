using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class BombardmentController
	{
		private readonly BombardBattleGame battleGame;
		private readonly MainGame mainGame;
		private readonly GameController gameController;
		private readonly Dictionary<Player, IBombardEventListener> playerListeners;
		
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
			this.checkNextPlayer(); //TODO(0.7) AI vs AI could cause stack overflow
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
			this.processor.Bombard(this.battleGame.Planets.First(x => x.PlanetData.Position == planetPosition));

			this.checkNextPlayer();
		}
		
		public void Leave()
		{
			gameController.BombardmentResolved(this.battleGame);
		}

		public StarData Star { get; private set; }
		
		public IEnumerable<CombatPlanetInfo> Planets 
		{
			get
			{
				var planets = this.mainGame.States.Planets.At[this.Star];
				
				for(int i = 0; i < this.battleGame.Planets.Length; i++)
					yield return new CombatPlanetInfo(this.battleGame.Planets[i]);
			}
		}
	}
}
