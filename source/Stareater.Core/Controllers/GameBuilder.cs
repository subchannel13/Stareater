using System;
using System.Collections.Generic;
using System.Linq;

using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.GameData.Databases.Tables;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.Players.Reports;
using Stareater.Ships;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.Controllers
{
	static class GameBuilder
	{
		public static MainGame CreateGame(Random rng, Player[] players, Player organellePlayer, NewGameController controller, IEnumerable<TracableStream> staticDataSources)
		{
			var statics = StaticsDB.Load(staticDataSources);
			var states = createStates(rng, controller, players, statics);
			var derivates = createDerivates(players, organellePlayer, controller.SelectedStart, statics, states);
			
			var game = new MainGame(players, organellePlayer, statics, states, derivates);
			initOrders(game);
			initPlayers(game);
			game.CalculateDerivedEffects();
			
			controller.SaveLastGame();
			return game;
		}
		
		public static MainGame LoadGame(IkonComposite saveData, IEnumerable<TracableStream> staticDataSources, StateManager stateManager)
		{
			var statics = StaticsDB.Load(staticDataSources);
			
			var deindexer = new ObjectDeindexer();
			
			deindexer.AddAll(statics.Constructables);
			deindexer.AddAll(statics.DevelopmentTopics);
			deindexer.AddAll(statics.PredeginedDesigns);
			deindexer.AddAll(statics.ResearchTopics);
			deindexer.AddAll(statics.Armors.Values);
			deindexer.AddAll(statics.Hulls.Values);
			deindexer.AddAll(statics.IsDrives.Values);
			deindexer.AddAll(statics.MissionEquipment.Values);
			deindexer.AddAll(statics.Reactors.Values);
			deindexer.AddAll(statics.Sensors.Values);
			deindexer.AddAll(statics.Shields.Values);
			deindexer.AddAll(statics.SpecialEquipment.Values);
			deindexer.AddAll(statics.Thrusters.Values);
			deindexer.AddAll(statics.Traits.Values);

			var game = stateManager.Load<MainGame>(
				saveData.To<IkonComposite>(), 
				deindexer,
				new Dictionary<Type, Action<object>>()
				{
					{ typeof(Design), x => ((Design)x).CalcHash(statics) }
				}
			);

			var derivates = initDerivates(statics, game.MainPlayers, game.StareaterOrganelles, game.States);
			game.Initialze(statics, derivates);
			foreach (var player in game.MainPlayers)
			{
				var playerProc = derivates.Players.Of[player];
				playerProc.Initialize(game);

				foreach (var design in game.States.Designs.OwnedBy[player])
					playerProc.Analyze(design, statics);
			}
			game.CalculateDerivedEffects();

			return game;
		}
		
		#region Creation helper methods
		private static TemporaryDB createDerivates(Player[] players, Player organellePlayer, StartingConditions startingConditions, StaticsDB statics, StatesDB states)
		{
			var derivates = new TemporaryDB(players, organellePlayer, statics.DevelopmentTopics);
			
			initColonies(players, states.Colonies, startingConditions, derivates, statics);
			initStellarises(derivates, states.Stellarises);
			
			derivates.Natives.Initialize(states, statics, derivates);
			
			return derivates;
		}

		private static StatesDB createStates(Random rng, NewGameController newGameData, Player[] players, StaticsDB statics)
		{
			var starPositions = newGameData.StarPositioner.Generate(rng, newGameData.PlayerList.Count);
			var starSystems = newGameData.StarPopulator.Generate(rng, starPositions, statics.Traits.Values).ToArray();
			
			var stars = createStars(starSystems);
			var wormholes = createWormholes(starSystems, newGameData.StarConnector.Generate(rng, starPositions));
			var planets = createPlanets(starSystems);
			var colonies = createColonies(players, starSystems, starPositions.HomeSystems, newGameData.SelectedStart);
			var stellarises = createStellarises(players, starSystems, starPositions.HomeSystems);
			var developmentAdvances = createDevelopmentAdvances(players, statics.DevelopmentTopics);
			var researchAdvances = createResearchAdvances(players, statics.ResearchTopics);

			return new StatesDB(stars, wormholes, planets, colonies, stellarises, developmentAdvances, researchAdvances,
			                    new TreatyCollection(), new ReportCollection(), new DesignCollection(), new FleetCollection(),
			                    new ColonizationCollection());
		}
		
		private static ColonyCollection createColonies(Player[] players, 
			IList<StarSystem> starSystems, IList<int> homeSystemIndices, StartingConditions startingConditions)
		{
			var colonies = new ColonyCollection();
			for(int playerI = 0; playerI < players.Length; playerI++) {
				//TODO(v0.7): pick top most suitable planets
				for(int colonyI = 0; colonyI < startingConditions.Colonies; colonyI++)
					colonies.Add(new Colony(
						1,	//TODO(v0.7): make a constant
						starSystems[homeSystemIndices[playerI]].Planets[colonyI],
						players[playerI]
					));
			}
			
			return colonies;
		}
		
		private static PlanetCollection createPlanets(IEnumerable<StarSystem> starSystems)
		{
			var planets = new PlanetCollection();
			foreach(var system in starSystems)
				planets.Add(system.Planets);
			
			return planets;
		}
		
		private static StarCollection createStars(IEnumerable<StarSystem> starList)
		{
			var stars = new StarCollection();
			stars.Add(starList.Select(x => x.Star));
			
			return stars;
		}
		
		private static StellarisCollection createStellarises(Player[] players, IList<StarSystem> starSystems, IList<int> homeSystemIndices)
		{
			var stellarises = new StellarisCollection();
			for(int playerI = 0; playerI < players.Length; playerI++)
				stellarises.Add(new StellarisAdmin(
					starSystems[homeSystemIndices[playerI]].Star,
					players[playerI]
				));
			
			return stellarises;
		}
		
		private static WormholeCollection createWormholes(IList<StarSystem> starList, IEnumerable<WormholeEndpoints> wormholeEndpoints)
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
		
		private static DevelopmentProgressCollection createDevelopmentAdvances(Player[] players, IEnumerable<DevelopmentTopic> technologies)
		{
			var techProgress = new DevelopmentProgressCollection();
			foreach(var player in players)
				foreach(var tech in technologies)
					techProgress.Add(new DevelopmentProgress(tech, player));
			
			return techProgress;
		}

		private static ResearchProgressCollection createResearchAdvances(Player[] players, IEnumerable<ResearchTopic> technologies)
		{
			var techProgress = new ResearchProgressCollection();
			foreach (var player in players)
				foreach (var tech in technologies)
					techProgress.Add(new ResearchProgress(tech, player));

			return techProgress;
		}

		private static void initColonies(Player[] players, ColonyCollection colonies, StartingConditions startingConditions, 
		                                 TemporaryDB derivates, StaticsDB statics)
		{
			foreach(Colony colony in colonies) {
				var colonyProc = new ColonyProcessor(colony);
				
				colonyProc.CalculateBaseEffects(statics, derivates.Players.Of[colony.Owner]);
				derivates.Colonies.Add(colonyProc);
			}
			
			foreach(Player player in players) {
				var weights = new ChoiceWeights<Colony>();
				
				foreach(Colony colony in colonies.OwnedBy[player])
					weights.Add(colony, derivates.Colonies.Of[colony].MaxPopulation);
				
				double totalPopulation = Math.Min(startingConditions.Population, weights.Total);
				double totalInfrastructure = Math.Min(startingConditions.Infrastructure, weights.Total);
				
				foreach(var colony in colonies.OwnedBy[player]) {
					colony.Population = weights.Relative(colony) * totalPopulation;
					//TODO(v0.7): add infrastructure to colony
					derivates.Colonies.Of[colony].CalculateBaseEffects(statics, derivates.Players.Of[player]);
				}
			}
		}

		private static void initOrders(MainGame game)
		{
			foreach (var player in game.AllPlayers)
			{
				var orders = game.Orders[player];

				foreach (var colony in game.States.Colonies.OwnedBy[player])
					orders.ConstructionPlans.Add(colony, new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio));

				foreach (var stellaris in game.States.Stellarises.OwnedBy[player])
					orders.ConstructionPlans.Add(stellaris, new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio));

				orders.DevelopmentFocusIndex = game.Statics.DevelopmentFocusOptions.Count / 2;
				//TODO(v0.7) focus can be null when all research is done
				orders.ResearchFocus = game.Statics.ResearchTopics.First().IdCode;
			}
		}

		private static void initPlayers(MainGame game)
		{
			foreach (var player in game.MainPlayers) {
				game.Derivates.Players.Of[player].Initialize(game);
				
				player.Intelligence.Initialize(game.States.Stars.Select(
					star => new StarSystem(star, game.States.Planets.At[star].ToArray())
				));
				
				foreach(var colony in game.States.Colonies.OwnedBy[player])
					player.Intelligence.StarFullyVisited(colony.Star, 0);
			}
		}
		
		private static void initStellarises(TemporaryDB derivates, IEnumerable<StellarisAdmin> stellarises)
		{
			foreach(var stellaris in stellarises)
				derivates.Stellarises.Add(new StellarisProcessor(stellaris));
		}
		#endregion
		
		#region Loading helper methods		
		private static TemporaryDB initDerivates(StaticsDB statics, Player[] players, Player organellePlayer, StatesDB states)
		{
			var derivates = new TemporaryDB(players, organellePlayer, statics.DevelopmentTopics);
			
			foreach(var colony in states.Colonies) {
				var colonyProc = new ColonyProcessor(colony);
				colonyProc.CalculateBaseEffects(statics, derivates.Players.Of[colony.Owner]);
				derivates.Colonies.Add(colonyProc);
			}
			
			foreach(var stellaris in states.Stellarises) {
				var stellarisProc = new StellarisProcessor(stellaris);
				stellarisProc.CalculateBaseEffects();
				derivates.Stellarises.Add(stellarisProc);
			}
			
			derivates.Natives.Initialize(states, statics, derivates);

			return derivates;
		}
		#endregion
	}
}
