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
		internal Player Owner { get; private set; }
		internal IDictionary<Constructable, double> Stockpile;

		internal IDictionary<string, double> Buildings;
		
		protected AConstructionSite(Player owner)
		{
			this.Buildings = new Dictionary<string, double>();
			this.Stockpile = new Dictionary<Constructable, double>();

			this.Owner = owner;
			this.id = NextId();
		}
		
		protected AConstructionSite(AConstructionSite original, Player owner) : this(owner)
		{
			foreach (var building in original.Buildings)
				this.Buildings.Add(building.Key, building.Value);
			
			foreach (var leftovers in original.Stockpile)
				this.Stockpile.Add(leftovers.Key, leftovers.Value);
		}

		public abstract SiteType Type { get; }
		
		#region object ID
		//TODO: make debug only
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
		#endregion
	}
}
