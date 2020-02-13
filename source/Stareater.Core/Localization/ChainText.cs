using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn;
using Stareater.Utils.Collections;

namespace Stareater.Localization
{
	class ChainText : IkadnBaseObject, IText
	{
		private readonly IEnumerable<IText> textRuns;
		private readonly HashSet<string> variableNames;

		public ChainText(IEnumerable<IText> textRuns)
		{
			this.textRuns = textRuns;
			this.variableNames = new HashSet<string>();

			foreach (var textRun in textRuns)
				this.variableNames.UnionWith(textRun.VariableNames());
		}

		protected override void DoCompose(IkadnWriter writer)
		{
			throw new InvalidOperationException(this.Tag + " is not meant to be serialized.");
		}

		public override T To<T>()
		{
			var target = typeof(T);

			if (target.IsInstanceOfType(this))
				return (T)(object)this;
			else
				throw new InvalidOperationException("Cast to " + target.Name + " is not supported for " + this.Tag);
		}

		public override object Tag
		{
			get { return "ChainText"; }
		}

		public IEnumerable<string> VariableNames()
		{
			return this.variableNames;
		}

		public string Text()
		{
			if (this.variableNames.Count != 0)
#pragma warning disable CA1303 // Do not pass literals as localized parameters
				throw new InvalidOperationException("This IText has variables, call an overload that sets their values.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

			var text = new StringBuilder();
			foreach (var textRun in this.textRuns)
				text.Append(textRun.Text());
			return text.ToString();
		}

		public string Text(double trivialVariable)
		{
			if (this.variableNames.Count == 0)
#pragma warning disable CA1303 // Do not pass literals as localized parameters
				throw new InvalidOperationException("This IText has no variables");
			else if (this.variableNames.Count > 1)
				throw new InvalidOperationException("This IText has more than one variable, call anoverload that sets all their values.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

			return Text(new Var(this.variableNames.First(), trivialVariable).Get);
		}

		public string Text(IDictionary<string, double> variables)
		{
			if (this.variableNames.Count > 0 && !this.variableNames.IsSubsetOf(variables.Keys))
#pragma warning disable CA1303 // Do not pass literals as localized parameters
				throw new ArgumentException("Keys of the given table of variables do not match with expected set of keys.", nameof(variables));
#pragma warning restore CA1303 // Do not pass literals as localized parameters

			var text = new StringBuilder();
			foreach (var textRun in this.textRuns)
				text.Append(textRun.Text(variables));

			return text.ToString();
		}

		public string Text(IDictionary<string, string> placeholderContents)
		{
			if (this.variableNames.Count > 0)
#pragma warning disable CA1303 // Do not pass literals as localized parameters
				throw new ArgumentException("No variables passed to IText with variables.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

			var text = new StringBuilder();
			foreach (var textRun in this.textRuns)
				text.Append(textRun.Text(placeholderContents));

			return text.ToString();
		}

		public string Text(IDictionary<string, double> variables, IDictionary<string, string> placeholderContents)
		{
			if (this.variableNames.Count > 0 && !this.variableNames.IsSubsetOf(variables.Keys))
#pragma warning disable CA1303 // Do not pass literals as localized parameters
				throw new ArgumentException("Keys of the given table of variables do not match with expected set of keys.", nameof(variables));
#pragma warning restore CA1303 // Do not pass literals as localized parameters

			var text = new StringBuilder();
			foreach (var textRun in this.textRuns)
				text.Append(textRun.Text(variables, placeholderContents));

			return text.ToString();
		}
	}
}
