using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Alati.Strukture
{
	public class MyDictionary<KeyType, ValueType> : Dictionary<KeyType, ValueType>, IDelayedRemoval<KeyType>
	{
		List<KeyType> toRemove = null;

		/// <summary>
		/// Mark element for removal.
		/// </summary>
		/// <param name="key">Key of element</param>
		public void PendRemove(KeyType key)
		{
			if (toRemove == null) toRemove = new List<KeyType>();
			toRemove.Add(key);
		}

		/// <summary>
		/// Remove all elements marked with PendRemove method.
		/// </summary>
		public void ApplyRemove()
		{
			if (toRemove != null && toRemove.Count > 0) {
				foreach (var key in toRemove)
					this.Remove(key);
				toRemove.Clear();
			}
		}
	}
}
