using System;
using System.Collections.Generic;
using System.Linq;
using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Ships;
using Stareater.Ships.Missions;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Databases
{
	class StatesDB
	{
		private const string DesignIdPrefix = "ShipDesign";
		
		public StarCollection Stars { get; private set; }
		public WormholeCollection Wormholes { get; private set; }
		
		public PlanetCollection Planets { get; private set; }
		public ColonyCollection Colonies { get; private set; }
		public StellarisCollection Stellarises { get; private set; }
		
		public ColonizationCollection ColonizationProjects { get; private set; }
		public FleetCollection Fleets { get; private set; }
		
		public DesignCollection Designs { get; private set; }
		public ReportCollection Reports { get; private set; }
		public TechProgressCollection TechnologyAdvances { get; private set; }
		
		private int nextDesignId = 0;
		
		public StatesDB(StarCollection stars, WormholeCollection wormholes, PlanetCollection planets, 
		                ColonyCollection Colonies, StellarisCollection stellarises, 
		                TechProgressCollection technologyProgresses, ReportCollection reports,
		                DesignCollection designs, FleetCollection idleFleets, ColonizationCollection colonizations)
		{
			this.Colonies = Colonies;
			this.Planets = planets;
			this.Stars = stars;
			this.Stellarises = stellarises;
			this.Wormholes = wormholes;
			this.TechnologyAdvances = technologyProgresses;
			this.Reports = reports;
			this.Designs = designs;
			this.Fleets = idleFleets;
			this.ColonizationProjects = colonizations;			
		}

		private StatesDB()
		{ }

		public string MakeDesignId()
		{
			this.nextDesignId++;
			
			return DesignIdPrefix + nextDesignId;
		}
		
		public StatesDB Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap)
		{
			var copy = new StatesDB();

			copy.Stars = new StarCollection();
			copy.Stars.Add(galaxyRemap.Stars.Values);

			copy.Wormholes = new WormholeCollection();
			copy.Wormholes.Add(this.Wormholes.Select(x => x.Copy(galaxyRemap)));

			copy.Planets = new PlanetCollection();
			copy.Planets.Add(galaxyRemap.Planets.Values);

			copy.Colonies = new ColonyCollection();
			copy.Colonies.Add(playersRemap.Colonies.Values);
			
			copy.Stellarises = new StellarisCollection();
			copy.Stellarises.Add(playersRemap.Stellarises.Values);

			copy.ColonizationProjects = new ColonizationCollection();
			copy.ColonizationProjects.Add(this.ColonizationProjects.Select(x => x.Copy(playersRemap, galaxyRemap)));

			copy.Fleets = new FleetCollection();
			copy.Fleets.Add(playersRemap.Fleets.Values);
			
			copy.Designs = new DesignCollection();
			copy.Designs.Add(playersRemap.Designs.Values);
			
			copy.TechnologyAdvances = new TechProgressCollection();
			copy.TechnologyAdvances.Add(this.TechnologyAdvances.Select(x => x.Copy(playersRemap)));
			
			return copy;
		}

		public GalaxyRemap CopyGalaxy()
		{
			var remap = new GalaxyRemap(new Dictionary<StarData, StarData>(), new Dictionary<Planet, Planet>());

			remap.Stars = this.Stars.ToDictionary(x => x, x => x.Copy());
			remap.Planets = this.Planets.ToDictionary(x => x, x => x.Copy(remap));

			return remap;
		}

		internal PlayersRemap CopyPlayers(Dictionary<Player, Player> playersRemap, GalaxyRemap galaxyRemap)
		{
			var remap = new PlayersRemap(
				playersRemap, 
				new Dictionary<AConstructionSite, Colony>(),
				new Dictionary<AConstructionSite, StellarisAdmin>(),
				new Dictionary<Design, Design>(),
				new Dictionary<Fleet, Fleet>(),
				new Dictionary<AMission, AMission>()
			);

			remap.Colonies = this.Colonies.ToDictionary(x => (AConstructionSite)x, x => x.Copy(remap, galaxyRemap));
			remap.Stellarises = this.Stellarises.ToDictionary(x => (AConstructionSite)x, x => x.Copy(remap, galaxyRemap));
			remap.Designs = this.Designs.ToDictionary(x => x, x => x.Copy(remap));
			remap.Missions = this.Fleets.ToDictionary(x => x.Mission, x => x.Mission.Copy(remap, galaxyRemap));
			remap.Fleets = this.Fleets.ToDictionary(x => x, x => x.Copy(remap));
			
			foreach(var player in playersRemap.Keys)
				foreach(var fleetOrders in player.Orders.ShipOrders.Values)
					foreach(var fleet in fleetOrders) 
					{
						if (!remap.Missions.ContainsKey(fleet.Mission))
							remap.Missions.Add(fleet.Mission, fleet.Mission.Copy(remap, galaxyRemap));
						remap.Fleets.Add(fleet, fleet.Copy(remap));
					}
			foreach(var colonization in this.ColonizationProjects)
				foreach(var fleet in colonization.Enroute)
				{
					foreach(var group in fleet.Ships)
						if (!remap.Designs.ContainsKey(group.Design))
							remap.Designs.Add(group.Design, group.Design.Copy(remap));
					remap.Missions.Add(fleet.Mission, fleet.Mission.Copy(remap, galaxyRemap));
					remap.Fleets.Add(fleet, fleet.Copy(remap));
				}
			
			return remap;
		}

		#region Saving
		
		internal IkonComposite Save(ObjectIndexer indexer)
		{
			var data = new IkonComposite(StatesTag);
			
			data.Add(StarsKey, new IkonArray().AddAll(this.Stars.Select(x => x.Save())));
			data.Add(WormholesKey, new IkonArray().AddAll(this.Wormholes.Select(x => x.Save(indexer))));
			data.Add(PlanetsKey, new IkonArray().AddAll(this.Planets.Select(x => x.Save(indexer))));

			data.Add(ColoniesKey, new IkonArray().AddAll(this.Colonies.Select(x => x.Save(indexer))));
			data.Add(StellarisesKey, new IkonArray().AddAll(this.Stellarises.Select(x => x.Save(indexer))));

			data.Add(ColonizationKey, new IkonArray().AddAll(this.ColonizationProjects.Select(x => x.Save(indexer))));
			data.Add(IdleFleetsKey, new IkonArray().AddAll(this.Fleets.Select(x => x.Save(indexer))));
			data.Add(DesignsKey, new IkonArray().AddAll(this.Designs.Select(x => x.Save(indexer))));
			data.Add(ReportsKey, new IkonArray().AddAll(this.Reports.Select(x => x.Save(indexer))));
			data.Add(TechnologyAdvancesKey, new IkonArray().AddAll(this.TechnologyAdvances.Select(x => x.Save(indexer))));
						
			return data;
		}

		private const string StatesTag = "States";
		public const string ColoniesKey = "colonies";
		public const string ColonizationKey = "colonizations";
		public const string DesignsKey = "designs";
		public const string IdleFleetsKey = "idleFleets";
		public const string PlanetsKey = "planets";
		public const string ReportsKey = "reports";
		public const string StarsKey = "stars";
		public const string StellarisesKey = "stellarises";
		public const string TechnologyAdvancesKey = "techAdvances";
		public const string WormholesKey = "wormholes";
		#endregion
		
	}
}
