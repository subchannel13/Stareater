using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Databases.Tables
{
	class TechnologyCollection : ICollection<Technology>, IDelayedRemoval<Technology>
	{
		HashSet<Technology> innerSet = new HashSet<Technology>();
		List<Technology> toRemove = new List<Technology>();

	
		public void Add(Technology item)
		{
			innerSet.Add(item); 
		}

		public void Add(IEnumerable<Technology> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

		}

		public bool Contains(Technology item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(Technology[] array, int arrayIndex)
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

		public bool Remove(Technology item)
		{
			if (innerSet.Remove(item)) {
			
				return true;
			}

			return false;
		}

		public IEnumerator<Technology> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(Technology element)
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
