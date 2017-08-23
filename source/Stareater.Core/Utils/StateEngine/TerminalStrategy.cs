using System;
using System.Collections.Generic;
using Ikadn.Ikon.Types;

namespace Stareater.Utils.StateEngine
{
	class TerminalStrategy : ITypeStrategy
	{
		private Func<object, SaveSession, IkonBaseObject> serializationMethod;
		private Func<IkonBaseObject, LoadSession, object> deserializationMethod;

		public TerminalStrategy(Func<object, SaveSession, IkonBaseObject> serializationMethod, Func<IkonBaseObject, LoadSession, object> deserializationMethod)
		{
			this.serializationMethod = serializationMethod;
			this.deserializationMethod = deserializationMethod;
		}

        #region ITypeStrategy implementation
        public void FillCopy(object originalValue, object copyInstance, CopySession session)
        {
            //No operation
        }

        public object Create(object originalValue)
        {
            return originalValue;
        }

		public IEnumerable<object> Dependencies(object originalValue)
		{
			yield break;
		}

		public IkonBaseObject Serialize(object originalValue, SaveSession session)
		{
			return this.serializationMethod(originalValue, session);
		}

		public object Deserialize(IkonBaseObject data, LoadSession session)
		{
			return this.deserializationMethod(data, session);
		}
		#endregion
	}
}
