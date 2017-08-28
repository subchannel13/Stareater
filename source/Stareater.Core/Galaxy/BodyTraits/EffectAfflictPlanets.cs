using Stareater.GameData.Databases;
using Stareater.Utils.StateEngine;
using System.Linq;

namespace Stareater.Galaxy.BodyTraits
{
	[StateType(saveTag: SaveTag)]
	class EffectAfflictPlanets : ITrait
	{
		[StateProperty]
		public TraitType Type { get; private set; }

		[StateProperty]
		public int Duration { get; set; }

		private string afflictionId = null;

		public EffectAfflictPlanets(TraitType traitType, int duration)
		{
			this.Type = traitType;
			this.Duration = duration;
		}

		private EffectAfflictPlanets()
		{ }

		public void PostcombatApply(StatesDB states, StaticsDB statics, StarData star)
		{
			if (this.Duration <= 0)
			{
				star.Traits.PendRemove(this);
				return;
			}

			this.pullAfflictionId();
			var trait = statics.Traits[afflictionId];

			foreach (var planet in states.Planets.At[star])
				if (planet.Traits.All(x => x.Type.IdCode != afflictionId))
					planet.Traits.Add(trait.Effect.Instantiate(trait));

			this.Duration--;
		}

		private void pullAfflictionId()
		{
			if (this.afflictionId == null)
				this.afflictionId = ((EffectTypeAfflictPlanets)this.Type.Effect).AfflictionId;
		}

		public const string SaveTag = "Afflict";
	}
}
