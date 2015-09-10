 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
using Stareater.Galaxy;

namespace Stareater.GameData 
{
	partial class ColonizationProject 
	{
		public Planet Destination { get; private set; }

		public ColonizationProject(Planet destination) 
		{
			this.Destination = destination;
 
			 
		} 


		private ColonizationProject(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var destinationSave = rawData[DestinationKey];
			this.Destination = deindexer.Get<Planet>(destinationSave.To<int>());
 
			 
		}

		internal ColonizationProject Copy(GalaxyRemap galaxyRemap) 
		{
			return new ColonizationProject(galaxyRemap.Planets[this.Destination]);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(DestinationKey, new IkonInteger(indexer.IndexOf(this.Destination)));
			return data;
 
		}

		public static ColonizationProject Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new ColonizationProject(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "ColonizationProject";
		private const string DestinationKey = "destination";
 
		#endregion

 
	}
}
