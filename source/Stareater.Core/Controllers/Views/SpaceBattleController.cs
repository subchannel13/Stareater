using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views.Combat;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class SpaceBattleController
	{
		private readonly SpaceBattleGame battleGame;
		private readonly MainGame mainGame;
		private readonly GameController gameController;
		private readonly PlayerController[] players;
		private readonly Dictionary<Player, IBattleEventListener> playerListeners;
		
		internal SpaceBattleController(SpaceBattleGame battleGame, GameController gameController, MainGame mainGame, PlayerController[] players)
		{
			this.playerListeners = new Dictionary<Player, IBattleEventListener>();
			
			this.battleGame = battleGame;
			this.mainGame = mainGame;
			this.gameController = gameController;
			this.Star = mainGame.States.Stars.At(battleGame.Location);
			this.players = players;
		}
	
		#region Battle information
		public static readonly int BattlefieldRadius = SpaceBattleGame.BattlefieldRadius;
		
		public StarData Star { get; private set; }
		
		public IEnumerable<CombatantInfo> Units
		{
			get 
			{ 
				return this.battleGame.Combatants.Select(x => new CombatantInfo(x, mainGame, this.battleGame.Processor.ValidMoves(x)));
			}
		}
		#endregion

		#region Unit actions
		public void MoveTo(Vector2D destination)
		{
			this.battleGame.Processor.MoveTo(destination);
			this.playNexUnit();
		}
		
		public void UnitDone()
		{
			this.battleGame.Processor.UnitDone();
			
			if (this.battleGame.Processor.IsOver)
				this.gameController.ConflictResolved(this.battleGame);
			else
				this.playNexUnit();
		}
		
		#endregion
		
		#region Unit order and battle event management
		internal void Register(PlayerController player, IBattleEventListener eventListener)
		{
			playerListeners.Add(mainGame.Players[player.PlayerIndex], eventListener);
		}
		
		internal void Start()
		{
			this.battleGame.Processor.MakeUnitOrder();
			this.playNexUnit();
		}

		private void playNexUnit()
		{
			var currentUnit = this.battleGame.PlayOrder.Peek();
			playerListeners[currentUnit.Owner].PlayUnit(new CombatantInfo(
				currentUnit, 
				mainGame, 
				this.battleGame.Processor.ValidMoves(currentUnit)
			));
		}
		#endregion
	}
}
