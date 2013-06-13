using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressionParser_Tests
{
	class Var
	{
		Dictionary<string, double> variables = new Dictionary<string, double>();

		public Var()
		{ }

		public Var(string name, double value)
		{
			variables.Add(name, value);
		}

		public Var And(string name, double value)
		{
			variables.Add(name, value);
			return this;
		}

		public static implicit operator Dictionary<string, double>(Var v)
		{
			return v.variables;
		}
	}
}
