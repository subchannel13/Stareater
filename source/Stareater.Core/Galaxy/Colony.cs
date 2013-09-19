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
		
		public double Population { get; set; }
		public double Infrastructure { get; set; }
		
		internal Colony(Player owner, Planet planet) : base(owner)
		{
			this.Location = planet;
			
			this.Infrastructure = 0;
			this.Population = 0;
		}
		
		public StarData Star
		{
			get {
				return Location.Star;
			}
		}
	}
}
