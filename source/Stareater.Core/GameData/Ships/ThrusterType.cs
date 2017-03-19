using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Ships
{
	class ThrusterType : AComponentType, IIncrementalComponent
	{
		public string ImagePath { get; private set; }

		public Formula Evasion;
		public Formula Speed;

		public ThrusterType(string code, string languageCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel, bool canPick, 
						 Formula evasion, Formula speed)
			: base(code, languageCode, prerequisites, maxLevel, canPick)
		{
			this.Evasion = evasion;
			this.Speed = speed;
			this.ImagePath = imagePath;
		}

		#region IIncrementalComponent implementation
		public double ComparisonValue(int level)
		{
			return this.Speed.Evaluate(new Var(AComponentType.LevelKey, level).Get);
		}
		#endregion
	}
}
