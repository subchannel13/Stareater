using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class ColonyCollection : ICollection<Colony>, IDelayedRemoval<Colony>
	{
		HashSet<Colony> innerSet = new HashSet<Colony>();
		List<Colony> toRemove = new List<Colony>();

		Dictionary<Player, List<Colony>> PlayersIndex = new Dictionary<Player, List<Colony>>();
		Dictionary<Planet, Colony> PlanetsIndex = new Dictionary<Planet, Colony>();
		Dictionary<StarData, List<Colony>> StarsIndex = new Dictionary<StarData, List<Colony>>();

		public IEnumerable<Colony> Players(Player key) {
			if (PlayersIndex.ContainsKey(key))
				foreach (var item in PlayersIndex[key])
					yield return item;
		}

		public Colony Planets(Planet key) {
			if (PlanetsIndex.ContainsKey(key))
				return PlanetsIndex[key];
				
			throw new KeyNotFoundException();
		}
		
		public bool PlanetsContains(Planet key) {
			return PlanetsIndex.ContainsKey(key);
		}

		public IEnumerable<Colony> Stars(StarData key) {
			if (StarsIndex.ContainsKey(key))
				foreach (var item in StarsIndex[key])
					yield return item;
		}
	
		public void Add(Colony item)
		{
			innerSet.Add(item); 

			if (!PlayersIndex.ContainsKey(item.Owner))
				PlayersIndex.Add(item.Owner, new List<Colony>());
			PlayersIndex[item.Owner].Add(item);
			if (!PlanetsIndex.ContainsKey(item.Location))
				PlanetsIndex.Add(item.Location, item);

			if (!StarsIndex.ContainsKey(item.Star))
				StarsIndex.Add(item.Star, new List<Colony>());
			StarsIndex[item.Star].Add(item);
		}

		public void Add(IEnumerable<Colony> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			PlayersIndex.Clear();
			PlanetsIndex.Clear();
			StarsIndex.Clear();
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
				PlayersIndex[item.Owner].Remove(item);
				PlanetsIndex.Remove(item.Location);
				StarsIndex[item.Star].Remove(item);
			
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

		public void PendRemove(Colony element)
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
