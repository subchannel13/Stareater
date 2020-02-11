using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	public class Var
	{
		private readonly Dictionary<string, double> variables = new Dictionary<string, double>();

		public Var()
		{ }

		public Var(string name, double value)
		{
			variables.Add(name, value);
		}

		public Var And(string name, double value)
		{
			variables.Add(name, value);
			return this;
		}

		public Var And(string name, bool value)
		{
			variables.Add(name, convert(value));
			return this;
		}

		public Var Set(string name, double value)
		{
			variables[name] = value;
			return this;
		}

		public Var Set(string name, bool value)
		{
			variables[name] = convert(value);
			return this;
		}

		public IDictionary<string, double> Get
		{
			get {
				return this.variables;
			}
		}
		
		public Var Init(IEnumerable<string> variableNames, bool initValue)
		{
			if (variableNames == null)
				throw new ArgumentNullException(nameof(variableNames));

			foreach(var name in variableNames)
				this.variables.Add(name, convert(initValue));
			
			return this;
		}
		
		public Var Init(IEnumerable<string> variableNames, double initValue)
		{
			if (variableNames == null)
				throw new ArgumentNullException(nameof(variableNames));

			foreach (var name in variableNames)
				this.variables.Add(name, initValue);
			
			return this;
		}
		
		public Var UnionWith(IEnumerable<KeyValuePair<string, double>> variables)
		{
			if (variables == null)
				throw new ArgumentNullException(nameof(variables));

			foreach (var pair in variables)
				this.variables[pair.Key] = pair.Value;
			
			return this;
		}

		public Var UnionWith(IEnumerable<string> variables)
		{
			if (variables == null)
				throw new ArgumentNullException(nameof(variables));

			foreach (var name in variables)
				this.variables[name] = 1;

			return this;
		}

		public Var UnionWith<T>(IEnumerable<T> collection, Func<T, string> keySelector, Func<T, double> valueSelector)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));

			foreach (var element in collection)
				this.variables[keySelector(element)] = valueSelector(element);
			
			return this;
		}
		
		public double this[string key]
		{
			get { return this.variables[key]; }
			set { this.variables[key] = value; }
		}

		private static double convert(bool value)
		{
			return value ? 1 : -1;
		}
	}
}
