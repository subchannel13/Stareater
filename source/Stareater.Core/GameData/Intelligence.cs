using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;

namespace Stareater.GameData
{
	class Intelligence
	{
		private Dictionary<StarData, StarIntelligence> starKnowledge = new Dictionary<StarData, StarIntelligence>();
		
		public void Initialize(IEnumerable<StarSystem> starSystems)
		{
			foreach(var system in starSystems)
				starKnowledge.Add(system.Star, new StarIntelligence(system.Planets));
		}
		
		public void StarFullyVisited(StarData star, int turn)
		{
			var starInfo = starKnowledge[star];
			
			starInfo.Visit(turn);
			foreach(var planetInfo in starInfo.Planets.Values) {
				planetInfo.Visit(turn);
				planetInfo.Explore(PlanetIntelligence.FullyExplored);
			}
		}
		
		public StarIntelligence About(StarData star)
		{
			return starKnowledge[star];
		}
	}
}
