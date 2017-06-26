using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Combat;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.GameLogic;
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
		
		private SpaceBattleProcessor processor = null;
		
		internal SpaceBattleController(Conflict conflict, GameController gameController, MainGame mainGame)
		{
			this.playerListeners = new Dictionary<Player, IBattleEventListener>();
			
			this.battleGame = new SpaceBattleGame(conflict.Location, mainGame);
			this.mainGame = mainGame;
			this.gameController = gameController;
			this.Star = mainGame.States.Stars.At[battleGame.Location];
			
			this.processor = new SpaceBattleProcessor(this.battleGame, mainGame);
			this.processor.Initialize(conflict.Fleets, conflict.StartTime);
		}
	
		#region Battle information
		public static readonly int BattlefieldRadius = SpaceBattleGame.BattlefieldRadius;
		
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
		
		public IEnumerable<CombatantInfo> Units
		{
			get 
			{ 
				return this.battleGame.Combatants.Select(x => new CombatantInfo(x, mainGame, this.processor.ValidMoves(x)));
			}
		}
		#endregion

		#region Unit actions
		public void MoveTo(Vector2D destination)
		{
			this.processor.MoveTo(destination);
			this.checkNextUnit();
		}
		
		public void UnitDone()
		{
			this.processor.UnitDone();
			this.checkNextUnit();
		}
		
		public void UseAbility(AbilityInfo ability, CombatantInfo target)
		{
			this.processor.UseAbility(ability.Index, ability.Quantity, target.Data);
			this.checkNextUnit();
		}
		
		public void UseAbility(AbilityInfo ability, CombatPlanetInfo planet)
		{
			this.processor.UseAbility(ability.Index, ability.Quantity, planet.Data);
			this.checkNextUnit();
		}
		
		public void UseAbilityOnStar(AbilityInfo ability)
		{
			this.processor.UseAbility(ability.Index, ability.Quantity, this.Star);
			this.checkNextUnit();
		}
		#endregion
		
		#region Unit order and battle event management
		internal IEnumerable<Player> Participants
		{
			get 
			{
				return this.battleGame.Combatants.Select(x => x.Owner).Concat(
					this.battleGame.Planets.Where(x => x.Colony != null).Select(x => x.Colony.Owner)
				).Distinct();
			}
		}
		
		internal void Register(PlayerController player, IBattleEventListener eventListener)
		{
			playerListeners.Add(player.PlayerInstance(this.mainGame), eventListener);
		}
		
		internal void Start()
		{
			this.checkNextUnit(); //TODO(0.7) AI vs AI could cause stack overflow
		}

		//TODO(v0.7) callers can cause stack overflow if they perform multiple unit action in a single function
		private void checkNextUnit()
		{
			if (this.processor.IsOver)
				this.gameController.SpaceCombatResolved(this.battleGame, this.processor.CanBombard);
			else
				this.playNexUnit();
		}
		
		private void playNexUnit()
		{
			var currentUnit = this.battleGame.PlayOrder.Peek();
			playerListeners[currentUnit.Owner].PlayUnit(new CombatantInfo(
				currentUnit, 
				mainGame, 
				this.processor.ValidMoves(currentUnit)
			));
		}
		#endregion
	}
}
