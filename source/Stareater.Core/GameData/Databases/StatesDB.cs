using System;
using System.Collections.Generic;
using System.Linq;

using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Ships;
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
		
		public IdleFleetCollection IdleFleets { get; private set; }
		
		public DesignCollection Designs { get; private set; }
		public TechProgressCollection TechnologyAdvances { get; private set; }
		
		private int nextDesignId = 0;
		
		public StatesDB(StarCollection stars, WormholeCollection wormholes, PlanetCollection planets, 
		                ColonyCollection Colonies, StellarisCollection stellarises, 
		                TechProgressCollection technologyProgresses)
		{
			this.Colonies = Colonies;
			this.Planets = planets;
			this.Stars = stars;
			this.Stellarises = stellarises;
			this.Wormholes = wormholes;
			this.TechnologyAdvances = technologyProgresses;
			
			this.Designs = new DesignCollection();
			this.IdleFleets = new IdleFleetCollection(); 
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
			StatesDB copy = new StatesDB();

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

			copy.IdleFleets = new IdleFleetCollection();
			copy.IdleFleets.Add(playersRemap.IdleFleets.Values);
			
			copy.Designs = new DesignCollection();
			copy.Designs.Add(playersRemap.Designs.Values);
			
			copy.TechnologyAdvances = new TechProgressCollection();
			copy.TechnologyAdvances.Add(this.TechnologyAdvances.Select(x => x.Copy(playersRemap)));
			
			return copy;
		}

		public GalaxyRemap CopyGalaxy()
		{
			GalaxyRemap remap = new GalaxyRemap(new Dictionary<StarData, StarData>(), new Dictionary<Planet, Planet>());

			remap.Stars = this.Stars.ToDictionary(x => x, x => x.Copy());
			remap.Planets = this.Planets.ToDictionary(x => x, x => x.Copy(remap));

			return remap;
		}

		internal PlayersRemap CopyPlayers(Dictionary<Player, Player> playersRemap, GalaxyRemap galaxyRemap)
		{
			PlayersRemap remap = new PlayersRemap(
				playersRemap, 
				new Dictionary<AConstructionSite, Colony>(),
				new Dictionary<AConstructionSite, StellarisAdmin>(),
				new Dictionary<Design, Design>(),
				new Dictionary<IdleFleet, IdleFleet>()
			);

			remap.Colonies = this.Colonies.ToDictionary(x => (AConstructionSite)x, x => x.Copy(remap, galaxyRemap));
			remap.Stellarises = this.Stellarises.ToDictionary(x => (AConstructionSite)x, x => x.Copy(remap, galaxyRemap));
			remap.Designs = this.Designs.ToDictionary(x => x, x => x.Copy(remap));
			remap.IdleFleets = this.IdleFleets.ToDictionary(x => x, x => x.Copy(remap, galaxyRemap));
			
			return remap;
		}

		#region Saving
		
		internal IkonComposite Save(ObjectIndexer indexer)
		{
			IkonComposite data = new IkonComposite(StatesTag);
			
			data.Add(StarsKey, new IkonArray().Add(this.Stars.Select(x => x.Save()).ToArray()));
			data.Add(WormholesKey, new IkonArray().Add(this.Wormholes.Select(x => x.Save(indexer)).ToArray()));
			data.Add(PlanetsKey, new IkonArray().Add(this.Planets.Select(x => x.Save(indexer)).ToArray()));
			
			data.Add(ColoniesKey, new IkonArray().Add(this.Colonies.Select(x => x.Save(indexer)).ToArray()));
			data.Add(StellarisesKey, new IkonArray().Add(this.Stellarises.Select(x => x.Save(indexer)).ToArray()));
			
			data.Add(IdleFleetsKey, new IkonArray().Add(this.IdleFleets.Select(x => x.Save(indexer)).ToArray()));
			data.Add(DesignsKey, new IkonArray().Add(this.Designs.Select(x => x.Save(indexer)).ToArray()));
			data.Add(TechnologyAdvancesKey, new IkonArray().Add(this.TechnologyAdvances.Select(x => x.Save(indexer)).ToArray()));
						
			return data;
		}

		private const string StatesTag = "States";
		private const string ColoniesKey = "colonies";
		private const string DesignsKey = "designs";
		private const string IdleFleetsKey = "idleFleets";
		private const string PlanetsKey = "planets";
		private const string StarsKey = "stars";
		private const string StellarisesKey = "stellarises";
		private const string TechnologyAdvancesKey = "techAdvances";
		private const string WormholesKey = "wormholes";
		#endregion
		
	}
}
