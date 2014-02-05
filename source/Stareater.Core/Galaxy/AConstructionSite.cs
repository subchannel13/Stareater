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

		private IEnumerable<object> buildings; //TODO: make type
		
		private long id;
		
		protected AConstructionSite(Player owner)
		{
			this.Stockpile = new Dictionary<Constructable, double>();

			this.Owner = owner;
			this.id = NextId();
		}

		public abstract SiteType Type { get; }
		
		#region object ID
		//TODO: make debug only
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
