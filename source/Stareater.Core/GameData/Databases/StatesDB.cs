using Stareater.Galaxy;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Utils;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.GameData.Databases
{
	class StatesDB
	{
		private const string DesignIdPrefix = "ShipDesign";
		
		[StatePropertyAttribute]
		public StarCollection Stars { get; private set; }
		[StatePropertyAttribute]
		public WormholeCollection Wormholes { get; private set; }
		[StatePropertyAttribute]
		//TODO(v0.8) maybe give natives stellarises with special buildings and use it in
		//derivates to deduce which star is the brain
		public StarData StareaterBrain { get; private set; }

		[StatePropertyAttribute]
		public PlanetCollection Planets { get; private set; }
		[StatePropertyAttribute]
		public ColonyCollection Colonies { get; private set; }
		[StatePropertyAttribute]
		public StellarisCollection Stellarises { get; private set; }

		[StatePropertyAttribute]
		public ColonizationCollection ColonizationProjects { get; private set; }
		[StatePropertyAttribute]
		public FleetCollection Fleets { get; private set; }

		[StatePropertyAttribute]
		public DesignCollection Designs { get; private set; }
		[StatePropertyAttribute]
		public ReportCollection Reports { get; private set; }
		[StatePropertyAttribute]
		public DevelopmentProgressCollection DevelopmentAdvances { get; private set; }
		[StatePropertyAttribute]
		public ResearchProgressCollection ResearchAdvances { get; private set; }
		[StatePropertyAttribute]
		public HashSet<Pair<Player>> Contacts { get; internal set; }
		[StatePropertyAttribute]
		public TreatyCollection Treaties { get; private set; }

		private int nextDesignId = 0; //TODO(v0.8) may not work correctly after loading
		
		public StatesDB(StarCollection stars, StarData stareaterBrain, WormholeCollection wormholes, PlanetCollection planets, 
		                ColonyCollection Colonies, StellarisCollection stellarises,
		                DevelopmentProgressCollection developmentAdvances, ResearchProgressCollection researchAdvances,
						HashSet<Pair<Player>> contacts, TreatyCollection treaties,ReportCollection reports, DesignCollection designs,
						FleetCollection fleets, ColonizationCollection colonizations)
		{
			this.Colonies = Colonies;
			this.Contacts = contacts;
			this.Planets = planets;
			this.Stars = stars;
			this.StareaterBrain = stareaterBrain;
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
	}
}
