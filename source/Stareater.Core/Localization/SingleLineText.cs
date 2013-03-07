using Ikadn;
using System;

namespace Stareater.Localization
{
	class SingleLineText : IkadnBaseValue, IText
	{
		string text;

		internal SingleLineText(string line)
		{
			this.text = line;
		}

		protected override void DoCompose(IkadnWriter writer)
		{
			throw new InvalidOperationException(TypeName + " is not meant to be serialized.");
		}

		public override string TypeName
		{
			get { return "SingleLineText"; }
		}

		public override T To<T>()
		{
			Type target = typeof(T);

			if (target.IsAssignableFrom(this.GetType()))
				return (T)(object)this;
			else
				throw new InvalidOperationException("Cast to " + target.Name + " is not supported for " + TypeName);
		}


		public System.Collections.Generic.IEnumerable<string> VariableNames()
		{
			return new string[] { };
		}

		public string Text()
		{
			return text;
		}

		public string Text(double trivialVariable)
		{
			throw new InvalidOperationException("This IText has no variables");
		}

		public string Text(System.Collections.Generic.IDictionary<string, double> variables)
		{
			throw new InvalidOperationException("This IText has no variables");
		}
	}
}
