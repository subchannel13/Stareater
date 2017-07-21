using Ikadn;
using System;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
	interface ITypeStrategy
	{
        object Create(object originalValue);
        void FillCopy(object originalValue, object copyInstance, CopySession session);
		IEnumerable<KeyValuePair<object, IkadnBaseObject>> Serialize(object originalValue, SaveSession session);
	}
}
