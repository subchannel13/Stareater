using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	class Collection2Index<TElement, TKey1, TKey2> : IIndex<TElement>
	{
		private readonly Func<TElement, Tuple<TKey1, TKey2>> keySelector;
		private readonly Dictionary<Tuple<TKey1, TKey2>, List<TElement>> elements = new Dictionary<Tuple<TKey1, TKey2>, List<TElement>>();
		private readonly Dictionary<TKey1, List<TElement>> primaryKeys = new Dictionary<TKey1, List<TElement>>();

		public Collection2Index(Func<TElement, Tuple<TKey1, TKey2>> keySelector)
		{
			this.keySelector = keySelector;
		}

		public IList<TElement> this[TKey1 key1, TKey2 key2]
		{
			get
			{
				var key = new Tuple<TKey1, TKey2>(key1, key2);
				return this.elements.ContainsKey(key) ? this.elements[key] : new List<TElement>();
			}
		}

		public IList<TElement> this[TKey1 key1]
		{
			get
			{
				return this.primaryKeys.ContainsKey(key1) ? this.primaryKeys[key1] : new List<TElement>();
			}
		}

		#region IIndex implementation
		public void Add(TElement item)
		{
			var key = keySelector(item);
			if (!this.elements.ContainsKey(key))
				this.elements.Add(key, new List<TElement>());

			if (!this.primaryKeys.ContainsKey(key.Item1))
				this.primaryKeys[key.Item1] = new List<TElement>();

			this.elements[key].Add(item);
			this.primaryKeys[key.Item1].Add(item);
		}
		public void Remove(TElement item)
		{
			var key = keySelector(item);

			this.elements[key].Remove(item);
			this.primaryKeys[key.Item1].Remove(item);

			if (this.elements[key].Count == 0)
			{
				this.elements.Remove(key);

				if (this.primaryKeys[key.Item1].Count == 0)
					this.primaryKeys.Remove(key.Item1);
			}
		}
		public void Clear()
		{
			this.elements.Clear();
			this.primaryKeys.Clear();
		}
		#endregion
	}
}
