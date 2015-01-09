using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.Controllers.Data;
using Stareater.Galaxy;

namespace Stareater.Controllers
{
	public class FleetController
	{
		private Game game;
		private IdleFleet fleet;
		
		internal FleetController(IdleFleet fleet, Game game)
		{
			this.fleet = fleet;
			this.game = game;
		}
		
		public IEnumerable<ShipGroupInfo> ShipGroups
		{
			get
			{
				return this.fleet.Ships.Select(x => new ShipGroupInfo(x));
			}
		}
		
		public bool Valid
		{
			get { return this.game.States.IdleFleets.Contains(this.fleet); }
		}
	}
}
