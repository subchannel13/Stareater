using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	public class ShipFormulaSet
	{
		public Formula Cloaking { get; private set; }
		public Formula CombatSpeed { get; private set; }
		public Formula Detection { get; private set; }
		public Formula Evasion { get; private set; }
		public Formula HitPoints { get; private set; }
		public Formula Jamming { get; private set; }
		
		public Formula ColonizerPopulation { get; private set; }
		public Dictionary<string, Formula> ColonizerBuildings { get; private set; }
		
		public double SensorRangePenalty { get; private set; }
		
		public ShipFormulaSet(Formula cloaking, Formula combatSpeed, Formula detection, Formula evasion, 
		                      Formula hitPoints, Formula jamming,
		                      Formula colonizerPopulation, Dictionary<string, Formula> colonizerBuildings,
		                      double sensorRangePenalty)
		{
			this.Cloaking = cloaking;
			this.CombatSpeed = combatSpeed;
			this.Detection = detection;
			this.Evasion = evasion;
			this.HitPoints = hitPoints;
			this.Jamming = jamming;
			this.ColonizerPopulation = colonizerPopulation;
			this.ColonizerBuildings = colonizerBuildings;
			this.SensorRangePenalty = sensorRangePenalty;
		}
	}
}
