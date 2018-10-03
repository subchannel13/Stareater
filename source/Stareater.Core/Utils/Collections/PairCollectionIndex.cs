using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
	class PairCollectionIndex<TElement, TKey> : IIndex<TElement>
	{
		private readonly Func<TElement, Pair<TKey>> keySelector;
		private readonly Dictionary<Pair<TKey>, List<TElement>> pairElements = new Dictionary<Pair<TKey>, List<TElement>>();
		private readonly Dictionary<TKey, List<TElement>> flatElements = new Dictionary<TKey, List<TElement>>();

		public PairCollectionIndex(Func<TElement, Pair<TKey>> keySelector)
		{
			this.keySelector = keySelector;
		}

		public IList<TElement> this[TKey key]
		{
			get
			{
				return this.flatElements.ContainsKey(key) ? this.flatElements[key] : new List<TElement>();
			}
		}

		public IList<TElement> this[Pair<TKey> key]
		{
			get
			{
				return this.pairElements.ContainsKey(key) ? this.pairElements[key] : new List<TElement>();
			}
		}

		public IList<TElement> this[TKey keyFirst, TKey keySecond]
		{
			get
			{
				var keyPair = new Pair<TKey>(keyFirst, keySecond);

				return this.pairElements.ContainsKey(keyPair) ? this.pairElements[keyPair] : new List<TElement>();
			}
		}

		#region IIndex implementation
		public void Add(TElement item)
		{
			var key = keySelector(item);

			if (!this.pairElements.ContainsKey(key))
				this.pairElements.Add(key, new List<TElement>());
			this.pairElements[key].Add(item);

			if (!this.flatElements.ContainsKey(key.First))
				this.flatElements.Add(key.First, new List<TElement>());
			this.flatElements[key.First].Add(item);

			if (!this.flatElements.ContainsKey(key.Second))
				this.flatElements.Add(key.Second, new List<TElement>());
			this.flatElements[key.Second].Add(item);
		}
		public void Remove(TElement item)
		{
			var key = keySelector(item);

			this.pairElements[key].Remove(item);
			this.flatElements[key.First].Remove(item);
			this.flatElements[key.Second].Remove(item);
		}
		public void Clear()
		{
			this.pairElements.Clear();
			this.flatElements.Clear();
		}
		#endregion
	}
}
