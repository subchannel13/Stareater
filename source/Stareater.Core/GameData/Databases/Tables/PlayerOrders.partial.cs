using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn;
using Ikadn.Ikon.Types;
using NGenerics.DataStructures.Mathematical;
using Stareater.Galaxy;
using Stareater.Ships;
using Stareater.Ships.Missions;
using Stareater.Utils.Collections;

namespace Stareater.GameData.Databases.Tables
{
	partial class PlayerOrders
	{
		//TODO(later): move or remove
		public const double DefaultSiteSpendingRatio = 1;
		
		private HashSet<Fleet> copyFleetRegroup(IEnumerable<Fleet> original, PlayersRemap playersRemap)
		{
			return new HashSet<Fleet>(original.Select(x => playersRemap.Fleets[x]));
		}
		
		private Design copyRefitTo(Design original, PlayersRemap playersRemap)
		{
			return (original == null) ? null : playersRemap.Designs[original];
		}
			
		private Dictionary<Planet, ColonizationPlan> loadColonizationOrders(IkadnBaseObject rawData, ObjectDeindexer deindexer)
		{
			var orders = new Dictionary<Planet, ColonizationPlan>();
			
			foreach(var orderData in rawData.To<IEnumerable<IkonComposite>>()) {
				var destination = deindexer.Get<Planet>(orderData[ColonizationDestinationTag].To<int>());
				var plan = new ColonizationPlan(destination);
				
				foreach(var sourceIndex in orderData[ColonizationSourcesTag].To<IEnumerable<int>>())
					plan.Sources.Add(deindexer.Get<StarData>(sourceIndex));
				
				orders.Add(destination, plan);
			}
				
			return orders;
		}
		
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
		
		private Dictionary<Vector2D, HashSet<Fleet>> loadShipOrders(IkadnBaseObject rawData, ObjectDeindexer deindexer)
		{
			var orders = new Dictionary<Vector2D, HashSet<Fleet>>();
			
			foreach(var orderData in rawData.To<IEnumerable<IkonComposite>>()) {
				var rawPosition = orderData[Position].To<double[]>();
				var fleets = new HashSet<Fleet>();
				
				foreach(var fleetData in orderData[Fleets].To<IEnumerable<IkonComposite>>())
					fleets.Add(Fleet.Load(fleetData, deindexer));
				
				orders.Add(new Vector2D(rawPosition[0], rawPosition[1]), fleets);
			}
				
			return orders;
		}
		
		private Dictionary<Design, Design> loadRefitOrders(IkadnBaseObject rawData, ObjectDeindexer deindexer)
		{
			var orders = new Dictionary<Design, Design>();
			
			foreach(var orderData in rawData.To<IEnumerable<IkonComposite>>())
				orders.Add(
					deindexer.Get<Design>(orderData[FromDesignKey].To<int>()),
					deindexer.Get<Design>(orderData[ToDesignKey].To<int>())
				);
				
			return orders;
		}
		
		private IkadnBaseObject saveColonizationOrders(ObjectIndexer indexer)
		{
			var queue = new IkonArray();
			
			foreach(var order in this.ColonizationOrders) {
				IkonComposite orderData;
				
				orderData = new IkonComposite(ColonizationOrdersTag);
				orderData.Add(ColonizationDestinationTag, new IkonInteger(indexer.IndexOf(order.Value.Destination)));
				orderData.Add(ColonizationSourcesTag, new IkonArray(order.Value.Sources.Select(x => new IkonInteger(indexer.IndexOf(x)))));
				queue.Add(orderData);
			}
			
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
		
		private IkadnBaseObject saveShipOrders(ObjectIndexer indexer)
		{
			var queue = new IkonArray();
			
			foreach(var order in this.ShipOrders) {
				var orderData = new IkonComposite(ShipOrderTag);
				
				orderData.Add(Position, new IkonArray().
				              Add(new IkonFloat(order.Key.X)).
				              Add(new IkonFloat(order.Key.Y))
				);
				
				orderData.Add(Fleets, new IkonArray(order.Value.Select(x => x.Save(indexer))));
				queue.Add(orderData);
			}
			
			return queue;
		}
		
		private IkadnBaseObject saveRefitOrders(ObjectIndexer indexer)
		{
			var list = new IkonArray();
			
			foreach(var order in this.RefitOrders)
				if (order.Value == null)
					list.Add(new IkonComposite(DisbandOrderTag));
				else
				{
					var orderData = new IkonComposite(RefitOrderTag);
					orderData.Add(FromDesignKey, new IkonInteger(indexer.IndexOf(order.Key)));
					orderData.Add(ToDesignKey, new IkonInteger(indexer.IndexOf(order.Value)));
					
					list.Add(orderData);
				}
			
			return list;
		}
		
		#region Saving keys
		private const string ColonyConstructionTag = "Colony";
		private const string DisbandOrderTag = "Disband";
		private const string RefitOrderTag = "Refit";
		private const string ShipOrderTag = "Order";
		private const string StellarisConstructionTag = "Stellaris";
		private const string ColonizationDestinationTag = "destination";
		private const string ColonizationSourcesTag = "sources";
		private const string IdKey = "id";
		private const string LocationKey = "id";
		private const string OrdersKey = "orders";
 		#endregion
	}
}
