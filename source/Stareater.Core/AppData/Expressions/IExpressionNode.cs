using System.Collections.Generic;

namespace Stareater.AppData.Expressions
{
	interface IExpressionNode
	{
		bool IsConstant { get; }
		IExpressionNode Simplified();
		IExpressionNode Substitute(Dictionary<string, Formula> mapping);
		
		double Evaluate(IDictionary<string, double> variables);
		IEnumerable<string> Variables { get; }
	}
}
