using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Ships;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class DesignCollection : ICollection<Design>, IDelayedRemoval<Design>
	{
		HashSet<Design> innerSet = new HashSet<Design>();
		List<Design> toRemove = new List<Design>();

		Dictionary<Player, List<Design>> OwnedByIndex = new Dictionary<Player, List<Design>>();

		public IList<Design> OwnedBy(Player key) {
			if (OwnedByIndex.ContainsKey(key))
				return OwnedByIndex[key];
			
			return new List<Design>();
		}
	
		public void Add(Design item)
		{
			innerSet.Add(item); 

			if (!OwnedByIndex.ContainsKey(item.Owner))
				OwnedByIndex.Add(item.Owner, new List<Design>());
			OwnedByIndex[item.Owner].Add(item);
		}

		public void Add(IEnumerable<Design> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OwnedByIndex.Clear();
		}

		public bool Contains(Design item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(Design[] array, int arrayIndex)
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

		public bool Remove(Design item)
		{
			if (innerSet.Remove(item)) {
				OwnedByIndex[item.Owner].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<Design> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(Design element)
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
