using Stareater.GameData.Databases;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy.BodyTraits
{
	[StateType(saveTag: SaveTag)]
	class EffectTemporary : ITrait
	{
		[StateProperty]
		public TraitType Type { get; private set; }

		[StateProperty]
		public int Duration { get; set; }

		public EffectTemporary(TraitType traitType, int duration)
		{
			this.Type = traitType;
			this.Duration = duration;
		}

		public void PostcombatApply(StatesDB states, StaticsDB statics, StarData star)
		{
			if (this.Duration <= 0)
			{
				star.Traits.PendRemove(this);
				return;
			}

			this.Duration--;
		}

		public const string SaveTag = "Temporary";
	}
}
