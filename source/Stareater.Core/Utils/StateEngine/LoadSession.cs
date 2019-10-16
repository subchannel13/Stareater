using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
	public class LoadSession
	{
		private readonly Dictionary<IkonBaseObject, object> deserialized = new Dictionary<IkonBaseObject, object>();

		private readonly Func<Type, ITypeStrategy> expertGetter;
		private readonly Dictionary<Type, Action<object>> postLoadActions;

		//TODO(v0.8) check if post load actions are still needed after removing hash from Design
		internal LoadSession(Func<Type, ITypeStrategy> expertGetter, ObjectDeindexer deindexer, Dictionary<Type, Action<object>> postLoadActions)
		{
			this.expertGetter = expertGetter;
			this.postLoadActions = postLoadActions;
			this.Deindexer = deindexer;
		}

		//TODO(v0.8) try to refactor to make it private
		public ObjectDeindexer Deindexer { get; private set; }

		public T Load<T>(Ikadn.IkadnBaseObject data)
		{
			return Load<T>(data.To<IkonBaseObject>());
		}

        public T Load<T>(IkonBaseObject data)
		{
			if (deserialized.ContainsKey(data))
				return (T)deserialized[data];

			if (this.Deindexer.HasType(typeof(T)))
				return this.Deindexer.Get<T>(data.To<string>());

            var expert = this.expertGetter(typeof(T));
			var result = expert.Deserialize(data, this);

			if (postLoadActions.ContainsKey(typeof(T)))
				postLoadActions[typeof(T)](result);
			this.deserialized[data] = result;

			return (T)result;
		}
	}
}