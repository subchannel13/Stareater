 
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
		public IDictionary<Planet, PlanetIntelligence> Planets { get; private set; }

		public StarIntelligence(IEnumerable<Planet> planets) 
		{
			this.LastVisited = NeverVisited;
			initPlanets(planets);
 
		} 

		private StarIntelligence(StarIntelligence original, GalaxyRemap galaxyRemap) 
		{
			this.LastVisited = original.LastVisited;
			copyPlanets(original, galaxyRemap);
 
		}

		internal StarIntelligence Copy(GalaxyRemap galaxyRemap) 
		{
			return new StarIntelligence(this, galaxyRemap);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			IkonComposite data = new IkonComposite(TableTag);
			
			data.Add(LastVisitedKey, new IkonInteger(this.LastVisited));

			data.Add(PlanetsKey, savePlanets(indexer));
 

			return data;
		}

		private const string TableTag = "StarIntelligence"; 
		private const string LastVisitedKey = "lastVisited";
		private const string PlanetsKey = "planets";
 
		#endregion
	}
}
