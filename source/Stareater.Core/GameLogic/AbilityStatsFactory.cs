using System;
using Stareater.Galaxy;
using Stareater.GameData.Databases;
using Stareater.GameData.Ships;
using Stareater.GameLogic;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	class AbilityStatsFactory : IAbilityVisitor
	{
		private int level;
		private StaticsDB statics;
		
		private int range = 0;
		private bool isInstantDamage = false;
		private bool targetColony = false;
		private bool targetShips = false;
		private bool targetStar = false;
		
		private double firePower = 0;
		private double accuracy = 0;
		private double energyCost = 0;
		private double accuracyRangePenalty = 0;
		private double armorEfficiency = 0;
		private double shieldEfficiency = 0;
		private double planetEfficiency = 0;
		
		private BodyTraitType appliesTrait = null;
		
		private AbilityStatsFactory(int level, StaticsDB statics)
		{
			this.level = level;
			this.statics = statics;
		}
		
		public static AbilityStats Create(AAbilityType type, int level, int quantity, StaticsDB statics)
		{
			var factory = new AbilityStatsFactory(level, statics);
			type.Accept(factory);
			
			return new AbilityStats(type, level, quantity, factory.range, factory.isInstantDamage, factory.targetColony, factory.targetShips, factory.targetStar,
			                       factory.firePower, factory.accuracy, factory.energyCost, 
			                       factory.accuracyRangePenalty, factory.armorEfficiency, factory.shieldEfficiency, factory.planetEfficiency,
			                       factory.appliesTrait);
		}

		#region IAbilityVisitor implementation

		public void Visit(DirectShootAbility ability)
		{
			var vars = new Var(AComponentType.LevelKey, this.level).Get;
			
			this.range = (int)ability.Range.Evaluate(vars);
			this.isInstantDamage = true;
			this.targetColony = true;
			this.targetShips = true;
			
			this.firePower = ability.FirePower.Evaluate(vars);
			this.accuracy = ability.Accuracy.Evaluate(vars);
			this.energyCost = ability.EnergyCost.Evaluate(vars);
			this.accuracyRangePenalty = ability.AccuracyRangePenalty.Evaluate(vars);
			this.armorEfficiency = ability.ArmorEfficiency.Evaluate(vars);
			this.shieldEfficiency = ability.ShieldEfficiency.Evaluate(vars);
			this.planetEfficiency = ability.PlanetEfficiency.Evaluate(vars);
		}

		public void Visit(StarShootAbility ability)
		{
			var vars = new Var(AComponentType.LevelKey, this.level).Get;
			
			this.range = (int)ability.Range.Evaluate(vars);
			this.isInstantDamage = true;
			this.targetStar = true;
			
			this.energyCost = ability.EnergyCost.Evaluate(vars);
			this.appliesTrait = statics.Traits[ability.AppliesTraitId];
		}
		#endregion
	}
}
