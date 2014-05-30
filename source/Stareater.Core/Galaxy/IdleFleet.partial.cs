using System;
using Stareater.GameData;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;

namespace Stareater.Galaxy
{
	partial class IdleFleet
	{
		/*public Player Owner { get; private set; }
		public StarData Location { get; private set; }
		
		public ShipGroupCollection Ships { get; private set; }
		
		public IdleFleet(Player owner, StarData location)
		{
			this.Owner = owner;
			this.Location = location;
			
			this.Ships = new ShipGroupCollection();
		}*/
		
		private void copyShips(IdleFleet original, PlayersRemap playersRemap)
		{
			foreach(var group in original.Ships)
				this.Ships.Add(group.Copy(playersRemap));
		}
	}
}
