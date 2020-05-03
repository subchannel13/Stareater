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

		internal LoadSession(Func<Type, ITypeStrategy> expertGetter, ObjectDeindexer deindexer)
		{
			this.expertGetter = expertGetter;
			this.Deindexer = deindexer;
		}

		//TODO(v0.8) try to refactor to make it private
		public ObjectDeindexer Deindexer { get; private set; }

		public T Load<T>(Ikadn.IkadnBaseObject data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			return Load<T>(data.To<IkonBaseObject>());
		}

        public T Load<T>(IkonBaseObject data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			if (deserialized.ContainsKey(data))
				return (T)deserialized[data];

			if (this.Deindexer.HasType(typeof(T)))
				return this.Deindexer.Get<T>(data.To<string>());

            var expert = this.expertGetter(typeof(T));
			var result = expert.Deserialize(data, this);

			this.deserialized[data] = result;

			return (T)result;
		}
	}
}