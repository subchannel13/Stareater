using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.AppData.Expressions;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Ships
{
	class ThrusterType : AComponentType
	{
		public string ImagePath { get; private set; }

		public Formula Evasion;
		public Formula Speed;

		public ThrusterType(string code, string nameCode, string descCode, string imagePath,
		                 IEnumerable<Prerequisite> prerequisites, int maxLevel,
						 Formula evasion, Formula speed)
			: base(code, nameCode, descCode, prerequisites, maxLevel)
		{
			this.Evasion = evasion;
			this.Speed = speed;
			this.ImagePath = imagePath;
		}

		//TODO(0.5) make reusable method in superclass
		public static Component<ThrusterType> MakeBest(IEnumerable<ThrusterType> thrusters, Dictionary<string, int> playersTechLevels)
		{
			Component<ThrusterType> bestComponent = null;
			var armorVars = new Var("level", 0).Get; //TODO(v0.5) make constants for variable names

			foreach (var thruster in thrusters.Where(x => x.IsAvailable(playersTechLevels)))
			{
				int level = thruster.HighestLevel(playersTechLevels);
				armorVars["level"] = level;

				if (bestComponent == null || (thruster.Speed.Evaluate(armorVars) > bestComponent.TypeInfo.Speed.Evaluate(armorVars)))
					bestComponent = new Component<ThrusterType>(thruster, level);
			}

			return bestComponent;
		}
	}
}
