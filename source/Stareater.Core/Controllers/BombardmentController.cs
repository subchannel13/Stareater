using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
using Stareater.Galaxy;
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
			
			this.Star = mainGame.States.Stars.At[battleGame.Location];
		}
		
		internal void Register(PlayerController player, IBombardEventListener eventListener)
		{
			playerListeners.Add(player.PlayerInstance(this.mainGame), eventListener);
		}
		
		internal void Start()
		{
			//TODO(v0.6) pick random player
			this.playerListeners.Values.First().BombardTurn();
		}

		public void Bombard(int planetPosition)
		{
			this.battleGame.Processor.Bombard(this.battleGame.Planets.First(x => x.PlanetData.Position == planetPosition));
			//TODO(v0.6) rotate players
			this.playerListeners.Values.First().BombardTurn();
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
