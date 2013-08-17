using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Tables
{
	class WormholeCollection : ICollection<Tuple<StarData, StarData>>, IDelayedRemoval<Tuple<StarData, StarData>>
	{
		HashSet<Tuple<StarData, StarData>> innerSet = new HashSet<Tuple<StarData, StarData>>();
		List<Tuple<StarData, StarData>> toRemove = new List<Tuple<StarData, StarData>>();

		Dictionary<StarData, List<Tuple<StarData, StarData>>> EndpointsIndex = new Dictionary<StarData, List<Tuple<StarData, StarData>>>();

		public IEnumerable<Tuple<StarData, StarData>> Endpoints(StarData key) {
			if (EndpointsIndex.ContainsKey(key))
				foreach (var item in EndpointsIndex[key])
					yield return item;
		}
	
		public void Add(Tuple<StarData, StarData> item)
		{
			innerSet.Add(item); 

			if (!EndpointsIndex.ContainsKey(item.Item1))
				EndpointsIndex.Add(item.Item1, new List<Tuple<StarData, StarData>>());
			EndpointsIndex[item.Item1].Add(item);

			if (!EndpointsIndex.ContainsKey(item.Item2))
				EndpointsIndex.Add(item.Item2, new List<Tuple<StarData, StarData>>());
			EndpointsIndex[item.Item2].Add(item);
		}

		public void Add(IEnumerable<Tuple<StarData, StarData>> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			EndpointsIndex.Clear();
		}

		public bool Contains(Tuple<StarData, StarData> item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(Tuple<StarData, StarData>[] array, int arrayIndex)
		{
			innerSet.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return innerSet.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(Tuple<StarData, StarData> item)
		{
			if (innerSet.Remove(item)) {
				EndpointsIndex[item.Item1].Remove(item);
				EndpointsIndex[item.Item2].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<Tuple<StarData, StarData>> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(Tuple<StarData, StarData> element)
		{
			toRemove.Add(element);
		}

		public void ApplyRemove()
		{
			foreach (var element in toRemove)
				this.Remove(element);
			toRemove.Clear();
		}
	}
}
