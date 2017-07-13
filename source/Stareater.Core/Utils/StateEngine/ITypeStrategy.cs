using System;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
	interface ITypeStrategy
	{
		object Copy(object originalValue, CopySession session);
		IEnumerable<Type> Dependencies { get; }
	}
}
