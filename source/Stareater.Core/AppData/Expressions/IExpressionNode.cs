using System.Collections.Generic;

namespace Stareater.AppData.Expressions
{
	interface IExpressionNode
	{
		IExpressionNode Simplified();
		bool IsConstant { get; }
		double Evaluate(IDictionary<string, double> variables);
		IEnumerable<string> Variables { get; }
	}
}
