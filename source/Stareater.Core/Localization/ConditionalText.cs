using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn;
using Stareater.AppData.Expressions;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.Localization
{
	class ConditionalText : IkadnBaseObject, IText
	{
		Formula forumla;
		IText text;
		HashSet<string> variables = new HashSet<string>();

		public ConditionalText(Formula forumla, IText text)
		{
			this.forumla = forumla;
			this.text = text;

			this.variables.UnionWith(forumla.Variables);
			this.variables.UnionWith(text.VariableNames());
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
			return variables;
		}

		public string Text()
		{
			throw new InvalidOperationException("This IText has one or more variables, call an overload that sets their values.");
		}

		public string Text(double trivialVariable)
		{
			if (variables.Count > 1)
				throw new InvalidOperationException("This IText has more than one variable, call an overload that sets all their values.");

			return Text(new Var(variables.First(), trivialVariable).Get);
		}

		public string Text(IDictionary<string, double> variables)
		{
			if (this.variables.Count > 0 && !this.variables.IsSubsetOf(variables.Keys))
				throw new ArgumentException("Keys of the given table of variables do not match with expected set of keys.", "variables");

			if (forumla.Evaluate(variables) >= 0)
				return text.Text(variables);
			else
				return "";
		}

		public string Text(IDictionary<string, string> placeholderContents)
		{
			throw new InvalidOperationException("This IText has a numeric variables, call an overload that supplies values for it.");
		}

		public string Text(IDictionary<string, double> variables, IDictionary<string, string> placeholderContents)
		{
			if (this.variables.Count > 0 && !this.variables.IsSubsetOf(variables.Keys))
				throw new ArgumentException("Keys of the given table of variables do not match with expected set of keys.", "variables");

			if (forumla.Evaluate(variables) >= 0)
				return text.Text(variables, placeholderContents);
			else
				return "";
		}
	}
}
