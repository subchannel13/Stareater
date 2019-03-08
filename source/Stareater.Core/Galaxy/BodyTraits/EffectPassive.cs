using Stareater.GameData.Databases;
using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.Galaxy.BodyTraits
{
	[StateType(saveTag: SaveTag)]
	class EffectPassive : IStarTrait
	{
		[StateProperty]
		public StarTraitType Type { get; private set; }

		public EffectPassive(StarTraitType type)
		{
			this.Type = type;
		}

		private EffectPassive()
		{ }

		public void PostcombatApply(StaticsDB statics, StarData star, IEnumerable<Planet> planets)
		{
			//no operation
		}

		public void InitialApply(StaticsDB statics, StarData star, IEnumerable<Planet> planets)
		{
			//no operation
		}

		public const string SaveTag = "Passive";
	}
}
