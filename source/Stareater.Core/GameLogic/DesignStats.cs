using System.Collections.Generic;

namespace Stareater.GameLogic 
{
	class DesignStats 
	{
		public double CombatSpeed;
		public double GalaxySpeed;
		public double ColonizerPopulation;
		public Dictionary<string, double> ColonizerBuildings;

		public DesignStats(double combatSpeed, double galaxySpeed, double colonizerPopulation, Dictionary<string, double> colonizerBuildings) 
		{
			this.CombatSpeed = combatSpeed;
			this.GalaxySpeed = galaxySpeed;
			this.ColonizerPopulation = colonizerPopulation;
			this.ColonizerBuildings = colonizerBuildings;
		}
	}
}
