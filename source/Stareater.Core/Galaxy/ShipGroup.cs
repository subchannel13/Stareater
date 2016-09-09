 

using Ikadn.Ikon.Types;
using Stareater.Utils.Collections;
using System;
using Stareater.GameData;
using Stareater.Ships;

namespace Stareater.Galaxy 
{
	class ShipGroup 
	{
		public Design Design { get; private set; }
		public long Quantity { get; set; }
		public double Damage { get; set; }
		public double UpgradePoints { get; set; }

		public ShipGroup(Design design, long quantity, double damage, double upgradePoints) 
		{
			this.Design = design;
			this.Quantity = quantity;
			this.Damage = damage;
			this.UpgradePoints = upgradePoints;
 
			 
		} 


		private ShipGroup(IkonComposite rawData, ObjectDeindexer deindexer) 
		{
			var designSave = rawData[DesignKey];
			this.Design = deindexer.Get<Design>(designSave.To<int>());

			var quantitySave = rawData[QuantityKey];
			this.Quantity = quantitySave.To<long>();

			var damageSave = rawData[DamageKey];
			this.Damage = damageSave.To<double>();

			var upgradePointsSave = rawData[UpgradePointsKey];
			this.UpgradePoints = upgradePointsSave.To<double>();
 
			 
		}

		internal ShipGroup Copy(PlayersRemap playersRemap) 
		{
			return new ShipGroup(playersRemap.Designs[this.Design], this.Quantity, this.Damage, this.UpgradePoints);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(DesignKey, new IkonInteger(indexer.IndexOf(this.Design)));

			data.Add(QuantityKey, new IkonInteger(this.Quantity));

			data.Add(DamageKey, new IkonFloat(this.Damage));

			data.Add(UpgradePointsKey, new IkonFloat(this.UpgradePoints));
			return data;
 
		}

		public static ShipGroup Load(IkonComposite rawData, ObjectDeindexer deindexer)
		{
			var loadedData = new ShipGroup(rawData, deindexer);
			deindexer.Add(loadedData);
			return loadedData;
		}
 

		private const string TableTag = "ShipGroup";
		private const string DesignKey = "design";
		private const string QuantityKey = "quantity";
		private const string DamageKey = "damage";
		private const string UpgradePointsKey = "upgradePoints";
 
		#endregion

 
	}
}
