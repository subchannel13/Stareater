using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci;

namespace Zvjezdojedac.Igra.Poruke
{
	public class PorukaZgradaKolonija : Poruka
	{
		public PorukaZgradaKolonija(Planet planet, Zgrada.ZgradaInfo zgradaInfo)
			: base(Tip.ZgradaKolonija)
		{
			this.planet = planet;
			this.zgradaInfo = zgradaInfo;
		}

		public Planet planet { get; private set; }
		private Zgrada.ZgradaInfo zgradaInfo;

		public override string tekst
		{
			get 
			{
				Dictionary<string, string> stringVars = new Dictionary<string, string>();
				stringVars.Add("ZGRADA", zgradaInfo.ime);
				stringVars.Add("PLANET", planet.ime);
				return Postavke.Jezik[Kontekst.FormPoruke, "porukaZgrada"].tekst(null, stringVars);
			}
		}

		#region Pohrana
		protected const string PohIzvor = "IZVOR";
		protected const string PohZgrada = "ZGRADA";
		public override void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohTip, (int)tip);
			izlaz.dodaj(PohZgrada, zgradaInfo);
			izlaz.dodaj(PohIzvor, planet.zvjezda.id + " " + planet.pozicija);
		}

		public static Poruka Ucitaj(PodaciCitac ulaz, Dictionary<int, Zvijezda> zvijezdeID)
		{
			string[] izvor = ulaz.podatak(PohIzvor).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			Zvijezda zvj = zvijezdeID[int.Parse(izvor[0])];
			int zgradaId = ulaz.podatakInt(PohZgrada);

			int planetId = int.Parse(izvor[1]);
			return new PorukaZgradaKolonija(zvj.planeti[planetId], Zgrada.ZgradaInfoID[zgradaId]);
		}
		#endregion
	}
}
