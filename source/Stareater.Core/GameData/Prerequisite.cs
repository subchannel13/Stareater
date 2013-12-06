using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Utils.Collections;

namespace Stareater.GameData
{
	class Prerequisite
	{
		public string Code { get; private set; }
		public Formula Level { get; private set; }
		
		public Prerequisite(string code, Formula level)
		{
			this.Code = code;
			this.Level = level;
		}
		
		public static bool AreSatisfied(IEnumerable<Prerequisite> prerequisites, int targetLevel, IDictionary<string, int> techLevels)
		{
			var levelVars = new Var("lvl", targetLevel).Get;
			foreach(Prerequisite prerequisite in prerequisites) {
				double requiredLevel = prerequisite.Level.Evaluate(levelVars);
				if (requiredLevel >= 0 && techLevels[prerequisite.Code] < requiredLevel)
					return false;
			}
			
			return true;
		}
	}
}
