 


using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using System;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData 
{
	partial class ColonizationProject 
	{
		[StateProperty]
		public Player Owner { get; private set; }
		[StateProperty]
		public Planet Destination { get; private set; }

		public ColonizationProject(Player owner, Planet destination) 
		{
			this.Owner = owner;
			this.Destination = destination;
 
			 
		} 


		private ColonizationProject(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var ownerSave = rawData[OwnerKey];
			this.Owner = deindexer.Get<Player>(ownerSave.To<int>());

			var destinationSave = rawData[DestinationKey];
			this.Destination = deindexer.Get<Planet>(destinationSave.To<int>());
 
			 
		}

		private ColonizationProject() 
		{ }
		internal ColonizationProject Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap) 
		{
			return new ColonizationProject(playersRemap.Players[this.Owner], galaxyRemap.Planets[this.Destination]);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

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
		private const string OwnerKey = "owner";
		private const string DestinationKey = "destination";
 
		#endregion

 
	}
}
