using System;
using System.Linq;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;

namespace Stareater.Controllers
{
	public class ColonyController : AConstructionSiteController
	{
		internal ColonyController(Game game, Colony colony, bool readOnly, Player player) : 
			base(colony, readOnly, game, player)
		{ }

		internal override AConstructionSiteProcessor Processor
		{
			get { return Game.Derivates.Of((Colony)Site); }
		}
		
		protected override void RecalculateSpending()
		{
			var colony = Site as Colony;
			var playerProc = Game.Derivates.Of(Site.Owner);
			
			Game.Derivates.Of(colony).CalculateSpending(
				Game.Statics,
				playerProc
			);
			
			Game.Derivates.Stellarises.At(colony.Star).CalculateSpending(
				playerProc,
				Game.Derivates.Colonies.At(colony.Star)
			);
			
			Game.Derivates.Of(colony).CalculateDerivedEffects(
				Game.Statics,
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
				return 1; //TODO(v0.5): make processor property
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
				return 0; //TODO(v0.5): calculate from farming and gardening
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
	}
}
