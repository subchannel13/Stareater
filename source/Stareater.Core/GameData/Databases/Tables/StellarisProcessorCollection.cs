using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	partial class StellarisProcessorCollection : ICollection<StellarisProcessor>, IDelayedRemoval<StellarisProcessor>
	{
		HashSet<StellarisProcessor> innerSet = new HashSet<StellarisProcessor>();
		List<StellarisProcessor> toRemove = new List<StellarisProcessor>();

		Dictionary<Player, List<StellarisProcessor>> OwnedByIndex = new Dictionary<Player, List<StellarisProcessor>>();
		Dictionary<StellarisAdmin, StellarisProcessor> OfIndex = new Dictionary<StellarisAdmin, StellarisProcessor>();

		public IList<StellarisProcessor> OwnedBy(Player key) {
			if (OwnedByIndex.ContainsKey(key))
				return OwnedByIndex[key];
			
			return new List<StellarisProcessor>();
		}

		public StellarisProcessor Of(StellarisAdmin key) {
			if (OfIndex.ContainsKey(key))
				return OfIndex[key];
				
			throw new KeyNotFoundException();
		}
		
		public bool OfContains(StellarisAdmin key) {
			return OfIndex.ContainsKey(key);
		}
	
		public void Add(StellarisProcessor item)
		{
			innerSet.Add(item); 

			if (!OwnedByIndex.ContainsKey(item.Owner))
				OwnedByIndex.Add(item.Owner, new List<StellarisProcessor>());
			OwnedByIndex[item.Owner].Add(item);
			if (!OfIndex.ContainsKey(item.Stellaris))
				OfIndex.Add(item.Stellaris, item);
		}

		public void Add(IEnumerable<StellarisProcessor> items)
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

		public bool Contains(StellarisProcessor item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(StellarisProcessor[] array, int arrayIndex)
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

		public bool Remove(StellarisProcessor item)
		{
			if (innerSet.Remove(item)) {
				OwnedByIndex[item.Owner].Remove(item);
				OfIndex.Remove(item.Stellaris);
			
				return true;
			}

			return false;
		}

		public IEnumerator<StellarisProcessor> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(StellarisProcessor element)
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
