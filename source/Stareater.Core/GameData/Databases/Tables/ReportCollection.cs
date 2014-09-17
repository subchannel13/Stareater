﻿using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Players;
using Stareater.Players.Reports;

namespace Stareater.GameData.Databases.Tables
{
	class ReportCollection : ICollection<IReport>, IDelayedRemoval<IReport>
	{
		HashSet<IReport> innerSet = new HashSet<IReport>();
		List<IReport> toRemove = new List<IReport>();

		Dictionary<Player, List<IReport>> OfIndex = new Dictionary<Player, List<IReport>>();

		public IList<IReport> Of(Player key) {
			if (OfIndex.ContainsKey(key))
				return OfIndex[key];
			
			return new List<IReport>();
		}
	
		public void Add(IReport item)
		{
			innerSet.Add(item); 

			if (!OfIndex.ContainsKey(item.Owner))
				OfIndex.Add(item.Owner, new List<IReport>());
			OfIndex[item.Owner].Add(item);
		}

		public void Add(IEnumerable<IReport> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OfIndex.Clear();
		}

		public bool Contains(IReport item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(IReport[] array, int arrayIndex)
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

		public bool Remove(IReport item)
		{
			if (innerSet.Remove(item)) {
				OfIndex[item.Owner].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<IReport> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(IReport element)
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
