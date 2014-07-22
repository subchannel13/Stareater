using System;
using System.Collections.Generic;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.Utils;
using Stareater.Utils.Collections;

namespace Stareater.GameData
{
	partial class StarIntelligence
	{
		public const int NeverVisited = -1;
		
		private void initPlanets(IEnumerable<Planet> planets)
		{
			this.Planets = new Dictionary<Planet, PlanetIntelligence>();
			foreach(var planet in planets)
				this.Planets.Add(planet, new PlanetIntelligence());
		}
		
		private void copyPlanets(StarIntelligence original, GalaxyRemap galaxyRemap)
		{
			this.Planets = new Dictionary<Planet, PlanetIntelligence>();
			foreach (var planetIntell in original.Planets)
				this.Planets.Add(galaxyRemap.Planets[planetIntell.Key], planetIntell.Value.Copy());
		}
		
		public bool IsVisited
		{
			get { return LastVisited != NeverVisited; }
		}
		
		public void Visit(int turn)
		{
			this.LastVisited = turn;
		}

		private IkadnBaseObject savePlanets(ObjectIndexer indexer)
		{
			var planets = new IkonArray();
			
			foreach(var planetIntell in this.Planets) {
				var planetIntellData = new IkonComposite(PlanetIntellTag);
				planetIntellData.Add(PlanetKey, new IkonInteger(indexer.IndexOf(planetIntell.Key)));
				planetIntellData.Add(PlanetIntellKey, planetIntell.Value.Save());
				planets.Add(planetIntellData);
			}
			
			return planets;
		}
		
		#region Saving keys
		private const string PlanetIntellTag = "PlanetIntell"; 
		private const string PlanetKey = "planet";
		private const string PlanetIntellKey = "intell";
 		#endregion
	}
}
