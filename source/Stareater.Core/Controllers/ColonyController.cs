﻿using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Data;
using Stareater.Galaxy;
using Stareater.GameData;

namespace Stareater.Controllers
{
	public class ColonyController : AConstructionSiteController
	{
		private Game game;
		private Colony colony;
		
		internal ColonyController(Game game, Colony colony): base(colony)
		{ 
			this.colony = colony;
			this.game = game;
		}
		
		public override IEnumerable<ConstructableItem> ConstructableItems
		{
			get {
				var playerTechs = game.AdvancmentOrder(game.Players[game.CurrentPlayer]);
				var techLevels = playerTechs.ToDictionary(x => x.Topic.IdCode, x => x.Level);
				var colonyEffencts = game.Derivates.Colonies.Of(colony).Effects();
			
				foreach(var constructable in game.Statics.Constructables)
					if (Prerequisite.AreSatisfied(constructable.Prerequisites, 0, x => techLevels[x]) && constructable.Condition.Evaluate(colonyEffencts) > 0)
						yield return new ConstructableItem(constructable, game.Derivates.Players.Of(game.Players[game.CurrentPlayer]));
			}
		}
		
		public override IEnumerable<ConstructableItem> ConstructionQueue
		{
			get {
				foreach(var item in game.Players[game.CurrentPlayer].Orders.Constructions[colony].Queue)
					yield return new ConstructableItem(item, game.Derivates.Players.Of(game.Players[game.CurrentPlayer]));
			}
		}
	}
}
