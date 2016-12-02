using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stareater.Controllers.Data;
using Stareater.Controllers.Views;
using Stareater.Players;
using Stareater.Players.Natives;

namespace Stareater.Controllers
{
	public class GameController
	{
		internal const string ReportContext = "Reports";
		
		private object threadLocker = new object();
		private Semaphore processingSync = new Semaphore(0, 1);
		private MainGame gameObj;
		
		private GameController endTurnCopy = null;
		private IGameStateListener stateListener;
		private Task aiGalaxyPhase = null;
		private PlayerController[] playerControllers = null;
		private HashSet<int> endedTurnPlayers = new HashSet<int>();

		public GameController()
		{ 
			this.State = GameState.NoGame;
		}
	
		public void CreateGame(NewGameController controller, IEnumerable<TextReader> staticDataSources)
		{
			if (State != GameState.NoGame)
				throw new InvalidOperationException("Game is already created.");

			//TODO(later): Pass organization to player
			var players = controller.PlayerList.Select(info =>
				new Player(info.Name, info.Color, /*info.Organization, */info.ControlType)
			).ToList();
			players.Add(new Player("no name", System.Drawing.Color.Gray, new PlayerType(PlayerControlType.Neutral, new StareaterPlayerFactory())));
			
			var rng = new Random();
			
			this.gameObj = GameBuilder.CreateGame(rng, players.ToArray(), controller, staticDataSources);
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
			//TODO(later): stop AI tasks
			//UNDONE(later): what else to do here?
		}
		
		public IEnumerable<PlayerController> LocalHumanPlayers()
		{
			return this.playerControllers.Where(x => x.PlayerInstance.ControlType == PlayerControlType.LocalHuman);
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
			this.playerControllers = new PlayerController[this.gameObj.Players.Length];
			
			for (int i = 0; i < this.gameObj.Players.Length; i++)
			{
				this.playerControllers[i] = new PlayerController(i, this);
				
				if (this.gameObj.Players[i].OffscreenControl != null)
					this.gameObj.Players[i].OffscreenControl.Controller = this.playerControllers[i];
			}
		}
		
		#region Turn processing
		public GameState State { get; private set; }
		
		public bool IsReadOnly
		{
			get 
			{ 
				lock(threadLocker)
					return endTurnCopy != null; 
			}
		}
		
		internal void EndGalaxyPhase(PlayerController player)
		{
			if (this.IsReadOnly)
				return;

			lock(threadLocker)
			{
				this.endedTurnPlayers.Add(player.PlayerIndex);
				
				if (this.endedTurnPlayers.Count < this.playerControllers.Length)
					return;
				
				this.endTurnCopy = new GameController();
				var gameCopy = gameObj.ReadonlyCopy();
				
				this.endTurnCopy.gameObj = gameCopy.Game;
				this.endTurnCopy.State = this.State;
			}
			
			Task.Factory.StartNew(turnProcessing).ContinueWith(checkTaskException);
		}

		internal void ConflictResolved(SpaceBattleGame battleGame)
		{
			this.gameObj.Processor.ConflictResolved(battleGame);
			processingSync.Release();
		}
		
		internal void BreakthroughReviewed(ResearchCompleteController controller)
		{
			this.gameObj.Derivates.Of(controller.Owner).BreakthroughReviewed(controller.SelectedPriorities, this.gameObj.States);
			processingSync.Release();
		}
		#endregion
		
		#region Background processing
		private void aiDoGalaxyPhase() 
		{
			foreach(var aiController in this.playerControllers.Where(x => x.PlayerInstance.ControlType == PlayerControlType.LocalAI))
				aiController.PlayerInstance.OffscreenControl.PlayTurn();
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

			while (gameObj.Processor.HasConflicts)
			{
				this.initaiteCombat();
				processingSync.WaitOne();
			}
			
			while (this.gameObj.Derivates.Players.Any(x => x.HasBreakthrough))
			{
				this.presentBreakthrough();
				processingSync.WaitOne(); //TODO(v0.6) per player sync instead of global
			}
			
			gameObj.Processor.ProcessPostcombat();
			
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

		private void initaiteCombat()
		{
			var conflict = gameObj.Processor.NextConflict();
			var controller = new SpaceBattleController(conflict, this, gameObj, playerControllers);
			var participants = conflict.Combatants.Select(x => x.Owner).Distinct().ToList();
				
			foreach(var player in participants)
			{
				var playerController = this.playerControllers.First(x => this.gameObj.Players[x.PlayerIndex] == player);
				
				if (player.ControlType == PlayerControlType.LocalAI)
					controller.Register(playerController, player.OffscreenControl.StartBattle(controller));
				else
					controller.Register(playerController, this.stateListener.OnDoCombat(controller));
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
				playerProc.Player.OffscreenControl.OnResearchComplete(controller); //TODO(0.6) do in separate thread/task
			else
				this.stateListener.OnResearchComplete(controller);
		}
		#endregion
	}
}
