using System;
using System.Collections.Generic;

namespace Stareater.Utils.Collections
{
    public class PairScalarIndex<TElement, TKey> : IIndex<TElement>
    {
        private readonly Func<TElement, Pair<TKey>> keySelector;
        private readonly Dictionary<Pair<TKey>, TElement> pairElements = new Dictionary<Pair<TKey>, TElement>();
        private readonly Dictionary<TKey, List<TElement>> flatElements = new Dictionary<TKey, List<TElement>>();

        public PairScalarIndex(Func<TElement, Pair<TKey>> keySelector)
        {
            this.keySelector = keySelector;
        }

        public bool Contains(Pair<TKey> key)
        {
            return this.pairElements.ContainsKey(key);
        }

        public bool Contains(TKey keyFirst, TKey keySecond)
        {
            return this.pairElements.ContainsKey(new Pair<TKey>(keyFirst, keySecond));
        }

        public TElement GetOrDefault(Pair<TKey> key)
        {
            return this.Contains(key) ? this[key] : default;
        }

        public TElement GetOrDefault(TKey keyFirst, TKey keySecond)
        {
            var key = new Pair<TKey>(keyFirst, keySecond);

            return this.Contains(key) ? this[key] : default;
        }

        public IList<TElement> this[TKey key]
        {
            get
            {
                return this.flatElements.ContainsKey(key) ? this.flatElements[key] : new List<TElement>();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1043:Use Integral Or String Argument For Indexers", Justification = "Class represents data store")]
        public TElement this[Pair<TKey> key]
        {
            get
            {
                return this.pairElements[key];
            }
        }

        public TElement this[TKey keyFirst, TKey keySecond]
        {
            get
            {
                return this.pairElements[new Pair<TKey>(keyFirst, keySecond)];
            }
        }

        #region IIndex implementation
        public void Add(TElement item)
        {
            var key = keySelector(item);

            this.pairElements[key] = item;

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
            this.pairElements.Remove(key);
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
