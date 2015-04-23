using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using NGenerics.DataStructures.Mathematical;

namespace Stareater.GameData.Databases.Tables
{
	class StarCollection : ICollection<StarData>, IDelayedCollection<StarData>
	{
		private HashSet<StarData> innerSet = new HashSet<StarData>();
		private readonly List<StarData> toAdd = new List<StarData>();
		private readonly List<StarData> toRemove = new List<StarData>();

		Dictionary<Vector2D, StarData> AtIndex = new Dictionary<Vector2D, StarData>();

		public StarData At(Vector2D key) {
			if (AtIndex.ContainsKey(key))
				return AtIndex[key];
				
			throw new KeyNotFoundException();
		}
		
		public bool AtContains(Vector2D key) 
		{
			return AtIndex.ContainsKey(key);
		}
	
		public void Add(StarData item)
		{
			innerSet.Add(item); 
			if (!AtIndex.ContainsKey(item.Position))
				AtIndex.Add(item.Position, item);
		}

		public void Add(IEnumerable<StarData> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			AtIndex.Clear();
		}

		public bool Contains(StarData item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(StarData[] array, int arrayIndex)
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

		public bool Remove(StarData item)
		{
			if (innerSet.Remove(item)) {
				AtIndex.Remove(item.Position);
			
				return true;
			}

			return false;
		}

		public IEnumerator<StarData> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendAdd(StarData element)
		{
			toAdd.Add(element);
		}
		
		public void PendRemove(StarData element)
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
