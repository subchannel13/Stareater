using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.GameLogic.Combat
{
	[StateType(true)]
	class DesignStats 
	{
		public double Size;
		public double GalaxySpeed;
		public double GalaxyPower;
		public double CombatSpeed;
		public double CombatPower;
		public List<AbilityStats> Abilities;
		public double ColonizerPopulation;
		public Dictionary<string, double> ColonizerBuildings;
		public double HitPoints;
		public double ShieldPoints;
		public double Evasion;
		public double ArmorReduction;
		public double ShieldReduction;
		public double ShieldRegeneration;
		public double ShieldThickness;
		public double Detection;
		public double Cloaking;
		public double Jamming;

		public DesignStats(double size, double galaxySpeed, double galaxyPower, double combatSpeed, double combatPower, 
		                   List<AbilityStats> abilities, double colonizerPopulation, Dictionary<string, double> colonizerBuildings, 
		                   double hitPoints, double shieldPoints, double evasion, double armorReduction, 
		                   double shieldReduction, double shieldRegeneration, double shieldThickness, double detection, double cloaking, double jamming)
		{
			this.Size = size;
			this.GalaxySpeed = galaxySpeed;
			this.GalaxyPower = galaxyPower;
			this.CombatSpeed = combatSpeed;
			this.CombatPower = combatPower;
			this.Abilities = abilities;
			this.ColonizerPopulation = colonizerPopulation;
			this.ColonizerBuildings = colonizerBuildings;
			this.HitPoints = hitPoints;
			this.ShieldPoints = shieldPoints;
			this.Evasion = evasion;
			this.ArmorReduction = armorReduction;
			this.ShieldReduction = shieldReduction;
			this.ShieldRegeneration = shieldRegeneration;
			this.ShieldThickness = shieldThickness;
			this.Detection = detection;
			this.Cloaking = cloaking;
			this.Jamming = jamming;
		}
	}
}
