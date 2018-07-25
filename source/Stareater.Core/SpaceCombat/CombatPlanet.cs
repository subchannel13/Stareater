using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.SpaceCombat
{
	class CombatPlanet
	{
		public Colony Colony { get; set; }
		public Planet PlanetData { get; set; }
		public Vector2D Position { get; private set; }
		
		public double PopulationHitPoints { get; private set; }
		
		public CombatPlanet(Colony colony, Planet planet, Vector2D position, 
		                    double populationHitPoints)
		{
			this.Colony = colony;
			this.PlanetData = planet;
			this.Position = position;
			
			this.PopulationHitPoints = populationHitPoints;
		}
	}
}
