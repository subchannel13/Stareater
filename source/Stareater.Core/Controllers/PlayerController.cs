using System;
using System.Collections.Generic;
using System.Linq;
using NGenerics.DataStructures.Mathematical;
using Stareater.Controllers.Data;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Ships;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils;

namespace Stareater.Controllers
{
	//TODO(later) filter invisible fleets
	public class PlayerController
	{
		public int PlayerIndex { get; private set; }
		private GameController gameController;
		
		internal PlayerController(int playerIndex, GameController gameController)
		{
			this.PlayerIndex = playerIndex;
			this.gameController = gameController;
		}
		
		internal Player PlayerInstance
		{
			get { return this.gameController.GameInstance.Players[this.PlayerIndex]; }
		}
		
		private MainGame gameInstance
		{
			get { return this.gameController.GameInstance; }
		}
		
		public PlayerInfo Info
		{
			get { return new PlayerInfo(this.PlayerInstance); }
		}

		public LibraryController Library 
		{
			get { return new LibraryController(this.gameController); }
		}
		
		#region Turn progression
		public void EndGalaxyPhase()
		{
			this.gameController.EndGalaxyPhase(this);
		}

		public bool IsReadOnly
		{
			get { return this.gameController.IsReadOnly; }
		}
		#endregion
			
		#region Map related
		public bool IsStarVisited(StarData star)
		{
			return this.PlayerInstance.Intelligence.About(star).IsVisited;
		}
		
		public IEnumerable<ColonyInfo> KnownColonies(StarData star)
		{
			var game = this.gameInstance;
			var starKnowledge = this.PlayerInstance.Intelligence.About(star);
			
			foreach(var colony in game.States.Colonies.AtStar(star))
				if (starKnowledge.Planets[colony.Location.Planet].LastVisited != PlanetIntelligence.NeverVisited)
					yield return new ColonyInfo(colony);
		}
		
		public StarSystemController OpenStarSystem(StarData star)
		{
			return new StarSystemController(this.gameInstance, star, this.IsReadOnly, this.PlayerInstance);
		}
		
		public StarSystemController OpenStarSystem(Vector2D position)
		{
			return this.OpenStarSystem(this.gameInstance.States.Stars.At(position));
		}
				
		public FleetController SelectFleet(FleetInfo fleet)
		{
			return new FleetController(fleet, this.gameInstance, this.PlayerInstance);
		}
		
		public IEnumerable<FleetInfo> Fleets
		{
			get
			{
				return this.gameInstance.States.Fleets.
					Where(x => x.Owner != this.PlayerInstance || !x.Owner.Orders.ShipOrders.ContainsKey(x.Position)).
					Concat(this.PlayerInstance.Orders.ShipOrders.SelectMany(x => x.Value)).
					Select(
						x => new FleetInfo(x, this.gameInstance.Derivates.Of(x.Owner), this.gameInstance.Statics)
					);
			}
		}
		
		public IEnumerable<FleetInfo> FleetsAt(Vector2D position)
		{
			var fleets = this.gameInstance.States.Fleets.At(position).Where(x => x.Owner != this.PlayerInstance || !x.Owner.Orders.ShipOrders.ContainsKey(x.Position));
			
			if (this.PlayerInstance.Orders.ShipOrders.ContainsKey(position))
				fleets = fleets.Concat(this.PlayerInstance.Orders.ShipOrders[position]);
			
			return fleets.Select(x => new FleetInfo(x, this.gameInstance.Derivates.Of(x.Owner), this.gameInstance.Statics));
		}
		
		public StarData Star(Vector2D position)
		{
			return this.gameInstance.States.Stars.At(position);
		}
		
		public int StarCount
		{
			get 
			{
				return this.gameInstance.States.Stars.Count;
			}
		}
		
		public IEnumerable<StarData> Stars
		{
			get
			{
				return this.gameInstance.States.Stars;
			}
		}

