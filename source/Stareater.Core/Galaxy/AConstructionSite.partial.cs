using System;
using System.Collections.Generic;
using System.Linq;

using Ikadn;
using Ikadn.Ikon.Types;
using Stareater.GameData;
using Stareater.Players;
using Stareater.Utils.Collections;

namespace Stareater.Galaxy
{
	abstract partial class AConstructionSite
	{
		public abstract SiteType Type { get; }
	}
}
