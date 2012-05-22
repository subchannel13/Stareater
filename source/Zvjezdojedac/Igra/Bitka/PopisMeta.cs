using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zvjezdojedac.Igra.Brodovi;
using Zvjezdojedac.Alati.Strukture;

namespace Zvjezdojedac.Igra.Bitka
{
	class PopisMeta
	{

		Dictionary<Oruzje.Ciljanje, BinaryIndexedTree<Borac>> mete = new Dictionary<Oruzje.Ciljanje, BinaryIndexedTree<Borac>>();

		public PopisMeta()
		{
			foreach (Oruzje.Ciljanje kategorijaMeta in Oruzje.OruzjeInfo.CiljanjeKod.Keys)
				mete.Add(kategorijaMeta, new BinaryIndexedTree<Borac>());
		}

		public void Dodaj(IEnumerable<Borac> borci)
		{
			foreach (Borac borac in borci) {
				double velicina = borac.Dizajn.trup.velicina;

				mete[Oruzje.Ciljanje.Obrana].Add(borac, 1 / velicina);
				mete[Oruzje.Ciljanje.Normalno].Add(borac, velicina);
				mete[Oruzje.Ciljanje.Veliki_brodovi].Add(borac, velicina * velicina);
			}
		}

		public Borac DajSlijedeci(Oruzje.Ciljanje kategorija, double tezina)
		{
			return mete[kategorija][tezina];
		}

		public bool jePrazan()
		{
			return mete[Oruzje.Ciljanje.Normalno].isEmpty();
		}

		public void Ukloni(Borac borac)
		{
			foreach (var grupe in mete)
				grupe.Value.Remove(borac);
		}
	}
}
