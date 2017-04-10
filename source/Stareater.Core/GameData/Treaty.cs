using System;
using Stareater.Players;

namespace Stareater.GameData
{
	class Treaty
	{
		public Player Party1 { get; private set; }
		public Player Party2 { get; private set; }
		
		public Treaty(Player party1, Player party2)
		{
			this.Party1 = party1;
			this.Party2 = party2;
		}
	}
}
