using Stareater.GameData.Databases;

namespace Stareater.Galaxy
{
	interface ITraitEffect
	{
		void PrecombatApply(StatesDB states, StaticsDB statics);
	}
}
