using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Igra.Brodovi.Dizajner;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Igra;
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.GUI
{
	public partial class FormFlote : Form
	{
		private enum InfoStranice
		{
			PrimarnaMisija = 0,
			SekundarnaMisija,
			Stit,
			Reaktor,
			Pokretljivost,
			Senzori,
			MZPogon,
			SpecijalnaOprema,
			Taktika
		}

		private const string SeparatorNDGrupa = "-----";
		private Dictionary<InfoStranice, string> nazivInfoStranice = new Dictionary<InfoStranice, string>();
		private Igrac igrac;
		private Dizajner dizajner;
		private InfoStranice prethodniNDinfo = InfoStranice.PrimarnaMisija;
		private int prethodnaNDprimMisija = 0;
		private int prethodnaNDsekMisija = 0;
		private bool zadrziNDInfo = false;
		private SpecijalnaOprema specijalnaOpremaZaOpis = null;

		public FormFlote(Igrac igrac)
		{
			InitializeComponent();
			btnNDZadrziInfo.Text = "";
			lstvDizajnovi.SmallImageList = new ImageList();
			lstvDizajnovi.SmallImageList.ImageSize = new Size(60, 40);
			this.igrac = igrac;

			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];
			nazivInfoStranice.Add(InfoStranice.MZPogon, jezik["infoMZPogon"].tekst());
			nazivInfoStranice.Add(InfoStranice.Pokretljivost, jezik["infoPokret"].tekst());
			nazivInfoStranice.Add(InfoStranice.PrimarnaMisija, jezik["infoPrimMisija"].tekst());
			nazivInfoStranice.Add(InfoStranice.Reaktor, jezik["infoReaktor"].tekst());
			nazivInfoStranice.Add(InfoStranice.SekundarnaMisija, jezik["infoSekMisija"].tekst());
			nazivInfoStranice.Add(InfoStranice.Senzori, jezik["infoSenzori"].tekst());
			nazivInfoStranice.Add(InfoStranice.SpecijalnaOprema, jezik["infoSpecOprema"].tekst());
			nazivInfoStranice.Add(InfoStranice.Stit, jezik["infoStit"].tekst());
			nazivInfoStranice.Add(InfoStranice.Taktika, jezik["infoTaktika"].tekst());

			#region Dizajnovi
			{
				foreach (DizajnZgrada dizajnZgrada in igrac.dizajnoviBrodova) {
					Dizajn dizajn = dizajnZgrada.dizajn;
					dodajDizajn(dizajn);
				}
			}
			#endregion

			#region Novi dizajn
			{
				dizajner = new Dizajner(igrac);

				foreach (Trup trup in dizajner.trupovi)
					cbNDvelicina.Items.Add(trup);

				cbNDprimMisija.Items.Add(new TagTekst<Oruzje>(null, jezik["bezMisije"].tekst()));
				cbNDsekMisija.Items.Add(new TagTekst<Oruzje>(null, jezik["bezMisije"].tekst()));
				foreach (Misija.Tip misija in dizajner.oruzja.Keys) {
					if (dizajner.oruzja[misija].Count == 0) continue;
					cbNDprimMisija.Items.Add(new TagTekst<Oruzje>(null, SeparatorNDGrupa));
					cbNDsekMisija.Items.Add(new TagTekst<Oruzje>(null, SeparatorNDGrupa));

					foreach (Oruzje oruzje in dizajner.oruzja[misija]) {
						cbNDprimMisija.Items.Add(new TagTekst<Oruzje>(oruzje, oruzje.info.naziv));
						cbNDsekMisija.Items.Add(new TagTekst<Oruzje>(oruzje, oruzje.info.naziv));
					}
				}

				cbNDstit.Items.Add(new TagTekst<int>(-1, jezik["bezStita"].tekst()));
				int i = 0;
				foreach (Stit stit in dizajner.trupKomponente.stitovi) {
					cbNDstit.Items.Add(new TagTekst<int>(i, stit.info.naziv));
					i++;
				}

				/*foreach (Pozicije taktika in Pozicije.Taktike.Keys)
					cbNDtaktika.Items.Add(new TagTekst<Pozicije>(taktika, taktika.naziv));
				*/
				i = 0;
				foreach (SpecijalnaOprema so in dizajner.trupKomponente.specijalnaOprema) {
					ListViewItem item = new ListViewItem("");
					item.SubItems.Add(so.naziv);
					item.SubItems.Add(so.velicina.ToString());
					item.Tag = i;
					lstvNDspecOprema.Items.Add(item);
					i++;
				}

				foreach (InfoStranice strana in Enum.GetValues(typeof(InfoStranice)))
					cbNDinfoStrana.Items.Add(new TagTekst<InfoStranice>(strana, nazivInfoStranice[strana]));

				cbNDvelicina.SelectedIndex = 0;
				cbNDprimMisija.SelectedIndex = 0;
				cbNDsekMisija.SelectedIndex = 0;
				cbNDstit.SelectedIndex = 0;
				//cbNDtaktika.SelectedIndex = 0;
				hscrUdioMisija.Value = 33;
			}
			#endregion

			btnSpremi.Text = jezik["btnSpremi"].tekst();
			btnNoviDizajn.Text = jezik["tabNoviDizajn"].tekst();
			btnUkloniDizajn.Text = jezik["btnUkloniDizajn"].tekst();
			chBrojBrodova.Text = jezik["chBrojBrodova"].tekst();
			chDizajnNaziv.Text = jezik["chDizajnNaziv"].tekst();
			chSpecOpNaziv.Text = jezik["chSpecOpNaziv"].tekst();
			chSpecOpVelicina.Text = jezik["chSpecOpVelicina"].tekst();
			chNDMZpogon.Text = jezik["chNDMZpogon"].tekst();
			lblDizajn.Text = jezik["lblDizajn"].tekst() + ":";
			lblNaziv.Text = jezik["lblNaziv"].tekst() + ":";
			lblPrimMisija.Text = jezik["lblPrimMisija"].tekst() + ":";
			lblSekMisija.Text = jezik["lblSekMisija"].tekst() + ":";
			lblSpecOprema.Text = jezik["lblSpecOprema"].tekst() + ":";
			lblStit.Text = jezik["lblStit"].tekst() + ":";
			lblTaktika.Text = jezik["lblTaktika"].tekst() + ":";
			lblUdioSek.Text = jezik["lblUdioSek"].tekst() + ":";
			lblVelicina.Text = jezik["lblVelicina"].tekst() + ":";
			tabDizajnovi.Text = jezik["tabDizajnovi"].tekst();
			tabNoviDizajn.Text = jezik["tabNoviDizajn"].tekst();
			this.Text = jezik["naslov"].tekst();

			this.Font = Postavke.FontSucelja(this.Font);
		}

		private static T izvadiTag<T>(ComboBox cb)
		{
			return ((TagTekst<T>)cb.SelectedItem).tag;
		}

		private static List<string> opisOruzja(bool primarno, Dizajn.Zbir<Oruzje> oruzje, bool cijene)
		{
			List<string> opis = new List<string>();

			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];
			if (primarno) opis.Add(jezik["opisPrimMis"].tekst() + ": ");
			else opis.Add(jezik["opisSekMis"].tekst() + ": ");

			if (oruzje == null) {
				opis.Add("");
				if (primarno) opis.Add(jezik["opisNemaPrimMis"].tekst());
				else opis.Add(jezik["opisNemaSekMis"].tekst());

				return opis;
			}

			Misija.Tip misijaTip = oruzje.komponenta.misija;
			Misija misija = Misija.Opisnici[misijaTip];
			
			opis.Add(misija.naziv);
			opis.Add((misija.grupirana) ?
				oruzje.komponenta.naziv :
				Fje.PrefiksFormater(oruzje.kolicina) + " x " + oruzje.komponenta.naziv);
			opis.Add("");
			if (oruzje.komponenta.maxNivo > 0) opis.Add(jezik["opisNivo"].tekst() + ": " + oruzje.komponenta.nivo);
			if (misija.imaCiljanje) opis.Add(jezik["opisCiljanje"].tekst() + ": " + Postavke.Jezik[Kontekst.Misije, Oruzje.OruzjeInfo.CiljanjeKod[oruzje.komponenta.ciljanje]].tekst());

			for (int i = 0; i < misija.brParametara; i++) {
				double vrijednost = oruzje.komponenta.parametri[i];
				if (misija.parametri[i].mnoziKolicinom)
					vrijednost *= oruzje.kolicina;
				switch (misija.parametri[i].tip) {
					case Misija.TipParameta.Cijelobrojni:
						opis.Add(misija.parametri[i].opis + ": " + Fje.PrefiksFormater(vrijednost));
						break;
					case Misija.TipParameta.Postotak:
						opis.Add(misija.parametri[i].opis + ": " + vrijednost.ToString("0.##"));
						break;
				}
			}

			if (cijene) {
				opis.Add("");
				opis.Add(jezik["opisSnaga"].tekst() + ": " + Fje.PrefiksFormater(oruzje.komponenta.snaga));
				opis.Add(jezik["opisCijena"].tekst() + ": " + Fje.PrefiksFormater(oruzje.komponenta.cijena * oruzje.kolicina));
			}

			return opis;
		}

		private List<string> opis(InfoStranice stranica, Dizajn dizajn)
		{
			return opis(stranica, dizajn, true);
		}

		private List<string> opis(InfoStranice stranica, Dizajn dizajn, bool cijene)
		{
			List<string> opis = new List<string>();
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];

			switch (stranica) {
				case InfoStranice.MZPogon:
					opis.Add(jezik["opisMZPogon"].tekst());
					opis.Add("");
					if (dizajn.MZPogon == null)
						opis.Add(jezik["opisNemaMZPogon"].tekst());
					else {
						opis.Add(dizajn.MZPogon.info.naziv);
						if (dizajn.MZPogon.maxNivo > 0)
							opis.Add(jezik["opisNivo"].tekst() + ": " + dizajn.MZPogon.nivo);
						opis.Add(jezik["opisMZPogonBrz"].tekst() + ": " + dizajn.MZbrzina.ToString("0.###"));
						if (cijene) {
							opis.Add(jezik["opisSnaga"].tekst() + ": " + Fje.PrefiksFormater(dizajn.MZPogon.snaga));
							opis.Add(jezik["opisCijena"].tekst() + ": " + Fje.PrefiksFormater(dizajn.MZPogon.cijena));
						}
					}
					break;

				case InfoStranice.Pokretljivost:
					opis.Add(jezik["opisPokret"].tekst());
					opis.Add("");
					opis.Add(dizajn.potisnici.naziv);
					if (dizajn.potisnici.maxNivo > 0)
						opis.Add(jezik["opisNivo"].tekst() + ": " + dizajn.reaktor.nivo);
					opis.Add(jezik["opisTromost"].tekst() + ": " + dizajn.inercija);
					opis.Add(jezik["opisPokret"].tekst() + ": " + Fje.PrefiksFormater(dizajn.pokretljivost));
					break;

				case InfoStranice.PrimarnaMisija:
					opis = opisOruzja(true, dizajn.primarnoOruzje, cijene);
					break;

				case InfoStranice.Reaktor:
					opis.Add(jezik["opisReaktor"].tekst());
					opis.Add("");
					opis.Add(dizajn.reaktor.naziv);
					if (dizajn.reaktor.maxNivo > 0)
						opis.Add(jezik["opisNivo"].tekst() + ": " + dizajn.reaktor.nivo);
					opis.Add(jezik["opisReaktorOp"].tekst() + ": " + Fje.PrefiksFormater(dizajn.opterecenjeReaktora));
					opis.Add(jezik["opisReaktorDost"].tekst() + ": " + Fje.PrefiksFormater(dizajn.snagaReaktora) + " (" + (dizajn.koefSnageReaktora * 100).ToString("0") + "%)");
					break;

				case InfoStranice.SekundarnaMisija:
					opis = opisOruzja(false, dizajn.sekundarnoOruzje, cijene);
					break;

				case InfoStranice.Senzori:
					opis.Add(jezik["opisSenzor"].tekst());
					opis.Add("");
					//opis.Add(Fje.PrefiksFormater(dizajn.brSenzora) + " x " + dizajn.senzor.naziv);
					if (dizajn.senzor.maxNivo > 0)
						opis.Add(jezik["opisNivo"].tekst() + ": " + dizajn.senzor.nivo);
					opis.Add(jezik["opisSenzorSn"].tekst() + ": " + Fje.PrefiksFormater(dizajn.snagaSenzora));
					//opis.Add(jezik["opisSenzorOm"].tekst() + ": " + Fje.PrefiksFormater(dizajn.ometanje));
					//opis.Add(jezik["opisSenzorPrik"].tekst() + ": " + Fje.PrefiksFormater(dizajn.prikrivenost));
					break;

				case InfoStranice.SpecijalnaOprema:
					opis.Add(jezik["opisSpecOp"].tekst());
					opis.Add("");
					if (specijalnaOpremaZaOpis != null) {
						if (dizajn.specijalnaOprema.ContainsKey(specijalnaOpremaZaOpis))
							opis.Add(dizajn.specijalnaOprema[specijalnaOpremaZaOpis] + " x " + specijalnaOpremaZaOpis.naziv);
						else
							opis.Add(specijalnaOpremaZaOpis.naziv);
						if (specijalnaOpremaZaOpis.maxNivo > 0)
							opis.Add(jezik["opisNivo"].tekst() + ": " + specijalnaOpremaZaOpis.nivo);
						opis.AddRange(specijalnaOpremaZaOpis.opisEfekata);
						if (cijene) {
							opis.Add("");
							opis.Add(jezik["opisVelicina"].tekst() + ": " + Fje.PrefiksFormater(specijalnaOpremaZaOpis.velicina));
							opis.Add(jezik["opisCijena"].tekst() + ": " + Fje.PrefiksFormater(specijalnaOpremaZaOpis.cijena));
						}
					}

					break;

				case InfoStranice.Stit:
					opis.Add(jezik["opisStit"].tekst());
					opis.Add("");
					if (dizajn.stit == null)
						opis.Add(jezik["opisNemaStit"].tekst());
					else {
						opis.Add(dizajn.stit.naziv);
						if (dizajn.stit.maxNivo > 0)
							opis.Add(jezik["opisNivo"].tekst() + ": " + dizajn.stit.nivo);
						opis.Add(jezik["opisStitIzd"].tekst() + ": " + Fje.PrefiksFormater(dizajn.stit.izdrzljivost));
						opis.Add(jezik["opisStitObn"].tekst() + ": " + Fje.PrefiksFormater(dizajn.stit.obnavljanje));
						opis.Add(jezik["opisStitDeb"].tekst() + ": " + Fje.PrefiksFormater(dizajn.stit.debljina));
						opis.Add(jezik["opisSenzorOm"].tekst() + ": " + dizajn.stit.ometanje.ToString("+0;-0"));
						opis.Add(jezik["opisSenzorPrik"].tekst() + ": +" + Fje.PrefiksFormater(dizajn.stit.prikrivanje));
						if (cijene) {
							opis.Add("");
							opis.Add(jezik["opisSnaga"].tekst() + ": " + Fje.PrefiksFormater(dizajn.stit.snaga));
							opis.Add(jezik["opisCijena"].tekst() + ": " + Fje.PrefiksFormater(dizajn.stit.cijena));
						}
					}
					break;

				case InfoStranice.Taktika:
					Pozicije.EfektUdaljenosti efektUdaljenosti = Pozicije.EfektUdaljenosti.Izracunaj(dizajn.pozeljnaPozicija + Pozicije.UobicajenaPozicija);
					opis.Add(jezik["opisTaktika"].tekst());
					opis.Add("");
					opis.Add(Pozicije.Naziv(dizajn.pozeljnaPozicija));
					opis.Add(jezik["opisTaktikaPrec"].tekst() + ": x" + efektUdaljenosti.Preciznost);
					opis.Add(jezik["opisSenzorSn"].tekst() + ": x" + efektUdaljenosti.SnagaSenzora);
					break;
			}

			return opis;
		}

		#region Novi dizajn
		private void ispisiOpis(InfoStranice stranica, Dizajn dizajn)
		{
			txtNDinfo.Lines = opis(stranica, dizajn).ToArray();
			prethodniNDinfo = stranica;
		}

		private void osvjeziNDstatistike()
		{
			Dizajn dizajn = dizajner.dizajn;
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];

			lblNDnosivost.Text = jezik["lblNDnosivost"].tekst() + ": " + Fje.PrefiksFormater(dizajner.odabranTrup.Nosivost);
			lblNDoklop.Text = jezik["lblNDoklop"].tekst() + " (" + dizajn.oklop.naziv + "): " + Fje.PrefiksFormater(dizajn.izdrzljivostOklopa);
			lblNDpokretljivost.Text = jezik["lblNDpokretljivost"].tekst() + " (" + dizajn.potisnici.naziv + "): " + Fje.PrefiksFormater(dizajn.pokretljivost);
			lblNDsenzori.Text = jezik["lblNDsenzori"].tekst() + " (" + dizajn.senzor.naziv + "): " + Fje.PrefiksFormater(dizajn.snagaSenzora);
			picNDSlika.Image = dizajner.odabranTrup.slika;
			lblNDcijena.Text = jezik["lblNDcijena"].tekst() + ": " + Fje.PrefiksFormater(dizajn.cijena);

			if (dizajn.primarnoOruzje != null)
				cbNDprimMisija.Items[cbNDprimMisija.SelectedIndex] = new TagTekst<Oruzje>(dizajn.primarnoOruzje.komponenta, Fje.PrefiksFormater(dizajn.primarnoOruzje.kolicina) + " x " + dizajn.primarnoOruzje.komponenta.naziv);
			else if (cbNDprimMisija.SelectedItem != null) {
				TagTekst<Oruzje> tagOruzje = (TagTekst<Oruzje>)cbNDprimMisija.SelectedItem;
				if (tagOruzje.tag != null) {
					tagOruzje.tekst = tagOruzje.tag.naziv;
					cbNDprimMisija.Items[cbNDprimMisija.SelectedIndex] = tagOruzje;
				}
			}

			if (dizajn.sekundarnoOruzje != null)
				cbNDsekMisija.Items[cbNDsekMisija.SelectedIndex] = new TagTekst<Oruzje>(dizajn.sekundarnoOruzje.komponenta, Fje.PrefiksFormater(dizajn.sekundarnoOruzje.kolicina) + " x " + dizajn.sekundarnoOruzje.komponenta.naziv);
			else if (cbNDsekMisija.SelectedItem != null) {
				TagTekst<Oruzje> tagOruzje = (TagTekst<Oruzje>)cbNDsekMisija.SelectedItem;
				if (tagOruzje.tag != null) {
					tagOruzje.tekst = tagOruzje.tag.naziv;
					cbNDsekMisija.Items[cbNDsekMisija.SelectedIndex] = tagOruzje;
				}
			}

			lblNDslobodno.Text = jezik["lblNDslobodno"].tekst() + ": " + Fje.PrefiksFormater(dizajner.slobodnaNosivost);
			ispisiOpis(prethodniNDinfo, dizajn);
			provjeriDizajn();
		}

		private void provjeriDizajn()
		{
			Dizajn dizajn = dizajner.dizajn;
			bool valid = true;

			if (dizajner.slobodnaNosivost < 0) valid = false;
			if (dizajn.ime == null) valid = false;
			else if (dizajn.ime.Trim().Length == 0) valid = false;

			if (!valid && btnSpremi.Enabled == true) btnSpremi.Enabled = false;
			if (valid && btnSpremi.Enabled == false) btnSpremi.Enabled = true;
		}

		private void prebaciNDopis(InfoStranice stranica)
		{
			if (zadrziNDInfo) {
				ispisiOpis(prethodniNDinfo, dizajner.dizajn);
				return;
			}

			if (cbNDinfoStrana.SelectedIndex == (int)stranica)
				ispisiOpis(stranica, dizajner.dizajn);
			else
				cbNDinfoStrana.SelectedIndex = (int)stranica;

			prethodniNDinfo = stranica;
		}

		private void cbNDvelicina_SelectedIndexChanged(object sender, EventArgs e)
		{
			Trup trup = (Trup)cbNDvelicina.SelectedItem;
			dizajner.odabranTrup = trup;

			if (dizajner.komponente[trup].mzPogon == null) {
				chNDMZpogon.Checked = false;
				chNDMZpogon.Enabled = false;
			}
			else
				chNDMZpogon.Enabled = true;

			foreach (ListViewItem item in lstvNDspecOprema.Items) {
				int indeks = (int)item.Tag;
				item.SubItems[2].Text = dizajner.trupKomponente.specijalnaOprema[indeks].velicina.ToString();
			}

			if (lstvNDspecOprema.SelectedItems.Count != 0) {
				SpecijalnaOprema so = dizajner.trupKomponente.specijalnaOprema[lstvNDspecOprema.SelectedIndices[0]];
				specijalnaOpremaZaOpis = so;
			}

			osvjeziNDstatistike();
			prebaciNDopis(InfoStranice.Pokretljivost);
		}

		private void cbNDprimMisija_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (prethodnaNDprimMisija == cbNDprimMisija.SelectedIndex)
				return;

			if (dizajner.dizajnPrimMisija != null) {
				TagTekst<Oruzje> tagTekst = (TagTekst<Oruzje>)cbNDprimMisija.Items[prethodnaNDprimMisija];
				cbNDprimMisija.Items[prethodnaNDprimMisija] = new TagTekst<Oruzje>(tagTekst.tag, tagTekst.tag.naziv);
			}
			prethodnaNDprimMisija = cbNDprimMisija.SelectedIndex;

			Oruzje misija = izvadiTag<Oruzje>(cbNDprimMisija);
			dizajner.dizajnPrimMisija = misija;
			osvjeziNDstatistike();
			prebaciNDopis(InfoStranice.PrimarnaMisija);
		}

		private void cbNDsekMisija_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (prethodnaNDsekMisija == cbNDsekMisija.SelectedIndex)
				return;

			if (dizajner.dizajnSekMisija != null) {
				TagTekst<Oruzje> tagTekst = (TagTekst<Oruzje>)cbNDsekMisija.Items[prethodnaNDsekMisija];
				cbNDsekMisija.Items[prethodnaNDsekMisija] = new TagTekst<Oruzje>(tagTekst.tag, tagTekst.tag.naziv);
			}
			prethodnaNDsekMisija = cbNDsekMisija.SelectedIndex;

			Oruzje misija = izvadiTag<Oruzje>(cbNDsekMisija);
			dizajner.dizajnSekMisija = misija;
			osvjeziNDstatistike();
			prebaciNDopis(InfoStranice.SekundarnaMisija);
		}

		private void chNDMZpogon_CheckedChanged(object sender, EventArgs e)
		{
			dizajner.dizajnMZPogon = chNDMZpogon.Checked;
			osvjeziNDstatistike();
			prebaciNDopis(InfoStranice.MZPogon);
		}

		private void hscrUdioMisija_ValueChanged(object sender, EventArgs e)
		{
			lblNDudioMisija.Text = hscrUdioMisija.Value + "%";
			dizajner.dizajnUdioPrimMisije = (100.0 - hscrUdioMisija.Value) / 100.0;
			osvjeziNDstatistike();

			if (prethodniNDinfo == InfoStranice.PrimarnaMisija)
				prebaciNDopis(InfoStranice.PrimarnaMisija);
			else
				prebaciNDopis(InfoStranice.SekundarnaMisija);
		}

		private void cbNDstit_SelectedIndexChanged(object sender, EventArgs e)
		{
			int stitIndeks = izvadiTag<int>(cbNDstit);
			if (stitIndeks < 0)
				dizajner.dizajnStit = null;
			else
				dizajner.dizajnStit = dizajner.komponente[dizajner.odabranTrup].stitovi[stitIndeks];

			osvjeziNDstatistike();
			prebaciNDopis(InfoStranice.Stit);
		}

		private void cbNDtaktika_SelectedIndexChanged(object sender, EventArgs e)
		{
			/*dizajner.dizajnPozicija = izvadiTag<Taktika>(cbNDtaktika);
			osvjeziNDstatistike();
			prebaciNDopis(InfoStranice.Taktika);*/
		}

		private void btnNDspecOpremaPlus_Click(object sender, EventArgs e)
		{
			if (lstvNDspecOprema.SelectedItems.Count == 0)
				return;

			promjeniNDspecOpremu(true, lstvNDspecOprema.SelectedIndices[0]);
		}

		private void btnNDspecOpremaMinus_Click(object sender, EventArgs e)
		{
			if (lstvNDspecOprema.SelectedItems.Count == 0)
				return;

			promjeniNDspecOpremu(false, lstvNDspecOprema.SelectedIndices[0]);
		}

		private void promjeniNDspecOpremu(bool dodaj, int soIndeks)
		{
			SpecijalnaOprema so = dizajner.trupKomponente.specijalnaOprema[soIndeks];
			int n;

			if (dodaj)
				n = dizajner.dodajSpecOpremu(so);
			else
				n = dizajner.makniSpecOpremu(so);

			if (n > 0)
				lstvNDspecOprema.Items[soIndeks].SubItems[0].Text = n + "x";
			else
				lstvNDspecOprema.Items[soIndeks].SubItems[0].Text = "";

			specijalnaOpremaZaOpis = so;

			osvjeziNDstatistike();
			prebaciNDopis(InfoStranice.SpecijalnaOprema);
		}

		private void cbNDinfoStrana_SelectedIndexChanged(object sender, EventArgs e)
		{
			InfoStranice stranica = ((TagTekst<InfoStranice>)cbNDinfoStrana.SelectedItem).tag;
			ispisiOpis(stranica, dizajner.dizajn);
		}

		private void txtNDnaziv_TextChanged(object sender, EventArgs e)
		{
			dizajner.dizajnIme = txtNDnaziv.Text;
			provjeriDizajn();
		}

		private void btnNDinfoPrethodna_Click(object sender, EventArgs e)
		{
			if (cbNDinfoStrana.SelectedIndex <= 0)
				cbNDinfoStrana.SelectedIndex = cbNDinfoStrana.Items.Count - 1;
			else
				cbNDinfoStrana.SelectedIndex--;
		}

		private void btnNDinfoSlijedeca_Click(object sender, EventArgs e)
		{
			if (cbNDinfoStrana.SelectedIndex + 1 >= cbNDinfoStrana.Items.Count)
				cbNDinfoStrana.SelectedIndex = 0;
			else
				cbNDinfoStrana.SelectedIndex++;
		}

		private void lstvNDspecOprema_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstvNDspecOprema.SelectedItems.Count == 0)
				return;

			SpecijalnaOprema so = dizajner.trupKomponente.specijalnaOprema[lstvNDspecOprema.SelectedIndices[0]];
			specijalnaOpremaZaOpis = so;
			prebaciNDopis(InfoStranice.SpecijalnaOprema);
		}

		private void lstvNDspecOprema_ItemActivate(object sender, EventArgs e)
		{
			if (lstvNDspecOprema.SelectedItems.Count == 0)
				return;

			promjeniNDspecOpremu(true, lstvNDspecOprema.SelectedIndices[0]);
		}

		private void btnSpremi_Click(object sender, EventArgs e)
		{
			HashSet<Sazetak> postojeciDizajnovi = new HashSet<Sazetak>();
			foreach (DizajnZgrada dizajnZgrada in igrac.dizajnoviBrodova)
				postojeciDizajnovi.Add(dizajnZgrada.dizajn.stil);

			if (postojeciDizajnovi.Contains(dizajner.dizajn.stil)) {
				Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];
				MessageBox.Show(jezik["opIstiDizajnTekst"].tekst(), jezik["opIstiDizajnNaslov"].tekst(), MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			igrac.dodajDizajn(dizajner.dizajn);
			dodajDizajn(dizajner.dizajn);
			tabvCtrlFlote.SelectedTab = tabDizajnovi;

			dizajner.reset(igrac);
			cbNDvelicina.SelectedItem = cbNDvelicina.SelectedItem;
			cbNDvelicina.SelectedIndex = cbNDvelicina.SelectedIndex;
			cbNDprimMisija.SelectedIndex = cbNDprimMisija.SelectedIndex;
			cbNDsekMisija.SelectedIndex = cbNDsekMisija.SelectedIndex;
			cbNDstit.SelectedIndex = cbNDstit.SelectedIndex;
			cbNDtaktika.SelectedIndex = cbNDtaktika.SelectedIndex;
			hscrUdioMisija.Value = hscrUdioMisija.Value;
			txtNDnaziv.Text = "";
		}

		private void bntNDZadrziInfo_Click(object sender, EventArgs e)
		{
			if (zadrziNDInfo) {
				zadrziNDInfo = false;
				btnNDZadrziInfo.Text = "";
			}
			else {
				zadrziNDInfo = true;
				btnNDZadrziInfo.Text = "*";
			}
		}
		#endregion

		private void dodajDizajn(Dizajn dizajn)
		{
			ListViewItem item = new ListViewItem(dizajn.ime);
			item.SubItems.Add(Fje.PrefiksFormater(dizajn.brojBrodova));
			item.Tag = dizajn;

			lstvDizajnovi.SmallImageList.Images.Add(dizajn.ikona);
			item.ImageIndex = lstvDizajnovi.SmallImageList.Images.Count - 1;

			lstvDizajnovi.Items.Add(item);
		}

		private void lstvDizajnovi_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstvDizajnovi.SelectedItems.Count == 0)
				return;
			Dizajn dizajn = (Dizajn)lstvDizajnovi.SelectedItems[0].Tag;

			picSlikaDizajna.Image = dizajn.slika;

			List<string> opisDizajna = new List<string>();
			opisDizajna.Add(dizajn.ime);
			opisDizajna.Add(dizajn.trup.naziv);
			opisDizajna.Add("");

			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];
			foreach (InfoStranice stranica in Enum.GetValues(typeof(InfoStranice))) {
				if (stranica == InfoStranice.SpecijalnaOprema)
					foreach (SpecijalnaOprema so in dizajn.specijalnaOprema.Keys) {
						specijalnaOpremaZaOpis = so;
						opisDizajna.AddRange(opis(stranica, dizajn, false));
						opisDizajna.Add("");
					}
				else {
					opisDizajna.AddRange(opis(stranica, dizajn, false));
					opisDizajna.Add("");
				}

				if (stranica == InfoStranice.SekundarnaMisija) {
					opisDizajna.Add(jezik["opisOklop"].tekst() + ":");
					opisDizajna.Add("");
					opisDizajna.Add(dizajn.oklop.naziv);
					opisDizajna.Add(jezik["opisOklopIzd"].tekst() + ": " + dizajn.izdrzljivostOklopa);
					opisDizajna.Add("");
				}
			}

			txtDizajnInfo.Lines = opisDizajna.ToArray();

			if (dizajn.brojBrodova == 0) btnUkloniDizajn.Enabled = true;
			else btnUkloniDizajn.Enabled = false;
		}

		private void btnUkloniDizajn_Click(object sender, EventArgs e)
		{
			if (lstvDizajnovi.SelectedItems.Count == 0)
				return;
			int indeks = lstvDizajnovi.SelectedIndices[0];

			if (igrac.dizajnoviBrodova[indeks].dizajn.brojBrodova == 0) {
				DizajnZgrada dizajnZgrada = igrac.dizajnoviBrodova[indeks];
				if (!igrac.dizajnoviUGradnji().Contains(dizajnZgrada.dizajn)) {
					igrac.dizajnoviBrodova.RemoveAt(indeks);
					lstvDizajnovi.Items.RemoveAt(indeks);
				}
			}
		}

		private void btnNoviDizajn_Click(object sender, EventArgs e)
		{
			using(var formDizajn = new FormDizajn(igrac))
				if (formDizajn.ShowDialog() == DialogResult.OK) {
					igrac.dodajDizajn(formDizajn.Dizajn);
					dodajDizajn(formDizajn.Dizajn);
				}
		}
	}
}
