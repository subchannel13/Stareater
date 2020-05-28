using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.GameData.Databases;

namespace Stareater.GameData
{
	class Intelligence 
	{
		[StatePropertyAttribute]
		private Dictionary<StarData, StarIntelligence> starKnowledge { get; set; }

		public Intelligence() 
		{
			this.starKnowledge = new Dictionary<StarData, StarIntelligence>();
			
		}

		public void Initialize(StatesDB states)
		{
			this.starKnowledge.Clear();
			foreach (var star in states.Stars)
				starKnowledge.Add(star, new StarIntelligence(states.Planets.At[star]));
		}

		public void StarFullyVisited(StarData star, int turn)
		{
			var starInfo = starKnowledge[star];

			starInfo.Visit(turn);
			foreach (var planetInfo in starInfo.Planets.Values)
			{
				planetInfo.Visit(turn);
				planetInfo.Discovered = true;
			}
		}

		public void StarVisited(StarData star, int turn)
		{
			var starInfo = starKnowledge[star];

			starInfo.Visit(turn);
			foreach (var planetInfo in starInfo.Planets.Values)
			{
				planetInfo.Visit(turn);
			}
		}

		public StarIntelligence About(StarData star)
		{
			return starKnowledge[star];
		}

		public bool IsKnown(Wormhole wormhole)
		{
			return this.starKnowledge[wormhole.Endpoints.First].IsVisited || 
				this.starKnowledge[wormhole.Endpoints.Second].IsVisited;
		}
	}
}
