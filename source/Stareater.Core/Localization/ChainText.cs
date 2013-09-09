using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.Localization
{
	class ChainText : IkadnBaseObject, IText
	{
		IEnumerable<IText> textRuns;
		HashSet<string> variables;

		public ChainText(IEnumerable<IText> textRuns)
		{
			this.textRuns = textRuns;
			this.variables = new HashSet<string>();

			foreach (var textRun in textRuns)
				this.variables.UnionWith(textRun.VariableNames());
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
			get { return "ChainText"; }
		}

		public IEnumerable<string> VariableNames()
		{
			return variables;
		}
		public string Text()
		{
			if (variables.Count != 0)
				throw new InvalidOperationException("This IText has variables, call an overload that sets their values.");

			StringBuilder text = new StringBuilder();
			foreach (var textRun in textRuns)
				text.Append(textRun.Text());
			return text.ToString();
		}

		public string Text(double trivialVariable)
		{
			if (variables.Count == 0)
				throw new InvalidOperationException("This IText has no variables");
			else if (variables.Count > 1)
				throw new InvalidOperationException("This IText has more than one variable, call anoverload that sets all their values.");

			return Text(new Var(this.variables.First(), trivialVariable).Get);
		}

		public string Text(IDictionary<string, double> variables)
		{
			if (!this.variables.IsSubsetOf(variables.Keys))
				throw new ArgumentException("Keys of the given table of variables do not match with expected set of keys.", "variables");

			StringBuilder text = new StringBuilder();
			foreach (var textRun in textRuns)
				text.Append(textRun.Text(variables));

			return text.ToString();
		}
	}
}
