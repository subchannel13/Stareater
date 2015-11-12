 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using NGenerics.DataStructures.Mathematical;
using Stareater.GameData;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;
using Stareater.Ships.Missions;

namespace Stareater.Galaxy 
{
	partial class Fleet 
	{
		public Player Owner { get; private set; }
		public Vector2D Position { get; private set; }
		public AMission Mission { get; private set; }
		public ShipGroupCollection Ships { get; private set; }

		public Fleet(Player owner, Vector2D position, AMission mission) 
		{
			this.Owner = owner;
			this.Position = position;
			this.Mission = mission;
			this.Ships = new ShipGroupCollection();
 
			 
		} 

		private Fleet(Fleet original, PlayersRemap playersRemap, Player owner, AMission mission) 
		{
			this.Owner = owner;
			this.Position = original.Position;
			this.Mission = mission;
			this.Ships = new ShipGroupCollection();
			foreach(var item in original.Ships)
				this.Ships.Add(item.Copy(playersRemap));
 
			 
		}

		private Fleet(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var ownerSave = rawData[OwnerKey];
			this.Owner = deindexer.Get<Player>(ownerSave.To<int>());

			var positionSave = rawData[PositionKey];
			var positionArray = positionSave.To<IkonArray>();
			double positionX = positionArray[0].To<double>();
			double positionY = positionArray[1].To<double>();
			this.Position = new Vector2D(positionX, positionY);

			if (rawData.Keys.Contains(MissionKey))
			{
				var missionSave = rawData[MissionKey];
				this.Mission = MissionFactory.Load(missionSave, deindexer);
			}

			var shipsSave = rawData[ShipsKey];
			this.Ships = new ShipGroupCollection();
			foreach(var item in shipsSave.To<IkonArray>())
				this.Ships.Add(ShipGroup.Load(item.To<IkonComposite>(), deindexer));
 
			 
		}

		internal Fleet Copy(PlayersRemap playersRemap) 
		{
			return new Fleet(this, playersRemap, playersRemap.Players[this.Owner], (this.Mission != null) ? playersRemap.Missions[this.Mission] : null);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

			var positionData = new IkonArray();
			positionData.Add(new IkonFloat(this.Position.X));
			positionData.Add(new IkonFloat(this.Position.Y));
			data.Add(PositionKey, positionData);

			if (this.Mission != null)
				data.Add(MissionKey, this.Mission.Save(indexer));

			var shipsData = new IkonArray();
			foreach(var item in this.Ships)
				shipsData.Add(item.Save(indexer));
			data.Add(ShipsKey, shipsData);
			return data;
 
		}

		public static Fleet Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new Fleet(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "Fleet";
		private const string OwnerKey = "owner";
		private const string PositionKey = "position";
		private const string MissionKey = "mission";
		private const string ShipsKey = "ships";
 
		#endregion

 
	}
}
