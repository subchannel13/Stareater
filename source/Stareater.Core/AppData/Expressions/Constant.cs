using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class Constant : IExpressionNode
	{
		double value;

		public Constant(double value)
		{
			this.value = value;
		}

		public IExpressionNode Simplified()
		{
			return this;
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
