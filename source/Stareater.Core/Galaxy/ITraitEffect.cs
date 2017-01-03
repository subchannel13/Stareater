using Stareater.GameData.Databases;

namespace Stareater.Galaxy
{
	interface ITraitEffect
	{
		void PostcombatApply(StatesDB states, StaticsDB statics);
	}
}
