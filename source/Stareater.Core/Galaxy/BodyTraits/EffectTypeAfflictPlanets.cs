namespace Stareater.Galaxy.BodyTraits
{
	class EffectTypeAfflictPlanets : ITraitEffectType
	{
		public Affliction[] Afflictions { get; private set; }

		public EffectTypeAfflictPlanets(Affliction[] afflictions) 
		{
			this.Afflictions = afflictions;
		}

		public ITrait Instantiate(TraitType traitType)
		{
			return new EffectAfflictPlanets(traitType);
		}
	}
}
