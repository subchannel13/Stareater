using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	partial class ColonyProcessorCollection : ICollection<ColonyProcessor>, IDelayedRemoval<ColonyProcessor>
	{
		HashSet<ColonyProcessor> innerSet = new HashSet<ColonyProcessor>();
		List<ColonyProcessor> toRemove = new List<ColonyProcessor>();

		Dictionary<Player, List<ColonyProcessor>> OwnedByIndex = new Dictionary<Player, List<ColonyProcessor>>();
		Dictionary<Colony, ColonyProcessor> OfIndex = new Dictionary<Colony, ColonyProcessor>();
		Dictionary<StarData, List<ColonyProcessor>> AtIndex = new Dictionary<StarData, List<ColonyProcessor>>();

		public IList<ColonyProcessor> OwnedBy(Player key) {
			if (OwnedByIndex.ContainsKey(key))
				return OwnedByIndex[key];
			
			return new List<ColonyProcessor>();
		}

		public ColonyProcessor Of(Colony key) {
			if (OfIndex.ContainsKey(key))
				return OfIndex[key];
				
			throw new KeyNotFoundException();
		}
		
		public bool OfContains(Colony key) {
			return OfIndex.ContainsKey(key);
		}

		public IList<ColonyProcessor> At(StarData key) {
			if (AtIndex.ContainsKey(key))
				return AtIndex[key];
			
			return new List<ColonyProcessor>();
		}
	
		public void Add(ColonyProcessor item)
		{
			innerSet.Add(item); 

			if (!OwnedByIndex.ContainsKey(item.Owner))
				OwnedByIndex.Add(item.Owner, new List<ColonyProcessor>());
			OwnedByIndex[item.Owner].Add(item);
			if (!OfIndex.ContainsKey(item.Colony))
				OfIndex.Add(item.Colony, item);

			if (!AtIndex.ContainsKey(item.Colony.Star))
				AtIndex.Add(item.Colony.Star, new List<ColonyProcessor>());
			AtIndex[item.Colony.Star].Add(item);
		}

		public void Add(IEnumerable<ColonyProcessor> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OwnedByIndex.Clear();
			OfIndex.Clear();
			AtIndex.Clear();
		}

		public bool Contains(ColonyProcessor item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(ColonyProcessor[] array, int arrayIndex)
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

		public bool Remove(ColonyProcessor item)
		{
			if (innerSet.Remove(item)) {
				OwnedByIndex[item.Owner].Remove(item);
				OfIndex.Remove(item.Colony);
				AtIndex[item.Colony.Star].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<ColonyProcessor> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(ColonyProcessor element)
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
