namespace Stareater.Galaxy.BodyTraits
{
	class EffectTypeTemporary : ITraitEffectType
	{
		private readonly int duration;

		public EffectTypeTemporary(double duration)
		{
			this.duration = (int)duration;
		}

		public ITrait Instantiate(TraitType traitType)
		{
			return new EffectTemporary(traitType, this.duration);
		}
	}
}
