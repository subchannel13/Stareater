 


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
		public List<Fleet> NewColonizers { get; private set; }

		public ColonizationProject(Planet destination) 
		{
			this.Destination = destination;
			this.NewColonizers = new List<Fleet>();
 
			 
		} 

		private ColonizationProject(ColonizationProject original, PlayersRemap playersRemap, GalaxyRemap galaxyRemap, Planet destination) 
		{
			this.Destination = destination;
			this.NewColonizers = new List<Fleet>();
			foreach(var item in original.NewColonizers)
				this.NewColonizers.Add(item.Copy(playersRemap, galaxyRemap));
 
			 
		}

		private ColonizationProject(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var destinationSave = rawData[DestinationKey];
			this.Destination = deindexer.Get<Planet>(destinationSave.To<int>());

			var newColonizersSave = rawData[NewColonizersKey];
			this.NewColonizers = new List<Fleet>();
			foreach(var item in newColonizersSave.To<IkonArray>())
				this.NewColonizers.Add(Fleet.Load(item.To<IkonComposite>(), deindexer));
 
			 
		}

		internal ColonizationProject Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap) 
		{
			return new ColonizationProject(this, playersRemap, galaxyRemap, galaxyRemap.Planets[this.Destination]);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(DestinationKey, new IkonInteger(indexer.IndexOf(this.Destination)));

			var newColonizersData = new IkonArray();
			foreach(var item in this.NewColonizers)
				newColonizersData.Add(item.Save(indexer));
			data.Add(NewColonizersKey, newColonizersData);
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
		private const string NewColonizersKey = "newColonizers";
 
		#endregion

 
	}
}
