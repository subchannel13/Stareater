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
using Stareater.Ships;
using Stareater.Utils;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using Stareater.GameLogic.Planning;
using Ikadn.Utilities;

namespace Stareater.Controllers
{
	static class GameBuilder
	{
		public static MainGame CreateGame(Random rng, Player[] players, Player organellePlayer, NewGameController controller)
		{
			var statics = controller.Statics;
			var states = createStates(rng, controller, players, statics);
			var derivates = createDerivates(players, organellePlayer, statics, states);
			
			var game = new MainGame(players, organellePlayer, statics, states, derivates);
			initColonies(game, controller.SelectedStart);
			initStellarises(game);
			initOrders(game);
			initPlayers(game);
			game.CalculateDerivedEffects();
			
			controller.SaveLastGame();
			return game;
		}
		
		public static MainGame LoadGame(IkonComposite saveData, IEnumerable<NamedStream> staticDataSources, StateManager stateManager)
		{
			var statics = StaticsDB.Load(staticDataSources);
			
			var deindexer = new ObjectDeindexer();

			deindexer.AddAll(PlayerAssets.OrganizationsRaw);
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
			deindexer.AddAll(statics.PlanetTraits.Values);
			deindexer.AddAll(statics.StarTraits.Values);

			var game = stateManager.Load<MainGame>(
				saveData.To<IkonComposite>(), 
				deindexer
			);

			var derivates = initDerivates(statics, game.MainPlayers, game.StareaterOrganelles, game.States);
			game.Initialze(statics, derivates);
			foreach (var player in game.MainPlayers)
			{
				var playerProc = derivates.Players.Of[player];
				playerProc.Initialize(game);
			}
			game.CalculateDerivedEffects();

			return game;
		}
		
		#region Creation helper methods
		private static TemporaryDB createDerivates(Player[] players, Player organellePlayer, StaticsDB statics, StatesDB states)
		{
			var derivates = new TemporaryDB(players, organellePlayer, statics.DevelopmentTopics);
			
			derivates.Natives.Initialize(states, statics, derivates);
			
			return derivates;
		}

		private static StatesDB createStates(Random rng, NewGameController newGameData, Player[] players, StaticsDB statics)
		{
			var starPositions = newGameData.StarPositioner.Generate(rng, newGameData.PlayerList.Count);
			var starSystems = newGameData.StarPopulator.Generate(rng, new SystemEvaluator(statics), starPositions).ToArray();
			
			var stars = createStars(starSystems);
			var wormholes = createWormholes(starSystems, newGameData.StarConnector.Generate(rng, starPositions));
			var planets = createPlanets(starSystems);
			var colonies = createColonies(players, starSystems, starPositions.HomeSystems, newGameData.SelectedStart, statics);
			var stellarises = createStellarises(players, starSystems, starPositions.HomeSystems);
			var developmentAdvances = createDevelopmentAdvances(players, statics.DevelopmentTopics);
			var researchAdvances = createResearchAdvances(players, statics.ResearchTopics);

			foreach (var star in stars)
			{
				foreach (var trait in star.Traits)
					trait.InitialApply(statics, star, planets.At[star]);
			}

			return new StatesDB(stars, starSystems[starPositions.StareaterMain].Star, wormholes, planets,
				colonies, stellarises, developmentAdvances, researchAdvances,
				new HashSet<Pair<Player>>(), new TreatyCollection(), new ReportCollection(), new DesignCollection(), new FleetCollection(),
				new ColonizationCollection()); ;
		}
		
		private static ColonyCollection createColonies(Player[] players, IList<StarSystemBuilder> starSystems, 
			IList<int> homeSystemIndices, StartingConditions startingConditions, StaticsDB statics)
		{
			var colonies = new ColonyCollection();
			for(int playerI = 0; playerI < players.Length; playerI++)
			{
				var planets = new HashSet<Planet>(starSystems[homeSystemIndices[playerI]].Planets);
				var fitness = planets.
					ToDictionary(x => x, x => ColonyProcessor.DesirabilityOf(x, statics));

				while (planets.Count > startingConditions.Colonies)
					planets.Remove(Methods.FindWorst(planets, x => fitness[x]));

				foreach (var planet in planets)
					colonies.Add(new Colony(0, planet, players[playerI]));
			}
			
			return colonies;
		}
		
		private static PlanetCollection createPlanets(IEnumerable<StarSystemBuilder> starSystems)
		{
			var planets = new PlanetCollection();
			foreach(var system in starSystems)
				planets.Add(system.Planets);
			
			return planets;
		}
		
		private static StarCollection createStars(IEnumerable<StarSystemBuilder> starList)
		{
			var stars = new StarCollection
			{
				starList.Select(x => x.Star)
			};

			return stars;
		}
		
		private static StellarisCollection createStellarises(Player[] players, IList<StarSystemBuilder> starSystems, IList<int> homeSystemIndices)
		{
			var stellarises = new StellarisCollection();
			for(int playerI = 0; playerI < players.Length; playerI++)
				stellarises.Add(new StellarisAdmin(
					starSystems[homeSystemIndices[playerI]].Star,
					players[playerI]
				));
			
			return stellarises;
		}
		
