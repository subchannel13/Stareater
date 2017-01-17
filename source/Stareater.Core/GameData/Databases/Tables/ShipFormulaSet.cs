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

		public double NaturalCloakBonus { get; private set; }
		public double SensorRangePenalty { get; private set; }

		public Formula LevelRefitCost { get; private set; }
		public double ArmorCostPortion { get; private set; }
		public double ReactorCostPortion { get; private set; }
		public double SensorCostPortion { get; private set; }
		public double ThrustersCostPortion { get; private set; }
		
		public ShipFormulaSet(Formula cloaking, Formula combatSpeed, Formula detection, Formula evasion, 
		                      Formula hitPoints, Formula jamming,
		                      Formula colonizerPopulation, Dictionary<string, Formula> colonizerBuildings,
		                      double naturalCloakBonus, double sensorRangePenalty, Formula levelRefitCost,
		                      double armorCostPortion, double reactorCostPortion, double sensorCostPortion, double thrustersCostPortion)
		{
			this.Cloaking = cloaking;
			this.CombatSpeed = combatSpeed;
			this.Detection = detection;
			this.Evasion = evasion;
			this.HitPoints = hitPoints;
			this.Jamming = jamming;
			this.ColonizerPopulation = colonizerPopulation;
			this.ColonizerBuildings = colonizerBuildings;
			this.NaturalCloakBonus = naturalCloakBonus;
			this.SensorRangePenalty = sensorRangePenalty;
			this.LevelRefitCost = levelRefitCost;
			this.ArmorCostPortion = armorCostPortion;
			this.ReactorCostPortion = reactorCostPortion;
			this.SensorCostPortion = sensorCostPortion;
			this.ThrustersCostPortion = thrustersCostPortion;
		}
	}
}
