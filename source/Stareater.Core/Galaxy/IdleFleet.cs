 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using Stareater.GameData;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;

namespace Stareater.Galaxy 
{
	partial class IdleFleet 
	{
		public Player Owner { get; private set; }
		public StarData Location { get; private set; }
		public ShipGroupCollection Ships { get; private set; }

		public IdleFleet(Player owner, StarData location) 
		{
			this.Owner = owner;
			this.Location = location;
			this.Ships = new ShipGroupCollection();
 
		} 

		private IdleFleet(IdleFleet original, Player owner, StarData location) 
		{
			this.Owner = owner;
			this.Location = location;
			this.Ships = new ShipGroupCollection();
			foreach(var item in original.Ships)
				this.Ships.Add(item);
 
		}

		private  IdleFleet(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var ownerSave = rawData[OwnerKey];
			this.Owner = deindexer.Get<Player>(ownerSave.To<int>());

			var locationSave = rawData[LocationKey];
			this.Location = deindexer.Get<StarData>(locationSave.To<int>());

			var shipsSave = rawData[ShipsKey];
			this.Ships = new ShipGroupCollection();
			foreach(var item in shipsSave.To<IkonArray>())
				this.Ships.Add(ShipGroup.Load(item.To<IkonComposite>(), deindexer));
 
		}

		internal IdleFleet Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap) 
		{
			return new IdleFleet(this, playersRemap.Players[this.Owner], galaxyRemap.Stars[this.Location]);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

			data.Add(LocationKey, new IkonInteger(indexer.IndexOf(this.Location)));

			var shipsData = new IkonArray();
			foreach(var item in this.Ships)
				shipsData.Add(item.Save(indexer));
			data.Add(ShipsKey, shipsData);
			return data;
 
		}
		
		public static IdleFleet Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new IdleFleet(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}

		private const string TableTag = "IdleFleet";
		private const string OwnerKey = "owner";
		private const string LocationKey = "location";
		private const string ShipsKey = "ships";
 
		#endregion
	}
}
