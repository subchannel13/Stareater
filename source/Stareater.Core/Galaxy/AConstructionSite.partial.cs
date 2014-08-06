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
		private AConstructionSite()
		{
			#if DEBUG
			this.id = NextId();
			#endif
		}

		public abstract SiteType Type { get; }
		
		#region object ID
		#if DEBUG
		private long id;
		
		public override string ToString()
		{
			return "Construction site " + id;
		}

		private static long LastId = 0;

		private static long NextId()
		{
			lock (typeof(Colony)) {
				LastId++;
				return LastId;
			}
		}
		#endif
		#endregion
	}
}
