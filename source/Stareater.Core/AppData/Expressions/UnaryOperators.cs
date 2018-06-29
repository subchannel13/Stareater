using System.Collections.Generic;

namespace Stareater.AppData.Expressions
{
	abstract class UnaryOperator : IExpressionNode
	{
		protected readonly IExpressionNode child;

		protected UnaryOperator(IExpressionNode child)
		{
			this.child = child;
		}

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
		public Negation(IExpressionNode child) : base(child)
		{ }

		public override double Evaluate(IDictionary<string, double> variables)
		{
			return -child.Evaluate(variables);
		}
	}

	class ToBoolean : UnaryOperator
	{
		public ToBoolean(IExpressionNode child) : base(child)
		{ }

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
		public NegateToBoolean(IExpressionNode child) : base(child)
		{ }

		public override double Evaluate(IDictionary<string, double> variables)
		{
			return child.Evaluate(variables) >= 0 ? -1 : 1;
		}
	}
}
