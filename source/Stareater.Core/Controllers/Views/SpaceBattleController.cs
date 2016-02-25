using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views.Combat;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class SpaceBattleController
	{
		private readonly SpaceBattleGame battleGame;
		private readonly MainGame mainGame;
		private readonly PlayerController[] players;
		private readonly Dictionary<Player, IBattleEventListener> playerListeners;
		
		internal SpaceBattleController(SpaceBattleGame battleGame, MainGame mainGame, PlayerController[] players)
		{
			this.playerListeners = new Dictionary<Player, IBattleEventListener>();
			
			this.battleGame = battleGame;
			this.mainGame = mainGame;
			this.Star = mainGame.States.Stars.At(battleGame.Location);
			this.players = players;
		}
	
		public static readonly int BattlefieldRadius = SpaceBattleGame.BattlefieldRadius;
		
		public StarData Star { get; private set; }
		
		public IEnumerable<CombatantInfo> Units
		{
			get { return this.battleGame.Combatants.Select(x => new CombatantInfo(x, mainGame)); }
		}

		public void Register(PlayerController player, IBattleEventListener eventListener)
		{
			playerListeners.Add(mainGame.Players[player.PlayerIndex], eventListener);
		}
		
		internal void Start()
		{
			this.battleGame.Processor.MakeUnitOrder();
			this.playNexUnit();
		}

		void playNexUnit()
		{
			var currentUnit = battleGame.PlayOrder.Peek();
			playerListeners[currentUnit.Owner].PlayUnit(new CombatantInfo(currentUnit, mainGame));
		}
	}
}
