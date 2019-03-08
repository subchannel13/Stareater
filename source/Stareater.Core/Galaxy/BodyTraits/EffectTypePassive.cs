namespace Stareater.Galaxy.BodyTraits
{
	class EffectTypePassive : ITraitEffectType
	{
		public IStarTrait Instantiate(StarTraitType traitType)
		{
			return new EffectPassive(traitType);
		}
	}
}
