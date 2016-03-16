using System;
using Stareater.GameData.Ships;

namespace Stareater.GameLogic
{
	class Ability
	{
		public AAbilityType Type { get; private set; }
		public int Level { get; private set; }
		public int Quantity { get; private set; }
		
		public Ability(AAbilityType type, int level, int quantity)
		{
			this.Type = type;
			this.Level = level;
			this.Quantity = quantity;
		}
	}
}
