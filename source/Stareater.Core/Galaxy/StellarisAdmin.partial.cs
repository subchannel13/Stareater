using System;
using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;

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
