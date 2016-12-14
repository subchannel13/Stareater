using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Ships
{
	class ArmorType : AComponentType, IIncrementalComponent
	{
		public string ImagePath { get; private set; }
		
		public Formula ArmorFactor;
		public Formula Absorption;
		public Formula AbsorptionMax;
		
		public ArmorType(string code, string nameCode, string descCode, bool isVirtual, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, 
		                 Formula armorFactor, Formula absorption, Formula absorptionMax)
			: base(code, nameCode, descCode, isVirtual, prerequisites, maxLevel)
		{
			this.ArmorFactor = armorFactor;
			this.Absorption = absorption;
			this.AbsorptionMax = absorptionMax;
			this.ImagePath = imagePath;
		}

		#region IIncrementalComponent implementation
		public double ComparisonValue(int level)
		{
			return this.ArmorFactor.Evaluate(new Var(AComponentType.LevelKey, level).Get);
		}
		#endregion
	}
}
