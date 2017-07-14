 


using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System;
using System.Collections.Generic;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases.Tables 
{
	class ColonizationPlan 
	{
		[StateProperty]
		public Planet Destination { get; private set; }
		[StateProperty]
		public List<StarData> Sources { get; private set; }

		public ColonizationPlan(Planet destination) 
		{
			this.Destination = destination;
			this.Sources = new List<StarData>();
 
			 
		} 

		private ColonizationPlan(ColonizationPlan original, GalaxyRemap galaxyRemap, Planet destination) 
		{
			this.Destination = destination;
			this.Sources = new List<StarData>();
			foreach(var item in original.Sources)
				this.Sources.Add(galaxyRemap.Stars[item]);
 
			 
		}

		private ColonizationPlan(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var destinationSave = rawData[DestinationKey];
			this.Destination = deindexer.Get<Planet>(destinationSave.To<int>());

			var sourcesSave = rawData[SourcesKey];
			this.Sources = new List<StarData>();
			foreach(var item in sourcesSave.To<IkonArray>())
				this.Sources.Add(deindexer.Get<StarData>(item.To<int>()));
 
			 
		}

		private ColonizationPlan() 
		{ }
		internal ColonizationPlan Copy(GalaxyRemap galaxyRemap) 
		{
			return new ColonizationPlan(this, galaxyRemap, galaxyRemap.Planets[this.Destination]);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(DestinationKey, new IkonInteger(indexer.IndexOf(this.Destination)));

			var sourcesData = new IkonArray();
			foreach(var item in this.Sources)
				sourcesData.Add(new IkonInteger(indexer.IndexOf(item)));
			data.Add(SourcesKey, sourcesData);
			return data;
 
		}

		public static ColonizationPlan Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new ColonizationPlan(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "ColonizationPlan";
		private const string DestinationKey = "destination";
		private const string SourcesKey = "sources";
 
		#endregion

 
	}
}
