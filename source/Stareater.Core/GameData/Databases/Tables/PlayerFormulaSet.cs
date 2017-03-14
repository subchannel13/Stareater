using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameData.Databases.Tables
{
	class PlayerFormulaSet
	{
		public double Research { get; private set; }
		public double FocusedResearchWeight { get; private set; }
		
		public PlayerFormulaSet(Formula research, Formula focusedResearchWeight)
		{
			this.FocusedResearchWeight = focusedResearchWeight.Evaluate(null);
			this.Research = research.Evaluate(null);
		}
	}
}
