using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Players.Reports;
using Stareater.Utils;

namespace Stareater.Controllers
{
	public class GameController
	{
		internal const string ReportContext = "Reports";
		
		private Game game;
		
		private GameController endTurnCopy = null;
		private IGameStateListener stateListener;
		private Task aiGalaxyPhase = null;
		private Task turnProcessing = null;

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
			
			Random rng = new Random();
			
			this.game = GameBuilder.CreateGame(rng, players, controller);
		}
		
		internal void LoadGame(Game game)
		{
			this.game = game;
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
			//UNDONE: what else to do here?
		}
		
		internal Game GameInstance
		{
			get { return (this.IsReadOnly) ? this.endTurnCopy.game : this.game; }
		}

		#region Turn processing
		public GameState State { get; private set; }
		
		public int CurrentPlayer 
		{ 
			get { return this.game.CurrentPlayerIndex; }
		}
		
		private void aiDoGalaxyPhase() 
		{
			foreach(var player in game.Players)
				if (player.ControlType == PlayerControlType.LocalAI)
					player.OffscreenControl.PlayTurn();
			
			stateListener.OnAiGalaxyPhaseDone();
		}
		
		private void precombatTurnProcessing()
		{
			game.ProcessPrecombat();
			
 			stateListener.OnCombatPhaseStart();
		}
		
		private void postcombatTurnProcessing()
		{
			game.ProcessPostcombat();
			
 			this.endTurnCopy = null;
 			
 			stateListener.OnNewTurn();
 			restartAiGalaxyPhase();
		}
		
		private void restartAiGalaxyPhase()
		{
			this.aiGalaxyPhase = new Task(aiDoGalaxyPhase);
			this.aiGalaxyPhase.Start();
		}
		
		public void EndGalaxyPhase()
		{
			if (this.IsReadOnly)
				return;

			//FIXME(later): presumes single human player
			
			this.endTurnCopy = new GameController();
			var gameCopy = game.ReadonlyCopy();
			
			this.endTurnCopy.game = gameCopy.Game;
			this.endTurnCopy.State = this.State;
			
			if (!aiGalaxyPhase.IsCompleted)
				return;
			
			this.turnProcessing = new Task(precombatTurnProcessing);
			this.turnProcessing.Start();
		}

		public void EndCombatPhase()
		{
			this.turnProcessing = new Task(postcombatTurnProcessing);
			this.turnProcessing.Start();
		}
		
		public bool IsReadOnly
		{
			get { return endTurnCopy != null; }
		}
		#endregion
		
		#region Map related
		public Methods.VisualPositionFunc IdleFleetVisualPositioner { get; set; }
		
		public bool IsStarVisited(StarData star)
		{
			return this.GameInstance.CurrentPlayer.Intelligence.About(star).IsVisited;
		}
		
		public IEnumerable<ColonyInfo> KnownColonies(StarData star)
		{
			var game = this.GameInstance;
			var starKnowledge = game.CurrentPlayer.Intelligence.About(star);
			
			foreach(var colony in game.States.Colonies.AtStar(star))
				if (starKnowledge.Planets[colony.Location.Planet].LastVisited != PlanetIntelligence.NeverVisited)
					yield return new ColonyInfo(colony);
		}
		
		public StarSystemController OpenStarSystem(StarData star)
		{
			return new StarSystemController(this.GameInstance, star, IsReadOnly);
		}
		
		public StarSystemController OpenStarSystem(Vector2D position)
		{
			return this.OpenStarSystem(this.game.States.Stars.At(position));
		}
		
		public GalaxySearchResult FindClosest(float x, float y, float searchRadius)
		{
			var search = new GalaxySearch(x, y, searchRadius);
			search.Compare(this.game.States.Stars);
			search.Compare(this.game.States.Fleets, this.IdleFleetVisualPositioner);
			
			return search.Finish(game, this.IdleFleetVisualPositioner);
		}
		
		public FleetController SelectFleet(IdleFleetInfo idleFleet)
		{
			return new FleetController(idleFleet.Fleet, this.game);
		}
		
		public IEnumerable<IdleFleetInfo> IdleFleets
		{
			get
			{
				var game = this.GameInstance;
				
				//TODO(v0.5) add fleets of other players 
				return game.States.Fleets.OwnedBy(game.CurrentPlayer).Select(x => new IdleFleetInfo(x, game, this.IdleFleetVisualPositioner));
			}
		}
		
		public StarData Star(Vector2D position)
		{
			return this.GameInstance.States.Stars.At(position);
		}
		
		public int StarCount
		{
			get 
			{
				return this.GameInstance.States.Stars.Count;
			}
		}
		
		public IEnumerable<StarData> Stars
		{
			get
			{
				return this.GameInstance.States.Stars;
			}
		}

		public IEnumerable<Wormhole> Wormholes
		{
			get
			{
				foreach (var wormhole in this.GameInstance.States.Wormholes)
					yield return wormhole;
			}
		}
		#endregion
		
		#region Ship designs
		public ShipDesignController NewDesign()
		{
			return new ShipDesignController(game); //FIXME(v0.5) check if the game is read only 
		}
		
