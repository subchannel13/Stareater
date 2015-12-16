using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Players.Reports;
using Stareater.Ships.Missions;
using Stareater.Utils;
using Stareater.Controllers.Data;

namespace Stareater.Controllers
{
	public class GameController
	{
		internal const string ReportContext = "Reports";
		
		private Game gameObj;
		private GalaxyObjects mapCache = new GalaxyObjects();
		
		private GameController endTurnCopy = null;
		private IGameStateListener stateListener;
		private Task aiGalaxyPhase = null;

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
			this.rebuildCache();
		}
		
		internal void LoadGame(Game game)
		{
			this.gameObj = game;
			this.rebuildCache();
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
		
		internal Game GameInstance
		{
			get { return (this.IsReadOnly) ? this.endTurnCopy.gameObj : this.gameObj; }
		}

		#region Turn processing
		public GameState State { get; private set; }
		
		public int CurrentPlayer 
		{ 
			get { return this.gameObj.CurrentPlayerIndex; }
		}

		#region Background processing
		private void aiDoGalaxyPhase() 
		{
			foreach(var player in gameObj.Players)
				if (player.ControlType == PlayerControlType.LocalAI)
					player.OffscreenControl.PlayTurn();
			
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

 			stateListener.OnCombatPhaseStart();
		}
		
		private void postcombatTurnProcessing()
		{
			gameObj.ProcessPostcombat();
			this.rebuildCache();
			
 			this.endTurnCopy = null;
 			
 			stateListener.OnNewTurn();
 			restartAiGalaxyPhase();
		}
		
		private void restartAiGalaxyPhase()
		{
			this.aiGalaxyPhase = Task.Factory.StartNew(aiDoGalaxyPhase).ContinueWith(checkTaskException);
		}
		#endregion

		public void EndGalaxyPhase()
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

		public void EndCombatPhase()
		{
			Task.Factory.StartNew(postcombatTurnProcessing).ContinueWith(checkTaskException);
		}
		
		public bool IsReadOnly
		{
			get { return endTurnCopy != null; }
		}
		#endregion
		
		#region Map related
		private IVisualPositioner visualPositioner = null;
		public IVisualPositioner VisualPositioner
		{ 
			get { return this.visualPositioner; }
			set
			{
				this.visualPositioner = value;
				this.rebuildCache();
			}
		}
		
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
			return this.OpenStarSystem(this.gameObj.States.Stars.At(position));
		}
		
		public GalaxySearchResult FindClosest(float x, float y, float searchRadius)
		{

			return this.mapCache.Search(x, y, searchRadius);
		}
		
		public FleetController SelectFleet(FleetInfo fleet)
		{
			return new FleetController(fleet, this.gameObj, this.mapCache, this.VisualPositioner);
		}
		
		public IEnumerable<FleetInfo> Fleets
		{
			get
			{
				return this.mapCache.Fleets;
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

		private void rebuildCache()
		{
			var fleets = new List<FleetInfo>();

			//TODO(later) filter invisible fleets
			foreach (var fleet in gameObj.States.Fleets) {
				if (fleet.Owner == gameObj.CurrentPlayer && gameObj.CurrentPlayer.Orders.ShipOrders.ContainsKey(fleet.Position))
					foreach(var newFleet in gameObj.CurrentPlayer.Orders.ShipOrders[fleet.Position])
						fleets.Add(new FleetInfo(newFleet, this.gameObj.States.Stars.AtContains(fleet.Position), this.VisualPositioner, gameObj.Derivates.Of(fleet.Owner)));
				else
					fleets.Add(new FleetInfo(fleet, this.gameObj.States.Stars.AtContains(fleet.Position), this.VisualPositioner, gameObj.Derivates.Of(fleet.Owner)));
			}

			this.mapCache.Rebuild(this.GameInstance.States.Stars, fleets);
		}
		#endregion
		
		#region Stellarises and colonies
		public IEnumerable<StellarisInfo> Stellarises()
		{
			foreach(var stellaris in this.GameInstance.States.Stellarises.OwnedBy(this.GameInstance.CurrentPlayer))
				yield return new StellarisInfo(stellaris);
		}
		#endregion
		
		#region Ship designs
		public ShipDesignController NewDesign()
		{
			return new ShipDesignController(gameObj); //FIXME(v0.5) check if the game is read only 
		}
		
		public IEnumerable<DesignInfo> ShipsDesigns()
		{
			var game = this.GameInstance;
			return game.States.Designs.OwnedBy(game.CurrentPlayer).Select(x => new DesignInfo(x, game.Derivates.Of(game.CurrentPlayer).DesignStats[x]));
		}
		#endregion
		
		#region Colonization related
		public IEnumerable<ColonizationController> ColonizationProjects()
		{
			var planets = new HashSet<Planet>();
			planets.UnionWith(this.GameInstance.States.ColonizationProjects.OwnedBy(this.GameInstance.CurrentPlayer).Select(x => x.Destination));
			planets.UnionWith(this.GameInstance.CurrentPlayer.Orders.ColonizationOrders.Keys);
			
			foreach(var planet in planets)
				yield return new ColonizationController(this.GameInstance, planet, this.IsReadOnly);
		}
		
		public IEnumerable<FleetInfo> EnrouteColonizers(Planet destination)
		{
			var finder = new ColonizerFinder(destination);
			
			foreach(var fleet in mapCache.Fleets.Where(x => x.Owner.Data == this.GameInstance.CurrentPlayer))
				if (finder.Check(fleet.FleetData))
					yield return fleet;
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

			var modelQueue = gameObj.CurrentPlayer.Orders.DevelopmentQueue;
			modelQueue.Clear();
			
			int i = 0;
			foreach (var idCode in idCodeOrder) {
				modelQueue.Add(idCode, i);
				i++;
			}
			
			gameObj.Derivates.Of(gameObj.CurrentPlayer).InvalidateDevelopment();
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
				
				if (value >= 0 && value < gameObj.Statics.DevelopmentFocusOptions.Count)
					gameObj.CurrentPlayer.Orders.DevelopmentFocusIndex = value;
				
				gameObj.Derivates.Of(gameObj.CurrentPlayer).InvalidateDevelopment();
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
				
				var playerTechs = gameObj.Derivates.Of(gameObj.CurrentPlayer).ResearchOrder(gameObj.States.TechnologyAdvances).ToList();
				if (value >= 0 && value < playerTechs.Count) {
					this.gameObj.CurrentPlayer.Orders.ResearchFocus = playerTechs[value].Topic.IdCode;
					gameObj.Derivates.Of(gameObj.CurrentPlayer).InvalidateResearch();
				}
			}
		}
		
		public StarData ResearchCenter
		{
			get { return gameObj.Derivates.Of(gameObj.CurrentPlayer).ResearchCenter; }
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
