using System;
using Stareater.GameData.Ships;
using Stareater.GameLogic;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	class AbilityStatsFactory : IAbilityVisitor
	{
		private int level;
		
		private int range = 0;
		private bool isInstantDamage = false;
		
		private double firePower = 0;
		private double accuracy = 0;
		private double energyCost = 0;
		private double armorEfficiency = 0;
		private double shieldEfficiency = 0;
		
		private AbilityStatsFactory(int level)
		{
			this.level = level;
		}
		
		public static AbilityStats Create(AAbilityType type, int level, int quantity)
		{
			var factory = new AbilityStatsFactory(level);
			type.Accept(factory);
			
			return new AbilityStats(type, level, quantity, factory.range, factory.isInstantDamage,
			                       factory.firePower, factory.accuracy, factory.energyCost, factory.armorEfficiency, factory.shieldEfficiency);
		}

		#region IAbilityVisitor implementation

		public void Visit(DirectShootAbility ability)
		{
			var vars = new Var(AComponentType.LevelKey, this.level).Get;
			
			this.range = (int)ability.Range.Evaluate(vars);
			this.isInstantDamage = true;
			
			this.firePower = ability.FirePower.Evaluate(vars);
			this.accuracy = ability.Accuracy.Evaluate(vars);
			this.energyCost = ability.EnergyCost.Evaluate(vars);
			this.armorEfficiency = ability.ArmorEfficiency.Evaluate(vars);
			this.shieldEfficiency = ability.ShieldEfficiency.Evaluate(vars);
		}

		#endregion
	}
}