		public IEnumerable<Wormhole> Wormholes
		{
			get
			{
				foreach (var wormhole in this.gameInstance.States.Wormholes)
					yield return wormhole;
			}
		}
		#endregion
		
		#region Stellarises and colonies
		public IEnumerable<StellarisInfo> Stellarises()
		{
			foreach(var stellaris in this.gameInstance.States.Stellarises.OwnedBy(this.PlayerInstance))
				yield return new StellarisInfo(stellaris, this.gameInstance);
		}
		#endregion
		
		#region Ship designs
		public ShipDesignController NewDesign()
		{
			return (!this.gameController.IsReadOnly) ? new ShipDesignController(this.gameInstance, this.PlayerInstance) : null;
		}
		
		public IEnumerable<DesignInfo> ShipsDesigns()
		{
			var game = this.gameInstance;
			return game.States.Designs.
				OwnedBy(this.PlayerInstance).
				Select(x => new DesignInfo(x, game.Derivates.Of(this.PlayerInstance).DesignStats[x], game.Statics));
		}
		
		public void DisbandDesign(DesignInfo design)
		{
			this.PlayerInstance.Orders.RefitOrders[design.Data] = null;
		}

		public bool IsMarkedForRemoval(DesignInfo design)
		{
			return this.PlayerInstance.Orders.RefitOrders.ContainsKey(design.Data) && this.PlayerInstance.Orders.RefitOrders[design.Data] == null;
		}
		
		public void KeepDesign(DesignInfo design)
		{
			this.PlayerInstance.Orders.RefitOrders.Remove(design.Data);
		}
		
		public void RefitDesign(DesignInfo design, DesignInfo refitWith)
		{
			//TODO(v0.6) check refit compatibility, if designs are for same hull
			if (!refitWith.Constructable || this.PlayerInstance.Orders.RefitOrders.ContainsKey(refitWith.Data))
				return;
			
			this.PlayerInstance.Orders.RefitOrders[design.Data] = refitWith.Data;
		}
		
		public DesignInfo RefittingWith(DesignInfo design)
		{
			if (!this.PlayerInstance.Orders.RefitOrders.ContainsKey(design.Data))
				return null;
			
			var targetDesign = this.PlayerInstance.Orders.RefitOrders[design.Data];
			
			return targetDesign != null ?
				new DesignInfo(targetDesign, this.gameInstance.Derivates.Of(targetDesign.Owner).DesignStats[targetDesign], this.gameInstance.Statics) :
				null;
		}
		
		public long ShipCount(DesignInfo design)
		{
			return this.gameInstance.States.Fleets.
				OwnedBy(this.PlayerInstance).
				SelectMany(x => x.Ships).
				Where(x => x.Design == design.Data).
				Aggregate(0L, (sum, x) => sum + x.Quantity);
		}
		#endregion
		
		#region Colonization related
		public IEnumerable<ColonizationController> ColonizationProjects()
		{
			var planets = new HashSet<Planet>();
			planets.UnionWith(this.gameInstance.States.ColonizationProjects.OwnedBy(this.PlayerInstance).Select(x => x.Destination));
			planets.UnionWith(this.PlayerInstance.Orders.ColonizationOrders.Keys);
			
			foreach(var planet in planets)
				yield return new ColonizationController(this.gameInstance, planet, this.IsReadOnly, this.PlayerInstance);
		}
		
		public IEnumerable<FleetInfo> EnrouteColonizers(Planet destination)
		{
			var finder = new ColonizerFinder(destination);
			
			foreach(var fleet in this.gameInstance.States.Fleets.Where(x => x.Owner == this.PlayerInstance))
				if (finder.Check(fleet))
					yield return new FleetInfo(fleet, this.gameInstance.Derivates.Of(fleet.Owner), this.gameInstance.Statics);
		}
		#endregion
		
