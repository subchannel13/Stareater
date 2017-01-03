using System;
using System.Linq;
using Stareater.GameData.Databases;

namespace Stareater.Galaxy
{
	class TraitEffectTypePassive : ITraitEffectType
	{
		public ITraitEffect Instantiate(LocationBody location, BodyTrait parentTrait)
		{
			return new TraitEffectPassive();
		}

		class TraitEffectPassive : ITraitEffect
		{
			public void PostcombatApply(StatesDB states, StaticsDB statics)
			{
				//no operation
			}
		}
	}
}