		private static WormholeCollection createWormholes(IList<StarSystemBuilder> starList, IEnumerable<WormholeEndpoints> wormholeEndpoints)
		{
			var wormholes = new WormholeCollection
			{
				wormholeEndpoints.Select(
				x => new Wormhole(
					starList[x.FromIndex].Star,
					starList[x.ToIndex].Star
				)
			)
			};

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

		private static void initColonies(MainGame game, StartingConditions startingConditions)
		{
			var colonies = game.States.Colonies;
			foreach (var colony in colonies) {
				var colonyProc = new ColonyProcessor(colony);
				
				colonyProc.CalculateBaseEffects(game.Statics, game.Derivates.Players.Of[colony.Owner]);
				game.Derivates.Colonies.Add(colonyProc);
			}
			
			foreach(var player in game.MainPlayers) {
				var weights = new ChoiceWeights<Colony>();

				foreach (var colony in colonies.OwnedBy[player])
					weights.Add(colony, game.Derivates.Colonies.Of[colony].Desirability);

				Methods.DistributePoints(
					startingConditions.Population, 
					colonies.OwnedBy[player].Select(colony => new PointReceiver(
						game.Derivates.Colonies.Of[colony].Desirability,
						game.Derivates.Colonies.Of[colony].MaxPopulation,
						x => { colony.Population = x; }
					))
				);

				var playerProcessor = game.Derivates[player];
				foreach (var building in startingConditions.Buildings)
				{
					var project = game.Statics.Constructables.First(x => x.IdCode == building.Id);

					Methods.DistributePoints(
						startingConditions.Population,
						colonies.OwnedBy[player].Select(colony => new PointReceiver(
							game.Derivates.Colonies.Of[colony].Desirability,
							project.TurnLimit.Evaluate(
								game.Derivates[colony].
								LocalEffects(game.Statics).
								UnionWith(playerProcessor.TechLevels).Get
							),
							x => 
							{
								foreach (var effect in project.Effects)
									effect.Apply(game, colony, (long)x);
							}
						))
					);
				}
			}
		}

		private static void initOrders(MainGame game)
		{
			foreach (var player in game.AllPlayers)
			{
				var orders = game.Orders[player];

				foreach (var colony in game.States.Colonies.OwnedBy[player])
				{
					orders.ConstructionPlans[colony] = new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio);
					orders.AutomatedConstruction[colony] = new ConstructionOrders(0);
				}

				foreach (var stellaris in game.States.Stellarises.OwnedBy[player])
				{
					orders.ConstructionPlans.Add(stellaris, new ConstructionOrders(PlayerOrders.DefaultSiteSpendingRatio));
					orders.AutomatedConstruction[stellaris] = new ConstructionOrders(0);
					orders.Policies.Add(stellaris, game.Statics.Policies.First());
					orders.ColonizationSources.Add(stellaris);
				}

				orders.DevelopmentFocusIndex = game.Statics.DevelopmentFocusOptions.Count / 2;
				orders.ResearchFocus = game.Statics.ResearchTopics.Select(x => x.IdCode).FirstOrDefault() ?? "";
			}
		}

		private static void initPlayers(MainGame game)
		{
			foreach (var player in game.MainPlayers)
			{
				var developments = game.States.DevelopmentAdvances.Of;
                foreach (var topic in player.Organization.ResearchAffinities)
				{
					var research = game.States.ResearchAdvances.Of[player, topic];
					if (!research.CanProgress)
						continue;

					research.Progress(new ResearchResult(1, 0, research, 0));
					foreach (var unlock in research.Topic.Unlocks[research.Level])
						developments[player, unlock].Priority = 0;
				}
				foreach (var topic in developments[player].Where(x => !game.Statics.DevelopmentRequirements.ContainsKey(x.Topic.IdCode)))
					topic.Progress(new DevelopmentResult(1, 0, topic, 0));

				game.Derivates.Players.Of[player].Initialize(game);
				
				player.Intelligence.Initialize(game.States);
				
				foreach(var stellaris in game.States.Stellarises.OwnedBy[player])
					player.Intelligence.StarFullyVisited(stellaris.Location.Star, game.States);
			}
		}
		
		private static void initStellarises(MainGame game)
		{
			foreach(var stellaris in game.States.Stellarises)
				game.Derivates.Stellarises.Add(new StellarisProcessor(stellaris));
		}
		#endregion
		
		#region Loading helper methods		
		private static TemporaryDB initDerivates(StaticsDB statics, Player[] players, Player organellePlayer, StatesDB states)
		{
			var derivates = new TemporaryDB(players, organellePlayer, statics.DevelopmentTopics);
			
			foreach(var colony in states.Colonies) 
				derivates.Colonies.Add(new ColonyProcessor(colony));
			
			foreach(var stellaris in states.Stellarises)
				derivates.Stellarises.Add(new StellarisProcessor(stellaris));
			
			derivates.Natives.Initialize(states, statics, derivates);

			return derivates;
		}
		#endregion
	}
}
