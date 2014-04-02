using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Players;
using Stareater.GameData;

namespace Stareater.Galaxy
{
	abstract class AConstructionSite
	{
		internal LocationBody Location { get; private set; }
		internal Player Owner { get; private set; }
		
		internal IDictionary<string, double> Buildings;
		internal IDictionary<Constructable, double> Stockpile;
		
		protected AConstructionSite(Player owner, LocationBody location)
		{
			this.Buildings = new Dictionary<string, double>();
			this.Stockpile = new Dictionary<Constructable, double>();

			this.Location = location;
			this.Owner = owner;
			
			#if DEBUG
			this.id = NextId();
			#endif
		}
		
		protected AConstructionSite(AConstructionSite original, Player owner, LocationBody location) : this(owner, location)
		{
			foreach (var building in original.Buildings)
				this.Buildings.Add(building.Key, building.Value);
			
			foreach (var leftovers in original.Stockpile)
				this.Stockpile.Add(leftovers.Key, leftovers.Value);
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
