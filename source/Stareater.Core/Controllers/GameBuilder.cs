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
			int turn = saveData[MainGame.TurnKey].To<int>();
			
			deindexer.AddAll(statics.Constructables, x => x.IdCode);
			deindexer.AddAll(statics.DevelopmentTopics, x => x.IdCode);
			deindexer.AddAll(statics.PredeginedDesigns);
			deindexer.AddAll(statics.ResearchTopics, x => x.IdCode);
			deindexer.AddAll(statics.Armors.Values, x => x.IdCode);
			deindexer.AddAll(statics.Hulls.Values, x => x.IdCode);
			deindexer.AddAll(statics.IsDrives.Values, x => x.IdCode);
			deindexer.AddAll(statics.MissionEquipment.Values, x => x.IdCode);
			deindexer.AddAll(statics.Reactors.Values, x => x.IdCode);
			deindexer.AddAll(statics.Sensors.Values, x => x.IdCode);
			deindexer.AddAll(statics.Shields.Values, x => x.IdCode);
			deindexer.AddAll(statics.SpecialEquipment.Values, x => x.IdCode);
			deindexer.AddAll(statics.Thrusters.Values, x => x.IdCode);
			deindexer.AddAll(statics.Traits.Values, x => x.IdCode);

			return stateManager.Load<MainGame>(saveData);
			var loadedStates = loadSaveData(saveData, deindexer, statics);
			var states = loadedStates.Item1;
			var players = loadedStates.Item2;
			var organellePlayer = loadedStates.Item3;
			var derivates = initDerivates(statics, players, organellePlayer, states);
			
			var game = new MainGame(players.ToArray(), organellePlayer, statics, states, derivates);
			foreach (var player in players)
			{
				var playerProc = derivates.Players.Of[player];
				playerProc.Initialize(game);

				foreach (var design in states.Designs.OwnedBy[player])
					playerProc.Analyze(design, statics);
			}

			game.Turn = turn;
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
		private static Tuple<StatesDB, Player[], Player> loadSaveData(IkonComposite saveData, ObjectDeindexer deindexer, StaticsDB statics)
		{
			var stateData = saveData[MainGame.StatesKey].To<IkonComposite>();
			var ordersData = saveData[MainGame.OrdersKey].To<IkonArray>();
			
			var stars = new StarCollection();
			foreach(var rawData in stateData[StatesDB.StarsKey].To<IEnumerable<IkonComposite>>())
				stars.Add(StarData.Load(rawData, deindexer));
			
			var planets = new PlanetCollection();
			foreach(var rawData in stateData[StatesDB.PlanetsKey].To<IEnumerable<IkonComposite>>())
				planets.Add(Planet.Load(rawData, deindexer));
			
			var wormholes = new WormholeCollection();
			foreach(var rawData in stateData[StatesDB.WormholesKey].To<IEnumerable<IkonComposite>>())
				wormholes.Add(Wormhole.Load(rawData, deindexer));
			
			var players = new List<Player>();
			foreach(var rawData in saveData[MainGame.PlayersKey].To<IEnumerable<IkonComposite>>())
				players.Add(Player.Load(rawData, deindexer));

			var organellePlayer = Player.Load(saveData[MainGame.OrganellePlayerKey].To<IkonComposite>(), deindexer);

			var developments = new DevelopmentProgressCollection();
			foreach (var rawData in stateData[StatesDB.DevelopmentAdvancesKey].To<IEnumerable<IkonComposite>>())
				developments.Add(DevelopmentProgress.Load(rawData, deindexer));

			var research = new ResearchProgressCollection();
			foreach (var rawData in stateData[StatesDB.ResearchAdvancesKey].To<IEnumerable<IkonComposite>>())
				research.Add(ResearchProgress.Load(rawData, deindexer));
			
			var treaties = new TreatyCollection();
			foreach (var rawData in stateData[StatesDB.TreatiesKey].To<IEnumerable<IkonComposite>>())
				treaties.Add(Treaty.Load(rawData, deindexer));
				
			var reports = new ReportCollection();
			foreach(var rawData in stateData[StatesDB.ReportsKey].To<IEnumerable<IkonComposite>>())
				reports.Add(ReportFactory.Load(rawData, deindexer));
			        
			var designs = new DesignCollection();
			foreach(var rawData in stateData[StatesDB.DesignsKey].To<IEnumerable<IkonComposite>>()) {
				var design = Design.Load(rawData, deindexer);
				design.CalcHash(statics);
				designs.Add(design);
				deindexer.Add(design.ConstructionProject, design.ConstructionProject.IdCode);
			}
			
			var colonizations = new ColonizationCollection();
			foreach(var rawData in stateData[StatesDB.ColonizationKey].To<IEnumerable<IkonComposite>>())
				colonizations.Add(ColonizationProject.Load(rawData, deindexer));
				
			var fleets = new FleetCollection();
			foreach(var rawData in stateData[StatesDB.IdleFleetsKey].To<IEnumerable<IkonComposite>>())
				fleets.Add(Fleet.Load(rawData, deindexer));
			
			var colonies = new ColonyCollection();
			foreach(var rawData in stateData[StatesDB.ColoniesKey].To<IEnumerable<IkonComposite>>())
				colonies.Add(Colony.Load(rawData, deindexer));
			
			var stellarises = new StellarisCollection();
			foreach(var rawData in stateData[StatesDB.StellarisesKey].To<IEnumerable<IkonComposite>>())
				stellarises.Add(StellarisAdmin.Load(rawData, deindexer));
			
			/*for(int i = 0; i < players.Count; i++)
				players[i].Orders = PlayerOrders.Load(ordersData[i].To<IkonComposite>(), deindexer);
			organellePlayer.Orders = PlayerOrders.Load(saveData[MainGame.OrganelleOrdersKey].To<IkonComposite>(), deindexer);
			*/

			return new Tuple<StatesDB, Player[], Player>(
				new StatesDB(stars, wormholes, planets, colonies, stellarises, developments, research, treaties, reports, designs, fleets, colonizations),
				players.ToArray(),
				organellePlayer
			);
		}
		
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
