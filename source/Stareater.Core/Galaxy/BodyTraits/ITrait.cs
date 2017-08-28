using Stareater.GameData.Databases;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy.BodyTraits
{
	[StateBaseType("LoadTrait", typeof(TraitType))]
	interface ITrait
	{
		TraitType Type { get; }

		void PostcombatApply(StatesDB states, StaticsDB statics, StarData star);
	}
}
