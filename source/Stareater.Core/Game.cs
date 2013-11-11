using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Utils;

namespace Stareater
{
	class Game
	{
		public Player[] Players { get; private set; }
		public int Turn { get; private set; }
		public int CurrentPlayer { get; private set; }
		private IEnumerable<object> conflicts; //TODO: make type
		private object phase; //TODO: make type

		public StaticsDB Statics { get; private set; }
		public StatesDB States { get; private set; }
		public TemporaryDB Derivates { get; private set; }
			
		public Game(StaticsDB statics, StarSystem[] starSystems, IEnumerable<Tuple<int, int>> wormholeEndpoints, Player[] players, int[] homeSystemIndices, StartingConditions startingConditions)
		{
			this.Turn = 0;
			
			this.Players = players;
			this.CurrentPlayer = 0;
			
			this.Derivates = new TemporaryDB(players, statics.Technologies);
			this.Statics = statics;
			this.States = new StatesDB(
				initStars(starSystems), 
				initWormholes(starSystems, wormholeEndpoints), 
				initPlanets(starSystems), 
				initColonies(players, starSystems, homeSystemIndices, startingConditions, this.Derivates, this.Statics.ColonyFormulas), 
				initTechAdvances(players, statics.Technologies)
			);
			
			foreach (var player in players) {
				player.Intelligence.Initialize(starSystems);
				
				foreach(var colony in States.Colonies.OwnedBy(player))
					player.Intelligence.StarFullyVisited(colony.Star, this.Turn);
			}
		}

		private Game()
		{ }

		public Tuple<Game, PlayersRemap, GalaxyRemap> ReadonlyCopy()
		{
			Game copy = new Game();

			GalaxyRemap galaxyRemap = this.States.CopyGalaxy();
			PlayersRemap playersRemap = this.States.CopyPlayers(
				this.Players.ToDictionary(x => x, x => x.Copy(galaxyRemap)),
				galaxyRemap);

			foreach (var playerPair in playersRemap.Players)
				playerPair.Value.Orders = playerPair.Key.Orders.Copy(playersRemap, galaxyRemap);

			copy.Players = this.Players.Select(p => playersRemap.Players[p]).ToArray();
			copy.Turn = this.Turn;
			copy.CurrentPlayer = this.CurrentPlayer;

			copy.Statics = this.Statics;
			copy.States = this.States.Copy(playersRemap.Players, galaxyRemap);
			copy.Derivates = this.Derivates.Copy(playersRemap);

			return new Tuple<Game, PlayersRemap, GalaxyRemap>(copy, playersRemap, galaxyRemap);
		}

		#region Initialization
		private static ColonyCollection initColonies(Player[] players, StarSystem[] starSystems, int[] homeSystemIndices, StartingConditions startingConditions, 
		                                             TemporaryDB derivates, ColonyFormulaSet colonyFormulas)
		{
			var colonies = new ColonyCollection();
			for(int playerI = 0; playerI < players.Length; playerI++) {
				var weights = new ChoiceWeights<Colony>();
				//TODO: pick top most suitable planets
				for(int colonyI = 0; colonyI < startingConditions.Colonies; colonyI++) {
					var colony = new Colony(players[playerI], starSystems[homeSystemIndices[playerI]].Planets[colonyI]);
					players[playerI].Orders.Constructions.Add(colony, new ConstructionOrders(ChangesDB.DefaultSiteSpendingRatio));
					
					var colonyProc = new ColonyProcessor(colony);
					colonyProc.Calculate(colonyFormulas, derivates.Players.Of(players[playerI]));
					derivates.Colonies.Add(colonyProc);
					//TODO: use habitability instead of population limit
					weights.Add(colony, colonyProc.MaxPopulation);
					
					colonies.Add(colony);
				}
				
				double totalPopulation = Math.Min(startingConditions.Population, weights.Total);
				double totalInfrastructure = Math.Min(startingConditions.Infrastructure, weights.Total);
				foreach(var colony in colonies.OwnedBy(players[playerI])) {
					colony.Population = weights.Relative(colony) * totalPopulation;
					colony.Infrastructure = weights.Relative(colony) * totalInfrastructure;
					derivates.Colonies.Of(colony).Calculate(colonyFormulas, derivates.Players.Of(players[playerI]));
				}
			}
			
			return colonies;
		}
			
		private static PlanetCollection initPlanets(StarSystem[] starSystems)
		{
			var planets = new PlanetCollection();
			foreach(var system in starSystems)
				planets.Add(system.Planets);
			
			return planets;
		}
		
		private static StarCollection initStars(StarSystem[] starList)
		{
			var stars = new StarCollection();
			stars.Add(starList.Select(x => x.Star));
			
			return stars;
		}
		
		private static TechProgressCollection initTechAdvances(Player[] players, IEnumerable<Technology> technologies)
		{
			var techProgress = new TechProgressCollection();
			foreach(Player player in players)
				foreach(Technology tech in technologies)
					techProgress.Add(new TechnologyProgress(tech, player));
			
			return techProgress;
		}
		
		private static WormholeCollection initWormholes(StarSystem[] starList, IEnumerable<Tuple<int, int>> wormholeEndpoints)
		{
			var wormholes = new WormholeCollection();
			wormholes.Add(wormholeEndpoints.Select(
				x => new Tuple<StarData, StarData>(
					starList[x.Item1].Star, 
					starList[x.Item2].Star
				)
			));
			
			return wormholes;
		}
		#endregion
		
		#region Technology related
		private int technologyOrderKey(TechnologyProgress tech)
		{
			if (tech.Owner.Orders.DevelopmentQueue.ContainsKey(tech.Topic.IdCode))
				return tech.Owner.Orders.DevelopmentQueue[tech.Topic.IdCode];
			
			if (tech.Order != TechnologyProgress.Unordered)
				return tech.Order;
			
			return int.MaxValue;
		}
		
		private int technologySort(TechnologyProgress leftTech, TechnologyProgress rightTech)
		{
			int primaryComparison = technologyOrderKey(leftTech).CompareTo(technologyOrderKey(rightTech));
			
			if (primaryComparison == 0)
				return leftTech.Topic.IdCode.CompareTo(rightTech.Topic.IdCode);
			
			return primaryComparison;
		}
		
		public IEnumerable<TechnologyProgress> AdvancmentOrder(Player player)
		{
			var playerTechs = States.TechnologyAdvances.Of(player).ToList();
			playerTechs.Sort(technologySort);
			
			return playerTechs;
		}
		#endregion
	}
}
