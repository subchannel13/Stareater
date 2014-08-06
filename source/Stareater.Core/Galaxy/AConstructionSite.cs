 

using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy 
{
	abstract partial class AConstructionSite 
	{
		public LocationBody Location { get; private set; }
		public Player Owner { get; private set; }
		public Dictionary<string, double> Buildings { get; private set; }
		public Dictionary<Constructable, double> Stockpile { get; private set; }

		public AConstructionSite(LocationBody location, Player owner) : this() 
		{
			this.Location = location;
			this.Owner = owner;
			this.Buildings = new Dictionary<string, double>();
			this.Stockpile = new Dictionary<Constructable, double>();
 
		} 

		protected AConstructionSite(AConstructionSite original, LocationBody location, Player owner) : this(location, owner) 
		{
			this.Buildings = new Dictionary<string, double>();
			foreach(var item in original.Buildings)
				this.Buildings.Add(item.Key, item.Value);
			this.Stockpile = new Dictionary<Constructable, double>();
			foreach(var item in original.Stockpile)
				this.Stockpile.Add(item.Key, item.Value);
 
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
	}
}
