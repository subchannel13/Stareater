using System;
using System.Linq;
using Stareater.Galaxy;
using System.Collections.Generic;
using Stareater.Controllers.Views;
using Stareater.GameLogic;
using Stareater.GameData;
using Stareater.Utils;

namespace Stareater.Controllers
{
	public abstract class AConstructionSiteController
	{
		private const int NotOrder = -1;
		
		internal Game Game { get; private set; }
		internal AConstructionSite Site { get; private set; }
		
		private List<int> orderIndex = new List<int>();
		
		internal AConstructionSiteController(AConstructionSite site, bool readOnly, Game game)
		{
			this.Site = site;
			this.IsReadOnly = readOnly;
			this.Game = game;
		}

		public bool IsReadOnly { get; private set; }

		public SiteType SiteType
		{
			get { return Site.Type; }
		}

		internal abstract AConstructionSiteProcessor Processor { get; }
		
		#region Buildings
		protected abstract void RecalculateSpending();

		public double DesiredSpendingRatio
		{
			get
			{
				return this.Site.Owner.Orders.ConstructionPlans[this.Site].SpendingRatio;
			}
			set
			{
				this.Site.Owner.Orders.ConstructionPlans[this.Site].SpendingRatio = Methods.Clamp(value, 0, 1);
				this.RecalculateSpending();
			}
		}

		public IEnumerable<BuildingInfo> Buildings
		{
			get
			{
				return Site.Buildings
					.Select(x => new BuildingInfo(Game.Statics.Buildings[x.Key], x.Value))
					.OrderBy(x => x.Name);
			}
		}
			
		public virtual IEnumerable<ConstructableItem> ConstructableItems
		{
			get
			{
				var playerTechs = Game.States.TechnologyAdvances.Of(Game.CurrentPlayer);
				var techLevels = playerTechs.ToDictionary(x => x.Topic.IdCode, x => x.Level);
				var localEffencts = Processor.LocalEffects(Game.Statics).Get;

				foreach (var constructable in Game.Statics.Constructables)
					if (Prerequisite.AreSatisfied(constructable.Prerequisites, 0, techLevels) &&
						constructable.ConstructableAt == Site.Type &&
						constructable.Condition.Evaluate(localEffencts) > 0)
						yield return new ConstructableItem(constructable, Game.Derivates.Players.Of(Game.CurrentPlayer));
			}
		}
		
		public IEnumerable<ConstructableItem> ConstructionQueue
		{
			get
			{
				orderIndex.Clear();
				int orderI = 0;
				
				foreach(var item in Processor.SpendingPlan)
				{
					if (!item.Type.IsVirtual && item.Type == Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue[orderI])
					{
						orderIndex.Add(orderI);
						orderI++;
					}
					else
						orderIndex.Add(NotOrder);
					    
					yield return new ConstructableItem(
						item.Type, 
						Game.Derivates.Players.Of(Game.CurrentPlayer), 
						item.CompletedCount,
						Site.Stockpile.ContainsKey(item.Type) ? Site.Stockpile[item.Type] : 0,
						item.InvestedPoints + item.FromStockpile
					);
				}
			}
		}
		
		public bool CanPick(ConstructableItem data)
		{
			return Processor.SpendingPlan.All(x => x.Type.IdCode != data.IdCode);	//TODO(later): consider building count
		}
		
		public void Enqueue(ConstructableItem data)
		{
			if (IsReadOnly)
				return;
			
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.Add(data.Constructable);
			this.RecalculateSpending();
		}
		
		public void Dequeue(int index)
		{
			if (IsReadOnly || orderIndex[index] == NotOrder)
				return;
			
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.RemoveAt(orderIndex[index]);
			this.RecalculateSpending();
		}
		
		public void ReorderQueue(int fromIndex, int toIndex)
		{
			if (IsReadOnly || orderIndex[fromIndex] == NotOrder || orderIndex[toIndex] == NotOrder)
				return;
			
			var item = Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue[orderIndex[fromIndex]];
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.RemoveAt(orderIndex[fromIndex]);
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.Insert(orderIndex[toIndex], item);
			this.RecalculateSpending();
		}
		#endregion
		
		public StarData HostStar
		{
			get { return Site.Location.Star; }
		}
	}
}
