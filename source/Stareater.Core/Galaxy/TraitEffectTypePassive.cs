using System;
using Ikadn.Ikon.Types;
using Stareater.GameData.Databases;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy
{
	class TraitEffectTypePassive : ITraitEffectType
	{
		public ITraitEffect Instantiate(LocationBody location, BodyTrait parentTrait)
		{
			return new TraitEffectPassive();
		}
		
		public ITraitEffect Load(LocationBody location, BodyTrait bodyTrait, IkonComposite loadData)
		{
			return new TraitEffectPassive();
		}

		[StateType(saveMethod: "Save")]
		class TraitEffectPassive : ITraitEffect
		{
			public void PostcombatApply(StatesDB states, StaticsDB statics)
			{
				//no operation
			}

			public ITraitEffect Copy()
			{
				return new TraitEffectPassive();
			}

			public void SaveInto(IkonComposite destination)
			{
				//no operation
			}

			public IkonBaseObject Save(SaveSession session)
			{
				return new IkonComposite("Passive");
            }
		}
	}
}
