using Ikadn;
using Stareater.AppData.Expressions;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.Localization
{
	class ConditionalText : IkadnBaseObject, IText
	{
		private readonly Formula forumla;
		private readonly IText text;
		private readonly HashSet<string> variableNames = new HashSet<string>();

		public ConditionalText(Formula forumla, IText text)
		{
			this.forumla = forumla;
			this.text = text;

			this.variableNames.UnionWith(forumla.Variables);
			this.variableNames.UnionWith(text.VariableNames());
		}

		protected override void DoCompose(IkadnWriter writer)
		{
			throw new InvalidOperationException(this.Tag + " is not meant to be serialized.");
		}

		public override T To<T>()
		{
			var target = typeof(T);

			if (target.IsAssignableFrom(this.GetType()))
				return (T)(object)this;
			else
				throw new InvalidOperationException("Cast to " + target.Name + " is not supported for " + this.Tag);
		}

		public override object Tag
		{
			get { return "ExpressionText"; }
		}

		public IEnumerable<string> VariableNames()
		{
			return this.variableNames;
		}

		public string Text()
		{
#pragma warning disable CA1303 // Do not pass literals as localized parameters
			throw new InvalidOperationException("This IText has one or more variables, call an overload that sets their values.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
		}

		public string Text(double trivialVariable)
		{
			if (this.variableNames.Count > 1)
#pragma warning disable CA1303 // Do not pass literals as localized parameters
				throw new InvalidOperationException("This IText has more than one variable, call an overload that sets all their values.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters

			return Text(new Var(this.variableNames.First(), trivialVariable).Get);
		}

		public string Text(IDictionary<string, double> variables)
		{
			if (this.variableNames.Count > 0 && !this.variableNames.IsSubsetOf(variables.Keys))
#pragma warning disable CA1303 // Do not pass literals as localized parameters
				throw new ArgumentException("Keys of the given table of variables do not match with expected set of keys.", nameof(variables));
#pragma warning restore CA1303 // Do not pass literals as localized parameters

			if (this.forumla.Evaluate(variables) >= 0)
				return this.text.Text(variables);
			else
				return "";
		}

		public string Text(IDictionary<string, string> placeholderContents)
		{
#pragma warning disable CA1303 // Do not pass literals as localized parameters
			throw new InvalidOperationException("This IText has a numeric variables, call an overload that supplies values for it.");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
		}

		public string Text(IDictionary<string, double> variables, IDictionary<string, string> placeholderContents)
		{
			if (this.variableNames.Count > 0 && !this.variableNames.IsSubsetOf(variables.Keys))
#pragma warning disable CA1303 // Do not pass literals as localized parameters
				throw new ArgumentException("Keys of the given table of variables do not match with expected set of keys.", nameof(variables));
#pragma warning restore CA1303 // Do not pass literals as localized parameters

			if (this.forumla.Evaluate(variables) >= 0)
				return this.text.Text(variables, placeholderContents);
			else
				return "";
		}
	}
}
