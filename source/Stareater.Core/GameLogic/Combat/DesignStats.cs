using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.GameLogic.Combat
{
	[StateTypeAttribute(true)]
	class DesignStats 
	{
		public double Size { get; private set; }
		public double ShieldSize { get; private set; }

		public double GalaxySpeed { get; private set; }
		public double GalaxyPower { get; private set; }
		public double ScanRange { get; private set; }
		public double CombatSpeed { get; private set; }
		public double CombatPower { get; private set; }
		public List<AbilityStats> Abilities { get; private set; }
		public double CarryCapacity { get; internal set; }
		public double TowCapacity { get; internal set; }
		public double ColonizerPopulation { get; private set; }
		public Dictionary<string, double> ColonizerBuildings { get; private set; }
		public double HitPoints { get; private set; }
		public double ShieldPoints { get; private set; }
		public double Evasion { get; private set; }
		public double ArmorReduction { get; private set; }
		public double ShieldReduction { get; private set; }
		public double ShieldRegeneration { get; private set; }
		public double ShieldThickness { get; private set; }
		public double Detection { get; private set; }
		public double Cloaking { get; private set; }
		public double Jamming { get; private set; }

		public DesignStats(double size, double shieldSize,
						   double galaxySpeed, double galaxyPower, double scanRange,
						   double combatSpeed, double combatPower, List<AbilityStats> abilities,
						   double carryCapacity, double towCapacity,
						   double colonizerPopulation, Dictionary<string, double> colonizerBuildings, 
		                   double hitPoints, double shieldPoints, double evasion, double armorReduction, 
		                   double shieldReduction, double shieldRegeneration, double shieldThickness, double detection, double cloaking, double jamming)
		{
			this.Abilities = abilities;
			this.ArmorReduction = armorReduction;
			this.CarryCapacity = carryCapacity;
			this.Cloaking = cloaking;
			this.ColonizerPopulation = colonizerPopulation;
			this.ColonizerBuildings = colonizerBuildings;
			this.CombatPower = combatPower;
			this.CombatSpeed = combatSpeed;
			this.Detection = detection;
			this.Evasion = evasion;
			this.GalaxyPower = galaxyPower;
			this.GalaxySpeed = galaxySpeed;
			this.HitPoints = hitPoints;
			this.Jamming = jamming;
			this.ScanRange = scanRange;
			this.ShieldPoints = shieldPoints;
			this.ShieldReduction = shieldReduction;
			this.ShieldRegeneration = shieldRegeneration;
			this.ShieldSize = shieldSize;
			this.ShieldThickness = shieldThickness;
			this.Size = size;
			this.TowCapacity = towCapacity;
		}
	}
}
