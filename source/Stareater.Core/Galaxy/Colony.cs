using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Players;

namespace Stareater.Galaxy
{
	class Colony : AConstructionSite
	{
		public Planet Location { get; private set; }
		
		public Colony(Player owner, Planet planet) : base(owner)
		{
			this.Location = planet;
		}
	}
}
