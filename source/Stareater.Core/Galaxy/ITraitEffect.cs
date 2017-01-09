using Ikadn.Ikon.Types;
using Stareater.GameData.Databases;

namespace Stareater.Galaxy
{
	interface ITraitEffect
	{
		void PostcombatApply(StatesDB states, StaticsDB statics);
		
		ITraitEffect Copy();
		void Save(IkonComposite destination);
	}
}
