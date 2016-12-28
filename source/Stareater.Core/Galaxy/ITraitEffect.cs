using Stareater.GameData.Databases;

namespace Stareater.Galaxy
{
	interface ITraitEffect
	{
		void PostcombatApply(LocationBody location, StatesDB states, StaticsDB statics);
	}
}
