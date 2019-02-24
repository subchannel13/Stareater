using System.Collections.Generic;
using System.Linq;

namespace Stareater.AppData.Expressions
{
	public class Formula
	{
		private ISet<string> variables = null;
		internal IExpressionNode Root { get; private set; }

		internal Formula(IExpressionNode root)
		{
			this.Root = root;
		}

		internal Formula(double constValue) : this(new Constant(constValue))
		{ }
		
		internal Formula(bool constCondition) : this(new Constant(constCondition ? 1 : -1))
		{ }
		
		//TODO(v0.8) add trivial variable overload or maybe for multiple variables in alphabetical order
		public double Evaluate(IDictionary<string, double> variables)
		{
			try {
				return Root.Evaluate(variables);
			}
			catch(KeyNotFoundException)
			{
				ErrorReporter.Get.Report(new KeyNotFoundException("Missing vars: " + string.Join(" ", this.Variables.Where(x => !variables.ContainsKey(x)).ToArray())));
			}

			return double.NaN;
		}

		public Formula Substitute(Dictionary<string, Formula> mapping)
		{
			return new Formula(this.Root.Substitute(mapping));
		}
		
		public ISet<string> Variables 
		{
			get 
			{
				if (variables == null)
					variables = new HashSet<string>(this.Root.Variables);
				
				return variables;
			}
		}
	}
}
