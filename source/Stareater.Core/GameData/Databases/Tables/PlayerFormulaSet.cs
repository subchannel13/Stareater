using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	class PlayerFormulaSet
	{
		public double FocusedResearchWeight { get; private set; }
		
		public PlayerFormulaSet(Formula focusedResearchWeight)
		{
			this.FocusedResearchWeight = focusedResearchWeight.Evaluate(null);
		}
	}
}
