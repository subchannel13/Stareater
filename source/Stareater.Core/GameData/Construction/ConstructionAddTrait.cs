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
			var trait = game.Statics.PlanetTraits[this.traitId].Make();

			if (site.Location.Planet != null)
				site.Location.Planet.Traits.Add(trait);
			else
				site.Location.Star.Traits.Add(trait);
		}
	}
}
