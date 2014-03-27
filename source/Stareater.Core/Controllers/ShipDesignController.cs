using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.Controllers.Data;
using Stareater.Ships;

namespace Stareater.Controllers
{
	public class ShipDesignController
	{
		private Game game;
		
		internal ShipDesignController(Game game)
		{
			this.game = game;
		}
		
		private Dictionary<string, int> playersTechLevels()
		{
			return game.States.TechnologyAdvances.Of(game.CurrentPlayer)
				.ToDictionary(x => x.Topic.IdCode, x => x.Level);
		}
		
		#region Component lists
		public IEnumerable<HullInfo> Hulls()
		{
			var techLevels = playersTechLevels();
			
			return game.Statics.Hulls.Values.Select(x => new HullInfo(x, x.HighestLevel(techLevels)));
		}
		#endregion
		
		#region Designer actions
		public string Name { get; set; } //TODO(v0.5): check length and uniqueness
		public HullInfo Hull { get; set; } //TODO(v0.5): reset image index?
		public int ImageIndex { get; set; } //TODO(v0.5): check value
		
		public bool IsDesignValid
		{
			get
			{
				return Hull != null;
			}
		}
		
		public void Commit()
		{
			if (!IsDesignValid)
				return;
			
			game.States.Designs.Add(new Design(
				game.CurrentPlayer,
				Name,
				new Hull(Hull.HullType, Hull.Level, ImageIndex)
			)); //TODO(v0.5) add to changes DB and propagate to states during turn processing
		}
		#endregion
	}
}
