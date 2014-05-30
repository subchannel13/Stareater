 
using System;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Ships 
{
	partial class Design 
	{
		public Player Owner { get; private set; }
		public string Name { get; private set; }
		public Hull Hull { get; private set; }
		public double Cost { get; private set; }

		public Design(Player owner, string name, Hull hull) 
		{
			this.Owner = owner;
			this.Name = name;
			this.Hull = hull;
			initCost(hull);
 
		} 


		internal Design Copy(PlayersRemap playersRemap)
		{
			return new Design(playersRemap.Players[this.Owner], this.Name, this.Hull);
		}
 
	}
}
