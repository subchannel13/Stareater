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
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Stareater.GameLogic.Combat;

namespace Stareater.Controllers
{
	public class SpaceBattleController
	{
		private readonly SpaceBattleGame battleGame;
		private readonly MainGame mainGame;
		private readonly GameController gameController;
		private readonly Dictionary<Player, IBattleEventListener> playerListeners;
		private readonly StarData star;

		private BlockingCollection<Action> messageQueue = new BlockingCollection<Action>(1);
		private SpaceBattleProcessor processor = null;
		
		internal SpaceBattleController(Conflict conflict, GameController gameController, MainGame mainGame)
		{
			this.playerListeners = new Dictionary<Player, IBattleEventListener>();
			
			this.battleGame = new SpaceBattleGame(conflict.Location, SpaceBattleProcessor.ConflictDuration(conflict.StartTime), mainGame);
			this.mainGame = mainGame;
			this.gameController = gameController;
			this.star = mainGame.States.Stars.At[battleGame.Location];
			
			this.processor = new SpaceBattleProcessor(this.battleGame, mainGame);
			this.processor.Initialize(conflict.Fleets);
		}
	
		#region Battle information
		public static readonly int BattlefieldRadius = SpaceBattleGame.BattlefieldRadius;
		
		public StarInfo HostStar
		{
			get { return new StarInfo(this.star); }
		}
		
		public IEnumerable<CombatPlanetInfo> Planets
		{
			get 
			{
				var planets = this.mainGame.States.Planets.At[this.star];
				
				for(int i = 0; i < this.battleGame.Planets.Length; i++)
					yield return new CombatPlanetInfo(this.battleGame.Planets[i]);
			}
		}

		public IEnumerable<ProjectileInfo> Projectiles
		{
			get
			{
				return this.battleGame.Projectiles.Select(x => new ProjectileInfo(x));
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
			this.messageQueue.Add(() => this.processor.MoveTo(destination));
		}
		
		public void UnitDone()
		{
			this.messageQueue.Add(() => this.processor.UnitDone());
		}
		
		public void UseAbility(AbilityInfo ability, CombatantInfo target)
		{
			this.messageQueue.Add(() => this.processor.UseAbility(ability.Index, ability.Quantity, target.Data));
		}
		
		public void UseAbility(AbilityInfo ability, CombatPlanetInfo planet)
		{
			this.messageQueue.Add(() => this.processor.UseAbility(ability.Index, ability.Quantity, planet.Data));
		}
		
		public void UseAbilityOnStar(AbilityInfo ability)
		{
			this.messageQueue.Add(() => this.processor.UseAbility(ability.Index, ability.Quantity, this.star));
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
			this.checkNextUnit();

			while (!this.processor.IsOver)
			{
				var message = this.messageQueue.Take();
				message();
				this.checkNextUnit();
			}
		}

		private void checkNextUnit()
		{
			if (this.processor.IsOver)
				this.gameController.SpaceCombatResolved(this.battleGame, this.processor.CanBombard);
			else
				this.playNextUnit();
		}
		
		private void playNextUnit()
		{
			var currentUnit = this.battleGame.PlayOrder.Peek();

			Task.Factory.StartNew(() =>
				playerListeners[currentUnit.Owner].PlayUnit(new CombatantInfo(
					currentUnit, 
					mainGame, 
					this.processor.ValidMoves(currentUnit)
				))
			);
			//TODO(later) check for exception
		}
		#endregion
	}
}
