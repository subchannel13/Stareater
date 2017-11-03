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

		public Formula ReactorSize { get; private set; }
		public Formula ShieldSize { get; private set; }

		public double RepairCostFactor { get; private set; }
		public Formula LevelRefitCost { get; private set; }
		public double ArmorCostPortion { get; private set; }
		public double ReactorCostPortion { get; private set; }
		public double SensorCostPortion { get; private set; }
		public double ThrustersCostPortion { get; private set; }
		
		public ShipFormulaSet(Formula cloaking, Formula combatSpeed, Formula detection, Formula evasion, 
		                      Formula hitPoints, Formula jamming,
		                      Formula colonizerPopulation, Dictionary<string, Formula> colonizerBuildings,
							  Formula reactorSize, Formula shieldSize,
                              double naturalCloakBonus, double sensorRangePenalty, double repairCostFactor, Formula levelRefitCost,
		                      double armorCostPortion, double reactorCostPortion, double sensorCostPortion, double thrustersCostPortion)
		{
			this.ArmorCostPortion = armorCostPortion;
			this.Cloaking = cloaking;
			this.ColonizerBuildings = colonizerBuildings;
			this.ColonizerPopulation = colonizerPopulation;
			this.CombatSpeed = combatSpeed;
			this.Detection = detection;
			this.Evasion = evasion;
			this.HitPoints = hitPoints;
			this.Jamming = jamming;
			this.LevelRefitCost = levelRefitCost;
			this.NaturalCloakBonus = naturalCloakBonus;
			this.ReactorCostPortion = reactorCostPortion;
			this.ReactorSize = reactorSize;
			this.RepairCostFactor = repairCostFactor;
			this.SensorCostPortion = sensorCostPortion;
			this.SensorRangePenalty = sensorRangePenalty;
			this.ShieldSize = shieldSize;
			this.ThrustersCostPortion = thrustersCostPortion;
		}
	}
}
