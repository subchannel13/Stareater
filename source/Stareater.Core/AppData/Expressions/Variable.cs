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

		public bool IsConstant
		{
			get { return false; }
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return variables[name];
		}
		
		public IEnumerable<string> Variables 
		{ 
			get
			{
				yield return name;
			}
		}
		
		public override string ToString()
		{
			return "Var: " + name;
		}
		
	}
}
