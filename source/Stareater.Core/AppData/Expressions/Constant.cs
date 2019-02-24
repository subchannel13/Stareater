using System.Collections.Generic;

namespace Stareater.AppData.Expressions
{
	class Constant : IExpressionNode
	{
		private readonly double value;

		public Constant(double value)
		{
			this.value = value;
		}

		public IExpressionNode Simplified()
		{
			return this;
		}

		public IExpressionNode Substitute(Dictionary<string, Formula> mapping)
		{
			return new Constant(this.value);
		}

		public bool IsConstant
		{
			get { return true; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return value;
		}
		
		public IEnumerable<string> Variables 
		{ 
			get
			{
				yield break;
			}
		}
		
		public override string ToString()
		{
			return "Const: " + value;
		}
	}
}
