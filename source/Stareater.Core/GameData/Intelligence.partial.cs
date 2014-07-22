using System;
using System.Collections.Generic;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.Galaxy.Builders;
using Stareater.Utils.Collections;

namespace Stareater.GameData
{
	partial class Intelligence
	{
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
		
		private void copyStars(Intelligence original, GalaxyRemap galaxyRemap)
		{
			this.starKnowledge = new Dictionary<StarData, StarIntelligence>();
			foreach (var starIntell in original.starKnowledge)
				this.starKnowledge.Add(galaxyRemap.Stars[starIntell.Key], starIntell.Value.Copy(galaxyRemap));
		}
		
		private IkadnBaseObject saveStars(ObjectIndexer indexer)
		{
			var stars = new IkonArray();
			
			foreach(var starIntell in this.starKnowledge) {
				var starIntellData = new IkonComposite(StarIntellTag);
				starIntellData.Add(StarKey, new IkonInteger(indexer.IndexOf(starIntell.Key)));
				starIntellData.Add(StarIntellKey, starIntell.Value.Save(indexer));
				stars.Add(starIntellData);
			}
			
			return stars;
		}
		
		#region Saving keys
		private const string StarIntellTag = "StarIntell"; 
		private const string StarKey = "star";
		private const string StarIntellKey = "intell";
 		#endregion
	}
}
