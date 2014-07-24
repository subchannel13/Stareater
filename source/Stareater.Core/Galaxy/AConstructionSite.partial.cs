using System;
using System.Collections.Generic;
using System.Linq;

using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy
{
	abstract partial class AConstructionSite
	{
		private AConstructionSite()
		{
			#if DEBUG
			this.id = NextId();
			#endif
		}

		public abstract SiteType Type { get; }
		
		private void copyBuildings(AConstructionSite original)
		{
			foreach (var building in original.Buildings)
				this.Buildings.Add(building.Key, building.Value);
		}
		
		private void copyStockpile(AConstructionSite original)
		{
			foreach (var leftovers in original.Stockpile)
				this.Stockpile.Add(leftovers.Key, leftovers.Value);
		}
		
		private IkadnBaseObject saveBuildings()
		{
			var buildings = new IkonArray();
			
			foreach(var building in this.Buildings) {
				var buildingData = new IkonComposite(BuildingTag);
				buildingData.Add(BuildingTypeKey, new IkonText(building.Key));
				buildingData.Add(GeneralAmountKey, new IkonFloat(building.Value));
				buildings.Add(buildingData);
			}
			
			return buildings;
		}
		
		private IkadnBaseObject saveStockpile()
		{
			var stockpiles = new IkonArray();
			
			foreach(var stockpile in this.Stockpile) {
				var stockpileData = new IkonComposite(StockpileTag);
				stockpileData.Add(StockpileGroupKey, new IkonText(stockpile.Key.IdCode));
				stockpileData.Add(GeneralAmountKey, new IkonFloat(stockpile.Value));
				stockpiles.Add(stockpileData);
			}
			
			return stockpiles;
		}
		
		#region Saving keys
		private const string BuildingTag = "Building";
		private const string StockpileTag = "Stockpile";
		private const string BuildingTypeKey = "type";
		private const string GeneralAmountKey = "amount";
		private const string StockpileGroupKey = "group";
 		#endregion
		
		#region object ID
		#if DEBUG
		private long id;
		
		public override string ToString()
		{
			return "Construction site " + id;
		}

		private static long LastId = 0;

		private static long NextId()
		{
			lock (typeof(Colony)) {
				LastId++;
				return LastId;
			}
		}
		#endif
		#endregion
	}
}
