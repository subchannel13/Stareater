using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class GameController
	{
		private Game game;
		private Dictionary<Player, StarData> lastSelectedStar = new Dictionary<Player, StarData>();

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
			this.initStarSelection();
		}
		
		internal void LoadGame(Game game)
		{
			this.game = game;
			//TODO(later) consider loading from save data
			this.initStarSelection();
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
		
		private void initStarSelection()
		{
			foreach(Player player in this.game.Players) {
				//TODO(v0.5): utilize stellar administration instead iterating colonies
				var colonies = this.game.States.Colonies.OwnedBy(player);
				var perStar = colonies.GroupBy(x => x.Star);
				var starPopulation = perStar.Select(x => new KeyValuePair<StarData, double>(
					x.Key, 
					x.Aggregate(0.0, (sum, colony) => sum + colony.Population)
				));
				var maxPopulationStar = starPopulation.Aggregate((a, b) => (a.Value > b.Value) ? a : b).Key;
				
				this.lastSelectedStar.Add(player, maxPopulationStar);
			}
		}

		#region Turn processing
		public GameState State { get; private set; }
		
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
			this.endTurnCopy.lastSelectedStar = new Dictionary<Player, StarData>();

			foreach (var originalSelection in lastSelectedStar)
				endTurnCopy.lastSelectedStar.Add(
					gameCopy.Players.Players[originalSelection.Key],
					gameCopy.Map.Stars[originalSelection.Value]);

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
		
		public StarSystemController OpenStarSystem(float x, float y, float searchRadius)
		{
			StarData closest = closestStar(x, y, searchRadius);
			
			if (closest != null)
				return new StarSystemController(this.GameInstance, closest, IsReadOnly);
			else
				return null;
		}
		
		public void SelectClosest(float x, float y, float searchRadius)
		{
			StarData closest = closestStar(x, y, searchRadius);
			
			if (closest != null)
				SelectedStar = closest;
		}
		
		public StarData SelectedStar
		{
			get
			{
				var lastSelectedStar = (this.IsReadOnly) ? this.endTurnCopy.lastSelectedStar : this.lastSelectedStar;

				return lastSelectedStar[this.GameInstance.CurrentPlayer];
			}
			private set
			{
				if (this.IsReadOnly)
					return;
				
				this.lastSelectedStar[this.game.CurrentPlayer] = value;
			}
		}
		
		public int StarCount
		{
			get 
			{
				return this.GameInstance.States.Stars.Count;
			}
		}
		
		public IEnumerable<IdleFleetInfo> IdleFleets
		{
			get
			{
				var game = this.GameInstance;
				
				//TODO(v0.5) add fleets of other players 
				return game.States.IdleFleets.OwnedBy(game.CurrentPlayer).Select(x => new IdleFleetInfo(x));
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

		private StarData closestStar(float x, float y, float searchRadius)
		{
			var game = this.GameInstance;

			Vector2D point = new Vector2D(x, y);
			StarData closestStar = game.States.Stars.First();
			foreach (var star in game.States.Stars)
				if ((star.Position - point).Magnitude() < (closestStar.Position - point).Magnitude())
					closestStar = star;
			
			if ((closestStar.Position - point).Magnitude() <= searchRadius)
				return closestStar;
			else
				return null;
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
		#endregion
	}
}
