 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData 
{
	partial class ColonizationProject 
	{
		public Player Owner { get; private set; }
		public Planet Destination { get; private set; }
		public List<ShipGroup> Arrived { get; private set; }

		public ColonizationProject(Player owner, Planet destination) 
		{
			this.Owner = owner;
			this.Destination = destination;
			this.Arrived = new List<ShipGroup>();
 
			 
		} 

		private ColonizationProject(ColonizationProject original, PlayersRemap playersRemap, Player owner, Planet destination) 
		{
			this.Owner = owner;
			this.Destination = destination;
			this.Arrived = new List<ShipGroup>();
			foreach(var item in original.Arrived)
				this.Arrived.Add(item.Copy(playersRemap));
 
			 
		}

		private ColonizationProject(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var ownerSave = rawData[OwnerKey];
			this.Owner = deindexer.Get<Player>(ownerSave.To<int>());

			var destinationSave = rawData[DestinationKey];
			this.Destination = deindexer.Get<Planet>(destinationSave.To<int>());

			var arrivedSave = rawData[ArrivedKey];
			this.Arrived = new List<ShipGroup>();
			foreach(var item in arrivedSave.To<IkonArray>())
				this.Arrived.Add(ShipGroup.Load(item.To<IkonComposite>(), deindexer));
 
			 
		}

		internal ColonizationProject Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap) 
		{
			return new ColonizationProject(this, playersRemap, playersRemap.Players[this.Owner], galaxyRemap.Planets[this.Destination]);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

			data.Add(DestinationKey, new IkonInteger(indexer.IndexOf(this.Destination)));

			var arrivedData = new IkonArray();
			foreach(var item in this.Arrived)
				arrivedData.Add(item.Save(indexer));
			data.Add(ArrivedKey, arrivedData);
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
		private const string ArrivedKey = "arrived";
 
		#endregion

 
	}
}
