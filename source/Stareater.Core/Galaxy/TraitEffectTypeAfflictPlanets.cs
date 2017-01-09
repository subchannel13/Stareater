using System;
using System.Linq;
using Ikadn.Ikon.Types;
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
			return new TraitEffectAfflictPlanets(this, parentTrait, location.Star, (int)this.initialDuration);
		}
		
		public ITraitEffect Load(LocationBody location, BodyTrait parentTrait, IkonComposite loadData)
		{
			return new TraitEffectAfflictPlanets(this, parentTrait, location.Star, loadData[StaticsDB.DurationTraitId].To<int>());
		}

		class TraitEffectAfflictPlanets : ITraitEffect
		{
			private readonly TraitEffectTypeAfflictPlanets type;
			private readonly BodyTrait parentTrait;
			private readonly StarData star;
			
			private int duration;
			
			public TraitEffectAfflictPlanets(TraitEffectTypeAfflictPlanets type, BodyTrait parentTrait, StarData star, int duration)
			{
				this.type = type;
				this.parentTrait = parentTrait;
				this.star = star;
				this.duration = duration;
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
			
			public ITraitEffect Copy()
			{
				return new TraitEffectAfflictPlanets(this.type, this.parentTrait, this.star, this.duration);
			}
			
			public void Save(IkonComposite destination)
			{
				destination.Add(StaticsDB.DurationTraitId, new IkonInteger(this.duration));
			}
		}
	}
}
