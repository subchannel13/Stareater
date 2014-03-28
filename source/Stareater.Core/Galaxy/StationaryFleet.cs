using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Stareater.GameData.Databases.Tables;
using Stareater.Players;

namespace Stareater.Galaxy
{
	class StationaryFleet
	{
		public Player Owner { get; private set; }
		public StarData Location { get; private set; }
		
		public ShipGroupCollection Ships { get; private set; } //TODO(v0.5): make type
		private object mission; //TODO(v0.5): make type
		
		public StationaryFleet(Player owner, StarData location)
		{
			this.Owner = owner;
			this.Location = location;
			
			this.Ships = new ShipGroupCollection();
		}
	}
}
