using System;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
	class TerminalStrategy : ITypeStrategy
	{
		#region ITypeStrategy implementation
		public object Copy(object originalValue, CopySession session)
		{
			return originalValue;
		}
		
		public IEnumerable<Type> Dependencies
		{
			get { yield break; }
		}
		#endregion
	}
}
