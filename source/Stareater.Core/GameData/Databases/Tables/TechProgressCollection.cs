using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class TechProgressCollection : ICollection<TechnologyProgress>, IDelayedRemoval<TechnologyProgress>
	{
		HashSet<TechnologyProgress> innerSet = new HashSet<TechnologyProgress>();
		List<TechnologyProgress> toRemove = new List<TechnologyProgress>();

		Dictionary<Player, List<TechnologyProgress>> OfIndex = new Dictionary<Player, List<TechnologyProgress>>();

		public IEnumerable<TechnologyProgress> Of(Player key) {
			if (OfIndex.ContainsKey(key))
				foreach (var item in OfIndex[key])
					yield return item;
		}
	
		public void Add(TechnologyProgress item)
		{
			innerSet.Add(item); 

			if (!OfIndex.ContainsKey(item.Owner))
				OfIndex.Add(item.Owner, new List<TechnologyProgress>());
			OfIndex[item.Owner].Add(item);
		}

		public void Add(IEnumerable<TechnologyProgress> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OfIndex.Clear();
		}

		public bool Contains(TechnologyProgress item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(TechnologyProgress[] array, int arrayIndex)
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

		public bool Remove(TechnologyProgress item)
		{
			if (innerSet.Remove(item)) {
				OfIndex[item.Owner].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<TechnologyProgress> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(TechnologyProgress element)
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
