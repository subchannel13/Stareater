using System;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class ContactInfo
	{
		internal Player PlayerData { get; private set; }

		internal ContactInfo(Player player)
		{
			this.PlayerData = player;
		}

		public PlayerInfo Player
		{
			get { return new PlayerInfo(this.PlayerData); }
		}
	}
}
