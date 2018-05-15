using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	class CollectionIndex<TElement, TKey> : IIndex<TElement>
	{
		private Func<TElement, TKey> keySelector;
		private Dictionary<TKey, List<TElement>> elements = new Dictionary<TKey, List<TElement>>();
		
		public CollectionIndex(Func<TElement, TKey> keySelector)
		{
			this.keySelector = keySelector;
		}
		
		public IList<TElement> this[TKey key]
		{
			get
			{
				return this.elements.ContainsKey(key) ? this.elements[key] : new List<TElement>();
			}
		}
		
		#region IIndex implementation
		public void Add(TElement item)
		{
			var key = keySelector(item);
			if (!this.elements.ContainsKey(key))
				this.elements.Add(key, new List<TElement>());
				
			this.elements[key].Add(item);
		}
		public void Remove(TElement item)
		{
			this.elements[keySelector(item)].Remove(item);
		}
		public void Clear()
		{
			this.elements.Clear();
		}
		#endregion
	}
}
