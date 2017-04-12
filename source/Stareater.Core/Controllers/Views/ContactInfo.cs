using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.GameData;
using Stareater.Players;

namespace Stareater.Controllers.Views
{
	public class ContactInfo
	{
		internal Player PlayerData { get; private set; }

		internal ContactInfo(Player player, IEnumerable<Treaty> treaties)
		{
			this.PlayerData = player;
			this.Treaties = treaties.Select(x => new TreatyInfo());
		}

		public PlayerInfo Player
		{
			get { return new PlayerInfo(this.PlayerData); }
		}

		public IEnumerable<TreatyInfo> Treaties { get; private set; }
	}
}
