 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using System.Collections.Generic;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Galaxy 
{
	abstract partial class AConstructionSite 
	{
		public LocationBody Location { get; private set; }
		public Player Owner { get; private set; }
		public Dictionary<string, double> Buildings { get; private set; }
		public Dictionary<Constructable, double> Stockpile { get; private set; }

		protected AConstructionSite(LocationBody location, Player owner) 
		{
			this.Location = location;
			this.Owner = owner;
			this.Buildings = new Dictionary<string, double>();
			this.Stockpile = new Dictionary<Constructable, double>();
 
			#if DEBUG
			this.id = NextId();
			#endif
 
		} 

		protected AConstructionSite(AConstructionSite original, LocationBody location, Player owner) : this(location, owner) 
		{
			this.Buildings = new Dictionary<string, double>();
			foreach(var item in original.Buildings)
				this.Buildings.Add(item.Key, item.Value);
			this.Stockpile = new Dictionary<Constructable, double>();
			foreach(var item in original.Stockpile)
				this.Stockpile.Add(item.Key, item.Value);
 
			#if DEBUG
			this.id = NextId();
			#endif
 
		}

		protected AConstructionSite(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var locationSave = rawData[LocationKey];
			this.Location = LocationBody.Load(locationSave.To<IkonComposite>(), deindexer);

			var ownerSave = rawData[OwnerKey];
			this.Owner = deindexer.Get<Player>(ownerSave.To<int>());

			var buildingsSave = rawData[BuildingsKey];
			this.Buildings = new Dictionary<string, double>();
			foreach(var item in buildingsSave.To<IEnumerable<IkonComposite>>()) {
				var itemKey = item[BuildingTypeKey];
				var itemValue = item[BuildingAmountKey];
				this.Buildings.Add(
					itemKey.To<string>(),
					itemValue.To<double>()
				);
			}

			var stockpileSave = rawData[StockpileKey];
			this.Stockpile = new Dictionary<Constructable, double>();
			foreach(var item in stockpileSave.To<IEnumerable<IkonComposite>>()) {
				var itemKey = item[StockpileGroupKey];
				var itemValue = item[StockpileAmountKey];
				this.Stockpile.Add(
					deindexer.Get<Constructable>(itemKey.To<string>()),
					itemValue.To<double>()
				);
			}
 
			#if DEBUG
			this.id = NextId();
			#endif
 
		}

 

		#region Saving
		public virtual IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(LocationKey, this.Location.Save(indexer));

			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

			var buildingsData = new IkonArray();
			foreach(var item in this.Buildings) {
				var itemData = new IkonComposite(BuildingsTag);
				itemData.Add(BuildingTypeKey, new IkonText(item.Key));
				itemData.Add(BuildingAmountKey, new IkonFloat(item.Value));
				buildingsData.Add(itemData);
			}
			data.Add(BuildingsKey, buildingsData);

			var stockpileData = new IkonArray();
			foreach(var item in this.Stockpile) {
				var itemData = new IkonComposite(StockpileTag);
				itemData.Add(StockpileGroupKey, new IkonText(item.Key.IdCode));
				itemData.Add(StockpileAmountKey, new IkonFloat(item.Value));
				stockpileData.Add(itemData);
			}
			data.Add(StockpileKey, stockpileData);
			return data;
 
		}

 

		protected abstract string TableTag { get; }
		private const string LocationKey = "location";
		private const string OwnerKey = "owner";
		private const string BuildingsKey = "buildings";
		private const string BuildingsTag = "buildings";
		private const string BuildingTypeKey = "type";
		private const string BuildingAmountKey = "amount";
		private const string StockpileKey = "stockpile";
		private const string StockpileTag = "stockpile";
		private const string StockpileGroupKey = "group";
		private const string StockpileAmountKey = "amount";
 
		#endregion

		#region object ID
		#if DEBUG
		private long id;

		public override string ToString()
		{
			return "AConstructionSite " + id;
		}

		private static long LastId = 0;

		private static long NextId()
		{
			lock (typeof(AConstructionSite)) {
				LastId++;
				return LastId;
			}
		}
		#endif
		#endregion
 
	}
}
