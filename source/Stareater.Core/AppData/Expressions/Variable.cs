using System.Collections.Generic;

namespace Stareater.AppData.Expressions
{
	class Variable : IExpressionNode
	{
		private readonly string name;

		public Variable(string name)
		{
			this.name = name;
		}

		public IExpressionNode Simplified()
		{
			return this;
		}

		public IExpressionNode Substitute(Dictionary<string, Formula> mapping)
		{
			if (mapping.ContainsKey(this.name))
				return mapping[this.name].Root;
			else
				return new Variable(this.name);
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
