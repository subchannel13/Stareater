using System;
using Ikadn.Ikon.Types;

namespace Stareater.Utils.StateEngine
{
	class TerminalStrategy : ITypeStrategy
	{
		private Func<object, IkonBaseObject> serializationMethod;

		public TerminalStrategy(Func<object, IkonBaseObject> serializationMethod)
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

		public IkonBaseObject Serialize(object originalValue, SaveSession session)
		{
			return this.serializationMethod(originalValue);
		}
		#endregion
	}
}
