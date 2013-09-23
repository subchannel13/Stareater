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
		private IEnumerable<object> orderQueue; //TODO: make type
		private object leftovers; //TODO: make type
		
		protected AConstructionSite(Player owner)
		{
			this.Owner = owner;
		}
		
		#if DEBUG
		private bool dirty = true;
		
		public bool Dirty {
			get { return dirty; }
			protected set { dirty = value; }
		}
		
		public void Cleaned()
		{
			dirty = false;
		}		
		#endif
	}
}
