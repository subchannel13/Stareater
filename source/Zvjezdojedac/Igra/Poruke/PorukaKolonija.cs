using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci;

namespace Zvjezdojedac.Igra.Poruke
{
	public class PorukaKolonija : Poruka
	{
		public PorukaKolonija(Planet planet)
			: base(Tip.Kolonija)
		{
			this.planet = planet;
		}

		public Planet planet { get; private set; }

		public override string tekst
		{
			get
			{
				Dictionary<string, string> stringVars = new Dictionary<string, string>();
				stringVars.Add("PLANET", planet.ime);
				return Postavke.jezik[Kontekst.FormPoruke, "porukaKolonija"].tekst(null, stringVars);
			}
		}

		#region Pohrana
		protected const string PohIzvor = "IZVOR";
		public override void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohTip, (int)tip);
			izlaz.dodaj(PohIzvor, planet.zvjezda.id + " " + planet.pozicija);
		}

		public static Poruka Ucitaj(PodaciCitac ulaz, Dictionary<int, Zvijezda> zvijezdeID)
		{
			string[] izvor = ulaz.podatak(PohIzvor).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			Zvijezda zvj = zvijezdeID[int.Parse(izvor[0])];
			int planetId = int.Parse(izvor[1]);

			return new PorukaKolonija(zvj.planeti[planetId]);
		}
		#endregion
	}
}
