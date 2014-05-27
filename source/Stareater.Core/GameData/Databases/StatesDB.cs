using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases
{
	class StatesDB
	{
		public StarCollection Stars { get; private set; }
		public WormholeCollection Wormholes { get; private set; }
		
		public PlanetCollection Planets { get; private set; }
		public ColonyCollection Colonies { get; private set; }
		public StellarisCollection Stellarises { get; private set; }
		
		public IdleFleetCollection IdleFleets { get; private set; }
		
		public DesignCollection Designs { get; private set; }
		public TechProgressCollection TechnologyAdvances { get; private set; }
		
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
			copy.IdleFleets.Add(this.IdleFleets);
			
			copy.Designs = new DesignCollection();
			copy.Designs.Add(this.Designs);
			
			copy.TechnologyAdvances = new TechProgressCollection();
			copy.TechnologyAdvances.Add(this.TechnologyAdvances.Select(x => x.Copy(playersRemap.Players[x.Owner])));
			
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
				new Dictionary<AConstructionSite, StellarisAdmin>()
			);

			foreach (var colony in this.Colonies)
				remap.Colonies.Add(
					colony, 
					colony.Copy(playersRemap[colony.Owner], galaxyRemap.Planets[colony.Location.Planet])
				);

			foreach(var stellaris in this.Stellarises)
				remap.Stellarises.Add(
					stellaris, 
					stellaris.Copy(playersRemap[stellaris.Owner], galaxyRemap.Stars[stellaris.Location.Star])
				);
			
			return remap;
		}
	}
}
