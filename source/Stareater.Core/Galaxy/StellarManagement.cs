using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.Players;

namespace Stareater.Galaxy
{
	class StellarManagement : AConstructionSite
	{
		public StarData Location { get; private set; }
		
		public StellarManagement(Player owner, StarData star) : base(owner)
		{
			this.Location = star;
		}
	}
}
