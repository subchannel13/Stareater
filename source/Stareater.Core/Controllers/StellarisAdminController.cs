using System;
using System.Collections.Generic;
using Stareater.Controllers.Data;
using Stareater.Galaxy;

namespace Stareater.Controllers
{
	public class StellarisAdminController : AConstructionSiteController
	{
		private Game game;
		private StellarisAdmin stellaris; //TODO: make type
		
		internal StellarisAdminController(Game game, StellarisAdmin stellaris, bool readOnly): base(stellaris, readOnly, game)
		{ }
		
		public override IEnumerable<ConstructableItem> ConstructionQueue {
			get {
				//TODO
				//throw new NotImplementedException();
				yield break;
			}
		}
		
		public override IEnumerable<ConstructableItem> ConstructableItems {
			get {
				foreach(var item in Game.Players[Game.CurrentPlayer].Orders.Constructions[Site].Queue)
					yield return new ConstructableItem(item, Game.Derivates.Players.Of(Game.Players[Game.CurrentPlayer]));
			}
		}
	}
}
