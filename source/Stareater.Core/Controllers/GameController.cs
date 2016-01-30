using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stareater.Controllers.Data;
using Stareater.Controllers.Views;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class GameController
	{
		internal const string ReportContext = "Reports";
		
		private Game gameObj;
		
		private GameController endTurnCopy = null;
		private IGameStateListener stateListener;
		private Task aiGalaxyPhase = null;
		private PlayerController[] playerControllers = null;

		public GameController()
		{ 
			this.State = GameState.NoGame;
		}
	
		public void CreateGame(NewGameController controller)
		{
			if (State != GameState.NoGame)
				throw new InvalidOperationException("Game is already created.");

			//TODO: Pass organization to player
			Player[] players = controller.PlayerList.Select(info =>
				new Player(info.Name, info.Color, /*info.Organization, */info.ControlType)
			).ToArray();
			
			var rng = new Random();
			
			this.gameObj = GameBuilder.CreateGame(rng, players, controller);
			makePlayers();
		}
		
		internal void LoadGame(Game game)
		{
			this.gameObj = game;
			makePlayers(); //TODO(v0.5) invalidate player controllers at view
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
			//UNDONE(later): what else to do here?
		}
		
		public IEnumerable<PlayerController> LocalHumanPlayers()
		{
			return this.playerControllers.Where(x => x.PlayerInstance.ControlType == PlayerControlType.LocalHuman);
		}
		
		internal Game GameInstance
		{
			get { return (this.IsReadOnly) ? this.endTurnCopy.gameObj : this.gameObj; }
		}

		private void makePlayers()
		{
			this.playerControllers = new PlayerController[this.gameObj.Players.Length];
			
			for (int i = 0; i < this.gameObj.Players.Length; i++)
				this.playerControllers[i] = new PlayerController(i, this);
		}
		
		#region Turn processing
		public GameState State { get; private set; }
		
		public bool IsReadOnly
		{
			get { return endTurnCopy != null; }
		}
		
		internal void EndGalaxyPhase()
		{
			if (this.IsReadOnly)
				return;

			//FIXME(later): presumes single human player
			
			this.endTurnCopy = new GameController();
			var gameCopy = gameObj.ReadonlyCopy();
			
			this.endTurnCopy.gameObj = gameCopy.Game;
			this.endTurnCopy.State = this.State;
			
			if (!aiGalaxyPhase.IsCompleted)
				return;

			Task.Factory.StartNew(precombatTurnProcessing).ContinueWith(checkTaskException);
		}

		internal void EndCombatPhase()
		{
			Task.Factory.StartNew(postcombatTurnProcessing).ContinueWith(checkTaskException);
		}
		#endregion
		
		#region Background processing
		private void aiDoGalaxyPhase() 
		{
			foreach(var aiController in this.playerControllers.Where(x => x.PlayerInstance.ControlType == PlayerControlType.LocalAI))
				aiController.PlayerInstance.OffscreenControl.PlayTurn(aiController);
				
			stateListener.OnAiGalaxyPhaseDone();
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

		private void precombatTurnProcessing()
		{
			gameObj.ProcessPrecombat();

			if (gameObj.Processor.HasConflicts)
 				stateListener.OnCombatPhaseStart();
			else
				this.EndCombatPhase();
		}
		
		private void postcombatTurnProcessing()
		{
			gameObj.ProcessPostcombat();
			this.endTurnCopy = null;
			
			foreach(var player in this.playerControllers)
				player.RebuildCache();
 			
 			stateListener.OnNewTurn();
 			restartAiGalaxyPhase();
		}
		
		private void restartAiGalaxyPhase()
		{
			this.aiGalaxyPhase = Task.Factory.StartNew(aiDoGalaxyPhase).ContinueWith(checkTaskException);
		}
		#endregion
	}
}
