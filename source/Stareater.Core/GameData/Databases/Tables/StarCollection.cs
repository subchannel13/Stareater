using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.GameData.Databases.Tables
{
	class StarCollection : ICollection<StarData>, IDelayedRemoval<StarData>
	{
		HashSet<StarData> innerSet = new HashSet<StarData>();
		List<StarData> toRemove = new List<StarData>();

	
		public void Add(StarData item)
		{
			innerSet.Add(item); 
		}

		public void Add(IEnumerable<StarData> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

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
