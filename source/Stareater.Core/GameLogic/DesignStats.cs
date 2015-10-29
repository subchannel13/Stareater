using System.Collections.Generic;
using Stareater.GameData;

namespace Stareater.GameLogic 
{
	class DesignStats 
	{
		public double GalaxySpeed { get; private set; }
		
		public double ColonizerPopulation { get; private set; }
		public Dictionary<string, double> ColonizerBuildings { get; private set; }

		public DesignStats(double galaxySpeed, double colonizerPopulation, Dictionary<string, double> colonizerBuildings)
		{
			this.GalaxySpeed = galaxySpeed;
			this.ColonizerPopulation = colonizerPopulation;
			this.ColonizerBuildings = colonizerBuildings;
			
		}
	}
}
