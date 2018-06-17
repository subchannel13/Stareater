using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class ColonyController : AConstructionSiteController
	{
		internal ColonyController(MainGame game, Colony colony, bool readOnly, Player player) :
			base(colony, readOnly, game, player)
		{ }

		internal override AConstructionSiteProcessor Processor
		{
			get { return Game.Derivates.Of((Colony)Site); }
		}

		public override IEnumerable<TraitInfo> Traits
		{
			get
			{
				return (this.Site as Colony).Location.Planet.Traits.Select(x => new TraitInfo(x.Type));
			}
		}

		public override double DesiredSpendingRatio
		{
			get
			{
				return base.DesiredSpendingRatio;
			}
			set
			{
				//no operation
			}
		}

		protected override void recalculateSpending()
		{
			var colony = Site as Colony;
			var playerProc = this.Game.Derivates.Of(Site.Owner);

			this.Game.Derivates.Of(colony).CalculateSpending(
				this.Game,
				playerProc
			);

			this.Game.Derivates.Stellarises.At[colony.Star].CalculateSpending(this.Game);

			this.Game.Derivates.Of(colony).CalculateDerivedEffects(
				this.Game.Statics,
				playerProc
			);
		}

		#region Population
		public double Organization
		{
			get
			{
				return Game.Derivates.Of(Site as Colony).Organization;
			}
		}

		public double Population
		{
			get
			{
				return (Site as Colony).Population;
			}
		}

		public double PopulationGrowth
		{
			get
			{
				return Game.Derivates.Of(Site as Colony).PopulationGrowth;
			}
		}

		public double PopulationMax
		{
			get
			{
				return Game.Derivates.Of(Site as Colony).MaxPopulation;
			}
		}
		#endregion

		#region Planet
		public Planet PlanetBody
		{
			get { return Site.Location.Planet; }
		}

		public double PlanetEnvironment
		{
			get
			{
				return Game.Derivates.Of(Site as Colony).Environment;
			}
		}

		public double PlanetSize
		{
			get
			{
				return (Site as Colony).Location.Planet.Size;
			}
		}
		#endregion

		#region Productivity
		public double DevelopmentPerPop
		{
			get
			{
				return Game.Derivates.Of(Site as Colony).ScientistEfficiency;
			}
		}

		public double DevelopmentTotal
		{
			get
			{
				return Game.Derivates.Of(Site as Colony).Development;
			}
		}

		public double FoodPerPop
		{
			get
			{
				var colonyStats = this.Game.Derivates.Of(Site as Colony);

				return
					(colonyStats.FarmerEfficiency * colonyStats.Farmers + colonyStats.GardenerEfficiency * colonyStats.Gardeners) /
					(colonyStats.Farmers + colonyStats.Gardeners);
			}
		}

		public double IndustryPerPop
		{
			get
			{
				return Game.Derivates.Of(Site as Colony).BuilderEfficiency;
			}
		}

		public double IndustryTotal
		{
			get
			{
				return Game.Derivates.Of(Site as Colony).SpendingPlan.Sum(x => x.InvestedPoints);
			}
		}

		public double OrePerPop
		{
			get
			{
				return Game.Derivates.Of(Site as Colony).MinerEfficiency;
			}
		}
		#endregion

		public override PolicyInfo Policy
		{
			get
			{
				return new PolicyInfo(
					this.Game.Orders[this.Site.Owner].
					Policies[this.Game.States.Stellarises.At[this.Site.Location.Star].First(x => x.Owner == this.Site.Owner)]
				);
			}

			set
			{
				if (this.IsReadOnly)
					return;

				var stellaris = this.Game.States.Stellarises.At[this.Site.Location.Star].First(x => x.Owner == this.Site.Owner);

				this.Game.Derivates.Of(stellaris).UndoPolicy(this.Game);
				this.Game.Orders[this.Site.Owner].Policies[stellaris] = value.Data;
				this.Game.Derivates.Of(stellaris).ApplyPolicy(this.Game, value.Data);
				this.recalculateSpending();
			}
		}
	}
}
