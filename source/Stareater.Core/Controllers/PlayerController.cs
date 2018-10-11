using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Construction;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Ships.Missions;
using Stareater.Utils;

namespace Stareater.Controllers
{
	public class PlayerController
	{
		public int PlayerIndex { get; private set; }
		private readonly GameController gameController;
		
		internal PlayerController(int playerIndex, GameController gameController)
		{
			this.PlayerIndex = playerIndex;
			this.gameController = gameController;
		}
		
		private MainGame gameInstance
		{
			get { return this.gameController.GameInstance; }
		}

		internal Player PlayerInstance(MainGame game)
		{
			if (this.PlayerIndex < game.MainPlayers.Length)
				return game.MainPlayers[this.PlayerIndex];
			else
				return game.StareaterOrganelles;
		}
		
		public PlayerInfo Info
		{
			get { return new PlayerInfo(this.PlayerInstance(this.gameInstance)); }
		}

		public StareaterController Stareater
		{
			get
			{
				var game = this.gameInstance;
				return new StareaterController(game, this.PlayerInstance(game));
			}
		}

		public LibraryController Library 
		{
			get { return new LibraryController(this.gameController); }
		}

		public int Turn
		{
			get { return this.gameInstance.Turn + 1; }
		}

		#region Turn progression
		public void EndGalaxyPhase()
		{
			this.gameController.EndGalaxyPhase(this);
		}

		public bool IsReadOnly
		{
			get { return this.gameInstance.IsReadOnly; }
		}
		#endregion
			
		#region Map related
		public bool IsStarVisited(StarInfo star)
		{
			return this.PlayerInstance(this.gameInstance).Intelligence.About(star.Data).IsVisited;
		}
		
		public IEnumerable<ColonyInfo> KnownColonies(StarInfo star)
		{
			var game = this.gameInstance;
			var starKnowledge = this.PlayerInstance(game).Intelligence.About(star.Data);
			
			foreach(var colony in game.States.Colonies.AtStar[star.Data])
				if (starKnowledge.Planets[colony.Location.Planet].LastVisited != PlanetIntelligence.NeverVisited)
					yield return new ColonyInfo(colony);
		}
		
		public StarSystemController OpenStarSystem(StarInfo star)
		{
			var game = this.gameInstance;

			if (!game.States.Stars.Contains(star.Data))
				throw new ArgumentException("Star doesn't exist");

			return new StarSystemController(game, star.Data, game.IsReadOnly, this);
		}
		
		public StarSystemController OpenStarSystem(Vector2D position)
		{
			return this.OpenStarSystem(new StarInfo(this.gameInstance.States.Stars.At[position]));
		}
				
		public FleetController SelectFleet(FleetInfo fleet)
		{
			var game = this.gameInstance;
			return new FleetController(fleet, game, this.PlayerInstance(game));
		}
		
		public IEnumerable<FleetInfo> Fleets
		{
			get
			{
				var game = this.gameInstance;
				var orders = game.Orders[this.PlayerInstance(game)];
				var player = this.PlayerInstance(game);
				var playerProc = game.Derivates[player];

				return game.States.Fleets.
					Where(x => playerProc.CanSee(x, game)).
					Concat(orders.ShipOrders.SelectMany(x => x.Value)).
					Select(
						x => new FleetInfo(x, game.Derivates[x.Owner], game.Statics)
					);
			}
		}
		
		public IEnumerable<FleetInfo> FleetsAt(Vector2D position)
		{
			var game = this.gameInstance;
			var player = this.PlayerInstance(game);
			var orders = game.Orders[this.PlayerInstance(game)];
			var fleets = game.States.Fleets.At[position].Where(x => x.Owner != player || !game.Orders[x.Owner].ShipOrders.ContainsKey(x.Position));

			if (orders.ShipOrders.ContainsKey(position))
				fleets = fleets.Concat(orders.ShipOrders[position]);
			
			return fleets.Select(x => new FleetInfo(x, game.Derivates[x.Owner], game.Statics));
		}
		
		public IEnumerable<Circle> ScanAreas()
		{
			var game = this.gameInstance;

			return game.Derivates[this.PlayerInstance(game)].ScanRanges.GetAll();
		}

		public StarInfo Star(Vector2D position)
		{
			return new StarInfo(this.gameInstance.States.Stars.At[position]);
		}
		
		public int StarCount
		{
			get 
			{
				return this.gameInstance.States.Stars.Count;
			}
		}
		
		public IEnumerable<StarInfo> Stars
		{
			get
			{
				return this.gameInstance.States.Stars.Select(x => new StarInfo(x));
			}
		}

