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
				return (T)postprocess(typeof(T), deserialized[data]);

			if (this.Deindexer.HasType(typeof(T)))
				return (T)postprocess(typeof(T), this.Deindexer.Get<T>(data.To<int>()));

            var expert = this.expertGetter(typeof(T));
			return (T)postprocess(typeof(T), expert.Deserialize(data, this));
		}

		private object postprocess(Type type, object obj)
		{
			if (postLoadActions.ContainsKey(type))
				postLoadActions[type](obj);

			return obj;
		}
	}
}