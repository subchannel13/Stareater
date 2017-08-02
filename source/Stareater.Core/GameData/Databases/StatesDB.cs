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
using Stareater.Utils.StateEngine;

namespace Stareater.GameData.Databases
{
	class StatesDB
	{
		private const string DesignIdPrefix = "ShipDesign";
		
		[StateProperty]
		public StarCollection Stars { get; private set; }
		[StateProperty]
		public WormholeCollection Wormholes { get; private set; }

		[StateProperty]
		public PlanetCollection Planets { get; private set; }
		[StateProperty]
		public ColonyCollection Colonies { get; private set; }
		[StateProperty]
		public StellarisCollection Stellarises { get; private set; }

		[StateProperty]
		public ColonizationCollection ColonizationProjects { get; private set; }
		[StateProperty]
		public FleetCollection Fleets { get; private set; }

		[StateProperty]
		public DesignCollection Designs { get; private set; }
		[StateProperty]
		public ReportCollection Reports { get; private set; }
		[StateProperty]
		public DevelopmentProgressCollection DevelopmentAdvances { get; private set; }
		[StateProperty]
		public ResearchProgressCollection ResearchAdvances { get; private set; }
		[StateProperty]
		public TreatyCollection Treaties { get; private set; }
		
		private int nextDesignId = 0;
		
		public StatesDB(StarCollection stars, WormholeCollection wormholes, PlanetCollection planets, 
		                ColonyCollection Colonies, StellarisCollection stellarises,
		                DevelopmentProgressCollection developmentAdvances, ResearchProgressCollection researchAdvances,
						TreatyCollection treaties,ReportCollection reports, DesignCollection designs,
						FleetCollection fleets, ColonizationCollection colonizations)
		{
			this.Colonies = Colonies;
			this.Planets = planets;
			this.Stars = stars;
			this.Stellarises = stellarises;
			this.Wormholes = wormholes;
			this.DevelopmentAdvances = developmentAdvances;
			this.ResearchAdvances = researchAdvances;
			this.Reports = reports;
			this.Designs = designs;
			this.Fleets = fleets;
			this.ColonizationProjects = colonizations;
			this.Treaties = treaties;
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
			copy.ColonizationProjects.Add(playersRemap.Colonizations.Values);

			copy.Fleets = new FleetCollection();
			copy.Fleets.Add(playersRemap.Fleets.Values);
			
			copy.Designs = new DesignCollection();
			copy.Designs.Add(playersRemap.Designs.Values);
			
			copy.DevelopmentAdvances = new DevelopmentProgressCollection();
			copy.DevelopmentAdvances.Add(this.DevelopmentAdvances.Select(x => x.Copy(playersRemap)));

			copy.ResearchAdvances = new ResearchProgressCollection();
			copy.ResearchAdvances.Add(this.ResearchAdvances.Select(x => x.Copy(playersRemap)));
			
			copy.Treaties = new TreatyCollection();
			copy.Treaties.Add(this.Treaties.Select(x => x.Copy(playersRemap)));
			
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
				new Dictionary<ColonizationProject, ColonizationProject>(),
				new Dictionary<AMission, AMission>()
			);

			remap.Colonies = this.Colonies.ToDictionary(x => (AConstructionSite)x, x => x.Copy(remap, galaxyRemap));
			remap.Stellarises = this.Stellarises.ToDictionary(x => (AConstructionSite)x, x => x.Copy(remap, galaxyRemap));
			remap.Designs = this.Designs.ToDictionary(x => x, x => x.Copy(remap));
			remap.Colonizations = this.ColonizationProjects.ToDictionary(x => x, x => x.Copy(remap, galaxyRemap));
			remap.Missions = this.Fleets.SelectMany(x => x.Missions).Distinct().ToDictionary(x => x, x => x.Copy(remap, galaxyRemap));
			remap.Fleets = this.Fleets.ToDictionary(x => x, x => x.Copy(remap));
			
			/*foreach(var player in playersRemap.Keys)
				foreach(var fleetOrders in player.Orders.ShipOrders.Values)
					foreach(var fleet in fleetOrders) 
					{
						foreach(var mission in fleet.Missions.Where(x => !remap.Missions.ContainsKey(x)))
							remap.Missions.Add(mission, mission.Copy(remap, galaxyRemap));
						remap.Fleets.Add(fleet, fleet.Copy(remap));
					}*/
			
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
			data.Add(DevelopmentAdvancesKey, new IkonArray().AddAll(this.DevelopmentAdvances.Select(x => x.Save(indexer))));
			data.Add(ResearchAdvancesKey, new IkonArray().AddAll(this.ResearchAdvances.Select(x => x.Save(indexer))));
			data.Add(TreatiesKey, new IkonArray().AddAll(this.Treaties.Select(x => x.Save(indexer))));
						
			return data;
		}

		private const string StatesTag = "States";
		public const string ColoniesKey = "colonies";
		public const string ColonizationKey = "colonizations";
		public const string DesignsKey = "designs";
		public const string DevelopmentAdvancesKey = "developmentAdvances";
		public const string IdleFleetsKey = "idleFleets";
		public const string PlanetsKey = "planets";
		public const string ReportsKey = "reports";
		public const string ResearchAdvancesKey = "researchAdvances";
		public const string StarsKey = "stars";
		public const string StellarisesKey = "stellarises";
		public const string TreatiesKey = "treaties";
		public const string WormholesKey = "wormholes";
		#endregion
		
	}
}