		public IEnumerable<WormholeInfo> Wormholes
		{
			get
			{
				var game = this.gameInstance;
				var intell = this.PlayerInstance(game).Intelligence;

				return game.States.Wormholes.Where(intell.IsKnown).Select(x => new WormholeInfo(x));
			}
		}
		#endregion
		
		#region Stellarises and colonies
		public IEnumerable<StellarisInfo> Stellarises()
		{
			var game = this.gameInstance;
			foreach(var stellaris in game.States.Stellarises.OwnedBy[this.PlayerInstance(game)])
				yield return new StellarisInfo(stellaris, game);
		}
		#endregion
		
		#region Ship designs
		public ShipDesignController NewDesign()
		{
			var game = this.gameInstance;
			return (!game.IsReadOnly) ? new ShipDesignController(game, this.PlayerInstance(game)) : null;
		}
		
		public IEnumerable<DesignInfo> ShipsDesigns()
		{
			var game = this.gameInstance;
			return game.States.Designs.
				OwnedBy[this.PlayerInstance(game)].
				Select(x => new DesignInfo(x, game.Derivates[this.PlayerInstance(game)].DesignStats[x], game.Statics));
		}
		
		public void DisbandDesign(DesignInfo design)
		{
			var game = this.gameInstance;
			if (game.IsReadOnly)
				return;

			game.Orders[this.PlayerInstance(game)].RefitOrders[design.Data] = null;
		}

		public bool IsMarkedForRemoval(DesignInfo design)
		{
			var game = this.gameInstance;
			var player = this.PlayerInstance(game);
			var orders = game.Orders[player];
			return orders.RefitOrders.ContainsKey(design.Data) && orders.RefitOrders[design.Data] == null;
		}
		
		public void KeepDesign(DesignInfo design)
		{
			var game = this.gameInstance;
			if (game.IsReadOnly)
				return;

			game.Orders[this.PlayerInstance(game)].RefitOrders.Remove(design.Data);
		}
		
		public IEnumerable<DesignInfo> RefitCandidates(DesignInfo design)
		{
			var game = this.gameInstance;
			var playerProc = game.Derivates[this.PlayerInstance(game)];
			
			return playerProc.RefitCosts[design.Data].
				Where(x => !x.Key.IsObsolete && !x.Key.IsVirtual).
				Select(x => new DesignInfo(x.Key, playerProc.DesignStats[x.Key], game.Statics));
		}
		
		public double RefitCost(DesignInfo design, DesignInfo refitWith)
		{
			var game = this.gameInstance;
			return game.Derivates[this.PlayerInstance(game)].RefitCosts[design.Data][refitWith.Data];
		}
		
		public void RefitDesign(DesignInfo design, DesignInfo refitWith)
		{
			var game = this.gameInstance;
			if (game.IsReadOnly)
				return;
			
			var player = this.PlayerInstance(game);
			var playerProc = game.Derivates[this.PlayerInstance(game)];
			var orders = game.Orders[player];

			if (!refitWith.Constructable ||
				orders.RefitOrders.ContainsKey(refitWith.Data) || 
			    !playerProc.RefitCosts[design.Data].ContainsKey(refitWith.Data))
				return;

			orders.RefitOrders[design.Data] = refitWith.Data;
		}
		
		public DesignInfo RefittingWith(DesignInfo design)
		{
			var game = this.gameInstance;
			var player = this.PlayerInstance(game);
			var orders = game.Orders[player];

			if (game.IsReadOnly || !orders.RefitOrders.ContainsKey(design.Data))
				return null;
			
			var targetDesign = orders.RefitOrders[design.Data];
			
			return targetDesign != null ?
				new DesignInfo(targetDesign, game.Derivates[targetDesign.Owner].DesignStats[targetDesign], game.Statics) :
				null;
		}
		
		public long ShipCount(DesignInfo design)
		{
			var game = this.gameInstance;

			return game.States.Fleets.
				OwnedBy[this.PlayerInstance(game)].
				SelectMany(x => x.Ships).
				Where(x => x.Design == design.Data).
				Aggregate(0L, (sum, x) => sum + x.Quantity);
		}
		#endregion
		
		#region Colonization related
		public IEnumerable<ColonizationController> ColonizationProjects()
		{
			var game = this.gameInstance;
			var player = this.PlayerInstance(game);
			var planets = new HashSet<Planet>();
			planets.UnionWith(game.States.ColonizationProjects.OwnedBy[player].Select(x => x.Destination));
			planets.UnionWith(game.Orders[player].ColonizationOrders.Keys);
			
			foreach(var planet in planets)
				yield return new ColonizationController(game, planet, game.IsReadOnly, this);
		}
		
