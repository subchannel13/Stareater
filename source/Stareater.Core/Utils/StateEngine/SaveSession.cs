using Ikadn;
using System;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
	class SaveSession
	{
		private readonly Func<Type, ITypeStrategy> expertGetter;
		private readonly Dictionary<object, string> savedReferences = new Dictionary<object, string>();

		public SaveSession(Func<Type, ITypeStrategy> expertGetter)
		{
			this.expertGetter = expertGetter;
		}

		internal IEnumerable<IkadnBaseObject> Save(object originalValue)
		{
			if (originalValue == null || this.savedReferences.ContainsKey(originalValue))
				yield break;

			foreach (var data in this.expertGetter(originalValue.GetType()).Serialize(originalValue, this))
			{
				//TODO(v0.7) add reference string
				yield return data.Value;
			}
		}
	}
}
