using System;
using System.Collections.Generic;
using Stareater.Galaxy;

namespace Stareater.GameData
{
	class Intelligence
	{
		private Dictionary<StarData, StarIntelligence> starKnowledge = new Dictionary<StarData, StarIntelligence>();
		
		public void Initialize(IEnumerable<StarData> stars, Func<StarData, IEnumerable<Planet>> starPlanets)
		{
			foreach(var star in stars)
				starKnowledge.Add(star, new StarIntelligence(starPlanets(star)));
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
