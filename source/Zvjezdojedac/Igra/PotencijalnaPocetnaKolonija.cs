using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zvjezdojedac.Igra
{
	struct PotencijalnaPocetnaKolonija
	{
		public Planet planet;
		public double prikladnost;
		public double populacijaMax;
		public double rudePoRudaru;

		public PotencijalnaPocetnaKolonija(Planet planet, double prikladnost, double populacijaMax, double rudePoRudaru)
		{
			this.planet = planet;
			this.populacijaMax = populacijaMax;
			this.prikladnost = prikladnost;
			this.rudePoRudaru = rudePoRudaru;
		}

		public override string ToString()
		{
			return planet.pozicija + ". planet, prikladnost: " + prikladnost + " max pop.:" + populacijaMax + " rude: " + rudePoRudaru;
		}
	}
}
