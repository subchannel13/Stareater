using Ikadn;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
	public class LoadSession
	{
		private readonly Dictionary<IkadnBaseObject, object> deserialized = new Dictionary<IkadnBaseObject, object>();

		private Func<Type, ITypeStrategy> expertGetter;

		internal LoadSession(Func<Type, ITypeStrategy> expertGetter, ObjectDeindexer deindexer)
		{
			this.expertGetter = expertGetter;
			this.Deindexer = deindexer;
		}

		public ObjectDeindexer Deindexer { get; private set; }

		internal T Load<T>(IkadnBaseObject data)
		{
			if (deserialized.ContainsKey(data))
				return (T)deserialized[data];

			if (this.Deindexer.HasType(typeof(T)))
				return this.Deindexer.Get<T>(data.To<int>());

            var expert = this.expertGetter(typeof(T));
			return (T)expert.Deserialize(data, this);
		}

	}
}