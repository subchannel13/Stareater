using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using Stareater.GameLogic;

namespace Stareater.Controllers
{
	public class StellarisAdminController : AConstructionSiteController
	{
		internal StellarisAdminController(Game game, StellarisAdmin stellaris, bool readOnly): base(stellaris, readOnly, game)
		{ }

		internal override AConstructionSiteProcessor Processor
		{
			get { return Game.Derivates.Of((StellarisAdmin)Site); }
		}
		
		protected override void RecalculateSpending()
		{
			Game.Derivates.Stellarises.At(Location).CalculateSpending(
				Game.Derivates.Of(Site.Owner),
				Game.Derivates.Colonies.At(Location)
			);
		}
		
		protected StarData Location 
		{
			get 
			{ 
				return (Site as StellarisAdmin).Location;
			}
		}
		
		#region Colonies
		public double OrganisationAverage 
		{
			get 
			{ 
				var workplaces = Game.Derivates.Colonies.
					At(Location).
					Where(x => x.Owner == Site.Owner).
					Sum(x => x.Organization * x.Colony.Population);
				
				return workplaces / PopulationTotal; //FIXME: possible div by 0
			}
		}
		public double PopulationTotal
		{
			get 
			{ 
				return Game.States.Colonies.
					AtStar(Location).
					Where(x => x.Owner == Site.Owner).
					Sum(x => x.Population);
			}
		}
		#endregion
		
		#region Output
		public double IndustryTotal 
		{
			get 
			{ 
				return Game.Derivates.Stellarises.
					Of(Site as StellarisAdmin).
					SpendingPlan.Sum(x => x.InvestedPoints);
			}
		}
		
		public double DevelopmentTotal 
		{
			get 
			{ 
				return Game.Derivates.Colonies.
					At(Location).
					Where(x => x.Owner == Site.Owner).
					Sum(x => x.Development);
			}
		}
		
		public double Research 
		{
			get 
			{ 
				return 0; //TODO
			}
		}
		#endregion
	}
}
