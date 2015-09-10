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

		Dictionary<Planet, ColonizationProject> OfIndex = new Dictionary<Planet, ColonizationProject>();

		public ColonizationProject Of(Planet key) {
			if (OfIndex.ContainsKey(key))
				return OfIndex[key];
				
			throw new KeyNotFoundException();
		}
		
		public bool OfContains(Planet key) 
		{
			return OfIndex.ContainsKey(key);
		}
	
		public void Add(ColonizationProject item)
		{
			innerSet.Add(item); 
			if (!OfIndex.ContainsKey(item.Destination))
				OfIndex.Add(item.Destination, item);
		}

		public void Add(IEnumerable<ColonizationProject> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

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
				OfIndex.Remove(item.Destination);
			
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
