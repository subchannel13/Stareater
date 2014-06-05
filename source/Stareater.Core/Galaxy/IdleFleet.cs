 
using System;
using Stareater.GameData;
using Stareater.GameData.Databases.Tables;
using Stareater.Players;

namespace Stareater.Galaxy 
{
	partial class IdleFleet 
	{
		public Player Owner { get; private set; }
		public StarData Location { get; private set; }
		public ShipGroupCollection Ships { get; private set; }

		public IdleFleet(Player owner, StarData location) 
		{
			this.Owner = owner;
			this.Location = location;
			this.Ships = new ShipGroupCollection();
 
		} 

		private IdleFleet(IdleFleet original, PlayersRemap playersRemap, Player owner, StarData location) : this(owner, location) 
		{
			copyShips(original, playersRemap);
 
		}

		internal IdleFleet Copy(PlayersRemap playersRemap, GalaxyRemap galaxyRemap) 
		{
			return new IdleFleet(this, playersRemap, playersRemap.Players[this.Owner], galaxyRemap.Stars[this.Location]);
 
		} 
 
	}
}
