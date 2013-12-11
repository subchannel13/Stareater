using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Galaxy;
using Stareater.Players;
using Stareater.GameData;

namespace Stareater.GameLogic
{
	class StellarisProcessor
	{
		public StellarisAdmin Stellaris { get; set; }

		public StellarisProcessor(StellarisAdmin stellaris)
		{
			this.Stellaris = stellaris;
		}

		public Player Owner
		{
			get
			{
				return Stellaris.Owner;
			}
		}

		internal StellarisProcessor Copy(PlayersRemap playerRemap)
		{
			StellarisProcessor copy = new StellarisProcessor(playerRemap.Stellarises[this.Stellaris]);

			return copy;
		}
	}
}
