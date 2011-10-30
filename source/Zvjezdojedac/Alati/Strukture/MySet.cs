using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Alati.Strukture
{
	public class MySet<T> : HashSet<T>, IDelayedRemoval<T>
	{
		List<T> toRemove = null;

		public void PendRemove(T element)
		{
			if (toRemove == null) toRemove = new List<T>();
			toRemove.Add(element);
		}

		public void ApplyRemove()
		{
			if (toRemove != null && toRemove.Count > 0) {
				foreach (var element in toRemove)
					this.Remove(element);
				toRemove.Clear();
			}
		}
	}
}