		public IEnumerable<FleetInfo> EnrouteColonizers(PlanetInfo destination)
		{
			var game = this.gameInstance;
			var player = this.PlayerInstance(game);

			foreach (var fleet in game.States.Fleets.OwnedBy[player].Where(x => x.Missions.Any(m => m is ColonizationMission)))
				yield return new FleetInfo(fleet, game.Derivates[fleet.Owner], game.Statics);
		}

		public DesignInfo ColonizerDesign
		{
			get
			{
				//TODO(v0.8) read order
				return null;
			}
			set
			{
				//TODO(v0.8) set order
			}
		}

		public IEnumerable<DesignInfo> ColonizerDesignOptions
		{
			get
			{
				var game = this.gameInstance;
				var playerProc = game.Derivates[this.PlayerInstance(game)];

				return playerProc.ColonizerDesignOptions.
					Select(x => new DesignInfo(x, playerProc.DesignStats[x], game.Statics));
			}
		}
		#endregion

		#region Development related
		public IEnumerable<DevelopmentTopicInfo> DevelopmentTopics()
		{
			var game = this.gameInstance;
			var player = this.PlayerInstance(game);
			var playerTechs = game.Derivates[player].DevelopmentOrder(game.States.DevelopmentAdvances, game.States.ResearchAdvances, game);

			if (game.Derivates[player].DevelopmentPlan == null)
				game.Derivates[player].CalculateDevelopment(
					game,
					game.Derivates.Colonies.OwnedBy[player]
				);

			var investments = game.Derivates[player].DevelopmentPlan.ToDictionary(x => x.Item);
			
			foreach(var techProgress in playerTechs)
				if (investments.ContainsKey(techProgress))
					yield return new DevelopmentTopicInfo(techProgress, investments[techProgress]);
				else
					yield return new DevelopmentTopicInfo(techProgress);
			
		}
		
		public IEnumerable<DevelopmentTopicInfo> ReorderDevelopmentTopics(IEnumerable<string> idCodeOrder)
		{
			var game = this.gameInstance;
			if (game.IsReadOnly)
				return DevelopmentTopics();

			var modelQueue = game.Orders[this.PlayerInstance(game)].DevelopmentQueue;
			modelQueue.Clear();
			
			int i = 0;
			foreach (var idCode in idCodeOrder) {
				modelQueue.Add(idCode, i);
				i++;
			}

			game.Derivates[this.PlayerInstance(game)].InvalidateDevelopment();
			return DevelopmentTopics();
		}
		
		public DevelopmentFocusInfo[] DevelopmentFocusOptions()
		{
			return this.gameInstance.Statics.DevelopmentFocusOptions.Select(x => new DevelopmentFocusInfo(x)).ToArray();
		}
		
		public int DevelopmentFocusIndex 
		{ 
			get
			{
				var game = this.gameInstance;
				return game.Orders[this.PlayerInstance(game)].DevelopmentFocusIndex;
			}
			
			set
			{
				var game = this.gameInstance;
				if (game.IsReadOnly)
					return;
				
				var player = this.PlayerInstance(game);

				if (value >= 0 && value < game.Statics.DevelopmentFocusOptions.Count)
					game.Orders[player].DevelopmentFocusIndex = value;

				game.Derivates[player].InvalidateDevelopment();
			}
		}
		
		public double DevelopmentPoints 
		{ 
			get
			{
				var game = this.gameInstance;
				
				return game.Derivates.Colonies.OwnedBy[this.PlayerInstance(game)].Sum(x => x.Development);
			}
		}
		#endregion
		
		#region Research related
		public IEnumerable<ResearchTopicInfo> ResearchTopics()
		{
			var game = this.gameInstance;
			var player = this.PlayerInstance(game);
			var playerTechs = game.Derivates[player].ResearchOrder(game.States.ResearchAdvances);

			if (game.Derivates[player].ResearchPlan == null)
				game.Derivates[player].CalculateResearch(
					game,
					game.Derivates.Colonies.OwnedBy[player]
				);

			var investments = game.Derivates[player].ResearchPlan.ToDictionary(x => x.Item);
			
			foreach(var techProgress in playerTechs)
				if (investments.ContainsKey(techProgress))
					yield return new ResearchTopicInfo(techProgress, investments[techProgress], game.Statics.DevelopmentTopics);
				else
					yield return new ResearchTopicInfo(techProgress, game.Statics.DevelopmentTopics);
		}
		
