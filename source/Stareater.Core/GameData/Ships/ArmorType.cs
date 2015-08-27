using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Ships
{
	class ArmorType : AComponentType
	{
		private Formula armorFactor;
		private Formula absorption;
		private Formula absorptionMax;
		
		public ArmorType(string code, string nameCode, string descCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, 
		                 Formula armorFactor, Formula absorption, Formula absorptionMax)
			: base(code, nameCode, descCode, prerequisites, maxLevel)
		{
			this.armorFactor = armorFactor;
			this.absorption = absorption;
			this.absorptionMax = absorptionMax;
		}
	}
}
