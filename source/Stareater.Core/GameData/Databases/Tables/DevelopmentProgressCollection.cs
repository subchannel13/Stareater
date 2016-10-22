using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class ResearchProgressCollection : ICollection<ResearchProgress>, IDelayedCollection<ResearchProgress>
	{
		private HashSet<ResearchProgress> innerSet = new HashSet<ResearchProgress>();
		private readonly List<ResearchProgress> toAdd = new List<ResearchProgress>();
		private readonly List<ResearchProgress> toRemove = new List<ResearchProgress>();

		Dictionary<Player, List<ResearchProgress>> OfIndex = new Dictionary<Player, List<ResearchProgress>>();

		public IList<ResearchProgress> Of(Player key) 
		{
			return (OfIndex.ContainsKey(key)) ? 
				OfIndex[key] : 
				new List<ResearchProgress>();
		}
	
		public void Add(ResearchProgress item)
		{
			innerSet.Add(item); 

			if (!OfIndex.ContainsKey(item.Owner))
				OfIndex.Add(item.Owner, new List<ResearchProgress>());
			OfIndex[item.Owner].Add(item);
		}

		public void Add(IEnumerable<ResearchProgress> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OfIndex.Clear();
		}

		public bool Contains(ResearchProgress item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(ResearchProgress[] array, int arrayIndex)
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

		public bool Remove(ResearchProgress item)
		{
			if (innerSet.Remove(item)) {
				OfIndex[item.Owner].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<ResearchProgress> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendAdd(ResearchProgress element)
		{
			toAdd.Add(element);
		}
		
		public void PendRemove(ResearchProgress element)
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
