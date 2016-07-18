using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class StellarisCollection : ICollection<StellarisAdmin>, IDelayedCollection<StellarisAdmin>
	{
		private HashSet<StellarisAdmin> innerSet = new HashSet<StellarisAdmin>();
		private readonly List<StellarisAdmin> toAdd = new List<StellarisAdmin>();
		private readonly List<StellarisAdmin> toRemove = new List<StellarisAdmin>();

		Dictionary<Player, List<StellarisAdmin>> OwnedByIndex = new Dictionary<Player, List<StellarisAdmin>>();
		Dictionary<StarData, List<StellarisAdmin>> AtIndex = new Dictionary<StarData, List<StellarisAdmin>>();

		public IList<StellarisAdmin> OwnedBy(Player key) 
		{
			return (OwnedByIndex.ContainsKey(key)) ? 
				OwnedByIndex[key] : 
				new List<StellarisAdmin>();
		}

		public IList<StellarisAdmin> At(StarData key) 
		{
			return (AtIndex.ContainsKey(key)) ? 
				AtIndex[key] : 
				new List<StellarisAdmin>();
		}
	
		public void Add(StellarisAdmin item)
		{
			innerSet.Add(item); 

			if (!OwnedByIndex.ContainsKey(item.Owner))
				OwnedByIndex.Add(item.Owner, new List<StellarisAdmin>());
			OwnedByIndex[item.Owner].Add(item);

			if (!AtIndex.ContainsKey(item.Location.Star))
				AtIndex.Add(item.Location.Star, new List<StellarisAdmin>());
			AtIndex[item.Location.Star].Add(item);
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
				AtIndex[item.Location.Star].Remove(item);
			
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

		public void PendAdd(StellarisAdmin element)
		{
			toAdd.Add(element);
		}
		
		public void PendRemove(StellarisAdmin element)
		{
			toRemove.Add(element);
		}
		
		public void ApplyPending()
		{
			foreach (var element in toRemove)
				this.Remove(element);
			toRemove.Clear();
			
			foreach (var element in toAdd)
				this.Add(element);
			toAdd.Clear();
		}
	}
}
