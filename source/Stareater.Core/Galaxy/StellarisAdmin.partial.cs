using System;
using Stareater.Players;
using Stareater.GameData;

namespace Stareater.Galaxy
{
	partial class StellarisAdmin : AConstructionSite
	{
		public override SiteType Type
		{
			get { return SiteType.StarSystem; }
		}
	}
}
