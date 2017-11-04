using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.AppData.Expressions;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using Stareater.GameData.Construction;

namespace Stareater.GameLogic
{
	class StellarisProcessor : AConstructionSiteProcessor
	{
		[StateProperty]
		public StellarisAdmin Stellaris { get; set; }

		public StellarisProcessor(StellarisAdmin stellaris) : base()
		{
			this.Stellaris = stellaris;
		}

		private StellarisProcessor(StellarisAdmin stellaris, StellarisProcessor original) : base(original)
		{
			this.Stellaris = stellaris;
		}

        private StellarisProcessor()
        { }
		
		public Player Owner
		{
			get
			{
				return Stellaris.Owner;
			}
		}

		public StarData Location
		{
			get
			{
				return Stellaris.Location.Star;
			}
		}
		
		public void CalculateBaseEffects()
		{
			/*
			 * TODO(v0.7) Preprocess stars
			 * - Calculate system effects
			 */
			//TODO(v0.7) Where to calculate stuff like migration?
		}

		public void CalculateSpending(MainGame game, PlayerProcessor playerProcessor, IEnumerable<ColonyProcessor> systemColonies)
		{
			var vars = new Var().UnionWith(playerProcessor.TechLevels).Get;

			double industryPotential = systemColonies.Sum(x =>
				(1 - x.SpendingRatioEffective) *
				x.WorkingPopulation *
				x.BuilderEfficiency *
				x.SpaceliftFactor
			);
			double industryPoints = 
				game.Orders[Stellaris.Owner].ConstructionPlans[Stellaris].SpendingRatio *
				industryPotential;

			var normalQueue = game.Orders[Stellaris.Owner].ConstructionPlans[Stellaris].Queue;
			this.SpendingPlan = SimulateSpending(
				Stellaris,
				industryPoints,
				colonizationQueue(game, playerProcessor).Concat(normalQueue),
				vars
			);
			this.Production = this.SpendingPlan.Sum(x => x.InvestedPoints);

			this.SpendingRatioEffective = (industryPotential > 0) ?
				this.Production / industryPotential :
				0;

			foreach (var colonyProc in systemColonies)
				colonyProc.CalculateDevelopment(this.SpendingRatioEffective);
		}

		protected override AConstructionSite Site 
		{
			get 
			{
				return Stellaris;
			}
		}
		
		private IEnumerable<IConstructionProject> colonizationQueue(MainGame game, PlayerProcessor playerProcessor)
		{
			foreach(var plan in game.Orders[this.Site.Owner].ColonizationOrders.Values)
				if (plan.Sources.Contains(this.Site.Location.Star))
				{
					var colonizer = (plan.Destination.Star == this.Site.Location.Star) ?
						playerProcessor.SystemColonizerDesign :
						playerProcessor.ColonyShipDesign;

					yield return new ColonizerProject(colonizer, plan);
				}
		}
	}
}
