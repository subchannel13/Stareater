using System;
using System.Collections.Generic;
using Ikadn;

namespace Stareater.Utils.StateEngine
{
	class TerminalStrategy : ITypeStrategy
	{
		private Func<object, IkadnBaseObject> serializationMethod;

		public TerminalStrategy(Func<object, IkadnBaseObject> serializationMethod)
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

		public IEnumerable<KeyValuePair<object, IkadnBaseObject>> Serialize(object originalValue, SaveSession session)
		{
			yield return new KeyValuePair<object, IkadnBaseObject>(originalValue, this.serializationMethod(originalValue));
		}
		#endregion
	}
}
