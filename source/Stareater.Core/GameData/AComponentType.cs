using System;
using System.Collections.Generic;

namespace Stareater.GameData
{
	abstract class AComponentType
	{
		public string NameCode { get; private set; }
		public string DescCode { get; private set; }

		public IEnumerable<Prerequisite> Prerequisites { get; private set; }
		public int MaxLevel { get; private set; }
		
		protected AComponentType(string nameCode, string descCode, 
		                      IEnumerable<Prerequisite> prerequisites, int maxLevel)
		{
			this.NameCode = nameCode;
			this.DescCode = descCode;
			this.Prerequisites = prerequisites;
			this.MaxLevel = maxLevel;
		}
		
		public bool IsAvailable(IDictionary<string, int> techLevels)
		{
			return Prerequisite.AreSatisfied(Prerequisites, 0, techLevels);
		}
		
		public int HighestLevel(IDictionary<string, int> techLevels)
		{
			if (!IsAvailable(techLevels))
				throw new InvalidOperationException();
			
			for(int level = MaxLevel; level > 0; level--)
				if (Prerequisite.AreSatisfied(Prerequisites, level, techLevels))
					return level;
			
			return 0;
		}
	}
}
