using System;
using System.Linq;
using Stareater.GameData.Databases;

namespace Stareater.Galaxy
{
	class TraitEffectTypeAfflictPlanets : ITraitEffectType
	{
		private readonly string afflictionId;
		private readonly double initialDuration;
		
		public TraitEffectTypeAfflictPlanets(string afflictionId, double initialDuration) 
		{
			this.afflictionId = afflictionId;
			this.initialDuration = initialDuration;
		}

		public ITraitEffect Instantiate(LocationBody location, BodyTrait parentTrait)
		{
			return new TraitEffectAfflictPlanets(this, parentTrait, location.Star);
		}

		class TraitEffectAfflictPlanets : ITraitEffect
		{
			private readonly TraitEffectTypeAfflictPlanets type;
			private readonly BodyTrait parentTrait;
			private readonly StarData star;
			
			private double duration;
			
			public TraitEffectAfflictPlanets(TraitEffectTypeAfflictPlanets type, BodyTrait parentTrait, StarData star)
			{
				this.type = type;
				this.parentTrait = parentTrait;
				this.star = star;
				this.duration = type.initialDuration;
			}
			
			public void PostcombatApply(StatesDB states, StaticsDB statics)
			{
				if (this.duration <= 0)
				{
					this.star.Traits.PendRemove(this.parentTrait);
					return;
				}
				
				var trait = statics.Traits[this.type.afflictionId];
	
				foreach (var planet in states.Planets.At[this.star])
					if (planet.Traits.All(x => x.Type.IdCode != this.type.afflictionId))
						planet.Traits.Add(trait.Instantiate(planet));
				
				this.duration--;
			}
		}
	}
}