		public IEnumerable<DesignInfo> ShipsDesigns()
		{
			var game = this.GameInstance;
			return game.States.Designs.OwnedBy(game.CurrentPlayer).Select(x => new DesignInfo(x));
		}
		#endregion
		
		#region Development related
		public IEnumerable<TechnologyTopic> DevelopmentTopics()
		{
			var game = this.GameInstance;
			var playerTechs = game.Derivates.Of(game.CurrentPlayer).DevelopmentOrder(game.States.TechnologyAdvances);
		
			if (game.Derivates.Of(game.CurrentPlayer).DevelopmentPlan == null)
				game.Derivates.Of(game.CurrentPlayer).CalculateDevelopment(
					game.Statics,
					game.States,
					game.Derivates.Colonies.OwnedBy(game.CurrentPlayer)
				);
			
			var investments = game.Derivates.Of(game.CurrentPlayer).DevelopmentPlan.ToDictionary(x => x.Item);
			
			foreach(var techProgress in playerTechs)
				if (investments.ContainsKey(techProgress))
					yield return new TechnologyTopic(techProgress, investments[techProgress]);
				else
					yield return new TechnologyTopic(techProgress);
			
		}
		
		public IEnumerable<TechnologyTopic> ReorderDevelopmentTopics(IEnumerable<string> idCodeOrder)
		{
			if (this.IsReadOnly)
				return DevelopmentTopics();

			var modelQueue = game.CurrentPlayer.Orders.DevelopmentQueue;
			modelQueue.Clear();
			
			int i = 0;
			foreach (var idCode in idCodeOrder) {
				modelQueue.Add(idCode, i);
				i++;
			}
			
			game.Derivates.Of(game.CurrentPlayer).InvalidateDevelopment();
			return DevelopmentTopics();
		}
		
		public DevelopmentFocusInfo[] DevelopmentFocusOptions()
		{
			return this.GameInstance.Statics.DevelopmentFocusOptions.Select(x => new DevelopmentFocusInfo(x)).ToArray();
		}
		
		public int DevelopmentFocusIndex 
		{ 
			get
			{
				return this.GameInstance.CurrentPlayer.Orders.DevelopmentFocusIndex;
			}
			
			set
			{
				if (this.IsReadOnly)
					return;
				
				if (value >= 0 && value < game.Statics.DevelopmentFocusOptions.Count)
					game.CurrentPlayer.Orders.DevelopmentFocusIndex = value;
				
				game.Derivates.Of(game.CurrentPlayer).InvalidateDevelopment();
			}
		}
		
		public double DevelopmentPoints 
		{ 
			get
			{
				var game = this.GameInstance;
				
				return game.Derivates.Colonies.OwnedBy(game.CurrentPlayer).Sum(x => x.Development);
			}
		}
		#endregion
		
		#region Research related
		public IEnumerable<TechnologyTopic> ResearchTopics()
		{
			var game = this.GameInstance;
			var playerTechs = game.Derivates.Of(game.CurrentPlayer).ResearchOrder(game.States.TechnologyAdvances);
		
			if (game.Derivates.Of(game.CurrentPlayer).ResearchPlan == null)
				game.Derivates.Of(game.CurrentPlayer).CalculateResearch(
					game.Statics,
					game.States,
					game.Derivates.Colonies.OwnedBy(game.CurrentPlayer)
				);
			
			var investments = game.Derivates.Of(game.CurrentPlayer).ResearchPlan.ToDictionary(x => x.Item);
			
			foreach(var techProgress in playerTechs)
				if (investments.ContainsKey(techProgress))
					yield return new TechnologyTopic(techProgress, investments[techProgress]);
				else
					yield return new TechnologyTopic(techProgress);
			
		}
		
		public int ResearchFocus
		{
			get 
			{
				var game = this.GameInstance;
				string focused = game.CurrentPlayer.Orders.ResearchFocus;
				var playerTechs = game.Derivates.Of(game.CurrentPlayer).ResearchOrder(game.States.TechnologyAdvances).ToList();
				
				for (int i = 0; i < playerTechs.Count; i++)
					if (playerTechs[i].Topic.IdCode == focused)
						return i;
				
				return 0; //TODO(later) think of some smarter default research
			}
			
			set
			{
				if (this.IsReadOnly)
					return;
				
				var playerTechs = game.Derivates.Of(game.CurrentPlayer).ResearchOrder(game.States.TechnologyAdvances).ToList();
				if (value >= 0 && value < playerTechs.Count) {
					this.game.CurrentPlayer.Orders.ResearchFocus = playerTechs[value].Topic.IdCode;
					game.Derivates.Of(game.CurrentPlayer).InvalidateResearch();
				}
			}
		}
		
		public StarData ResearchCenter
		{
			get { return game.Derivates.Of(game.CurrentPlayer).ResearchCenter; }
		}
		#endregion
		
		#region Report related
		public IEnumerable<IReportInfo> Reports
		{
			get {
				var game = this.GameInstance;
				var wrapper = new ReportWrapper();
				
				foreach(var report in game.States.Reports.Of(game.CurrentPlayer))
					yield return wrapper.Wrap(report);
			}
		}
		#endregion
	}
}
