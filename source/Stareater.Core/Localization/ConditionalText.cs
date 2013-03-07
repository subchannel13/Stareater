using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn;
using Stareater.Utils;

namespace Stareater.Localization
{
	class ConditionalText : IkadnBaseValue, IText
	{
		string variableName;
		IText text;
		HashSet<string> variables = new HashSet<string>();
		TableSubset<string> nestedVariables;

		public ConditionalText(string variableName, IText text)
		{
			this.variableName = variableName;
			this.text = text;
			this.variables.Add(variableName);

			string[] textVariables = text.VariableNames().ToArray();
			if (textVariables.Length > 0) {
				this.nestedVariables = new TableSubset<string>(textVariables);
				this.variables.UnionWith(textVariables);
			}
			else {
				this.nestedVariables = null;
			}
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
			return (IEnumerable<string>)variables ?? new string[] { variableName };
		}

		public string Text()
		{
			throw new NotImplementedException();
		}

		public string Text(double trivialVariable)
		{
			if (nestedVariables != null)
				throw new InvalidOperationException("This IText has more than one variable, call overload that set all their values.");

			return Text(new Dictionary<string, double>()
			{
				{variableName, trivialVariable}
			});
		}

		public string Text(IDictionary<string, double> variables)
		{
			if (!this.variables.SetEquals(variables.Keys))
				throw new ArgumentException("Keys of the given table of variables do not match with expected set of keys.", "variables");

			if (variables[variableName] >= 0)
				return (nestedVariables != null) ?
					text.Text(nestedVariables.Extract(variables)) :
					text.Text();
			else
				return "";
		}
	}
}
