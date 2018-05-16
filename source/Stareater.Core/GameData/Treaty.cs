using Stareater.Utils.StateEngine;
using Stareater.Players;
using Stareater.Utils;

namespace Stareater.GameData
{
	class Treaty 
	{
		[StateProperty]
		public Pair<Player> Parties { get; private set; }

		public Treaty(Pair<Player> parties) 
		{
			this.Parties = parties;
		} 

		private Treaty() 
		{ }
	}
}
