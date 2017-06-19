using System.Collections.Generic;
using System;

namespace Stareater.Utils.Collections
{
	public class PendableSet<T> : HashSet<T>, IDelayedCollection<T>
	{
		List<T> toAdd = null;
		List<T> toRemove = null;
		
		public PendableSet()
		{ }
			
		public PendableSet(IEnumerable<T> elements) : base(elements)
		{ }
		
		public void PendAdd(T element)
		{
			if (toAdd == null) toAdd = new List<T>();
			toAdd.Add(element);
		}

		public void PendRemove(T element)
		{
			if (toRemove == null) toRemove = new List<T>();
			toRemove.Add(element);
		}

		public void ApplyPending()
		{
			if (toRemove != null && toRemove.Count > 0) {
				foreach (var element in toRemove)
					this.Remove(element);
				toRemove.Clear();
			}
			
			if (toAdd != null && toAdd.Count > 0) {
				foreach (var element in toAdd)
					this.Remove(element);
				toAdd.Clear();
			}
		}
	}
}
