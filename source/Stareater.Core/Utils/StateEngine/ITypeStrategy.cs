using System;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
	interface ITypeStrategy
	{
        object Create(object originalValue);
        void FillCopy(object originalValue, object copyInstance, CopySession session);
    }
}
