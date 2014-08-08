 

using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Utils.Collections;

namespace Stareater.GameData 
{
	partial class StarIntelligence 
	{
		public int LastVisited { get; private set; }
		public Dictionary<Planet, PlanetIntelligence> Planets { get; private set; }

		public StarIntelligence(IEnumerable<Planet> planets) 
		{
			this.LastVisited = NeverVisited;
			this.Planets = new Dictionary<Planet, PlanetIntelligence>();
			foreach(var item in planets)
				this.Planets.Add(item, new PlanetIntelligence());
 
		} 

		private StarIntelligence(StarIntelligence original, GalaxyRemap galaxyRemap) 
		{
			this.LastVisited = original.LastVisited;
			this.Planets = new Dictionary<Planet, PlanetIntelligence>();
			foreach(var item in original.Planets)
				this.Planets.Add(galaxyRemap.Planets[item.Key], item.Value.Copy());
 
		}

		internal StarIntelligence Copy(GalaxyRemap galaxyRemap) 
		{
			return new StarIntelligence(this, galaxyRemap);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(LastVisitedKey, new IkonInteger(this.LastVisited));

			var planetsData = new IkonArray();
			foreach(var item in this.Planets) {
				var itemData = new IkonComposite(PlanetIntellTag);
				itemData.Add(PlanetKey, new IkonInteger(indexer.IndexOf(item.Key)));
				itemData.Add(PlanetIntelligenceKey, item.Value.Save());
				planetsData.Add(itemData);
			}
			data.Add(PlanetsKey, planetsData);
			return data;
 
		}

		private const string TableTag = "StarIntelligence";
		private const string LastVisitedKey = "lastVisited";
		private const string PlanetsKey = "planets";
		private const string PlanetIntellTag = "PlanetIntell";
		private const string PlanetKey = "planet";
		private const string PlanetIntelligenceKey = "intell";
 
		#endregion
	}
}
