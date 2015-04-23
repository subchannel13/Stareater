using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class ColonyCollection : ICollection<Colony>, IDelayedCollection<Colony>
	{
		private HashSet<Colony> innerSet = new HashSet<Colony>();
		private readonly List<Colony> toAdd = new List<Colony>();
		private readonly List<Colony> toRemove = new List<Colony>();

		Dictionary<Player, List<Colony>> OwnedByIndex = new Dictionary<Player, List<Colony>>();
		Dictionary<Planet, Colony> AtPlanetIndex = new Dictionary<Planet, Colony>();
		Dictionary<StarData, List<Colony>> AtStarIndex = new Dictionary<StarData, List<Colony>>();

		public IList<Colony> OwnedBy(Player key) 
		{
			return (OwnedByIndex.ContainsKey(key)) ? 
				OwnedByIndex[key] : 
				new List<Colony>();
		}

		public Colony AtPlanet(Planet key) {
			if (AtPlanetIndex.ContainsKey(key))
				return AtPlanetIndex[key];
				
			throw new KeyNotFoundException();
		}
		
		public bool AtPlanetContains(Planet key) 
		{
			return AtPlanetIndex.ContainsKey(key);
		}

		public IList<Colony> AtStar(StarData key) 
		{
			return (AtStarIndex.ContainsKey(key)) ? 
				AtStarIndex[key] : 
				new List<Colony>();
		}
	
		public void Add(Colony item)
		{
			innerSet.Add(item); 

			if (!OwnedByIndex.ContainsKey(item.Owner))
				OwnedByIndex.Add(item.Owner, new List<Colony>());
			OwnedByIndex[item.Owner].Add(item);
			if (!AtPlanetIndex.ContainsKey(item.Location.Planet))
				AtPlanetIndex.Add(item.Location.Planet, item);

			if (!AtStarIndex.ContainsKey(item.Star))
				AtStarIndex.Add(item.Star, new List<Colony>());
			AtStarIndex[item.Star].Add(item);
		}

		public void Add(IEnumerable<Colony> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			OwnedByIndex.Clear();
			AtPlanetIndex.Clear();
			AtStarIndex.Clear();
		}

		public bool Contains(Colony item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(Colony[] array, int arrayIndex)
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

		public bool Remove(Colony item)
		{
			if (innerSet.Remove(item)) {
				OwnedByIndex[item.Owner].Remove(item);
				AtPlanetIndex.Remove(item.Location.Planet);
				AtStarIndex[item.Star].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<Colony> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendAdd(Colony element)
		{
			toAdd.Add(element);
		}
		
		public void PendRemove(Colony element)
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
