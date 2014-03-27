using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	interface IExpressionNode
	{
		IExpressionNode Simplified();
		bool isConstant { get; }
		double Evaluate(IDictionary<string, double> variables);
		IEnumerable<string> Variables { get; } //TODO(later): add to unit tests
	}
}
