using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class StellarisCollection : ICollection<StellarisAdmin>, IDelayedRemoval<StellarisAdmin>
	{
		HashSet<StellarisAdmin> innerSet = new HashSet<StellarisAdmin>();
		List<StellarisAdmin> toRemove = new List<StellarisAdmin>();

		Dictionary<Player, List<StellarisAdmin>> OwnedByIndex = new Dictionary<Player, List<StellarisAdmin>>();
		Dictionary<StarData, StellarisAdmin> AtIndex = new Dictionary<StarData, StellarisAdmin>();

		public IList<StellarisAdmin> OwnedBy(Player key) {
			if (OwnedByIndex.ContainsKey(key))
				return OwnedByIndex[key];
			
			return new List<StellarisAdmin>();
		}

		public StellarisAdmin At(StarData key) {
			if (AtIndex.ContainsKey(key))
				return AtIndex[key];
				
			throw new KeyNotFoundException();
		}
		
		public bool AtContains(StarData key) {
			return AtIndex.ContainsKey(key);
		}
	
		public void Add(StellarisAdmin item)
		{
			innerSet.Add(item); 

			if (!OwnedByIndex.ContainsKey(item.Owner))
				OwnedByIndex.Add(item.Owner, new List<StellarisAdmin>());
			OwnedByIndex[item.Owner].Add(item);
			if (!AtIndex.ContainsKey(item.Location))
				AtIndex.Add(item.Location, item);
		}

		public void Add(IEnumerable<StellarisAdmin> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OwnedByIndex.Clear();
			AtIndex.Clear();
		}

		public bool Contains(StellarisAdmin item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(StellarisAdmin[] array, int arrayIndex)
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

		public bool Remove(StellarisAdmin item)
		{
			if (innerSet.Remove(item)) {
				OwnedByIndex[item.Owner].Remove(item);
				AtIndex.Remove(item.Location);
			
				return true;
			}

			return false;
		}

		public IEnumerator<StellarisAdmin> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(StellarisAdmin element)
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
