using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.GameData.Databases;

namespace Stareater.Galaxy
{
	class TraitEffectPassive : ITraitEffect
	{
		public void PostcombatApply(LocationBody location, StatesDB states, StaticsDB statics)
		{
			//no operation
		}
	}
}
