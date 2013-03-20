using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	class Variable : IExpressionNode
	{
		string name;

		public Variable(string name)
		{
			this.name = name;
		}

		public IExpressionNode Simplified()
		{
			return this;
		}

		public bool isConstant
		{
			get { return false; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return variables[name];
		}
	}
}
