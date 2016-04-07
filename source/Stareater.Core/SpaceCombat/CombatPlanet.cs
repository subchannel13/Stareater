using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;

namespace Stareater.SpaceCombat
{
	class CombatPlanet
	{
		public Colony Colony { get; set; }
		public Vector2D Position { get; private set; }
		
		public double PopulationHitPoints { get; private set; }
		
		public CombatPlanet(Colony colony, Vector2D position, 
		                    double populationHitPoints)
		{
			this.Colony = colony;
			this.Position = position;
			
			this.PopulationHitPoints = populationHitPoints;
		}
	}
}