		#region Development related
		public IEnumerable<DevelopmentTopicInfo> DevelopmentTopics()
		{
			var game = this.gameInstance;
			var playerTechs = game.Derivates.Of(this.PlayerInstance).DevelopmentOrder(game.States.DevelopmentAdvances, game.States.ResearchAdvances, game.Statics);
		
			if (game.Derivates.Of(this.PlayerInstance).DevelopmentPlan == null)
				game.Derivates.Of(this.PlayerInstance).CalculateDevelopment(
					game.Statics,
					game.States,
					game.Derivates.Colonies.OwnedBy(this.PlayerInstance)
				);
			
			var investments = game.Derivates.Of(this.PlayerInstance).DevelopmentPlan.ToDictionary(x => x.Item);
			
			foreach(var techProgress in playerTechs)
				if (investments.ContainsKey(techProgress))
					yield return new DevelopmentTopicInfo(techProgress, investments[techProgress]);
				else
					yield return new DevelopmentTopicInfo(techProgress);
			
		}
		
		public IEnumerable<DevelopmentTopicInfo> ReorderDevelopmentTopics(IEnumerable<string> idCodeOrder)
		{
			if (this.IsReadOnly)
				return DevelopmentTopics();

			var modelQueue = this.PlayerInstance.Orders.DevelopmentQueue;
			modelQueue.Clear();
			
			int i = 0;
			foreach (var idCode in idCodeOrder) {
				modelQueue.Add(idCode, i);
				i++;
			}
			
			this.gameInstance.Derivates.Of(this.PlayerInstance).InvalidateDevelopment();
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
				return this.PlayerInstance.Orders.DevelopmentFocusIndex;
			}
			
			set
			{
				if (this.IsReadOnly)
					return;
				
				if (value >= 0 && value < this.gameInstance.Statics.DevelopmentFocusOptions.Count)
					this.PlayerInstance.Orders.DevelopmentFocusIndex = value;
				
				this.gameInstance.Derivates.Of(this.PlayerInstance).InvalidateDevelopment();
			}
		}
		
		public double DevelopmentPoints 
		{ 
			get
			{
				var game = this.gameInstance;
				
				return game.Derivates.Colonies.OwnedBy(this.PlayerInstance).Sum(x => x.Development);
			}
		}
		#endregion
		
		#region Research related
		public IEnumerable<ResearchTopicInfo> ResearchTopics()
		{
			var game = this.gameInstance;
			var playerTechs = game.Derivates.Of(this.PlayerInstance).ResearchOrder(game.States.ResearchAdvances);
		
			if (game.Derivates.Of(this.PlayerInstance).ResearchPlan == null)
				game.Derivates.Of(this.PlayerInstance).CalculateResearch(
					game.Statics,
					game.States,
					game.Derivates.Colonies.OwnedBy(this.PlayerInstance)
				);
			
			var investments = game.Derivates.Of(this.PlayerInstance).ResearchPlan.ToDictionary(x => x.Item);
			
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
				string focused = this.PlayerInstance.Orders.ResearchFocus;
				var playerTechs = game.Derivates.Of(this.PlayerInstance).ResearchOrder(game.States.ResearchAdvances).ToList();
				
				for (int i = 0; i < playerTechs.Count; i++)
					if (playerTechs[i].Topic.IdCode == focused)
						return i;
				
				return 0; //TODO(later) think of some smarter default research
			}
			
			set
			{
				if (this.IsReadOnly)
					return;
				
				var playerTechs = this.gameInstance.Derivates.Of(this.PlayerInstance).ResearchOrder(this.gameInstance.States.ResearchAdvances).ToList();
				if (value >= 0 && value < playerTechs.Count) {
					this.PlayerInstance.Orders.ResearchFocus = playerTechs[value].Topic.IdCode;
					this.gameInstance.Derivates.Of(this.PlayerInstance).InvalidateResearch();
				}
			}
		}
		#endregion
		
		#region Report related
		public IEnumerable<IReportInfo> Reports
		{
			get {
				var game = this.gameInstance;
				var wrapper = new ReportWrapper();
				
				foreach(var report in game.States.Reports.Of(this.PlayerInstance))
					yield return wrapper.Wrap(report);
			}
		}
		#endregion
	}
}
