using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.GameData.Databases;

namespace Stareater.Galaxy
{
	class TraitEffectAfflictPlanets : ITraitEffect
	{
		private string afflictionId;

		public TraitEffectAfflictPlanets(string afflictionId)
		{
			this.afflictionId = afflictionId;
		}

		public void PostcombatApply(LocationBody location, StatesDB states, StaticsDB statics)
		{
			var trait = statics.Traits[afflictionId];

			foreach (var planet in states.Planets.At[location.Star])
				if (!planet.Traits.Contains(trait))
					planet.Traits.Add(trait);
		}
	}
}
