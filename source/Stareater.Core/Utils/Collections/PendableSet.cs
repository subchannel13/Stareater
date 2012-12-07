using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	public class PendableSet<T> : HashSet<T>, IDelayedRemoval<T>
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
