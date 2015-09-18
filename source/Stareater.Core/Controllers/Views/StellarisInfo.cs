using System;
using Stareater.Galaxy;

namespace Stareater.Controllers.Views
{
	public class StellarisInfo
	{
		internal StellarisAdmin Stellaris { get; private set; }
		
		public PlayerInfo Owner { get; private set; }
		
		internal StellarisInfo(StellarisAdmin stellaris)
		{
			this.Stellaris = stellaris;
			this.Owner = new PlayerInfo(stellaris.Owner);
		}
	}
}
