using System;
using System.Collections.Generic;

namespace Stareater.Utils.StateEngine
{
	class CopySession
	{
		private readonly Dictionary<Type, ITypeStrategy> experts;
		private readonly Dictionary<object, object> copies = new Dictionary<object, object>();
		
		public CopySession(Dictionary<Type, ITypeStrategy> experts)
		{
			this.experts = experts;
		}
		
		internal object CopyOf(object originalValue)
		{
			return (originalValue == null) ? null :
				this.copies.ContainsKey(originalValue) ? 
					this.copies[originalValue] :
					this.experts[originalValue.GetType()].Copy(originalValue, this);
			
		}
	}
}
