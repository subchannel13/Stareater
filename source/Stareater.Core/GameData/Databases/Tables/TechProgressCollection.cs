using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class TechProgressCollection : ICollection<DevelopmentProgress>, IDelayedCollection<DevelopmentProgress>
	{
		private HashSet<DevelopmentProgress> innerSet = new HashSet<DevelopmentProgress>();
		private readonly List<DevelopmentProgress> toAdd = new List<DevelopmentProgress>();
		private readonly List<DevelopmentProgress> toRemove = new List<DevelopmentProgress>();

		Dictionary<Player, List<DevelopmentProgress>> OfIndex = new Dictionary<Player, List<DevelopmentProgress>>();

		public IList<DevelopmentProgress> Of(Player key) 
		{
			return (OfIndex.ContainsKey(key)) ? 
				OfIndex[key] : 
				new List<DevelopmentProgress>();
		}
	
		public void Add(DevelopmentProgress item)
		{
			innerSet.Add(item); 

			if (!OfIndex.ContainsKey(item.Owner))
				OfIndex.Add(item.Owner, new List<DevelopmentProgress>());
			OfIndex[item.Owner].Add(item);
		}

		public void Add(IEnumerable<DevelopmentProgress> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OfIndex.Clear();
		}

		public bool Contains(DevelopmentProgress item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(DevelopmentProgress[] array, int arrayIndex)
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

		public bool Remove(DevelopmentProgress item)
		{
			if (innerSet.Remove(item)) {
				OfIndex[item.Owner].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<DevelopmentProgress> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendAdd(DevelopmentProgress element)
		{
			toAdd.Add(element);
		}
		
		public void PendRemove(DevelopmentProgress element)
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
