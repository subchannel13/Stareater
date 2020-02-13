using Stareater.GameData.Databases;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.Galaxy.BodyTraits
{
	[StateTypeAttribute(saveTag: SaveTag)]
	class EffectAfflictPlanets : IStarTrait
	{
		[StatePropertyAttribute]
		public StarTraitType Type { get; private set; }

		private Affliction[] afflictions = null;

		public EffectAfflictPlanets(StarTraitType traitType)
		{
			this.Type = traitType;
		}

		private EffectAfflictPlanets()
		{ }

		public void PostcombatApply(StaticsDB statics, StarData star, IEnumerable<Planet> planets)
		{
			this.InitialApply(statics, star, planets);
		}

		public void InitialApply(StaticsDB statics, StarData star, IEnumerable<Planet> planets)
		{
			this.pullAfflictions();
			var vars = new Var().
				Init(statics.StarTraits.Keys, false).
				Init(statics.PlanetTraits.Keys, false).
				UnionWith(star.Traits.Select(x => x.Type.IdCode));

			foreach (var affliction in this.afflictions)
			{
				var trait = statics.PlanetTraits[affliction.TraitId];

				foreach (var planet in planets)
				{
					vars.UnionWith(planet.Traits.Select(x => x.IdCode));
					vars.Set("position", planet.Position);
					planet.Traits.RemoveWhere(x => x.IdCode == trait.IdCode);

					if (affliction.Condition.Evaluate(vars.Get) >= 0)
						planet.Traits.Add(trait);
				}
			}
		}

		public override string ToString()
		{
			return this.Type.IdCode;
		}

		private void pullAfflictions()
		{
			if (this.afflictions == null)
				this.afflictions = ((EffectTypeAfflictPlanets)this.Type.Effect).Afflictions;
		}

		public const string SaveTag = "Afflict";
	}
}
