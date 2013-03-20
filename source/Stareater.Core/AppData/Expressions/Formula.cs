using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.AppData.Expressions
{
	public class Formula
	{
		private IExpressionNode root;

		internal Formula(IExpressionNode root)
		{
			this.root = root;
		}

		public double Evaluate(IDictionary<string, double> variables)
		{
			return root.Evaluate(variables);
		}
	}
}
