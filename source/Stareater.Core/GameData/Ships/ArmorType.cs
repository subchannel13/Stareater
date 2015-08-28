using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Ships
{
	class ArmorType : AComponentType
	{
		public string ImagePath { get; private set; }
		
		public Formula ArmorFactor;
		public Formula Absorption;
		public Formula AbsorptionMax;
		
		public ArmorType(string code, string nameCode, string descCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, 
		                 Formula armorFactor, Formula absorption, Formula absorptionMax)
			: base(code, nameCode, descCode, prerequisites, maxLevel)
		{
			this.ArmorFactor = armorFactor;
			this.Absorption = absorption;
			this.AbsorptionMax = absorptionMax;
			this.ImagePath = imagePath;
		}
		
		public static Component<ArmorType> MakeBest(IEnumerable<ArmorType> armors, Dictionary<string, int> playersTechLevels)
		{
			Component<ArmorType> bestComponent = null;
			var armorVars = new Var("level", 0).Get; //TODO(v0.5) make constants for variable names
				
			foreach (var armor in armors.Where(x => x.IsAvailable(playersTechLevels))) {
				int level = armor.HighestLevel(playersTechLevels);
				armorVars["level"] = level;

				if (bestComponent == null || (armor.ArmorFactor.Evaluate(armorVars) > bestComponent.TypeInfo.ArmorFactor.Evaluate(armorVars)))
					bestComponent = new Component<ArmorType>(armor, level);
			}

			return bestComponent;
		}
	}
}
