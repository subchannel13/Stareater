using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	public class TextVar
	{
		private readonly Dictionary<string, string> variables = new Dictionary<string, string>();

		public TextVar()
		{ }

		public TextVar(string name, string value)
		{
			variables.Add(name, value);
		}

		public TextVar And(string name, string value)
		{
			variables.Add(name, value);
			return this;
		}

		public IDictionary<string, string> Get
		{
			get {
				return this.variables;
			}
		}
		
		public TextVar UnionWith(IEnumerable<KeyValuePair<string, string>> variables)
		{
			if (variables == null)
				throw new ArgumentNullException(nameof(variables));

			foreach (var pair in variables)
				this.variables.Add(pair.Key, pair.Value);
			
			return this;
		}
	}
}
