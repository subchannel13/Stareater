using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stareater.Utils.StateEngine
{
	class SaveSession
	{
		private readonly Func<Type, ITypeStrategy> expertGetter;
		private readonly Dictionary<object, IkonBaseObject> savedData = new Dictionary<object, IkonBaseObject>();
		private readonly Dictionary<Type, int> nextReference = new Dictionary<Type, int>();

		public SaveSession(Func<Type, ITypeStrategy> expertGetter)
		{
			this.expertGetter = expertGetter;
		}

		internal IkonBaseObject Serialize(object originalValue)
		{
			if (originalValue == null)
				return null; //TODO(v0.7) how to serialize null?

			if (this.savedData.ContainsKey(originalValue))
				return new IkonReference(this.savedData[originalValue].ReferenceNames.First());

			var data = this.expertGetter(originalValue.GetType()).Serialize(originalValue, this);
			if (!this.savedData.ContainsKey(originalValue))
				this.SaveReference(originalValue, data);

			return data;
		}

		internal IEnumerable<IkonBaseObject> GetSerialzedData()
		{
			return this.savedData.Values;
		}

		internal IkonReference SaveReference(object originalValue, IkonBaseObject serializedValue)
		{
			if (!this.nextReference.ContainsKey(originalValue.GetType()))
				this.nextReference[originalValue.GetType()] = 0;

			var referenceName = originalValue.GetType().Name + this.nextReference[originalValue.GetType()].ToString();
			this.savedData[originalValue] = serializedValue;
			serializedValue.ReferenceNames.Add(referenceName);

			return new IkonReference(referenceName);
		}
	}
}
