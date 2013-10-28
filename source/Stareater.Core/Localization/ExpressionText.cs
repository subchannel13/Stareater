using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn;
using Stareater.AppData.Expressions;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;

namespace Stareater.Localization
{
	class ExpressionText : IkadnBaseObject, IText
	{
		private Formula formula;
		private Func<double, string> formatter;

		public ExpressionText(Formula formula, Func<double, string> formatter)
		{
			this.formula = formula;
			this.formatter = formatter;
		}

		protected override void DoCompose(IkadnWriter writer)
		{
			throw new InvalidOperationException(Tag + " is not meant to be serialized.");
		}

		public override T To<T>()
		{
			Type target = typeof(T);

			if (target.IsAssignableFrom(this.GetType()))
				return (T)(object)this;
			else
				throw new InvalidOperationException("Cast to " + target.Name + " is not supported for " + Tag);
		}

		public override object Tag
		{
			get { return "ExpressionText"; }
		}

		public IEnumerable<string> VariableNames()
		{
			foreach(var varName in formula.Variables)
				yield return varName;
		}

		public string Text()
		{
			if (formula.Variables.Count == 0)
				return formatter(formula.Evaluate(new Dictionary<string, double>()));
			else
				throw new InvalidOperationException("This IText has one or more variables, call an overload that sets all their values.");
		}

		public string Text(double trivialVariable)
		{
			if (formula.Variables.Count == 0)
				throw new InvalidOperationException("This IText has no variables");
			if (formula.Variables.Count > 1)
				throw new InvalidOperationException("This IText has more than one variable, call an overload that sets all their values.");
			
			var variable = new Var(formula.Variables.First(), trivialVariable).Get;
			return formatter(formula.Evaluate(variable));
		}

		public string Text(IDictionary<string, double> variables)
		{
			return formatter(formula.Evaluate(variables));
		}

		public string Text(IDictionary<string, double> variables, IDictionary<string, string> placeholderContents)
		{
			return formatter(formula.Evaluate(variables));
		}
	}
}
