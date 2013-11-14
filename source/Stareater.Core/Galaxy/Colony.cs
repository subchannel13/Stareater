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
			
			this.Population = 0;
		}

		public Colony(Player owner, Planet planet, double population) : base(owner)
		{
			this.Location = planet;

			this.Population = population;
		}

		public StarData Star
		{
			get {
				return Location.Star;
			}
		}
		
		public double Population { get; set; }
		//TODO: add generalized buildings instead infrastructure

		internal Colony Copy(Player player, Planet planet)
		{
			return new Colony(player, planet, this.Population);
		}
	}
}
