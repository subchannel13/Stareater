using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	abstract class UnaryOperator : IExpressionNode
	{
		public IExpressionNode child;

		public bool IsConstant
		{
			get { return child.IsConstant; }
		}

		public IExpressionNode Simplified()
		{
			if (child.IsConstant)
				return new Constant(this.Evaluate(null));

			return this;
		}

		public abstract double Evaluate(IDictionary<string, double> variables);
		
		public IEnumerable<string> Variables
		{ 
			get
			{
				foreach(var variable in child.Variables)
					yield return variable;
			}
		}
	}

	class Negation : UnaryOperator
	{
		public override double Evaluate(IDictionary<string, double> variables)
		{
			return -child.Evaluate(variables);
		}
	}

	class ToBoolean : UnaryOperator
	{
		public override double Evaluate(IDictionary<string, double> variables)
		{
			return Normalize(child.Evaluate(variables));
		}
		
		public static double Normalize(double x)
		{
			return x >= 0 ? 1 : -1;
		}
	}

	class NegateToBoolean : UnaryOperator
	{
		public override double Evaluate(IDictionary<string, double> variables)
		{
			return child.Evaluate(variables) >= 0 ? -1 : 1;
		}
	}
}
