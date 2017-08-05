using Ikadn;
using System;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
	public class LoadSession
	{
		private readonly Dictionary<IkadnBaseObject, object> deserialized = new Dictionary<IkadnBaseObject, object>();

		private Func<Type, ITypeStrategy> expertGetter;

		internal LoadSession(Func<Type, ITypeStrategy> expertGetter)
		{
			this.expertGetter = expertGetter;
		}

		internal object Load(Type type, IkadnBaseObject data)
		{
			if (deserialized.ContainsKey(data))
				return deserialized[data];

			var expert = this.expertGetter(type);
			return expert.Deserialize(data, this);
		}
	}
}