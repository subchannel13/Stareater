 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
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
		public LinkedList<AMission> Missions { get; private set; }
		public ShipGroupCollection Ships { get; private set; }

		public Fleet(Player owner, Vector2D position, LinkedList<AMission> missions) 
		{
			this.Owner = owner;
			this.Position = position;
			this.Missions = missions;
			this.Ships = new ShipGroupCollection();
 
			 
		} 

		private Fleet(Fleet original, PlayersRemap playersRemap, Player owner) 
		{
			this.Owner = owner;
			this.Position = original.Position;
			this.Missions = new LinkedList<AMission>();
			foreach(var item in original.Missions)
				this.Missions.AddLast(playersRemap.Missions[item]);
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

			var missionsSave = rawData[MissionsKey];
			this.Missions = new LinkedList<AMission>();
			foreach(var item in missionsSave.To<IkonArray>())
				this.Missions.AddLast(MissionFactory.Load(item, deindexer));

			var shipsSave = rawData[ShipsKey];
			this.Ships = new ShipGroupCollection();
			foreach(var item in shipsSave.To<IkonArray>())
				this.Ships.Add(ShipGroup.Load(item.To<IkonComposite>(), deindexer));
 
			 
		}

		internal Fleet Copy(PlayersRemap playersRemap) 
		{
			return new Fleet(this, playersRemap, playersRemap.Players[this.Owner]);
 
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

			var missionsData = new IkonArray();
			foreach(var item in this.Missions)
				missionsData.Add(item.Save(indexer));
			data.Add(MissionsKey, missionsData);

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
		private const string MissionsKey = "missions";
		private const string ShipsKey = "ships";
 
		#endregion

 
	}
}
