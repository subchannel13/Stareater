using System;
using System.Collections.Generic;
using System.Linq;

using Stareater.GameData;
using Stareater.Players;

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
		
		private void copyBuildings(AConstructionSite original)
		{
			foreach (var building in original.Buildings)
				this.Buildings.Add(building.Key, building.Value);
		}
		
		private void copyStockpile(AConstructionSite original)
		{
			foreach (var leftovers in original.Stockpile)
				this.Stockpile.Add(leftovers.Key, leftovers.Value);
		}
		
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
