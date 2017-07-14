 


using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System;

namespace Stareater.GameData 
{
	partial class PlanetIntelligence 
	{
		[StateProperty]
		public double Explored { get; private set; }
		[StateProperty]
		public int LastVisited { get; private set; }

		public PlanetIntelligence() 
		{
			this.Explored = Unexplored;
			this.LastVisited = NeverVisited;
 
			 
		} 

		private PlanetIntelligence(PlanetIntelligence original) 
		{
			this.Explored = original.Explored;
			this.LastVisited = original.LastVisited;
 
			 
		}

		private PlanetIntelligence(IkonComposite rawData) 
		{
			var exploredSave = rawData[ExploredKey];
			this.Explored = exploredSave.To<double>();

			var lastVisitedSave = rawData[LastVisitedKey];
			this.LastVisited = lastVisitedSave.To<int>();
 
			 
		}

		internal PlanetIntelligence Copy() 
		{
			return new PlanetIntelligence(this);
 
		} 
 

		#region Saving
		public IkonComposite Save() 
		{
			var data = new IkonComposite(TableTag);
			data.Add(ExploredKey, new IkonFloat(this.Explored));

			data.Add(LastVisitedKey, new IkonInteger(this.LastVisited));
			return data;
 
		}

		public static PlanetIntelligence Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new PlanetIntelligence(rawData);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "PlanetIntelligence";
		private const string ExploredKey = "explored";
		private const string LastVisitedKey = "lastVisited";
 
		#endregion

 
	}
}
