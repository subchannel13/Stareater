using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	class CollectionIndex<TElement, TKey> : IIndex<TElement>
	{
		private Func<TElement, TKey>[] keySelectors;
		private Dictionary<TKey, List<TElement>> elements = new Dictionary<TKey, List<TElement>>();
		
		public CollectionIndex(params Func<TElement, TKey>[] keySelectors)
		{
			this.keySelectors = keySelectors;
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
			foreach(var keySelector in this.keySelectors)
			{
				var key = keySelector(item);
				if (!this.elements.ContainsKey(key))
					this.elements.Add(key, new List<TElement>());
				
				this.elements[key].Add(item);
			}
		}
		public void Remove(TElement item)
		{
			foreach(var keySelector in this.keySelectors)
				this.elements[keySelector(item)].Remove(item);
		}
		public void Clear()
		{
			this.elements.Clear();
		}
		#endregion
	}
}
