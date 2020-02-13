using Stareater.GameData.Databases;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.Galaxy.BodyTraits
{
	[StateBaseTypeAttribute("LoadTrait", typeof(StarTraitType))]
	interface IStarTrait
	{
		StarTraitType Type { get; }

		void PostcombatApply(StaticsDB statics, StarData star, IEnumerable<Planet> planets);
		void InitialApply(StaticsDB statics, StarData star, IEnumerable<Planet> planets);
	}
}
