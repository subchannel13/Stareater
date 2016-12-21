using System;
using Stareater.Galaxy;
using Stareater.GameData.Ships;

namespace Stareater.GameLogic
{
	class AbilityStats
	{
		public AAbilityType Type { get; private set; }
		public int Level { get; private set; }
		public int Quantity { get; private set; }
		
		public int Range { get; private set; }
		public bool IsInstantDamage { get; private set; }
		public bool TargetColony { get; private set; }
		public bool TargetShips { get; private set; }
		public bool TargetStar { get; private set; }
		
		public double FirePower { get; private set; }
		public double Accuracy { get; private set; }
		public double EnergyCost { get; private set; }
		
		public double AccuracyRangePenalty { get; private set; }
		public double ArmorEfficiency { get; private set; }
		public double ShieldEfficiency { get; private set; }
		public double PlanetEfficiency { get; private set; }
		
		public BodyTraitType AppliesTrait { get; private set; }
		
		public AbilityStats(AAbilityType type, int level, int quantity, 
		                    int range, bool isInstantDamage, bool targetColony, bool targetShips, bool targetStar,
		                    double firePower, double accuracy, double energyCost, 
		                    double accuracyRangePenalty, double armorEfficiency, double shieldEfficiency, double planetEfficiency,
		                    BodyTraitType appliesTrait)
		{
			this.Type = type;
			this.Level = level;
			this.Quantity = quantity;
			
			this.Range = range;
			this.IsInstantDamage = isInstantDamage;
			this.TargetColony = targetColony;
			this.TargetShips = targetShips;
			this.TargetStar = targetStar;
			
			this.FirePower = firePower;
			this.Accuracy = accuracy;
			this.EnergyCost = energyCost;
			this.AccuracyRangePenalty = accuracyRangePenalty;
			this.ArmorEfficiency = armorEfficiency;
			this.ShieldEfficiency = shieldEfficiency;
			this.PlanetEfficiency = planetEfficiency;
			
			this.AppliesTrait = appliesTrait;
		}
	}
}
