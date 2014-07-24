 
using Ikadn.Ikon.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Databases.Tables 
{
	partial class ConstructionOrders 
	{
		public double SpendingRatio { get; set; }
		public IList<Constructable> Queue { get; private set; }

		public ConstructionOrders(double spendingRatio) 
		{
			this.SpendingRatio = spendingRatio;
			this.Queue = new List<Constructable>();
 
		} 

		private ConstructionOrders(ConstructionOrders original) 
		{
			this.SpendingRatio = original.SpendingRatio;
			this.Queue = original.Queue.ToList();
 
		}

		internal ConstructionOrders Copy() 
		{
			return new ConstructionOrders(this);
 
		} 
 

		#region Saving
		public  IkonComposite Save(ObjectIndexer indexer) 
		{
			var data = new IkonComposite(TableTag);
			data.Add(SpendingRatioKey, new IkonFloat(this.SpendingRatio));

			var queueData = new IkonArray();
			foreach(var item in this.Queue)
				queueData.Add(new IkonText(item.IdCode));
			data.Add(QueueKey, queueData);
			return data;
 
		}

		private const string TableTag = "ConstructionOrders"; 
		private const string SpendingRatioKey = "spendingRatio";
		private const string QueueKey = "queue";
 
		#endregion
	}
}
