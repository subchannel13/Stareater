 
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
		public IDictionary<string, double> Buildings { get; private set; }
		public IDictionary<Constructable, double> Stockpile { get; private set; }

		public AConstructionSite(LocationBody location, Player owner) : this() 
		{
			this.Location = location;
			this.Owner = owner;
			this.Buildings = new Dictionary<string, double>();
			this.Stockpile = new Dictionary<Constructable, double>();
 
		} 

		protected AConstructionSite(AConstructionSite original, LocationBody location, Player owner) : this(location, owner) 
		{
			copyBuildings(original);
			copyStockpile(original);
 
		}

 

		#region Saving
		public virtual IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(LocationKey, saveLocation(indexer));

			data.Add(OwnerKey, new IkonInteger(indexer.IndexOf(this.Owner)));

			data.Add(BuildingsKey, saveBuildings());

			data.Add(StockpileKey, saveStockpile());
			return data;
 
		}

		private const string TableTag = "AConstructionSite"; 
		private const string LocationKey = "location";
		private const string OwnerKey = "owner";
		private const string BuildingsKey = "buildings";
		private const string StockpileKey = "stockpile";
 
		#endregion
	}
}
