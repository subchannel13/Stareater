using Stareater.AppData.Expressions;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.GameData.Ships
{
	[StateTypeAttribute(true)]
	class ArmorType : AComponentType, IIncrementalComponent
	{
		public string ImagePath { get; private set; }
		
		public Formula ArmorFactor;
		public Formula Absorption;
		
		public ArmorType(string code, string languageCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, 
		                 bool canPick, Formula armorFactor, Formula absorption)
			: base(code, languageCode, prerequisites, maxLevel, canPick)
		{
			this.ArmorFactor = armorFactor;
			this.Absorption = absorption;
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
