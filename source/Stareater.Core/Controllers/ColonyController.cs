using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using Stareater.GameData;

namespace Stareater.Controllers
{
	public class ColonyController : AConstructionSiteController
	{
		internal ColonyController(Game game, Colony colony, bool readOnly) : 
			base(colony, readOnly, game)
		{ }
		
		public override IEnumerable<ConstructableItem> ConstructableItems
		{
			get {
				var playerTechs = Game.States.TechnologyAdvances.Of(Game.CurrentPlayer);
				var techLevels = playerTechs.ToDictionary(x => x.Topic.IdCode, x => x.Level);
				var colonyEffencts = Game.Derivates.Colonies.Of((Colony)Site).Effects();
			
				foreach(var constructable in Game.Statics.Constructables)
					if (Prerequisite.AreSatisfied(constructable.Prerequisites, 0, techLevels) && constructable.Condition.Evaluate(colonyEffencts) > 0)
						yield return new ConstructableItem(constructable, Game.Derivates.Players.Of(Game.CurrentPlayer));
			}
		}
		
		public override IEnumerable<ConstructableItem> ConstructionQueue
		{
			get {
				foreach(var item in Game.CurrentPlayer.Orders.ConstructionPlans[Site].Queue)
					yield return new ConstructableItem(item, Game.Derivates.Players.Of(Game.CurrentPlayer));
			}
		}
	}
}
