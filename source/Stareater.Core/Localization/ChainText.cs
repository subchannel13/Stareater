using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikon;
using Stareater.Utils;

namespace Stareater.Localization
{
	class ChainText : IkonBaseValue, IText
	{
		Tuple<IText, TableSubset<string>>[] textRuns;
		HashSet<string> variables;

		public ChainText(IEnumerable<IText> textRuns)
		{
			this.variables = new HashSet<string>();

			var textRunInfos = new List<Tuple<IText, TableSubset<string>>>();
			foreach (var textRun in textRuns) {
				textRunInfos.Add(new Tuple<IText, TableSubset<string>>(
					textRun,
					new TableSubset<string>(textRun.VariableNames())
					));
				this.variables.UnionWith(textRun.VariableNames());
			}
			this.textRuns = textRunInfos.ToArray();			
		}

		protected override void DoCompose(IkonWriter writer)
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
			get { return "ChainText"; }
		}

		public IEnumerable<string> VariableNames()
		{
			return variables;
		}
		public string Text()
		{
			if (variables.Count != 0)
				throw new InvalidOperationException("This IText has variables, call overload that sets their values.");

			StringBuilder text = new StringBuilder();
			foreach (var textRun in textRuns)
				text.Append(textRun.Item1.Text());
			return text.ToString();
		}

		public string Text(double trivialVariable)
		{
			if (variables.Count == 0)
				throw new InvalidOperationException("This IText has no variables");
			else if (variables.Count > 1)
				throw new InvalidOperationException("This IText has more than one variable, call overload that set all their values.");

			return Text(new Dictionary<string, double>()
			{
				{this.variables.First(), trivialVariable}
			});
		}

		public string Text(IDictionary<string, double> variables)
		{
			if (!this.variables.SetEquals(variables.Keys))
				throw new ArgumentException("Keys of the given table of variables do not match with expected set of keys.", "variables");

			StringBuilder text = new StringBuilder();
			foreach (var textRun in textRuns)
				if (textRun.Item2 == null)
					text.Append(textRun.Item1.Text());
				else
					text.Append(textRun.Item1.Text(textRun.Item2.Extract(variables)));

			return text.ToString();
		}
	}
}
