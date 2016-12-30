using Stareater.GameData.Databases;

namespace Stareater.Galaxy
{
	interface ITraitEffect
	{
		ITraitEffect Instantiate(LocationBody location);
		void PostcombatApply(StatesDB states, StaticsDB statics);
	}
}
