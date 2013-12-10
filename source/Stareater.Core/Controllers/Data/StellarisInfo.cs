using System;
using Stareater.Galaxy;

namespace Stareater.Controllers.Data
{
	public class StellarisInfo
	{
		public PlayerInfo Owner { get; private set; }
		
		internal StellarisInfo(StellarisAdmin stellaris)
		{
			this.Owner = new PlayerInfo(stellaris.Owner);
		}
	}
}
