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
				Game.Statics.ColonyFormulas,
				playerProc
			);
			
			Game.Derivates.Stellarises.At(colony.Star).CalculateSpending(
				playerProc,
				Game.Derivates.Colonies.At(colony.Star)
			);
		}
	}
}
