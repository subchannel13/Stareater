using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using Stareater.GameData;
using Stareater.GameLogic;

namespace Stareater.Controllers
{
	public class ColonyController : AConstructionSiteController
	{
		internal ColonyController(Game game, Colony colony, bool readOnly) : 
			base(colony, readOnly, game)
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
		public double PlanetEnvironment 
		{
			get 
			{ 
				return 1; //TODO: make processor property
			}
		}
		
		public double PlanetSize 
		{
			get 
			{ 
				return (Site as Colony).Location.Size;
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
				return 0; //TODO: calculate from farming and gardening
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
