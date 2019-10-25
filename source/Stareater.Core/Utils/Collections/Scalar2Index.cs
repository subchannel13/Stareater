using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	class Scalar2Index<TElement, TKey1, TKey2> : IIndex<TElement>
	{
		private readonly Func<TElement, Tuple<TKey1, TKey2>> keySelector;
		private readonly Dictionary<Tuple<TKey1, TKey2>, TElement> elements = new Dictionary<Tuple<TKey1, TKey2>, TElement>();
		private readonly Dictionary<TKey1, List<TElement>> primaryKeys = new Dictionary<TKey1, List<TElement>>();

		public Scalar2Index(Func<TElement, Tuple<TKey1, TKey2>> keySelector)
		{
			this.keySelector = keySelector;
		}

		public bool Contains(TKey1 key1, TKey2 key2)
		{
			return this.elements.ContainsKey(new Tuple<TKey1, TKey2>(key1, key2));
		}

		public IList<TElement> this[TKey1 key1]
		{
			get
			{
				return this.primaryKeys.ContainsKey(key1) ? this.primaryKeys[key1] : new List<TElement>();
			}
		}

		public TElement this[TKey1 key1, TKey2 key2]
		{
			get
			{
				return this.elements[new Tuple<TKey1, TKey2>(key1, key2)];
			}
		}

		#region IIndex implementation
		public void Add(TElement item)
		{
			var key = keySelector(item);
			if (!this.primaryKeys.ContainsKey(key.Item1))
				this.primaryKeys[key.Item1] = new List<TElement>();

			this.elements[key] = item;
			this.primaryKeys[key.Item1].Add(item);
		}

		public void Remove(TElement item)
		{
			var key = keySelector(item);

			this.elements.Remove(key);
			this.primaryKeys[key.Item1].Remove(item);

			if (this.primaryKeys[key.Item1].Count == 0)
				this.primaryKeys.Remove(key.Item1);
		}
		public void Clear()
		{
			this.elements.Clear();
			this.primaryKeys.Clear();
		}
		#endregion
	}
}
