using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases.Tables
{
	class PlanetCollection : ICollection<Planet>, IDelayedRemoval<Planet>
	{
		HashSet<Planet> innerSet = new HashSet<Planet>();
		List<Planet> toRemove = new List<Planet>();

		Dictionary<StarData, List<Planet>> StarSystemIndex = new Dictionary<StarData, List<Planet>>();

		public IEnumerable<Planet> StarSystem(StarData key) {
			if (StarSystemIndex.ContainsKey(key))
				foreach (var item in StarSystemIndex[key])
					yield return item;
		}
	
		public void Add(Planet item)
		{
			innerSet.Add(item); 

			if (!StarSystemIndex.ContainsKey(item.Star))
				StarSystemIndex.Add(item.Star, new List<Planet>());
			StarSystemIndex[item.Star].Add(item);
		}

		public void Add(IEnumerable<Planet> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			StarSystemIndex.Clear();
		}

		public bool Contains(Planet item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(Planet[] array, int arrayIndex)
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

		public bool Remove(Planet item)
		{
			if (innerSet.Remove(item)) {
				StarSystemIndex[item.Star].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<Planet> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(Planet element)
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
