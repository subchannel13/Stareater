using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.SpaceCombat;

namespace Stareater.Controllers
{
	public class SpaceBattleController
	{
		private readonly SpaceBattleGame battleGame;
		private readonly MainGame mainGame;
		private readonly GameController gameController;
		private readonly Dictionary<Player, IBattleEventListener> playerListeners;
		
		internal SpaceBattleController(SpaceBattleGame battleGame, GameController gameController, MainGame mainGame)
		{
			this.playerListeners = new Dictionary<Player, IBattleEventListener>();
			
			this.battleGame = battleGame;
			this.mainGame = mainGame;
			this.gameController = gameController;
			this.Star = mainGame.States.Stars.At[battleGame.Location];
		}
	
		#region Battle information
		public static readonly int BattlefieldRadius = SpaceBattleGame.BattlefieldRadius;
		
		public StarData Star { get; private set; }
		
		public IEnumerable<CombatPlanetInfo> Planets
		{
			get 
			{
				var colonies = this.mainGame.States.Colonies.AtStar[this.Star];
				var planets = this.mainGame.States.Planets.At[this.Star];
				
				for(int i = 0; i < this.battleGame.Planets.Length; i++)
					yield return new CombatPlanetInfo(this.battleGame.Planets[i]);
			}
		}
		
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
			this.checkNextUnit();
		}
		
		public void UnitDone()
		{
			this.battleGame.Processor.UnitDone();
			this.checkNextUnit();
		}
		
		public void UseAbility(AbilityInfo ability, CombatantInfo target)
		{
			this.battleGame.Processor.UseAbility(ability.Index, ability.Quantity, target.Data);
			this.checkNextUnit();
		}
		
		public void UseAbility(AbilityInfo ability, CombatPlanetInfo planet)
		{
			this.battleGame.Processor.UseAbility(ability.Index, ability.Quantity, planet.Data);
			this.checkNextUnit();
		}
		
		public void UseAbilityOnStar(AbilityInfo ability)
		{
			this.battleGame.Processor.UseAbility(ability.Index, ability.Quantity, this.Star);
			this.checkNextUnit();
		}
		#endregion
		
		#region Unit order and battle event management
		internal void Register(PlayerController player, IBattleEventListener eventListener)
		{
			playerListeners.Add(player.PlayerInstance(this.mainGame), eventListener);
		}
		
		internal void Start()
		{
			this.playNexUnit(); //TODO(0.6) AI vs AI could cause stack overflow
		}

		//TODO(v0.6) callers can cause stack overflow if they perform multiple unit action in a single function
		private void checkNextUnit()
		{
			if (this.battleGame.Processor.IsOver)
				this.gameController.SpaceCombatResolved(this.battleGame, true); //TODO(v0.6) check for bombardment
			else
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
