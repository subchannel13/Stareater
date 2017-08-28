namespace Stareater.Galaxy.BodyTraits
{
	class EffectTypePassive : ITraitEffectType
	{
		public ITrait Instantiate(TraitType traitType)
		{
			return new EffectPassive(traitType);
		}
	}
}
