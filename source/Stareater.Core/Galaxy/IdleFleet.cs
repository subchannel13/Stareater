 

using Ikadn.Ikon.Types;
using System;
using Stareater.GameData;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Utils.Collections;

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

		private IdleFleet(IdleFleet original, PlayersRemap playersRemap, Player owner, StarData location) 
		{
			this.Owner = owner;
			this.Location = location;
			this.Ships = new ShipGroupCollection();
			foreach(var item in original.Ships)
				this.Ships.Add(item.Copy(playersRemap));
 
		}

		internal IdleFleet Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap) 
		{
			return new IdleFleet(this, playersRemap, playersRemap.Players[this.Owner], galaxyRemap.Stars[this.Location]);
 
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

		private const string TableTag = "IdleFleet";
		private const string OwnerKey = "owner";
		private const string LocationKey = "location";
		private const string ShipsKey = "ships";
 
		#endregion
	}
}
