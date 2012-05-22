using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Igra.Brodovi;
using Zvjezdojedac.Alati.Strukture;

namespace Zvjezdojedac.Igra.Bitka
{
	public class Konflikt
	{
		Dictionary<Igrac, IgracevKonflikt> stanjeStrana = new Dictionary<Igrac, IgracevKonflikt>();
		public Zvijezda Lokacija { get; private set; }

		public Konflikt(IEnumerable<Igrac> strane, Zvijezda lokacija)
		{
			foreach (Igrac strana in strane)
				stanjeStrana.Add(strana, new IgracevKonflikt());

			this.Lokacija = lokacija;
		}

		public ModeratorBorbe ZapocniBorbu()
		{
			Dictionary<Igrac, Strana> sviBorci = new Dictionary<Igrac, Strana>();

			foreach (Igrac strana in stanjeStrana.Keys) {
				var borci = new MySet<Borac>();

				foreach(Brod brod in strana.floteStacionarne[Lokacija].brodovi.Values)
					for(int i = 0; i < brod.kolicina; i++)
						borci.Add(new Borac(brod.dizajn, strana));

				sviBorci.Add(strana, new Strana(borci));
			}

			return new ModeratorBorbe(sviBorci, this);
		}

		public StanjeKonflikta Faza
		{
			get
			{
				StanjeKonflikta rez = StanjeKonflikta.Razrijeseno;
				foreach (var strana in stanjeStrana.Values)
					if ((int)strana.Stanje < (int)rez)
						rez = strana.Stanje;

				return rez;
			}
		}

		public bool Razrijesen
		{
			get
			{
				foreach (var strana in stanjeStrana.Values)
					if (strana.Stanje != StanjeKonflikta.Razrijeseno)
						return false;

				return true;
			}
		}

		public int PreostaloKrugova
		{
			get
			{
				int rez = IgracevKonflikt.MaxBrPoteza;
				foreach (var strana in stanjeStrana.Values)
					rez = Math.Min(rez, strana.BrojPoteza);

				return rez;
			}
		}

		public void ZavrsiKrug()
		{
			foreach (var strana in stanjeStrana.Values)
				strana.ZavrsiKrug();
		}

		public void ZavrsiSvemirskuBitku()
		{
			if (Faza == StanjeKonflikta.SvemirskiSukob)
				foreach (var strana in stanjeStrana.Values)
					strana.SlijedecaFaza();
		}
	}
}
