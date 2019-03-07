using Stareater.AppData.Expressions;

namespace Stareater.Galaxy.BodyTraits
{
	class Affliction
	{
		public string TraitId { get; private set; }

		public Formula Condition { get; private set; }

		public Affliction(string traitId, Formula condition)
		{
			this.TraitId = traitId;
			this.Condition = condition;
		}
	}
}
