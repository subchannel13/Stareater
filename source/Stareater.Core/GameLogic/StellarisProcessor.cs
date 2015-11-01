using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Stareater.AppData.Expressions;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameData.Databases;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.GameLogic
{
	class StellarisProcessor : AConstructionSiteProcessor
	{
		public StellarisAdmin Stellaris { get; set; }

		public StellarisProcessor(StellarisAdmin stellaris) : base()
		{
			this.Stellaris = stellaris;
		}

		private StellarisProcessor(StellarisAdmin stellaris, StellarisProcessor original) : base(original)
		{
			this.Stellaris = stellaris;
		}
		
		internal StellarisProcessor Copy(PlayersRemap playerRemap)
		{
			return new StellarisProcessor(playerRemap.Stellarises[this.Stellaris], this);
		}
		
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
			 * TODO(v0.5): Preprocess stars
			 * - Calculate system effects
			 */
			//TODO(v0.5): Where to calculate stuff like migration?
		}

		public void CalculateSpending(
			PlayerProcessor playerProcessor, IEnumerable<ColonyProcessor> systemColonies)
		{
			var vars = new Var().UnionWith(playerProcessor.TechLevels).Get;

			//TODO(v0.5): lift (to orbit) penalty
			double industryPotential = systemColonies.Sum(x =>
				(1 - x.SpendingRatioEffective) *
				x.WorkingPopulation *
				x.BuilderEfficiency);
			double industryPoints = 
				Stellaris.Owner.Orders.ConstructionPlans[Stellaris].SpendingRatio *
				industryPotential;

			var normalQueue = Stellaris.Owner.Orders.ConstructionPlans[Stellaris].Queue;
			this.SpendingPlan = SimulateSpending(
				Stellaris,
				industryPoints,
				colonizationQueue(playerProcessor).Concat(normalQueue),
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
		
		private IEnumerable<Constructable> colonizationQueue(PlayerProcessor playerProcessor)
		{
			foreach(var plan in this.Site.Owner.Orders.ColonizationOrders.Values)
				if (plan.Sources.Contains(this.Site.Location.Star))
				{
					var colonizer = (plan.Destination.Star == this.Site.Location.Star) ?
						playerProcessor.SystemColonizerDesign :
						playerProcessor.ColonyShipDesign;
					
					yield return new Constructable(
						colonizer.Name, "", true, colonizer.ImagePath, colonizer.IdCode, 
						new Prerequisite[0], SiteType.StarSystem, true,
						new Formula(true), new Formula(colonizer.Cost), new Formula(double.PositiveInfinity), 
						new IConstructionEffect[] { new ConstructionAddColonizer(colonizer, plan.Destination) }
					);
				}
		}
	}
}
