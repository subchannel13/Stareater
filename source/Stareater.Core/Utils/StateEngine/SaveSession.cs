using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.Utils.StateEngine
{
	public class SaveSession
	{
		private readonly Func<Type, ITypeStrategy> expertGetter;

        private readonly Dictionary<object, IkonBaseObject> referencedData = new Dictionary<object, IkonBaseObject>();
		private readonly Dictionary<string, int> nextReference = new Dictionary<string, int>();

		private readonly HashSet<object> unreferencedData = new HashSet<object>();
		private readonly Dictionary<object, ICollection<object>> dependencies = new Dictionary<object, ICollection<object>>();

		public SaveSession(Func<Type, ITypeStrategy> expertGetter)
		{
			this.expertGetter = expertGetter;
		}

		public IkonBaseObject Serialize(object originalValue)
		{
			if (originalValue == null)
				throw new ArgumentNullException("Null can't be serialized");

			if (this.referencedData.ContainsKey(originalValue))
				return new IkonReference(this.referencedData[originalValue].ReferenceNames.First());

			var expert = this.expertGetter(originalValue.GetType());
			var serialized = expert.Serialize(originalValue, this);

			if (!this.referencedData.ContainsKey(originalValue))
				this.unreferencedData.Add(originalValue);

			this.dependencies[originalValue] = new HashSet<object>(
				expert.Dependencies(originalValue)
			);

			return serialized;
		}

		internal IEnumerable<IkonBaseObject> GetSerialzedData()
		{
			var dependencyNodes = new HashSet<object>(
				this.referencedData.Keys.Concat(this.unreferencedData)
			);
			var lastCount = dependencyNodes.Count;

			while(dependencyNodes.Count > 0)
			{
				foreach (var node in dependencyNodes.ToList())
					if (this.dependencies[node].All(x => !dependencyNodes.Contains(x)))
					{
						if (this.referencedData.ContainsKey(node))
							yield return this.referencedData[node];

						dependencyNodes.Remove(node);
					}

				if (lastCount == dependencyNodes.Count)
					throw new Exception("Infinite loop");
				lastCount = dependencyNodes.Count;
            }
		}

		internal IkonReference SaveReference(object originalValue, IkonBaseObject serializedValue, string namePrefix)
		{
			if (!this.nextReference.ContainsKey(namePrefix))
				this.nextReference[namePrefix] = 0;

			var referenceName = namePrefix + this.nextReference[namePrefix].ToString();
			this.referencedData[originalValue] = serializedValue;
			this.nextReference[namePrefix]++;
            serializedValue.ReferenceNames.Add(referenceName);

			return new IkonReference(referenceName);
		}
	}
}
