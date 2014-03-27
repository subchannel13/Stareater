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

namespace Stareater.Controllers
{
	static class GameBuilder
	{
		public static Game CreateGame(Random rng, Player[] players, NewGameController controller)
		{
			StaticsDB statics = loadStatics();
			StatesDB states = createStates(rng, controller, players, statics.Technologies);
			TemporaryDB derivates = createDerivates(players, controller.SelectedStart, statics, states);
			
			var game = new Game(players, statics, states, derivates);
			game.CalculateBaseEffects();
			game.CalculateSpendings();
			game.CalculateDerivedEffects();
			
			return game;
		}
		
		private static StaticsDB loadStatics()
		{
			StaticsDB statics = new StaticsDB();
			foreach(double p in statics.Load(StaticDataFiles))
				;
			
			return statics;
		}
		
		private static TemporaryDB createDerivates(Player[] players, StartingConditions startingConditions, StaticsDB statics, StatesDB states)
		{
			var derivates = new TemporaryDB(players, statics.Technologies);
			
			initColonies(players, states.Colonies, startingConditions, derivates, statics);
			initStellarises(derivates, states.Stellarises);
			initPlayers(derivates, players, states, statics);
			
			return derivates;
		}
		
		private static  StatesDB createStates(Random rng, NewGameController newGameData, Player[] players, IEnumerable<Technology> technologies)
		{
			var starPositions = newGameData.StarPositioner.Generate(rng, newGameData.PlayerList.Count);
			var starSystems = newGameData.StarPopulator.Generate(rng, starPositions).ToArray();
			
			var stars = createStars(starSystems);
			var wormholes = createWormholes(starSystems, newGameData.StarConnector.Generate(rng, starPositions));
			var planets = createPlanets(starSystems);
			var colonies = createColonies(players, starSystems, starPositions.HomeSystems, newGameData.SelectedStart);
			var stellarises = createStellarises(players, starSystems, starPositions.HomeSystems);
			var techAdvances = createTechAdvances(players, technologies);

			return new StatesDB(stars, wormholes, planets, colonies, stellarises, techAdvances);
		}
		
		private static ColonyCollection createColonies(Player[] players, 
			StarSystem[] starSystems, int[] homeSystemIndices, StartingConditions startingConditions)
		{
			var colonies = new ColonyCollection();
			for(int playerI = 0; playerI < players.Length; playerI++) {
				//TODO(v0.5): pick top most suitable planets
				for(int colonyI = 0; colonyI < startingConditions.Colonies; colonyI++)
					colonies.Add(new Colony(
						players[playerI], 
						starSystems[homeSystemIndices[playerI]].Planets[colonyI]
					));
			}
			
			return colonies;
		}
		
		private static PlanetCollection createPlanets(StarSystem[] starSystems)
		{
			var planets = new PlanetCollection();
			foreach(var system in starSystems)
				planets.Add(system.Planets);
			
			return planets;
		}
		
		private static StarCollection createStars(StarSystem[] starList)
		{
			var stars = new StarCollection();
			stars.Add(starList.Select(x => x.Star));
			
			return stars;
		}
		
		private static StellarisCollection createStellarises(Player[] players, StarSystem[] starSystems, int[] homeSystemIndices)
		{
			var stellarises = new StellarisCollection();
			for(int playerI = 0; playerI < players.Length; playerI++)
				stellarises.Add(new StellarisAdmin(
					players[playerI], 
					starSystems[homeSystemIndices[playerI]].Star
				));
			
			return stellarises;
		}
		
		private static WormholeCollection createWormholes(StarSystem[] starList, IEnumerable<WormholeEndpoints> wormholeEndpoints)
		{
			var wormholes = new WormholeCollection();
			wormholes.Add(wormholeEndpoints.Select(
				x => new Wormhole(
					starList[x.FromIndex].Star, 
					starList[x.ToIndex].Star
				)
			));
			
			return wormholes;
		}
		
		private static TechProgressCollection createTechAdvances(Player[] players, IEnumerable<Technology> technologies)
		{
			var techProgress = new TechProgressCollection();
			foreach(Player player in players)
				foreach(Technology tech in technologies)
					techProgress.Add(new TechnologyProgress(tech, player));
			
			return techProgress;
		}
		
		private static void initColonies(Player[] players, ColonyCollection colonies, StartingConditions startingConditions, 
		                                 TemporaryDB derivates, StaticsDB statics)
		{
			foreach(Colony colony in colonies) {
				var colonyProc = new ColonyProcessor(colony);
				
				colonyProc.CalculateBaseEffects(statics, derivates.Players.Of(colony.Owner));
				derivates.Colonies.Add(colonyProc);
			}
			
			foreach(Player player in players) {
				var weights = new ChoiceWeights<Colony>();
				
				foreach(Colony colony in colonies.OwnedBy(player))
					weights.Add(colony, derivates.Colonies.Of(colony).MaxPopulation);
				
				double totalPopulation = Math.Min(startingConditions.Population, weights.Total);
				double totalInfrastructure = Math.Min(startingConditions.Infrastructure, weights.Total);
				
				foreach(var colony in colonies.OwnedBy(player)) {
					colony.Population = weights.Relative(colony) * totalPopulation;
					//TODO(v0.5): add infrastructure to colony
					derivates.Colonies.Of(colony).CalculateBaseEffects(statics, derivates.Players.Of(player));
				}
			}

		}
		
		private static void initPlayers(TemporaryDB derivates, Player[] players, StatesDB states, StaticsDB statics)
		{
			foreach(Player player in players) {
				foreach(var colony in states.Colonies.OwnedBy(player))
					player.Orders.ConstructionPlans.Add(colony, new ConstructionOrders(ChangesDB.DefaultSiteSpendingRatio));
				
				foreach(var stellaris in states.Stellarises.OwnedBy(player))
					player.Orders.ConstructionPlans.Add(stellaris, new ConstructionOrders(ChangesDB.DefaultSiteSpendingRatio));
			}
			
			foreach (var player in players) {
				derivates.Players.Of(player).Calculate(states.TechnologyAdvances.Of(player));
				derivates.Players.Of(player).UnlockPredefinedDesigns(statics, states);
				
				player.Intelligence.Initialize(states.Stars.Select(
					star => new StarSystem(star, states.Planets.At(star).ToArray())
				));
				
				foreach(var colony in states.Colonies.OwnedBy(player))
					player.Intelligence.StarFullyVisited(colony.Star, 0);
			}
		}
		
		private static void initStellarises(TemporaryDB derivates, IEnumerable<StellarisAdmin> stellarises)
		{
			foreach(var stellaris in stellarises)
				derivates.Stellarises.Add(new StellarisProcessor(stellaris));
		}
		
		//TODO(later): try to avoid explicit list of files
		private static readonly string[] StaticDataFiles = new string[] {
			"./data/colonyBuildings.txt",
			"./data/colonyFormulas.txt",
			"./data/predefinedDesigns.txt",
			"./data/shipHulls.txt",
			"./data/systemBuildings.txt",
			"./data/techDevelopment.txt",
			"./data/techResearch.txt",
		};
	}
}
