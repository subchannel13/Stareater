using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.GameData.Databases;

namespace Stareater.Galaxy
{
	class TraitEffectPassive : ITraitEffect
	{
		public ITraitEffect Instantiate(LocationBody location)
		{
			return new TraitEffectPassive();
		}

		public void PostcombatApply(StatesDB states, StaticsDB statics)
		{
			//no operation
		}
	}
}
