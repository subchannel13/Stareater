using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alati;
using Prototip.Podaci;
using Prototip.Podaci.Jezici;

namespace Prototip.Igra.Poruke
{
	public class PorukaBrod : Poruka
	{
		public PorukaBrod(Zvijezda zvijezda, Dizajn dizajn, long kolicina)
			: base(Tip.Brod)
		{
			this.dizajn = dizajn;
			this.kolicina = kolicina;
			this.zvijezda = zvijezda;
		}

		private Dizajn dizajn;
		private long kolicina;
		public Zvijezda zvijezda { get; private set; }

		public override string tekst
		{
			get
			{
				Dictionary<string, double> doubleVars = new Dictionary<string, double>();
				Dictionary<string, string> stringVars = new Dictionary<string, string>();
				doubleVars.Add("KOLICINA", kolicina);
				stringVars.Add("DIZAJN", dizajn.ime);
				stringVars.Add("SUSTAV", zvijezda.ime);
				return Postavke.jezik[Kontekst.FormPoruke, "porukaBrod"].tekst(doubleVars, stringVars);
			}
		}

		#region Pohrana
		protected const string PohIzvor = "IZVOR";
		protected const string PohDizajn = "DIZAJN";
		protected const string PohKolicina = "KOL";
		public override void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohTip, (int)tip);
			izlaz.dodaj(PohIzvor, (IIdentifiable)zvijezda);
			izlaz.dodaj(PohDizajn, (IIdentifiable)dizajn);
			izlaz.dodaj(PohKolicina, kolicina);
		}

		public new static Poruka Ucitaj(PodaciCitac ulaz, Dictionary<int, Zvijezda> zvijezdeID, Dictionary<int, Dizajn> dizajnID)
		{
			Zvijezda zvj = zvijezdeID[ulaz.podatakInt(PohIzvor)];
			Dizajn dizajn = dizajnID[ulaz.podatakInt(PohDizajn)];
			long kolicina = ulaz.podatakLong(PohKolicina);

			return new PorukaBrod(zvj, dizajn, kolicina);
		}
		#endregion
	}
}
