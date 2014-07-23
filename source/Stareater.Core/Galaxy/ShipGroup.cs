 
using Ikadn.Ikon.Types;
using System;
using Stareater.GameData;
using Stareater.Ships;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy 
{
	class ShipGroup 
	{
		public Design Design { get; private set; }
		public long Quantity { get; set; }

		public ShipGroup(Design design, long quantity) 
		{
			this.Design = design;
			this.Quantity = quantity;
 
		} 


		internal ShipGroup Copy(PlayersRemap playersRemap) 
		{
			return new ShipGroup(playersRemap.Designs[this.Design], this.Quantity);
 
		} 
 

		#region Saving
		public IkonComposite Save(ObjectIndexer indexer) 
		{
			IkonComposite data = new IkonComposite(TableTag);
			
			data.Add(DesignKey, new IkonInteger(indexer.IndexOf(this.Design)));

			data.Add(QuantityKey, new IkonInteger(this.Quantity));
 

			return data;
		}

		private const string TableTag = "ShipGroup"; 
		private const string DesignKey = "design";
		private const string QuantityKey = "quantity";
 
		#endregion
	}
}
