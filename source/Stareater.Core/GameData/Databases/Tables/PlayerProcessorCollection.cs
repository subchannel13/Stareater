using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	partial class PlayerProcessorCollection : ICollection<PlayerProcessor>, IDelayedCollection<PlayerProcessor>
	{
		private HashSet<PlayerProcessor> innerSet = new HashSet<PlayerProcessor>();
		private readonly List<PlayerProcessor> toAdd = new List<PlayerProcessor>();
		private readonly List<PlayerProcessor> toRemove = new List<PlayerProcessor>();

		Dictionary<Player, PlayerProcessor> OfIndex = new Dictionary<Player, PlayerProcessor>();

		public PlayerProcessor Of(Player key) {
			if (OfIndex.ContainsKey(key))
				return OfIndex[key];
				
			throw new KeyNotFoundException();
		}
		
		public bool OfContains(Player key) 
		{
			return OfIndex.ContainsKey(key);
		}
	
		public void Add(PlayerProcessor item)
		{
			innerSet.Add(item); 
			if (!OfIndex.ContainsKey(item.Player))
				OfIndex.Add(item.Player, item);
		}

		public void Add(IEnumerable<PlayerProcessor> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OfIndex.Clear();
		}

		public bool Contains(PlayerProcessor item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(PlayerProcessor[] array, int arrayIndex)
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

		public bool Remove(PlayerProcessor item)
		{
			if (innerSet.Remove(item)) {
				OfIndex.Remove(item.Player);
			
				return true;
			}

			return false;
		}

		public IEnumerator<PlayerProcessor> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendAdd(PlayerProcessor element)
		{
			toAdd.Add(element);
		}
		
		public void PendRemove(PlayerProcessor element)
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
