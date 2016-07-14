using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class ColonizationCollection : ICollection<ColonizationProject>, IDelayedCollection<ColonizationProject>
	{
		private HashSet<ColonizationProject> innerSet = new HashSet<ColonizationProject>();
		private readonly List<ColonizationProject> toAdd = new List<ColonizationProject>();
		private readonly List<ColonizationProject> toRemove = new List<ColonizationProject>();

		Dictionary<Player, List<ColonizationProject>> OwnedByIndex = new Dictionary<Player, List<ColonizationProject>>();
		Dictionary<Planet, List<ColonizationProject>> OfIndex = new Dictionary<Planet, List<ColonizationProject>>();

		public IList<ColonizationProject> OwnedBy(Player key) 
		{
			return (OwnedByIndex.ContainsKey(key)) ? 
				OwnedByIndex[key] : 
				new List<ColonizationProject>();
		}

		public IList<ColonizationProject> Of(Planet key) 
		{
			return (OfIndex.ContainsKey(key)) ? 
				OfIndex[key] : 
				new List<ColonizationProject>();
		}
	
		public void Add(ColonizationProject item)
		{
			innerSet.Add(item); 

			if (!OwnedByIndex.ContainsKey(item.Owner))
				OwnedByIndex.Add(item.Owner, new List<ColonizationProject>());
			OwnedByIndex[item.Owner].Add(item);

			if (!OfIndex.ContainsKey(item.Destination))
				OfIndex.Add(item.Destination, new List<ColonizationProject>());
			OfIndex[item.Destination].Add(item);
		}

		public void Add(IEnumerable<ColonizationProject> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OwnedByIndex.Clear();
			OfIndex.Clear();
		}

		public bool Contains(ColonizationProject item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(ColonizationProject[] array, int arrayIndex)
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

		public bool Remove(ColonizationProject item)
		{
			if (innerSet.Remove(item)) {
				OwnedByIndex[item.Owner].Remove(item);
				OfIndex[item.Destination].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<ColonizationProject> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendAdd(ColonizationProject element)
		{
			toAdd.Add(element);
		}
		
		public void PendRemove(ColonizationProject element)
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
