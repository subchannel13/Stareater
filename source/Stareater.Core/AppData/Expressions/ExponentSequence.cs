using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class ExponentSequence : IExpressionNode
	{
		IExpressionNode[] sequence;

		public ExponentSequence(IEnumerable<IExpressionNode> sequence)
		{
			this.sequence = sequence.ToArray();
		}

		public IExpressionNode Simplified()
		{
			return (sequence.Length == 1) ? sequence[0] : this;
		}

		public bool isConstant
		{
			get
			{
				return sequence.All(element => element.isConstant);
			}
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			double result = sequence[sequence.Length - 1].Evaluate(variables);
			for (int i = sequence.Length - 2; i >= 0; i--)
				result = Math.Pow(sequence[i].Evaluate(variables), result);

			return result;
		}
	}
}
