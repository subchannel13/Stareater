using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn;
using Stareater.Utils.NumberFormatters;

namespace Stareater.Localization
{
	class ExpressionText : IkadnBaseValue, IText
	{
		string variableName;
		Func<double, string> formatter;

		public ExpressionText(string variableName, Func<double, string> formatter)
		{
			this.variableName = variableName;
			this.formatter = formatter;
		}

		protected override void DoCompose(IkadnWriter writer)
		{
			throw new InvalidOperationException(TypeName + " is not meant to be serialized.");
		}

		public override T To<T>()
		{
			Type target = typeof(T);

			if (target.IsAssignableFrom(this.GetType()))
				return (T)(object)this;
			else
				throw new InvalidOperationException("Cast to " + target.Name + " is not supported for " + TypeName);
		}

		public override string TypeName
		{
			get { return "ExpressionText"; }
		}

		public IEnumerable<string> VariableNames()
		{
			yield return variableName;
		}

		public string Text()
		{
			throw new NotImplementedException();
		}

		public string Text(double trivialVariable)
		{
			return formatter(trivialVariable);
		}

		public string Text(IDictionary<string, double> variables)
		{
			return formatter(variables[variableName]);
		}
	}
}
