using Stareater.GameData.Databases;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System.Linq;

namespace Stareater.Galaxy.BodyTraits
{
	[StateType(saveTag: SaveTag)]
	class EffectAfflictPlanets : ITrait
	{
		[StateProperty]
		public TraitType Type { get; private set; }

		private Affliction[] afflictions = null;

		public EffectAfflictPlanets(TraitType traitType)
		{
			this.Type = traitType;
		}

		private EffectAfflictPlanets()
		{ }

		public void PostcombatApply(StatesDB states, StaticsDB statics, StarData star)
		{
			this.pullAfflictions();
			var vars = new Var().
				Init(statics.StarTraits.Keys, false).
				UnionWith(star.Traits.Select(x => x.Type.IdCode)).
				Get;

			foreach (var affliction in this.afflictions)
			{
				var trait = statics.PlanetTraits[affliction.TraitId];

				foreach (var planet in states.Planets.At[star])
				{
					vars["position"] = planet.Position;
					planet.Traits.RemoveWhere(x => x.Type.IdCode == trait.IdCode);

					if (affliction.Condition.Evaluate(vars) >= 0)
						planet.Traits.Add(trait.Effect.Instantiate(trait));
				}
			}
		}

		private void pullAfflictions()
		{
			if (this.afflictions == null)
				this.afflictions = ((EffectTypeAfflictPlanets)this.Type.Effect).Afflictions;
		}

		public const string SaveTag = "Afflict";
	}
}
