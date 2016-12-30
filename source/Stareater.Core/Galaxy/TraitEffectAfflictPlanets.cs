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
		private StarData star;

		public TraitEffectAfflictPlanets(string afflictionId) : 
			this(afflictionId, null)
		{ }

		private TraitEffectAfflictPlanets(string afflictionId, StarData star)
		{
			this.afflictionId = afflictionId;
			this.star = star;
		}

		public ITraitEffect Instantiate(LocationBody location)
		{
			return new TraitEffectAfflictPlanets(this.afflictionId, location.Star);
		}

		public void PostcombatApply(StatesDB states, StaticsDB statics)
		{
			var trait = statics.Traits[afflictionId];

			foreach (var planet in states.Planets.At[this.star])
				if (planet.Traits.All(x => x.Type.IdCode != afflictionId))
					planet.Traits.Add(trait.Instantiate(planet));
		}
	}
}
