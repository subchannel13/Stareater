using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Podaci.Jezici;

namespace Zvjezdojedac.Igra.Poruke
{
	public class PorukaTehnologija : Poruka
	{
		public PorukaTehnologija(Tehnologija tehnologija, bool istrazivnje)
			: base(Tip.Tehnologija)
		{
			this.istrazivnje = istrazivnje;
			this.nivo = tehnologija.nivo;
			this.tehnologija = tehnologija.tip;
		}

		private PorukaTehnologija(Tehnologija.TechInfo tehnologija, long nivo, bool istrazivnje)
			: base(Tip.Tehnologija)
		{
			this.istrazivnje = istrazivnje;
			this.nivo = nivo;
			this.tehnologija = tehnologija;
		}

		bool istrazivnje;
		private long nivo;
		private Tehnologija.TechInfo tehnologija;

		public override string tekst
		{
			get 
			{
				Dictionary<string, double> doubleVars = new Dictionary<string, double>();
				Dictionary<string, string> stringVars = new Dictionary<string, string>();
				doubleVars.Add("NIVO", nivo);
				stringVars.Add("TEH_NAZIV", tehnologija.naziv);
				return Postavke.Jezik[Kontekst.FormPoruke, "porukaTehnologija"].tekst(doubleVars, stringVars);
			}
		}

		#region Pohrana
		protected const string PohNivo = "NIVO";
		protected const string PohTehno = "TEHNO";
		protected const string Istrazivanje = "IST";
		protected const string Razvoj = "RAZ";
		public override void pohrani(PodaciPisac izlaz)
		{
			string ist_raz = (istrazivnje == true) ? Istrazivanje : Razvoj;
			izlaz.dodaj(PohTip, (int)tip);
			izlaz.dodaj(PohNivo, nivo + " " + ist_raz);
			izlaz.dodaj(PohTehno, (IIdentifiable)tehnologija);
		}

		public static Poruka Ucitaj(PodaciCitac ulaz)
		{
			string[] nivoITip = ulaz.podatak(PohNivo).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			long nivo = long.Parse(nivoITip[0]);
			int tehnoId = ulaz.podatakInt(PohTehno);

			if (nivoITip[1] == Istrazivanje)
				return new PorukaTehnologija(Tehnologija.TechInfo.tehnologijeIstrazivanje[tehnoId], nivo, true);
			else
				return new PorukaTehnologija(Tehnologija.TechInfo.tehnologijeRazvoj[tehnoId], nivo, false);
		}
		#endregion
	}
}
