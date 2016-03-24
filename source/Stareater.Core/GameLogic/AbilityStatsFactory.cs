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
		
		private AbilityStatsFactory(int level)
		{
			this.level = level;
		}
		
		public static AbilityStats Create(AAbilityType type, int level, int quantity)
		{
			var factory = new AbilityStatsFactory(level);
			type.Accept(factory);
			
			return new AbilityStats(type, level, quantity, factory.range);
		}

		#region IAbilityVisitor implementation

		public void Visit(DirectShootAbility ability)
		{
			var vars = new Var(AComponentType.LevelKey, this.level).Get;
			
			this.range = (int)ability.Range.Evaluate(vars);
		}

		#endregion
	}
}
