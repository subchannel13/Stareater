﻿using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Galaxy;
using Stareater.GameLogic;
using Stareater.Players;
using Stareater.GameData.Construction;

namespace Stareater.Controllers
{
	public class StellarisAdminController : AConstructionSiteController
	{
		internal StellarisAdminController(MainGame game, StellarisAdmin stellaris, bool readOnly, Player player) : 
			base(stellaris, readOnly, game, player)
		{ }

		internal override AConstructionSiteProcessor Processor
		{
			get { return Game.Derivates.Of((StellarisAdmin)Site); }
		}
		
		public override IEnumerable<TraitInfo> Traits 
		{ 
			get
			{
				return this.Location.Traits.Select(x => new TraitInfo(x.Type));
			}
		}
		
		#region Buildings
		protected override void RecalculateSpending()
		{
			this.Game.Derivates.Stellarises.At[Location].CalculateSpending(this.Game);
		}
		
		public override IEnumerable<ConstructableInfo> ConstructableItems 
		{
			get 
			{ 
				foreach(var item in base.ConstructableItems)
					yield return item;

				var localEffencts = this.Processor.LocalEffects(this.Game.Statics).UnionWith(this.Game.Derivates.Players.Of[this.Player].TechLevels).Get;
				foreach (var design in this.Game.States.Designs.OwnedBy[this.Player].Where(x => !x.IsVirtual && !x.IsObsolete))
                    yield return new ConstructableInfo(new ShipProject(design), localEffencts, null, 0);
			}
		}
		#endregion
		
		protected StarData Location 
		{
			get 
			{ 
				return (Site as StellarisAdmin).Location.Star;
			}
		}
		
		#region Colonies
		public double OrganisationAverage 
		{
			get 
			{ 
				var workplaces = Game.Derivates.Colonies.
					At[Location].
					Where(x => x.Owner == Site.Owner).
					Sum(x => x.Organization * x.Colony.Population);
				
				return workplaces / PopulationTotal; //FIXME(later): possible div by 0
			}
		}
		public double PopulationTotal
		{
			get 
			{ 
				return Game.States.Colonies.
					AtStar[Location].
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
					Of[Site as StellarisAdmin].
					SpendingPlan.Sum(x => x.InvestedPoints);
			}
		}
		
		public double DevelopmentTotal 
		{
			get 
			{ 
				return Game.Derivates.Colonies.
					At[Location].
					Where(x => x.Owner == Site.Owner).
					Sum(x => x.Development);
			}
		}
		#endregion
	}
}
