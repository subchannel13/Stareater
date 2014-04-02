using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Ships;

namespace Stareater.GameData.Databases.Tables
{
	class IdleFleetCollection : ICollection<IdleFleet>, IDelayedRemoval<IdleFleet>
	{
		HashSet<IdleFleet> innerSet = new HashSet<IdleFleet>();
		List<IdleFleet> toRemove = new List<IdleFleet>();

		Dictionary<Player, List<IdleFleet>> OwnedByIndex = new Dictionary<Player, List<IdleFleet>>();
		Dictionary<StarData, List<IdleFleet>> AtStarIndex = new Dictionary<StarData, List<IdleFleet>>();

		public IList<IdleFleet> OwnedBy(Player key) {
			if (OwnedByIndex.ContainsKey(key))
				return OwnedByIndex[key];
			
			return new List<IdleFleet>();
		}

		public IList<IdleFleet> AtStar(StarData key) {
			if (AtStarIndex.ContainsKey(key))
				return AtStarIndex[key];
			
			return new List<IdleFleet>();
		}
	
		public void Add(IdleFleet item)
		{
			innerSet.Add(item); 

			if (!OwnedByIndex.ContainsKey(item.Owner))
				OwnedByIndex.Add(item.Owner, new List<IdleFleet>());
			OwnedByIndex[item.Owner].Add(item);

			if (!AtStarIndex.ContainsKey(item.Location))
				AtStarIndex.Add(item.Location, new List<IdleFleet>());
			AtStarIndex[item.Location].Add(item);
		}

		public void Add(IEnumerable<IdleFleet> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OwnedByIndex.Clear();
			AtStarIndex.Clear();
		}

		public bool Contains(IdleFleet item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(IdleFleet[] array, int arrayIndex)
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

		public bool Remove(IdleFleet item)
		{
			if (innerSet.Remove(item)) {
				OwnedByIndex[item.Owner].Remove(item);
				AtStarIndex[item.Location].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<IdleFleet> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(IdleFleet element)
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
