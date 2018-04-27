using System;
using System.Linq;
using Stareater.Galaxy;
using System.Collections.Generic;
using Stareater.Controllers.Views;
using Stareater.GameLogic;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils;
using Stareater.GameData.Construction;

namespace Stareater.Controllers
{
	public abstract class AConstructionSiteController
	{
		private const int NotOrder = -1;
		
		internal AConstructionSite Site { get; private set; }
		
		internal MainGame Game { get; private set; }
		internal Player Player { get; private set; }
		
		private List<int> orderIndex = new List<int>();
		
		internal AConstructionSiteController(AConstructionSite site, bool readOnly, MainGame game, Player player)
		{
			this.Site = site;
			this.IsReadOnly = readOnly;
			this.Game = game;
			this.Player = player;
		}

		public bool IsReadOnly { get; private set; }

		public SiteType SiteType
		{
			get { return Site.Type; }
		}

		internal abstract AConstructionSiteProcessor Processor { get; }
		
		public abstract IEnumerable<TraitInfo> Traits { get; }
		
		#region Buildings
		protected abstract void RecalculateSpending();

		public double DesiredSpendingRatio
		{
			get
			{
				return this.Game.Orders[this.Site.Owner].ConstructionPlans[this.Site].SpendingRatio;
			}
			set
			{
				this.Game.Orders[this.Site.Owner].ConstructionPlans[this.Site].SpendingRatio = Methods.Clamp(value, 0, 1);
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
			
		public virtual IEnumerable<ConstructableInfo> ConstructableItems
		{
			get
			{
				var playerTechs = this.Game.States.DevelopmentAdvances.Of[this.Player].ToDictionary(x => x.Topic.IdCode, x => (double)x.Level);
				var techLevels = this.Game.Derivates.Of(this.Player).TechLevels;
				var localEffencts = this.Processor.LocalEffects(this.Game.Statics).UnionWith(techLevels).Get;

				foreach (var constructable in this.Game.Statics.Constructables)
					if (Prerequisite.AreSatisfied(constructable.Prerequisites, 0, playerTechs) &&
						constructable.ConstructableAt == Site.Type &&
						constructable.Condition.Evaluate(localEffencts) >= 0)
						yield return new ConstructableInfo(new StaticProject(constructable), localEffencts, null, 0);
			}
		}
		
		public IEnumerable<ConstructableInfo> ConstructionQueue
		{
			get
			{
				orderIndex.Clear();
				int orderI = 0;
				
				var playerProcessor = this.Game.Derivates.Of(this.Player);
				var vars = this.Processor.LocalEffects(this.Game.Statics).UnionWith(playerProcessor.TechLevels).Get;
					
				foreach(var item in Processor.SpendingPlan)
				{
					if (!item.Project.IsVirtual && item.Project == this.Game.Orders[this.Player].ConstructionPlans[Site].Queue[orderI])
					{
						orderIndex.Add(orderI);
						orderI++;
					}
					else
						orderIndex.Add(NotOrder);
					
					yield return new ConstructableInfo(
						item.Project,
						vars,
						item,
						item.LeftoverPoints
					);
				}
			}
		}
		
		public bool CanPick(ConstructableInfo data)
		{
			return Processor.SpendingPlan.All(x => !x.Project.Equals(data.Project)); //TODO(v0.7): consider building count
		}
		
		public void Enqueue(ConstructableInfo data)
		{
			if (IsReadOnly)
				return;

			this.Game.Orders[this.Player].ConstructionPlans[Site].Queue.Add(data.Project);
			this.RecalculateSpending();
		}
		
		public void Dequeue(int index)
		{
			if (IsReadOnly || orderIndex[index] == NotOrder)
				return;

			this.Game.Orders[this.Player].ConstructionPlans[Site].Queue.RemoveAt(orderIndex[index]);
			this.RecalculateSpending();
		}
		
		public void ReorderQueue(int fromIndex, int toIndex)
		{
			if (IsReadOnly || orderIndex[fromIndex] == NotOrder || orderIndex[toIndex] == NotOrder)
				return;
			
			var item = this.Game.Orders[this.Player].ConstructionPlans[Site].Queue[orderIndex[fromIndex]];
			this.Game.Orders[this.Player].ConstructionPlans[Site].Queue.RemoveAt(orderIndex[fromIndex]);
			this.Game.Orders[this.Player].ConstructionPlans[Site].Queue.Insert(orderIndex[toIndex], item);
			this.RecalculateSpending();
		}
		#endregion
		
		public StarData HostStar
		{
			get { return Site.Location.Star; }
		}
	}
}
