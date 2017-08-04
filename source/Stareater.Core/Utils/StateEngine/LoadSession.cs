using Ikadn;
using System;

namespace Stareater.Utils.StateEngine
{
	public class LoadSession
	{
		private Func<Type, ITypeStrategy> expertGetter;

		internal LoadSession(Func<Type, ITypeStrategy> expertGetter)
		{
			this.expertGetter = expertGetter;
		}

		internal object Load(Type type, IkadnBaseObject data)
		{
			var expert = this.expertGetter(type);
			return expert.Deserialize(data, this);
		}
	}
}