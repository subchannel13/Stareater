using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stareater.Controllers.Views;
using Stareater.Players;
using Stareater.Players.Natives;
using Stareater.Utils;
using Stareater.Utils.StateEngine;
using Ikadn;
using Stareater.AppData;

namespace Stareater.Controllers
{
	public sealed class GameController
	{
		internal const string ReportContext = "Reports";
		private static StateManager stateManager = null;

		internal static StateManager GetStateManager()
		{
			if (stateManager == null)
				stateManager = new StateManager();

			return stateManager;
		}
		
		private readonly object threadLocker = new object();
		private readonly AutoResetEvent processingSync = new AutoResetEvent(true);
		private MainGame gameObj;
		
		private GameController endTurnCopy = null;
		private IGameStateListener stateListener;
		private Task aiGalaxyPhase = null;
		private Task combatPhase = null;
		private Task processingPhase = null;
		private PlayerController[] playerControllers = null;
		private PlayerController organelleController = null;
		private readonly HashSet<int> endedTurnPlayers = new HashSet<int>();

		public GameController()
		{ 
			this.State = GameState.NoGame;
		}
	
		public void CreateGame(NewGameController controller)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			if (State != GameState.NoGame)
#pragma warning disable CA1303 // Do not pass literals as localized parameters
				throw new InvalidOperationException("Game is already created.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

			var rng = new Random();
			var players = controller.PlayerList.Select(info =>
				new Player(info.Name, info.Color, NewGameController.Resolve(info.Organization, rng), info.ControlType)
			).ToArray();
			var organellePlayer = new Player(
				"no name", 
				System.Drawing.Color.Gray,
				null,
				new PlayerType(PlayerControlType.Neutral, new OrganellePlayerFactory())
			);

			this.gameObj = GameBuilder.CreateGame(rng, players, organellePlayer, controller);
			this.endedTurnPlayers.Clear();
			makePlayers();
		}
		
		internal void LoadGame(MainGame game)
		{
			this.gameObj = game;
			this.endedTurnPlayers.Clear();
			makePlayers();
		}

