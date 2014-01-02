using System;
using System.Collections.Generic;
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
			var stellaris = Site as StellarisAdmin;
			
			Game.Derivates.Stellarises.At(stellaris.Location).CalculateSpending(
				Game.Derivates.Of(Site.Owner),
				Game.Derivates.Colonies.At(stellaris.Location)
			);
		}
	}
}
