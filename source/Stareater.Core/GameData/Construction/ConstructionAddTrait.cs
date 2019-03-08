using Stareater.Galaxy;

namespace Stareater.GameData.Construction
{
	class ConstructionAddTrait : IConstructionEffect
	{
		private readonly string traitId;

		public ConstructionAddTrait(string traitId)
		{
			this.traitId = traitId;
		}

		public void Apply(MainGame game, AConstructionSite site, long quantity)
		{
			site.Location.Planet.Traits.Add(game.Statics.PlanetTraits[this.traitId]);
		}
	}
}
