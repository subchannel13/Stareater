using System;
using Stareater.GameData.Ships;

namespace Stareater.GameLogic
{
	class AbilityStats
	{
		public AAbilityType Type { get; private set; }
		public int Level { get; private set; }
		public int Quantity { get; private set; }
		
		public int Range { get; private set; }
		
		public AbilityStats(AAbilityType type, int level, int quantity, int range)
		{
			this.Type = type;
			this.Level = level;
			this.Quantity = quantity;
			
			this.Range = range;
		}
	}
}
