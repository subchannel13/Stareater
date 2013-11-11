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
			
			this.Infrastructure = 0;
			this.Population = 0;
		}

		public Colony(Player owner, Planet planet, double population) : base(owner)
		{
			this.Location = planet;

			this.Infrastructure = 0;
			this.Population = population;
		}

		public StarData Star
		{
			get {
				return Location.Star;
			}
		}
		
		#if !DEBUG
		public double Population { get; set; }
		public double Infrastructure { get; set; }
		#else
		
		private double population;
		public double Population 
		{
			get { return population;}
			set {
				population = value;
				Dirty = true;
			}
		}
		
		private double infrastructure;
		public double Infrastructure 
		{
			get { return infrastructure;}
			set {
				infrastructure = value;
				Dirty = true;
			}
		}
		#endif

		internal Colony Copy(Player player, Planet planet)
		{
			return new Colony(player, planet, this.population);
		}
	}
}
