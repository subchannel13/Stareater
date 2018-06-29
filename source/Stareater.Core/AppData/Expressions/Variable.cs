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
