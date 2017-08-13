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
		private Dictionary<Type, Action<object>> postLoadActions;

		internal LoadSession(Func<Type, ITypeStrategy> expertGetter, ObjectDeindexer deindexer, Dictionary<Type, Action<object>> postLoadActions)
		{
			this.expertGetter = expertGetter;
			this.postLoadActions = postLoadActions;
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
			var result = expert.Deserialize(data, this);

			if (postLoadActions.ContainsKey(typeof(T)))
				postLoadActions[typeof(T)](result);
			this.deserialized[data] = result;

			return (T)result;
		}
	}
}