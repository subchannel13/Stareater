using System;
using Stareater.AppData.Expressions;

namespace Stareater.GameLogic
{
	class ConstructionSetVar : AConstructionEffect
	{
		private string varName;
		private Formula outputValue;
		
		public ConstructionSetVar(string varName, Formula outputExpression)
		{
			this.varName = varName;
			this.outputValue = outputExpression;
		}
	}
}
