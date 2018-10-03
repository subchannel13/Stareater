using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	class ScalarIndex<TElement, TKey> : IIndex<TElement>
	{
		private readonly Func<TElement, TKey> keySelector;
		private readonly Dictionary<TKey, TElement> elements = new Dictionary<TKey, TElement>();
		
		public ScalarIndex(Func<TElement, TKey> keySelector)
		{
			this.keySelector = keySelector;
		}
		
		public bool Contains(TKey key)
		{
			return elements.ContainsKey(key);
		}
		
		public TElement this[TKey key]
		{
			get
			{
				return this.elements[key];
			}
		}
		
		#region IIndex implementation
		public void Add(TElement item)
		{
			this.elements[keySelector(item)] = item;
		}
		public void Remove(TElement item)
		{
			this.elements.Remove(keySelector(item));
		}
		public void Clear()
		{
			this.elements.Clear();
		}
		#endregion
	}
}
