using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.Galaxy;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Databases.Tables
{
	partial class PlayerOrders
	{
		//TODO(later): move or remove
		public const double DefaultSiteSpendingRatio = 1;
		
		//TODO(later) make separate collections for colony and stellaris construction orders
		private Dictionary<AConstructionSite, ConstructionOrders> loadConstruction(IkadnBaseObject rawData, ObjectDeindexer deindexer)
		{
			var queue = new Dictionary<AConstructionSite, ConstructionOrders>();
			
			foreach(var plan in rawData.To<IEnumerable<IkonComposite>>()) {
				AConstructionSite site = plan.Tag.Equals(StellarisConstructionTag) ?
					(AConstructionSite)deindexer.Get<StellarisAdmin>(plan[LocationKey].To<int>()) :
				    (AConstructionSite)deindexer.Get<Colony>(plan[LocationKey].To<int>());
				                          
				queue.Add(site, ConstructionOrders.Load(plan[OrdersKey].To<IkonComposite>(), deindexer));
			}
				
			return queue;
		}
		
		private Dictionary<string, int> loadDevelopmet(IkadnBaseObject rawData, ObjectDeindexer deindexer)
		{
			var queue = new Dictionary<string, int>();
			
			foreach(var topic in rawData.To<IEnumerable<string>>())
				queue.Add(topic, queue.Count);
			
			return queue;
		}
		
		private IkadnBaseObject saveConstruction(ObjectIndexer indexer)
		{
			var queue = new IkonArray();
			
			foreach(var plan in this.ConstructionPlans) {
				IkonComposite planData;
				
				if (plan.Key.Location.Planet == null)
				{
					planData = new IkonComposite(StellarisConstructionTag);
					planData.Add(LocationKey, new IkonInteger(indexer.IndexOf((StellarisAdmin)plan.Key)));
				}
				else
				{
					planData = new IkonComposite(ColonyConstructionTag);
					planData.Add(LocationKey, new IkonInteger(indexer.IndexOf((Colony)plan.Key)));
				}
				
				planData.Add(OrdersKey, plan.Value.Save(indexer));
				queue.Add(planData);
			}
			
			return queue;
		}
		
		private IkadnBaseObject saveDevelopment()
		{
			var queue = new IkonArray();
			
			foreach(var techId in this.DevelopmentQueue.OrderBy(x => x.Value))
				queue.Add(new IkonText(techId.Key));
			
			return queue;
		}
		
		#region Saving keys
		private const string ColonyConstructionTag = "Colony";
		private const string StellarisConstructionTag = "Stellaris";
		private const string LocationKey = "id";
		private const string OrdersKey = "orders";
 		#endregion
	}
}
