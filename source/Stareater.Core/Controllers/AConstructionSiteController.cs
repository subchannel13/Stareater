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
		internal Game Game { get; private set; }
		internal AConstructionSite Site { get; private set; }
		
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
				var spendingPlan = Processor.SpendingPlan.ToDictionary(x => x.Item);
				foreach (var item in Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue)
					yield return new ConstructableItem(
						item, 
						Game.Derivates.Players.Of(Game.CurrentPlayer), 
						spendingPlan[item].CompletedCount,
						Site.Stockpile.ContainsKey(item) ? Site.Stockpile[item] : 0,
						spendingPlan[item].InvestedPoints
					);
			}
		}
		
		public bool CanPick(ConstructableItem data)
		{
			return ConstructionQueue.Where(x => x.IdCode == data.IdCode).Count() == 0;	//TODO(later): consider building count
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
			if (IsReadOnly)
				return;
			
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.RemoveAt(index);
			this.RecalculateSpending();
		}
		
		public void ReorderQueue(int fromIndex, int toIndex)
		{
			if (IsReadOnly)
				return;
			
			var item = Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue[fromIndex];
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.RemoveAt(fromIndex);
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.Insert(toIndex, item);
			this.RecalculateSpending();
		}
		#endregion
	}
}
