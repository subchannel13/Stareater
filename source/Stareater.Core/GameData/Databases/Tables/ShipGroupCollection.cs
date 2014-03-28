using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Ships;

namespace Stareater.GameData.Databases.Tables
{
	class ShipGroupCollection : ICollection<ShipGroup>, IDelayedRemoval<ShipGroup>
	{
		HashSet<ShipGroup> innerSet = new HashSet<ShipGroup>();
		List<ShipGroup> toRemove = new List<ShipGroup>();

		Dictionary<Design, List<ShipGroup>> DesignIndex = new Dictionary<Design, List<ShipGroup>>();

		public IList<ShipGroup> Design(Design key) {
			if (DesignIndex.ContainsKey(key))
				return DesignIndex[key];
			
			return new List<ShipGroup>();
		}
	
		public void Add(ShipGroup item)
		{
			innerSet.Add(item); 

			if (!DesignIndex.ContainsKey(item.Design))
				DesignIndex.Add(item.Design, new List<ShipGroup>());
			DesignIndex[item.Design].Add(item);
		}

		public void Add(IEnumerable<ShipGroup> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			DesignIndex.Clear();
		}

		public bool Contains(ShipGroup item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(ShipGroup[] array, int arrayIndex)
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

		public bool Remove(ShipGroup item)
		{
			if (innerSet.Remove(item)) {
				DesignIndex[item.Design].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<ShipGroup> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(ShipGroup element)
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
