using System;
using System.Collections.Generic;
using Ikadn.Ikon.Types;

namespace Stareater.Utils.StateEngine
{
	class TerminalStrategy : ITypeStrategy
	{
		private Func<object, SaveSession, IkonBaseObject> serializationMethod;

		public TerminalStrategy(Func<object, SaveSession, IkonBaseObject> serializationMethod)
		{
			this.serializationMethod = serializationMethod;
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
		#endregion
	}
}
