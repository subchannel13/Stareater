using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGenerics.Extensions;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using NGenerics.DataStructures.Mathematical;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class GameController
	{
		private Game game;
		private Dictionary<Player, StarData> lastSelectedStar = new Dictionary<Player, StarData>();

		private GameController endTurnCopy = null;

		public GameController()
		{
			State = GameState.NoGame;
		}

		public void CreateGame(NewGameController controller)
		{
			if (State != GameState.NoGame)
				throw new InvalidOperationException("Game is already created.");

			StaticsDB statics = new StaticsDB();
			foreach(double p in statics.Load(StaticDataFiles))
				;
			
			Random rng = new Random();
			
			var starPositions = controller.StarPositioner.Generate(rng, controller.PlayerList.Count);
			var stars = controller.StarPopulator.Generate(rng, starPositions).ToArray();

			Player[] players = controller.PlayerList.Select(info =>
				new Player(info.Name, info.Color, info.Organization, info.ControlType)
			).ToArray();
			
			//var homeSystems = starPositions.HomeSystems.Select(x => stars[x].Star).ToArray();
			
			this.game = new Game(
				statics, 
				stars, 
				controller.StarConnector.Generate(rng, starPositions), 
				players,
				starPositions.HomeSystems,
				controller.SelectedStart
			);

			this.State = GameState.Running;

			//TODO: utilize stellar administration instead iterating colonies
			foreach (var player in players) {
				var colonies = game.States.Colonies.OwnedBy(player);
				var perStar = colonies.GroupBy(x => x.Star);
				var starPopulation = perStar.Select(x => new KeyValuePair<StarData, double>(
					x.Key, 
					x.Aggregate(0.0, (sum, colony) => sum + colony.Population)
				));
				var maxPopulationStar = starPopulation.Aggregate((a, b) => (a.Value > b.Value) ? a : b).Key;
				
				this.lastSelectedStar.Add(player, maxPopulationStar);
			}
		}

		public GameState State { get; private set; }

		public void EndTurn()
		{
			if (this.IsReadOnly)
				return;

			this.endTurnCopy = new GameController();
			var gameCopy = game.ReadonlyCopy();
			
			endTurnCopy.game = gameCopy.Item1;
			endTurnCopy.lastSelectedStar = new Dictionary<Player,StarData>();

			foreach (var originalSelection in lastSelectedStar)
				endTurnCopy.lastSelectedStar.Add(
					gameCopy.Item2.Players[originalSelection.Key],
					gameCopy.Item3.Stars[originalSelection.Value]);

			//UNDONE: start processing
			
			//TODO: Rotate players
			
			/*
			 * TODO: Preprocess players
 			 * - Calculate research points
 			 */
 			
 			/*
 			 * TODO: Preprocess stars
 			 * - Calculate system effects
 			 */
 			
 			/*
 			 * TODO: Colonies, 1st pass
 			 * - Build (consume construction queue)
 			 * - Apply instant effect buildings
 			 * - Apply terraforming
 			 * - Grow population
 			 */
 			
 			/*
 			 * TODO: Process stars
 			 * - Calculate effects from colonies
 			 * - Build
 			 * - Perform migration
 			 */
 			
 			// TODO: Colonise planets
 			
 			/*
 			 * TODO: Process ships
 			 * - Move ships
 			 * - Space combat
 			 * - Ground combat
 			 */
 			
 			// TODO: Research
 			
 			// TODO: Update ship designs
 			
 			// TODO: Upgrade and repair ships
 			
 			/*
 			 * TODO: Colonies, 2nd pass
 			 * - Apply normal effect buildings
 			 * - Check construction queue
 			 * - Recalculate colony effects
 			 */
		}

		public bool IsReadOnly
		{
			get { return endTurnCopy != null; }
		}
		
		#region Map related
		public bool IsStarVisited(StarData star)
		{
			var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;

			return game.Players[game.CurrentPlayer].Intelligence.About(star).IsVisited;
		}
		
		public IEnumerable<ColonyInfo> KnownColonies(StarData star)
		{
			var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;
			var starKnowledge = game.Players[game.CurrentPlayer].Intelligence.About(star);
			
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

				return lastSelectedStar[game.Players[game.CurrentPlayer]];
			}
			private set
			{
				var game = (this.IsReadOnly) ? this.endTurnCopy.game : this.game;
				
				this.lastSelectedStar[game.Players[game.CurrentPlayer]] = value;
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

		public IEnumerable<Tuple<StarData, StarData>> Wormholes
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

			var playerTechs = game.AdvancmentOrder(game.Players[game.CurrentPlayer]);
			var techLevels = playerTechs.ToDictionary(x => x.Topic.IdCode, x => x.Level);
			
			foreach(var techProgress in playerTechs)
				if (techProgress.Topic.Category == TechnologyCategory.Development && techProgress.CanProgress(x => techLevels[x]))
		        	yield return new TechnologyTopic(techProgress);
		}
		
		public void ReorderDevelopmentTopics(IEnumerable<string> idCodeOrder)
		{
			if (this.IsReadOnly)
				return;

			var modelQueue = game.Players[game.CurrentPlayer].Orders.DevelopmentQueue;
			modelQueue.Clear();
			
			int i = 0;
			foreach (var idCode in idCodeOrder) {
				modelQueue.Add(idCode, i);
				i++;
			}
		}
		#endregion
		
		private static readonly string[] StaticDataFiles = new string[] {
			"./data/buildings.txt",
			"./data/colonyFormulas.txt",
			"./data/techDevelopment.txt",
			"./data/techResearch.txt",
		};
	}
}
