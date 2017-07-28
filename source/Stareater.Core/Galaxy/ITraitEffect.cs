using Ikadn.Ikon.Types;
using Stareater.GameData.Databases;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy
{
	[StateType(saveMethod: "Save")]
	interface ITraitEffect
	{
		void PostcombatApply(StatesDB states, StaticsDB statics);
		
		ITraitEffect Copy();
		IkonBaseObject Save(SaveSession session);

		//TODO(v0.7) remove usage
		void SaveInto(IkonComposite destination);
	}
}
