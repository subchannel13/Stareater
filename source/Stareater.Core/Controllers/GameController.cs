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

		/// <summary>
		/// GameController constructor intended for game view (GUI or other kind of human interface).  
		/// </summary>
		/// <param name="stateListener">Listener with callbacks for game state changes. <remarks>No callback is called before running <see cref="CreateGame"></see> method.</remarks></param>
		public GameController(IGameStateListener stateListener)
		{
			this.stateListener = stateListener;
			this.State = GameState.NoGame;
		}

		private GameController()
		{ }
	
		public void CreateGame(NewGameController controller)
		{
			if (State != GameState.NoGame)
				throw new InvalidOperationException("Game is already created.");

			Player[] players = controller.PlayerList.Select(info =>
				new Player(info.Name, info.Color, info.Organization, info.ControlType)
			).ToArray();
			
			Random rng = new Random();
			
			this.game = GameBuilder.CreateGame(rng, players, controller);
			this.State = GameState.Running;

			foreach(Player player in players) {
				//TODO: utilize stellar administration instead iterating colonies
				var colonies = this.game.States.Colonies.OwnedBy(player);
				var perStar = colonies.GroupBy(x => x.Star);
				var starPopulation = perStar.Select(x => new KeyValuePair<StarData, double>(
					x.Key, 
					x.Aggregate(0.0, (sum, colony) => sum + colony.Population)
				));
				var maxPopulationStar = starPopulation.Aggregate((a, b) => (a.Value > b.Value) ? a : b).Key;
				
				this.lastSelectedStar.Add(player, maxPopulationStar);
			}
			
			restartAiGalaxyPhase();
		}
		
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

			//FIXME: presumes single human player
			
			this.endTurnCopy = new GameController();
			var gameCopy = game.ReadonlyCopy();
			
			endTurnCopy.game = gameCopy.Game;
			endTurnCopy.lastSelectedStar = new Dictionary<Player, StarData>();

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
		
		#region Map related
		public bool IsStarVisited(StarData star)
		{
			var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;

			return game.CurrentPlayer.Intelligence.About(star).IsVisited;
		}
		
		public IEnumerable<ColonyInfo> KnownColonies(StarData star)
		{
			var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;
			var starKnowledge = game.CurrentPlayer.Intelligence.About(star);
			
			foreach(var colony in game.States.Colonies.AtStar(star))
				if (starKnowledge.Planets[colony.Location].LastVisited != PlanetIntelligence.NeverVisited)
					yield return new ColonyInfo(colony);
		}
		
		public StarSystemController OpenStarSystem(StarData star)
		{
			var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;

			return new StarSystemController(game, star, IsReadOnly);
		}
		
		public StarSystemController OpenStarSystem(float x, float y, float searchRadius)
		{
			var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;
			StarData closest = closestStar(x, y, searchRadius);
			
			if (closest != null)
				return new StarSystemController(game, closest, IsReadOnly);
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
				var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;
				var lastSelectedStar = (this.IsReadOnly) ? this.endTurnCopy.lastSelectedStar : this.lastSelectedStar;

				return lastSelectedStar[game.CurrentPlayer];
			}
			private set
			{
				var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;
				
				this.lastSelectedStar[game.CurrentPlayer] = value;
			}
		}
		
		public int StarCount
		{
			get 
			{
				var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game; 
				
				return game.States.Stars.Count;
			}
		}
		
		public IEnumerable<StarData> Stars
		{
			get
			{
				var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;
				
				return game.States.Stars;
			}
		}

		public IEnumerable<Wormhole> Wormholes
		{
			get
			{
				var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;

				foreach (var wormhole in game.States.Wormholes)
					yield return wormhole;
			}
		}

		private StarData closestStar(float x, float y, float searchRadius)
		{
			var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;

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
		
		#region Technology related
		public IEnumerable<TechnologyTopic> DevelopmentTopics()
		{
			var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;
			var playerTechs = game.Derivates.Of(game.CurrentPlayer).DevelopmentOrder(game.States.TechnologyAdvances);
		
			foreach(var techProgress in playerTechs)
				yield return new TechnologyTopic(techProgress);
		}
		
		public void ReorderDevelopmentTopics(IEnumerable<string> idCodeOrder)
		{
			if (this.IsReadOnly)
				return;

			var modelQueue = game.CurrentPlayer.Orders.DevelopmentQueue;
			modelQueue.Clear();
			
			int i = 0;
			foreach (var idCode in idCodeOrder) {
				modelQueue.Add(idCode, i);
				i++;
			}
		}
		#endregion
	}
}
