using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.Igra
{
	public class PokretnaFlota : Flota
	{
		public Zvijezda polaznaZvj;
		public Zvijezda odredisnaZvj;

		public PokretnaFlota(Zvijezda polaznaZvj, Zvijezda odredisnaZvj,
			int id)
			: base(polaznaZvj, id)
		{
			this.odredisnaZvj = odredisnaZvj;
			this.polaznaZvj = polaznaZvj;
		}

		public PokretnaFlota(Zvijezda polaznaZvj, Zvijezda odredisnaZvj,
			int id, double x, double y)
			: base(x, y, id)
		{
			this.odredisnaZvj = odredisnaZvj;
			this.polaznaZvj = polaznaZvj;
		}

		public bool primakniCilju(double brzina)
		{
			double udaljenost = odredisnaZvj.udaljenost(x, y);
			if (udaljenost < brzina) {
				x = odredisnaZvj.x;
				y = odredisnaZvj.y;
				return true;
			}

			double dx = odredisnaZvj.x - x;
			double dy = odredisnaZvj.y - y;
			x += brzina * dx / udaljenost;
			y += brzina * dy / udaljenost;

			return false;
		}

		#region Pohrana
		public new const string PohranaTip = "POK_FLOTA";
		public const string PohPolaznaZvj = "POLAZ";
		public const string PohOdredisnaZvj = "CILJ";
		public override void pohrani(PodaciPisac izlaz)
		{
			izlaz.dodaj(PohId, id);
			izlaz.dodaj(PohX, x);
			izlaz.dodaj(PohY, y);
			izlaz.dodaj(PohPolaznaZvj, (IIdentifiable)polaznaZvj);
			izlaz.dodaj(PohOdredisnaZvj, (IIdentifiable)odredisnaZvj);

			izlaz.dodaj(Brod.PohranaTip, brodovi.Count);
			izlaz.dodajKolekciju(Brod.PohranaTip, brodovi.Values);
			izlaz.dodaj(Kolonizacija.PohranaTip, kolonizacije.Count);
			izlaz.dodajKolekciju(Kolonizacija.PohranaTip, kolonizacije);
		}

		public static PokretnaFlota Ucitaj(PodaciCitac ulaz, Dictionary<int, Dizajn> dizajnovi, Dictionary<int, Zvijezda> zvijezdeID)
		{
			int id = ulaz.podatakInt(PohId);
			double x = ulaz.podatakDouble(PohX);
			double y = ulaz.podatakDouble(PohY);
			int polaznaZvjId = ulaz.podatakInt(PohPolaznaZvj);
			int odredisnaZvjId = ulaz.podatakInt(PohOdredisnaZvj);
			PokretnaFlota flota = new PokretnaFlota(
				zvijezdeID[polaznaZvjId], 
				zvijezdeID[odredisnaZvjId], 
				id, x, y);

			int brBrodova = ulaz.podatakInt(Brod.PohranaTip);
			for (int i = 0; i < brBrodova; i++)
				flota.dodajBrod(Brod.Ucitaj(ulaz[Brod.PohranaTip + i], dizajnovi));

			List<Kolonizacija> kolonizacije = new List<Kolonizacija>();
			for (int i = 0; i < ulaz.podatakInt(Kolonizacija.PohranaTip); i++)
				kolonizacije.Add(Kolonizacija.Ucitaj(
					ulaz[Kolonizacija.PohranaTip + i],
					dizajnovi,
					flota));
			flota.kolonizacije.AddRange(kolonizacije);

			return flota;
		}
		#endregion
	}
}