		public int ResearchFocus
		{
			get 
			{
				var game = this.gameInstance;
				string focused = game.Orders[this.PlayerInstance(game)].ResearchFocus;
				var playerTechs = game.Derivates[this.PlayerInstance(game)].ResearchOrder(game.States.ResearchAdvances).ToList();
				
				for (int i = 0; i < playerTechs.Count; i++)
					if (playerTechs[i].Topic.IdCode == focused)
						return i;
				
				return 0;
			}
			
			set
			{
				var game = this.gameInstance;
				if (game.IsReadOnly)
					return;

				var playerTechs = game.Derivates[this.PlayerInstance(game)].ResearchOrder(game.States.ResearchAdvances).ToList();
				if (value >= 0 && value < playerTechs.Count) 
				{
					game.Orders[this.PlayerInstance(game)].ResearchFocus = playerTechs[value].Topic.IdCode;
					game.Derivates[this.PlayerInstance(game)].InvalidateResearch();
				}
			}
		}
		#endregion
		
		#region Report related
		public IEnumerable<IReportInfo> Reports
		{
			get 
			{
				var game = this.gameInstance;
				var wrapper = new ReportWrapper();

				foreach (var report in game.States.Reports.Of[this.PlayerInstance(game)])
					yield return wrapper.Wrap(report);
			}
		}
		#endregion

		#region Diplomacy related
		public IEnumerable<ContactInfo> DiplomaticContacts()
		{
			var game = this.gameInstance;

			foreach (var player in game.MainPlayers)
				if (player != this.PlayerInstance(game))
					yield return new ContactInfo(player, game.States.Treaties.Of[this.PlayerInstance(game), player]);
		}

		public bool IsAudienceRequested(ContactInfo contact)
		{
			var game = this.gameInstance;
			var contactIndex = Array.IndexOf(game.MainPlayers, contact.PlayerData);
			
			return game.Orders[this.PlayerInstance(game)].AudienceRequests.Contains(contactIndex);
		}
		
		public void RequestAudience(ContactInfo contact)
		{
			var game = this.gameInstance;
			if (game.IsReadOnly)
				return;
			
			var contactIndex = Array.IndexOf(game.MainPlayers, contact.PlayerData);
			game.Orders[this.PlayerInstance(game)].AudienceRequests.Add(contactIndex);
		}
		
		public void CancelAudience(ContactInfo contact)
		{
			var game = this.gameInstance;
			if (game.IsReadOnly)
				return;
			
			var contactIndex = Array.IndexOf(game.MainPlayers, contact.PlayerData);
			game.Orders[this.PlayerInstance(game)].AudienceRequests.Remove(contactIndex);
		}
		#endregion

		#region Automation
		internal void UpdateAutomation()
		{
			var game = this.gameInstance;
			var player = this.PlayerInstance(game);
			var orders = game.Orders[player];
			foreach (var stellaris in game.States.Stellarises.OwnedBy[player])
				orders.AutomatedConstruction[stellaris] = new ConstructionOrders(0);

			var colonizerDesign = game.Derivates[player].ColonyShipDesign;
			var colonizationSources = orders.ColonizationOrders.Values.SelectMany(x => x.Sources).Distinct().ToList();
			var designStats = game.Derivates[player].DesignStats;

			//TODO(v0.8) calculate needed number of ships and adjust spending accordingly
			foreach (var source in colonizationSources)
			{
				var plan = orders.AutomatedConstruction[game.States.Stellarises.At[source, player].First()];
				plan.SpendingRatio = 1;
				plan.Queue.Add(new ShipProject(colonizerDesign));

				var colonizationOrder = orders.ColonizationOrders.Values.First(x => x.Sources.Contains(source));

				foreach (var fleet in this.FleetsAt(source.Position).Where(isColonizerFleet))
				{
					var controller = new FleetController(fleet, game, player);
					foreach(var group in controller.ShipGroups.Where(x => x.PopulationCapacity > 0))
					{
						var toSelect = (long)Math.Floor(group.Data.PopulationTransport / designStats[group.Data.Design].ColonizerPopulation);
						if (toSelect < 1)
							continue;

						controller.SelectGroup(group, toSelect);
					}

					controller.Send(new Vector2D[] { colonizationOrder.Destination.Star.Position });
				}
			}
		}

		private bool isColonizerFleet(FleetInfo fleet)
		{
			return fleet.FleetData.Missions.All(x => x is LoadMission) && fleet.PopulationCapacity > 0;
		}
		#endregion
	}
}
