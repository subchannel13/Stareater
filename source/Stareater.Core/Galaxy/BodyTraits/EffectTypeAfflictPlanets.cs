namespace Stareater.Galaxy.BodyTraits
{
	class EffectTypeAfflictPlanets : ITraitEffectType
	{
		private readonly double initialDuration;

		public string AfflictionId { get; private set; }
		
		public EffectTypeAfflictPlanets(string afflictionId, double initialDuration) 
		{
			this.AfflictionId = afflictionId;
			this.initialDuration = initialDuration;
		}

		public ITrait Instantiate(TraitType traitType)
		{
			return new EffectAfflictPlanets(traitType, (int)this.initialDuration);
		}
	}
}
