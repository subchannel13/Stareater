using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Utils.Collections;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.GameData.Tables
{
	class StarsCollection : ICollection<StarData>, IDelayedRemoval<StarData>
	{
		HashSet<StarData> innerSet = new HashSet<StarData>();
		List<StarData> toRemove = new List<StarData>();

		Dictionary<Vector2D, List<StarData>> PositionsIndex = new Dictionary<Vector2D, List<StarData>>();

		public IEnumerable<StarData> Positions(Vector2D key) {
			if (PositionsIndex.ContainsKey(key))
				foreach (var item in PositionsIndex[key])
					yield return item;
		}
	
		public void Add(StarData item)
		{
			innerSet.Add(item); 

			if (!PositionsIndex.ContainsKey(item.Position))
				PositionsIndex.Add(item.Position, new List<StarData>());
			PositionsIndex[item.Position].Add(item);
		}

		public void Add(IEnumerable<StarData> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			PositionsIndex.Clear();
		}

		public bool Contains(StarData item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(StarData[] array, int arrayIndex)
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

		public bool Remove(StarData item)
		{
			if (innerSet.Remove(item)) {
				PositionsIndex[item.Position].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<StarData> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(StarData element)
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
