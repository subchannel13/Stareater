using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Players;

namespace Stareater.Galaxy
{
	abstract class AConstructionSite
	{
		internal Player Owner { get; private set; }
		
		private IEnumerable<object> buildings; //TODO: make type
		private object leftovers; //TODO: make type

		private long id;
		
		protected AConstructionSite(Player owner)
		{
			this.Owner = owner;
			this.id = NextId();
		}
		
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
	}
}
