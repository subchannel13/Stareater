using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.GameData;
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
				return Stellaris.Location;
			}
		}

		internal StellarisProcessor Copy(PlayersRemap playerRemap)
		{
			StellarisProcessor copy = new StellarisProcessor(playerRemap.Stellarises[this.Stellaris]);

			return copy;
		}
		
		public void CalculateBaseEffects()
		{
			/*
			 * TODO: Preprocess stars
			 * - Calculate system effects
			 */
			//TODO: Where to calculate stuff like migration?
		}

		public void CalculateSpending(
			PlayerProcessor playerProcessor, IEnumerable<ColonyProcessor> systemColonies)
		{
			var vars = new Var().UnionWith(playerProcessor.TechLevels).Get;

			//TODO: lift (to orbit) penalty
			double industryPotential = systemColonies.Sum(x =>
				x.SpendingRatioEffective *
				x.WorkingPopulation *
				x.BuilderEfficiency);
			double industryPoints = 
				Stellaris.Owner.Orders.ConstructionPlans[Stellaris].SpendingRatio *
				industryPotential;

			this.SpendingPlan = SimulateSpending(
				Stellaris,
				industryPoints,
				Stellaris.Owner.Orders.ConstructionPlans[Stellaris].Queue,
				vars
			);
			this.Production = this.SpendingPlan.Sum(x => x.InvestedPoints);

			this.SpendingRatioEffective = (industryPotential > 0) ?
				this.Production / industryPotential :
				0;

			foreach (var colonyProc in systemColonies)
				colonyProc.CalculateDevelopment(this.SpendingRatioEffective);
		}

		public override Var LocalEffects()
		{
			return new Var();
		}
	}
}
