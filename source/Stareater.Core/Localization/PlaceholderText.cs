using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Localization
{
	class PlaceholderText : IText
	{
		private string placeholderKey;

		public PlaceholderText(string placeholderKey)
		{
			this.placeholderKey = placeholderKey;
		}

		public string Text()
		{
			throw new InvalidOperationException("This IText has a text placeholder, call an overload that supplies content for it.");
		}

		public string Text(double trivialVariable)
		{
			throw new InvalidOperationException("This IText has a text placeholder, call an overload that supplies content for it.");
		}

		public string Text(IDictionary<string, double> variables)
		{
			throw new InvalidOperationException("This IText has a text placeholder, call an overload that supplies content for it.");
		}

		public string Text(IDictionary<string, double> variables, IDictionary<string, string> placeholderContents)
		{
			return placeholderContents[placeholderKey];
		}

		public IEnumerable<string> VariableNames()
		{
			yield break;
		}
	}
}
