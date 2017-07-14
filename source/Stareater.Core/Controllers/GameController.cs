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

namespace Stareater.Controllers
{
	public class GameController
	{
		internal const string ReportContext = "Reports";
		private static StateManager stateManager = new StateManager();
		
		private object threadLocker = new object();
		private AutoResetEvent processingSync = new AutoResetEvent(true);
		private MainGame gameObj;
		
		private GameController endTurnCopy = null;
		private IGameStateListener stateListener;
		private Task aiGalaxyPhase = null;
		private Task processingPhase = null;
		private PlayerController[] playerControllers = null;
		private PlayerController organelleController = null;
		private HashSet<int> endedTurnPlayers = new HashSet<int>();

		public GameController()
		{ 
			this.State = GameState.NoGame;
		}
	
		public void CreateGame(NewGameController controller, IEnumerable<TracableStream> staticDataSources)
		{
			if (State != GameState.NoGame)
				throw new InvalidOperationException("Game is already created.");

			//TODO(v0.7): Pass organization to player
			var players = controller.PlayerList.Select(info =>
				new Player(info.Name, info.Color, /*info.Organization, */info.ControlType)
			).ToArray();
			var organellePlayer = new Player("no name", System.Drawing.Color.Gray, new PlayerType(PlayerControlType.Neutral, new OrganellePlayerFactory()));
			
			var rng = new Random();
			
			this.gameObj = GameBuilder.CreateGame(rng, players, organellePlayer, controller, staticDataSources);
			makePlayers();
		}
		
		internal void LoadGame(MainGame game)
		{
			this.gameObj = game;
			makePlayers();
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
			if (this.processingPhase != null) this.processingPhase.Wait();
		}
		
		public IEnumerable<PlayerController> LocalHumanPlayers()
		{
			return this.playerControllers.Where(x => x.PlayerInstance(this.gameObj).ControlType == PlayerControlType.LocalHuman);
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
		public GameState State { get; private set; }
		
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
				var gameCopy = gameObj.ReadonlyCopy(stateManager);
				
				this.endTurnCopy.gameObj = gameCopy;
				this.endTurnCopy.State = this.State;
			}
			
			this.processingPhase = Task.Factory.StartNew(turnProcessing).ContinueWith(checkTaskException);
		}

		public void AudienceConcluded(AudienceController audienceController)
		{
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
		
		internal void BreakthroughReviewed(ResearchCompleteController controller)
		{
			this.gameObj.Derivates.Of(controller.Owner).BreakthroughReviewed(controller.SelectedPriorities, this.gameObj.States);
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
			if (!lastTask.IsFaulted)
				return;
#if DEBUG
			System.Diagnostics.Trace.TraceError(lastTask.Exception.ToString());
#else
			throw lastTask.Exception;
#endif
		}

		private void turnProcessing()
		{
			gameObj.Processor.ProcessPrecombat();

			while (gameObj.Processor.HasAudience)
			{
				processingSync.WaitOne(); //TODO(v0.7) try blocking only participants
				this.holdAudience();
			}
			//TODO(v0.7) diplomatic actions don't take place this turn
			processingSync.WaitOne(); //TODO(v0.7) make more orderly synchronization mechanism
			processingSync.Set();

			while (gameObj.Processor.HasConflict)
			{
				processingSync.WaitOne(); //TODO(v0.7) try blocking only participants
				this.initaiteCombat();
			}
			processingSync.WaitOne(); //TODO(v0.7) make more orderly synchronization mechanism
			processingSync.Set();
			
			while (this.gameObj.Derivates.Players.Any(x => x.HasBreakthrough))
			{
				processingSync.WaitOne(); //TODO(v0.7) per player sync instead of global
				this.presentBreakthrough();
			}
			
			processingSync.WaitOne();
			gameObj.Processor.ProcessPostcombat();
			processingSync.Set();
			
			lock(threadLocker)
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
			this.aiGalaxyPhase = Task.Factory.StartNew(aiDoGalaxyPhase).ContinueWith(checkTaskException);
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
			
			controller.Start();
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
			
			controller.Start();
		}
		
		private void presentBreakthrough()
		{
			var playerProc = this.gameObj.Derivates.Players.First(x => x.HasBreakthrough);
			var controller = new ResearchCompleteController(
				playerProc.Player, 
				playerProc.NextBreakthrough().Item.Topic,
				this,
				gameObj
			);
			
			if (playerProc.Player.ControlType == PlayerControlType.LocalAI)
				playerProc.Player.OffscreenControl.OnResearchComplete(controller); //TODO(0.7) do in separate thread/task
			else
				this.stateListener.OnResearchComplete(controller);
		}
		#endregion
	}
}
