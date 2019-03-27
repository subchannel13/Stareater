using Stareater.GameData.Databases;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.Galaxy.BodyTraits
{
	[StateType(saveTag: SaveTag)]
	class EffectTemporary : IStarTrait
	{
		[StateProperty]
		public StarTraitType Type { get; private set; }

		[StateProperty]
		public int Duration { get; set; }

		public EffectTemporary(StarTraitType traitType, int duration)
		{
			this.Type = traitType;
			this.Duration = duration;
		}

		private EffectTemporary()
		{ }

		public void PostcombatApply(StaticsDB statics, StarData star, IEnumerable<Planet> planets)
		{
			if (this.Duration <= 0)
			{
				star.Traits.PendRemove(this);
				return;
			}

			this.Duration--;
		}

		public void InitialApply(StaticsDB statics, StarData star, IEnumerable<Planet> planets)
		{
			//no operation
		}

		public override string ToString()
		{
			return this.Type.IdCode;
		}

		public const string SaveTag = "Temporary";
	}
}
