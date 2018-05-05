using System.Collections.Generic;
using System.Linq;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.Utils.Collections;
using Stareater.Utils.StateEngine;
using Stareater.GameData.Construction;
using System;
using Stareater.GameData.Databases;

namespace Stareater.GameLogic
{
	class StellarisProcessor : AConstructionSiteProcessor
	{
		[StateProperty]
		public StellarisAdmin Stellaris { get; set; }

		[StateProperty]
		public Dictionary<Colony, double> EmigrantionPlan = new Dictionary<Colony, double>();

		[StateProperty]
		public Dictionary<Colony, double> ImmigrantionPlan = new Dictionary<Colony, double>();

		public StellarisProcessor(StellarisAdmin stellaris) : base()
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
		}

		public void CalculateDerivedEffects(MainGame game)
		{
			var systemColonies = this.systemColonies(game).ToList();
			var destinations = new PendableSet<ColonyProcessor>(systemColonies.Where(x => x.Colony.Population < x.MaxPopulation));
			var plans = destinations.ToDictionary(x => x, x => 0.0);
			var immigrants = systemColonies.Sum(x => x.Emigrants);

			if (immigrants <= 0)
			{
				this.ImmigrantionPlan = new Dictionary<Colony, double>();
				this.EmigrantionPlan = new Dictionary<Colony, double>();
				return;
			}

			while(destinations.Count > 0 && immigrants > 0)
			{
				var weightSum = plans.Keys.Sum(x => x.Desirability);
				foreach (var site in destinations)
				{
					var colonyImmigrants = immigrants * site.Desirability / weightSum;
					var maxImmigrants = site.MaxPopulation - site.Colony.Population;
					if (colonyImmigrants > maxImmigrants)
					{
						colonyImmigrants = maxImmigrants;
						destinations.PendRemove(site);
					}

					plans[site] += colonyImmigrants;
					immigrants -= colonyImmigrants;
					weightSum -= 1;
				}
				destinations.ApplyPending();
			}

			this.ImmigrantionPlan = plans.ToDictionary(x => x.Key.Colony, x => x.Value);

			var emigrationPortion = 1 - immigrants / systemColonies.Sum(x => x.Emigrants);
			this.EmigrantionPlan = systemColonies.ToDictionary(x => x.Colony, x => x.Emigrants * emigrationPortion);
		}

		public void CalculateSpending(MainGame game)
		{
			var playerProcessor = game.Derivates.Of(this.Owner);
			var vars = new Var().UnionWith(playerProcessor.TechLevels).Get;
			var colonies = this.systemColonies(game).ToList();

			double industryPotential = colonies.Sum(x =>
				(1 - x.SpendingRatioEffective) *
				(1 - playerProcessor.MaintenanceRatio) * 
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

			foreach (var colonyProc in colonies)
				colonyProc.CalculateDevelopment(this.SpendingRatioEffective, playerProcessor.MaintenanceRatio);
		}

		protected override AConstructionSite Site 
		{
			get 
			{
				return Stellaris;
			}
		}

		public override void ProcessPrecombat(MainGame game)
		{
			base.ProcessPrecombat(game);

			foreach (var plan in this.EmigrantionPlan)
				plan.Key.Population -= plan.Value;
			foreach (var plan in this.ImmigrantionPlan)
				plan.Key.Population += plan.Value;
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

		private IEnumerable<ColonyProcessor> systemColonies(MainGame game)
		{
			return game.Derivates.Colonies.At[this.Location].Where(x => x.Owner == this.Owner);
		}
	}
}
