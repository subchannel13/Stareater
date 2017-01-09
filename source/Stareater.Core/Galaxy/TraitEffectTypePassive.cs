using System;
using System.Linq;
using Ikadn.Ikon.Types;
using Stareater.GameData.Databases;

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

			public void Save(IkonComposite destination)
			{
				//no operation
			}
		}
	}
}
