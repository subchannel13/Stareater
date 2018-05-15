using Stareater.Utils.StateEngine;
using Stareater.Players;
using Stareater.Utils;

namespace Stareater.GameData
{
	class Treaty 
	{
		[StateProperty]
		public Pair<Player> Parties { get; private set; }

		public Treaty(Player party1, Player party2) 
		{
			this.Parties = new Pair<Player>(party1, party2);
		} 

		private Treaty() 
		{ }
	}
}
