using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;

namespace Stareater.GameData.Ships
{
	[StateType(true)]
	class ArmorType : AComponentType, IIncrementalComponent
	{
		public string ImagePath { get; private set; }
		
		public Formula ArmorFactor;
		public Formula Absorption;
		public Formula AbsorptionMax;
		
		public ArmorType(string code, string languageCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, 
		                 bool canPick, Formula armorFactor, Formula absorption, Formula absorptionMax)
			: base(code, languageCode, prerequisites, maxLevel, canPick)
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
