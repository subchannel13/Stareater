using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases.Tables
{
	class WormholeCollection : ICollection<Wormhole>, IDelayedCollection<Wormhole>
	{
		private HashSet<Wormhole> innerSet = new HashSet<Wormhole>();
		private readonly List<Wormhole> toAdd = new List<Wormhole>();
		private readonly List<Wormhole> toRemove = new List<Wormhole>();

		Dictionary<StarData, List<Wormhole>> AtIndex = new Dictionary<StarData, List<Wormhole>>();

		public IList<Wormhole> At(StarData key) 
		{
			return (AtIndex.ContainsKey(key)) ? 
				AtIndex[key] : 
				new List<Wormhole>();
		}
	
		public void Add(Wormhole item)
		{
			innerSet.Add(item); 

			if (!AtIndex.ContainsKey(item.FromStar))
				AtIndex.Add(item.FromStar, new List<Wormhole>());
			AtIndex[item.FromStar].Add(item);

			if (!AtIndex.ContainsKey(item.ToStar))
				AtIndex.Add(item.ToStar, new List<Wormhole>());
			AtIndex[item.ToStar].Add(item);
		}

		public void Add(IEnumerable<Wormhole> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			AtIndex.Clear();
		}

		public bool Contains(Wormhole item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(Wormhole[] array, int arrayIndex)
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

		public bool Remove(Wormhole item)
		{
			if (innerSet.Remove(item)) {
				AtIndex[item.FromStar].Remove(item);
				AtIndex[item.ToStar].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<Wormhole> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendAdd(Wormhole element)
		{
			toAdd.Add(element);
		}
		
		public void PendRemove(Wormhole element)
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