		internal IkadnBaseObject Save()
		{
			return GetStateManager().Save(this.GameInstance);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stateListener">>Listener with callbacks for game state changes. <remarks>No callback is called before running <see cref="CreateGame"></see> method.</remarks></param>
		public void Start(IGameStateListener stateListener)
		{
			this.stateListener = stateListener;
			this.State = GameState.Running;

			restartAiGalaxyPhase();
		}
		
		public void Stop()
		{
			this.State = GameState.NoGame;
			
			if (this.aiGalaxyPhase != null) this.aiGalaxyPhase.Wait();
			if (this.combatPhase != null) this.combatPhase.Wait();
			if (this.processingPhase != null) this.processingPhase.Wait();
		}
		
		public IEnumerable<PlayerController> LocalHumanPlayers()
		{
			return this.playerControllers.Where(x => x.PlayerInstance(this.gameObj).ControlType == PlayerControlType.LocalHuman);
		}

		public ResultsController Results
		{
			get
			{
				if (!gameObj.Processor.IsOver)
#pragma warning disable CA1303 // Do not pass literals as localized parameters
					throw new InvalidOperationException("Game is not over yet");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

				return new ResultsController(this.gameObj);
			}
		}

		internal MainGame GameInstance
		{
			get 
			{ 
				lock(threadLocker)
					return (endTurnCopy != null) ? this.endTurnCopy.gameObj : this.gameObj; 
			}
		}

		private void makePlayers()
		{
			this.playerControllers = new PlayerController[this.gameObj.MainPlayers.Length];
			
			for (int i = 0; i < this.gameObj.MainPlayers.Length; i++)
			{
				this.playerControllers[i] = new PlayerController(i, this);
				
				if (this.gameObj.MainPlayers[i].OffscreenControl != null)
					this.gameObj.MainPlayers[i].OffscreenControl.Controller = this.playerControllers[i];
			}
			
			this.organelleController = new PlayerController(this.gameObj.MainPlayers.Length, this);
			this.gameObj.StareaterOrganelles.OffscreenControl.Controller = this.organelleController;
		}

		#region Turn processing
		public GameState State { get; private set; } //TODO(v0.8) remove this property, new game and load game should instantiate new game controller

		internal void EndGalaxyPhase(PlayerController player)
		{
			if (this.GameInstance.IsReadOnly)
				return;

			lock(threadLocker)
			{
				this.endedTurnPlayers.Add(player.PlayerIndex);
				
				if (this.endedTurnPlayers.Count < this.playerControllers.Length)
					return;
				
				this.endTurnCopy = new GameController();
				var gameCopy = gameObj.ReadonlyCopy(GetStateManager());
				
				this.endTurnCopy.gameObj = gameCopy;
				this.endTurnCopy.State = this.State;
			}

			this.processingPhase = Task.Factory.
				StartNew(this.turnProcessing, Task.Factory.CancellationToken, TaskCreationOptions.None, TaskScheduler.Default).
				ContinueWith(checkTaskException, TaskScheduler.Default);
		}

		public void AudienceConcluded(AudienceController audienceController)
		{
			if (audienceController == null)
				throw new ArgumentNullException(nameof(audienceController));

			this.gameObj.Processor.AudienceConcluded(audienceController.Participants, audienceController.TreatyData);
			processingSync.Set();
		}
		
		internal void SpaceCombatResolved(SpaceBattleGame battleGame, bool doBombardment)
		{
			if (doBombardment)
				this.initiateBombardment(battleGame);
			else 
			{
				this.gameObj.Processor.ConflictResolved(battleGame);
				processingSync.Set();
			}
		}
		
		internal void BombardmentResolved(ABattleGame battleGame)
		{
			this.gameObj.Processor.ConflictResolved(battleGame);
			processingSync.Set();
		}
		#endregion
		
		#region Background processing
		private void aiDoGalaxyPhase() 
		{
			organelleController.PlayerInstance(this.gameObj).OffscreenControl.PlayTurn();

			foreach (var aiController in this.playerControllers.Where(x => x.PlayerInstance(this.gameObj).ControlType == PlayerControlType.LocalAI))
				aiController.PlayerInstance(this.gameObj).OffscreenControl.PlayTurn();
		}

		private void checkTaskException(Task lastTask)
		{
			if (lastTask.IsFaulted)
				ErrorReporter.Get.Report(lastTask.Exception);
		}

#if DEBUG
		internal static ShipDebugger ShipCounter;
#endif

		private void turnProcessing()
		{
#if DEBUG
			ShipCounter = new ShipDebugger(this.gameObj.States.Fleets);
#endif
			gameObj.Processor.ProcessPrecombat();
#if DEBUG
			ShipCounter.Check("Precombat", this.gameObj.States.Fleets);
#endif

			while (gameObj.Processor.HasAudience)
			{
				processingSync.WaitOne(); //TODO(v0.9) try blocking only participants
				this.holdAudience();
			}
			//TODO(v0.9) diplomatic actions don't take place this turn
			processingSync.WaitOne(); //TODO(v0.9) make more orderly synchronization mechanism
			processingSync.Set();

			while (gameObj.Processor.HasConflict)
			{
				processingSync.WaitOne(); //TODO(v0.9) try blocking only participants
				this.initaiteCombat();
			}
			processingSync.WaitOne(); //TODO(v0.9) make more orderly synchronization mechanism
			processingSync.Set();
			
			gameObj.Processor.ProcessPostcombat();
#if DEBUG
			ShipCounter.Check("Postcombat", this.gameObj.States.Fleets);
#endif
			processingSync.Set();

			lock (threadLocker)
			{
				this.endTurnCopy = null;
				this.endedTurnPlayers.Clear();
			}

			if (gameObj.Processor.IsOver)
				stateListener.OnGameOver();
			else
			{
 				stateListener.OnNewTurn();
 				restartAiGalaxyPhase();
			}
		}

		private void restartAiGalaxyPhase()
		{
			this.aiGalaxyPhase = Task.Factory.
				StartNew(aiDoGalaxyPhase, Task.Factory.CancellationToken, TaskCreationOptions.None, TaskScheduler.Default).
				ContinueWith(checkTaskException, TaskScheduler.Default);
		}

		private void holdAudience()
		{
			var participants = gameObj.Processor.NextAudience();
			var controller = new AudienceController(participants, this, gameObj);
			
			this.stateListener.OnDoAudience(controller);
			//TODO(later) inform AIs				
		}

		private void initaiteCombat()
		{
			var conflict = gameObj.Processor.NextConflict();
			var controller = new SpaceBattleController(conflict, this, gameObj);

			foreach(var player in controller.Participants)
			{
				var playerController = (player.ControlType == PlayerControlType.Neutral) ?
					this.organelleController :
					this.playerControllers.First(x => this.gameObj.MainPlayers[x.PlayerIndex] == player);
				
				if (player.OffscreenControl != null)
					controller.Register(playerController, player.OffscreenControl.StartBattle(controller));
				else
					controller.Register(playerController, this.stateListener.OnDoCombat(controller));
			}

			this.combatPhase = Task.Factory.
				StartNew(controller.Start, Task.Factory.CancellationToken, TaskCreationOptions.None, TaskScheduler.Default).
				ContinueWith(checkTaskException, TaskScheduler.Default);
		}

		void initiateBombardment(SpaceBattleGame battleGame)
		{
			var controller = new BombardmentController(new BombardBattleGame(battleGame), this.gameObj, this);

			foreach(var player in controller.Participants)
			{
				var playerController = (player.ControlType == PlayerControlType.Neutral) ?
					this.organelleController :
					this.playerControllers.First(x => this.gameObj.MainPlayers[x.PlayerIndex] == player);
				if (player.OffscreenControl != null)
					controller.Register(playerController, player.OffscreenControl.StartBombardment(controller));
				else
					controller.Register(playerController, this.stateListener.OnDoBombardment(controller));
			}

			this.combatPhase = Task.Factory.
				StartNew(controller.Start, Task.Factory.CancellationToken, TaskCreationOptions.None, TaskScheduler.Default).
				ContinueWith(checkTaskException, TaskScheduler.Default);
		}
		#endregion
	}
}
