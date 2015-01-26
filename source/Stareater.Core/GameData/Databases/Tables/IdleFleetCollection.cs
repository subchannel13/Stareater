using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Ships;

namespace Stareater.GameData.Databases.Tables
{
	class FleetCollection : ICollection<Fleet>, IDelayedRemoval<Fleet>
	{
		HashSet<Fleet> innerSet = new HashSet<Fleet>();
		List<Fleet> toRemove = new List<Fleet>();

		Dictionary<Player, List<Fleet>> OwnedByIndex = new Dictionary<Player, List<Fleet>>();
		Dictionary<Vector2D, List<Fleet>> AtIndex = new Dictionary<Vector2D, List<Fleet>>();

		public IList<Fleet> OwnedBy(Player key) {
			if (OwnedByIndex.ContainsKey(key))
				return OwnedByIndex[key];
			
			return new List<Fleet>();
		}

		public IList<Fleet> At(Vector2D key) {
			if (AtIndex.ContainsKey(key))
				return AtIndex[key];
			
			return new List<Fleet>();
		}
	
		public void Add(Fleet item)
		{
			innerSet.Add(item); 

			if (!OwnedByIndex.ContainsKey(item.Owner))
				OwnedByIndex.Add(item.Owner, new List<Fleet>());
			OwnedByIndex[item.Owner].Add(item);

			if (!AtIndex.ContainsKey(item.Position))
				AtIndex.Add(item.Position, new List<Fleet>());
			AtIndex[item.Position].Add(item);
		}

		public void Add(IEnumerable<Fleet> items)
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

		public bool Contains(Fleet item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(Fleet[] array, int arrayIndex)
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

		public bool Remove(Fleet item)
		{
			if (innerSet.Remove(item)) {
				OwnedByIndex[item.Owner].Remove(item);
				AtIndex[item.Position].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<Fleet> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(Fleet element)
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
