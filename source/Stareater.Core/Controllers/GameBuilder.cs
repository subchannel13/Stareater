using System;
using System.Collections.Generic;
using System.IO;
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

namespace Stareater.Controllers
{
	static class GameBuilder
	{
		public static MainGame CreateGame(Random rng, Player[] players, NewGameController controller, IEnumerable<TextReader> staticDataSources)
		{
			var statics = StaticsDB.Load(staticDataSources);
			var states = createStates(rng, controller, players, statics);
			var derivates = createDerivates(players, controller.SelectedStart, statics, states);
			
			var game = new MainGame(players, statics, states, derivates);
			game.CalculateDerivedEffects();
			
			Stareater.AppData.Settings.Get.LastGame.StartConditions = controller.SelectedStart;
			return game;
		}
		
		public static MainGame LoadGame(IkonComposite saveData, IEnumerable<TextReader> staticDataSources)
		{
			var statics = StaticsDB.Load(staticDataSources);
			
			var deindexer = new ObjectDeindexer();
			int turn = saveData[MainGame.TurnKey].To<int>();
			
			deindexer.AddAll(statics.Constructables, x => x.IdCode);
			deindexer.AddAll(statics.PredeginedDesigns);
			deindexer.AddAll(statics.Armors.Values, x => x.IdCode);
			deindexer.AddAll(statics.Hulls.Values, x => x.IdCode);
			deindexer.AddAll(statics.IsDrives.Values, x => x.IdCode);
			deindexer.AddAll(statics.MissionEquipment.Values, x => x.IdCode);
			deindexer.AddAll(statics.Reactors.Values, x => x.IdCode);
			deindexer.AddAll(statics.Sensors.Values, x => x.IdCode);
			deindexer.AddAll(statics.Shields.Values, x => x.IdCode);
			deindexer.AddAll(statics.SpecialEquipment.Values, x => x.IdCode);
			deindexer.AddAll(statics.DevelopmentTopics, x => x.IdCode);
			deindexer.AddAll(statics.Thrusters.Values, x => x.IdCode);
			deindexer.AddAll(statics.Traits.Values, x => x.IdCode);
			
			var loadedStates = loadSaveData(saveData, deindexer, statics);
			var states = loadedStates.Item1;
			var players = loadedStates.Item2;
			var derivates = initDerivates(statics, players, states);
			
			var game = new MainGame(players.ToArray(), statics, states, derivates);
			game.CalculateDerivedEffects();
			
			return game;
		}
		
		#region Creation helper methods
		private static TemporaryDB createDerivates(Player[] players, StartingConditions startingConditions, StaticsDB statics, StatesDB states)
		{
			var derivates = new TemporaryDB(players, statics.DevelopmentTopics);
			
			initColonies(players, states.Colonies, startingConditions, derivates, statics);
			initStellarises(derivates, states.Stellarises);
			initPlayers(derivates, players, states, statics);
			
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
			var techAdvances = createTechAdvances(players, statics.DevelopmentTopics);

			return new StatesDB(stars, wormholes, planets, colonies, stellarises, techAdvances,
			                    new ReportCollection(), new DesignCollection(), new FleetCollection(),
			                    new ColonizationCollection());
		}
		
		private static ColonyCollection createColonies(Player[] players, 
			IList<StarSystem> starSystems, IList<int> homeSystemIndices, StartingConditions startingConditions)
		{
			var colonies = new ColonyCollection();
			for(int playerI = 0; playerI < players.Length; playerI++) {
				//TODO(later): pick top most suitable planets
				for(int colonyI = 0; colonyI < startingConditions.Colonies; colonyI++)
					colonies.Add(new Colony(
						1,	//TODO(v0.6): make a constant
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
		
		private static TechProgressCollection createTechAdvances(Player[] players, IEnumerable<DevelopmentTopic> technologies)
		{
			var techProgress = new TechProgressCollection();
			foreach(Player player in players)
				foreach(DevelopmentTopic tech in technologies)
					techProgress.Add(new DevelopmentProgress(tech, player));
			
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
					//TODO(later): add infrastructure to colony
					derivates.Colonies.Of(colony).CalculateBaseEffects(statics, derivates.Players.Of(player));
				}
			}
		}
		
		private static void initPlayers(TemporaryDB derivates, Player[] players, StatesDB states, StaticsDB statics)
		{
			foreach(Player player in players) {
				foreach(var colony in states.Colonies.OwnedBy(player))
					player.Orders.ConstructionPlans.Add(colony, new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio));
				
				foreach(var stellaris in states.Stellarises.OwnedBy(player))
					player.Orders.ConstructionPlans.Add(stellaris, new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio));
				
				player.Orders.DevelopmentFocusIndex = statics.DevelopmentFocusOptions.Count / 2;
				//TODO(v0.6) focus can be null when all research is done
				player.Orders.ResearchFocus = statics.ResearchTopics.First().IdCode;
				//player.Orders.ResearchFocus = derivates.Of(player).ResearchOrder(states.TechnologyAdvances).First().Topic.IdCode;
			}
			
			foreach (var player in players) {
				derivates.Players.Of(player).Initialize(statics, states);
				
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
		#endregion
		
		#region Loading helper methods
		private static Tuple<StatesDB, Player[]> loadSaveData(IkonComposite saveData, ObjectDeindexer deindexer, StaticsDB statics)
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
			
			var techs = new TechProgressCollection();
			foreach(var rawData in stateData[StatesDB.DevelopmentAdvancesKey].To<IEnumerable<IkonComposite>>())
				techs.Add(DevelopmentProgress.Load(rawData, deindexer));
			
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
			
			for(int i = 0; i < players.Count; i++)
				players[i].Orders = PlayerOrders.Load(ordersData[i].To<IkonComposite>(), deindexer);
				                                  
			return new Tuple<StatesDB, Player[]>(
				new StatesDB(stars, wormholes, planets, colonies, stellarises, techs, reports, designs, fleets, colonizations),
				players.ToArray()
			);
		}
		
		private static TemporaryDB initDerivates(StaticsDB statics, Player[] players, StatesDB states)
		{
			var derivates = new TemporaryDB(players, statics.DevelopmentTopics);
			
			foreach(var colony in states.Colonies) {
				var colonyProc = new ColonyProcessor(colony);
				colonyProc.CalculateBaseEffects(statics, derivates.Players.Of(colony.Owner));
				derivates.Colonies.Add(colonyProc);
			}
			
			foreach(var stellaris in states.Stellarises) {
				var stellarisProc = new StellarisProcessor(stellaris);
				stellarisProc.CalculateBaseEffects();
				derivates.Stellarises.Add(stellarisProc);
			}
			
			foreach (var player in players) {
				var playerProc = derivates.Players.Of(player);
				playerProc.Initialize(statics, states);
				
				foreach(var design in states.Designs.OwnedBy(player))
					playerProc.Analyze(design, statics);
			}

			return derivates;
		}
		#endregion
	}
}
