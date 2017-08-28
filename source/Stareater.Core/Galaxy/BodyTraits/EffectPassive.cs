using Stareater.GameData.Databases;
using Stareater.Utils.StateEngine;

namespace Stareater.Galaxy.BodyTraits
{
	[StateType(saveTag: SaveTag)]
	class EffectPassive : ITrait
	{
		[StateProperty]
		public TraitType Type { get; private set; }

		public EffectPassive(TraitType type)
		{
			this.Type = type;
		}

		private EffectPassive()
		{ }

		public void PostcombatApply(StatesDB states, StaticsDB statics, StarData star)
		{
			//no operation
		}

		public const string SaveTag = "Passive";
	}
}
