using System;
using System.Collections.Generic;

namespace Zvjezdojedac.Igra.Brodovi.Dizajner
{
	public class Dizajner
	{
		public Oklop oklop { get; private set; }
		public Potisnici potisnici { get; private set; }
		public Dictionary<Misija.Tip, List<Oruzje>> oruzja { get; private set; }
		public Senzor senzor { get; private set; }
		public List<Trup> trupovi { get; private set; }
		public Dictionary<Trup, Komponente> komponente { get; private set; }

		private bool promjenjenDizajn;
		private Dizajn _dizajn;
		private Trup _dizajnTrup;
		private string _dizajnIme;
		private Oruzje _dizajnPrimMisija;
		private Oruzje _dizajnSekMisija;
		private double _dizajnUdioPrimMisije = 1;
		private Stit _dizajnStit;
		private Dictionary<SpecijalnaOprema, int> _dizajnSpecOprema;
		private bool _dizajnMZPogon;
		private Taktika _dizajnTaktika;

		public Dizajner(Igrac igrac)
		{
			reset(igrac);
		}

		public void reset(Igrac igrac)
		{
			oklop = Oklop.OklopInfo.NajboljiOklop(igrac.efekti);
			potisnici = Potisnici.PotisnikInfo.NajboljiPotisnici(igrac.efekti);
			oruzja = Oruzje.OruzjeInfo.DostupnaOruzja(igrac.efekti);
			senzor = Senzor.SenzorInfo.NajboljiSenzor(igrac.efekti);
			trupovi = Trup.TrupInfo.DostupniTrupovi(igrac.efekti);

			komponente = new Dictionary<Trup, Komponente>();
			foreach (Trup trup in trupovi)
				komponente.Add(trup, new Komponente(igrac, trup));

			_dizajn = null;
			odabranTrup = trupovi[0];
			_dizajnSpecOprema = new Dictionary<SpecijalnaOprema, int>();
			promjenjenDizajn = true;
		}

		#region Odabir komponenti
		public Trup odabranTrup
		{
			get { return _dizajnTrup; }
			set
			{

				_dizajnTrup = value;
				Komponente komponente = this.komponente[_dizajnTrup];

				if (_dizajnStit != null)
					_dizajnStit = komponente.stitoviInfo[_dizajnStit.info];

				Dictionary<SpecijalnaOprema, int> novaSpecOprema = new Dictionary<SpecijalnaOprema, int>();
				if (_dizajnSpecOprema != null)
					foreach (KeyValuePair<SpecijalnaOprema, int> par in _dizajnSpecOprema)
						novaSpecOprema.Add(
							komponente.specijalnaOpremaInfo[par.Key.info],
							par.Value);
				_dizajnSpecOprema = novaSpecOprema;

				promjenjenDizajn = true;
			}
		}
		public string dizajnIme
		{
			get { return _dizajnIme; }
			set
			{
				_dizajnIme = value;
				promjenjenDizajn = true;
			}
		}
		public Oruzje dizajnPrimMisija
		{
			get { return _dizajnPrimMisija; }
			set
			{
				_dizajnPrimMisija = value;
				promjenjenDizajn = true;
			}
		}
		public Oruzje dizajnSekMisija
		{
			get { return _dizajnSekMisija; }
			set
			{
				_dizajnSekMisija = value;
				promjenjenDizajn = true;
			}
		}
		public double dizajnUdioPrimMisije
		{
			get { return _dizajnUdioPrimMisije; }
			set
			{
				_dizajnUdioPrimMisije = value;
				promjenjenDizajn = true;
			}
		}
		public Stit dizajnStit
		{
			get { return _dizajnStit; }
			set
			{
				_dizajnStit = value;
				promjenjenDizajn = true;
			}
		}
		public Dictionary<SpecijalnaOprema, int> dizajnSpecOprema
		{
			get { return _dizajnSpecOprema; }
		}
		public bool dizajnMZPogon
		{
			get { return _dizajnMZPogon; }
			set
			{
				_dizajnMZPogon = value;
				promjenjenDizajn = true;
			}
		}
		public Taktika dizajnTaktika
		{
			get { return _dizajnTaktika; }
			set
			{
				_dizajnTaktika = value;
				promjenjenDizajn = true;
			}
		}

		public int dodajSpecOpremu(SpecijalnaOprema so)
		{
			if (!dizajnSpecOprema.ContainsKey(so) && so.maxKolicina > 0)
				dizajnSpecOprema.Add(so, 0);

			if (dizajnSpecOprema[so] < so.maxKolicina) {
				dizajnSpecOprema[so]++;
				promjenjenDizajn = true;
			}

			if (dizajnSpecOprema.ContainsKey(so))
				return dizajnSpecOprema[so];
			else
				return 0;
		}
		public int makniSpecOpremu(SpecijalnaOprema so)
		{
			if (!dizajnSpecOprema.ContainsKey(so))
				return 0;

			dizajnSpecOprema[so]--;
			promjenjenDizajn = true;

			if (dizajnSpecOprema[so] <= 0) {
				dizajnSpecOprema.Remove(so);
				return 0;
			}
			else
				return dizajnSpecOprema[so];
		}
		#endregion

		public Dizajn dizajn
		{
			get
			{
				if (promjenjenDizajn) {
					MZPogon mzPogon = null;
					if (dizajnMZPogon) mzPogon = komponente[odabranTrup].mzPogon;

					dizajn = new Dizajn(_dizajnIme, odabranTrup,
						dizajnPrimMisija, dizajnSekMisija, dizajnUdioPrimMisije,
						oklop, dizajnStit, dizajnSpecOprema, senzor,
						potisnici, mzPogon, komponente[odabranTrup].reaktor,
						dizajnTaktika
						);
					promjenjenDizajn = false;
				}

				return _dizajn;
			}

			private set
			{
				_dizajn = value;
			}
		}

		public double slobodnaNosivost
		{
			get
			{
				double suma = _dizajnTrup.Nosivost;
				if (_dizajnStit != null) suma -= _dizajnTrup.VelicinaStita;
				if (_dizajnMZPogon) suma -= _dizajnTrup.VelicinaMZPogona;
				foreach (SpecijalnaOprema so in _dizajnSpecOprema.Keys)
					suma -= so.velicina * _dizajnSpecOprema[so];

				return suma;
			}
		}

		public Komponente trupKomponente
		{
			get { return komponente[odabranTrup]; }
		}
	}
}

