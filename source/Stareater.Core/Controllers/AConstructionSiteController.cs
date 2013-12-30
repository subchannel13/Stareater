using System;
using System.Linq;
using Stareater.Galaxy;
using System.Collections.Generic;
using Stareater.Controllers.Data;
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

		internal abstract AConstructionSiteProcessor Processor { get; }

		public double DesiredSpendingRatio
		{
			get
			{
				return Site.Owner.Orders.ConstructionPlans[Site].SpendingRatio;
			}
			set
			{
				Site.Owner.Orders.ConstructionPlans[Site].SpendingRatio = Methods.Clamp(value, 0, 1);
			}
		}

		public IEnumerable<ConstructableItem> ConstructableItems
		{
			get
			{
				var playerTechs = Game.States.TechnologyAdvances.Of(Game.CurrentPlayer);
				var techLevels = playerTechs.ToDictionary(x => x.Topic.IdCode, x => x.Level);
				var localEffencts = Processor.LocalEffects().Get;

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
				foreach (var item in Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue)
					yield return new ConstructableItem(item, Game.Derivates.Players.Of(Game.CurrentPlayer));
			}
		}
		
		public bool CanPick(ConstructableItem data)
		{
			return ConstructionQueue.Where(x => x.IdCode == data.IdCode).Count() == 0;	//TODO: consider building count
		}
		
		public void Enqueue(ConstructableItem data)
		{
			if (IsReadOnly)
				return;
			
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.Add(data.Constructable);
		}
		
		public void Dequeue(int index)
		{
			if (IsReadOnly)
				return;
			
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.RemoveAt(index);
		}
		
		public void ReorderQueue(int fromIndex, int toIndex)
		{
			if (IsReadOnly)
				return;
			
			var item = Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue[fromIndex];
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.RemoveAt(fromIndex);
			Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue.Insert(toIndex, item);
		}
	}
}
