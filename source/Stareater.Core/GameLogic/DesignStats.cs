using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.Utils;

namespace Stareater.GameLogic 
{
	class DesignStats 
	{
		public double CombatSpeed;
		public double GalaxySpeed;
		public List<Ability> Abilities;
		public double ColonizerPopulation;
		public Dictionary<string, double> ColonizerBuildings;

		public DesignStats(double combatSpeed, double galaxySpeed, List<Ability> abilities, double colonizerPopulation, Dictionary<string, double> colonizerBuildings) 
		{
			this.CombatSpeed = combatSpeed;
			this.GalaxySpeed = galaxySpeed;
			this.Abilities = abilities;
			this.ColonizerPopulation = colonizerPopulation;
			this.ColonizerBuildings = colonizerBuildings;
		}
	}
}
