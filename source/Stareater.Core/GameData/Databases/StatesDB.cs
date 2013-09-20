using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;

namespace Stareater.GameData.Databases
{
	class StatesDB
	{
		public StarCollection Stars { get; private set; }
		public WormholeCollection Wormholes { get; private set; }
		
		public PlanetCollection Planets { get; private set; }
		public ColonyCollection Colonies { get; private set; }
		
		public TechProgressCollection TechnologyAdvances { get; private set; }
		
		public StatesDB(StarCollection stars, WormholeCollection wormholes, PlanetCollection planets, ColonyCollection Colonies, TechProgressCollection technologyProgresses)
		{
			this.Colonies = Colonies;
			this.Planets = planets;
			this.Stars = stars;
			this.Wormholes = wormholes;
			this.TechnologyAdvances = technologyProgresses;
		}
	}
}
