using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Igra.Poruke;
using Zvjezdojedac.Igra;
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.GUI
{
	public partial class FormIgra : Form
	{
		const int RazineUvecanja = 10;

		private PrikazMape prikazMape;
		private IgraZvj igra;
		private Igrac igrac;
		private Tocka<double> pomakPogleda;

		FormFlotaPokret frmFlotaPokret;
		int razinaUvecanja = 4;
		BriefPlanetItem[] planetInfoi = new BriefPlanetItem[15];

		public FormIgra(IgraZvj igra)
		{
			InitializeComponent();

			this.igra = igra;
			igrac = igra.trenutniIgrac();

			this.frmFlotaPokret = new FormFlotaPokret(this);
			this.AddOwnedForm(frmFlotaPokret);

			pomakPogleda = null;
			prikazMape = new PrikazMape(igra);
			this.picMapa.Image = prikazMape.slikaMape;

			tabCtrlDesno.ImageList = new ImageList();
			tabCtrlDesno.ImageList.ImageSize = new Size(32, 32);
			tabCtrlDesno.ImageList.Images.Add(new Bitmap(1, 1));
			tabCtrlDesno.ImageList.Images.Add(Slike.PlanetTab[Planet.Tip.ASTEROIDI][0].image);
			tabCtrlDesno.ImageList.Images.Add(Slike.FlotaTab);
			tabPageZvijezda.ImageIndex = 0;
			tabPageKolonija.ImageIndex = 1;
			tabPageFlote.ImageIndex = 2;

			//listViewPlaneti.LargeImageList = new ImageList();
			//listViewPlaneti.LargeImageList.ImageSize = new Size(32, 32);
			//Image[] planetImages = new Image[Slike.PlanetImageIndex.Count];
			//foreach (Image img in Slike.PlanetImageIndex.Keys)
			//	planetImages[Slike.PlanetImageIndex[img]] = img;
			//listViewPlaneti.LargeImageList.Images.AddRange(planetImages);

			btnCivilnaGradnja.Text = "";
            btnVojnaGradnja.Text = "";
			
			tvFlota.ImageList = new ImageList();
			tvFlota.ImageList.ImageSize = new Size(20, 20);
			postaviJezik();
			postaviAkcijeBroda();

			for (int i = 0; i < 15; i++) {
				BriefPlanetItem planetInfo = new BriefPlanetItem();
				planetInfo.Click += this.listViewPlaneti_Click;

				planetInfoi[i] = planetInfo;
				planetiFlowPanel.Controls.Add(planetInfo);
			}

			this.Font = Postavke.FontSucelja(this.Font);
		}

		private void postaviJezik()
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormIgra];

			btnEndTurn.Text = jezik["btnEndTurn"].tekst();
			btnFlotaPokret.Text = jezik["btnFlotaPokret"].tekst();
			btnPlanetInfo.Text = jezik["btnPlanetInfo"].tekst();

			floteMenu.Text = jezik["floteMenu"].tekst();
			izbornikMenu.Text = jezik["izbornikMenu"].tekst();
			izlazMenu.Text = jezik["izlazMenu"].tekst();
			kolonijeMenu.Text = jezik["kolonijeMenu"].tekst();
			novaIgraMenu.Text = jezik["novaIgraMenu"].tekst();
			spremiMenu.Text = jezik["spremiMenu"].tekst();
			tehnologijeMenu.Text = jezik["tehnologijeMenu"].tekst();
			ucitajMenu.Text = jezik["ucitajMenu"].tekst();

			jezik = Postavke.Jezik[Kontekst.Kolonija];
			groupPoStan.Text = jezik["groupPoStan"].tekst();
			groupCivGradnja.Text = jezik["Civilna_Gradnja"].tekst();
			//groupVojGradnja.Text = jezik["Vojna_Gradnja"].tekst();
		}

		private void frmIgra_Load(object sender, EventArgs e)
		{
			noviKrugPogled();
		}

		private void noviKrugPogled()
		{
			Dictionary<string, double> vars = new Dictionary<string, double>();
			vars.Add("KRUG", igra.brKruga);
			vars.Add("BR_PORUKA", igrac.poruke.Count);
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormIgra];

			lblBrojKruga.Text = jezik["lblBrojKruga"].tekst(vars);
			odaberiZvijezdu(igrac.odabranaZvijezda, false);
			centrirajZvijezdu(igrac.odabranaZvijezda);

			novostiMenu.Text = jezik["novostiMenu"].tekst(vars);

			odaberiPlanet(igrac.OdabranPlanet, true);			
		}

		private void centrirajZvijezdu(Zvijezda zvijezda)
		{
			int x = prikazMape.XsaMape(igrac.odabranaZvijezda.x);
			int y = prikazMape.YsaMape(igrac.odabranaZvijezda.y);
			x = odrediPomakScrolla(picMapa.Width, pnlMapa.Width, pnlMapa.HorizontalScroll, x, 0);
			y = odrediPomakScrolla(picMapa.Height, pnlMapa.Height, pnlMapa.VerticalScroll, y, 0);
			pnlMapa.HorizontalScroll.Value += x;
			pnlMapa.VerticalScroll.Value += y;
			pomakPogleda = null;
		}

		private void picMapa_Click(object sender, EventArgs e)
		{
			MouseEventArgs mouseEvent = (MouseEventArgs)e;
			int x = mouseEvent.X;
			int y = mouseEvent.Y;

			Zvijezda odabranaZvijezda = igra.mapa.najblizaZvijezda(
				prikazMape.XnaMapi(x),
				prikazMape.YnaMapi(y), 0.5);

			if (odabranaZvijezda != null) {
				if (frmFlotaPokret != null && frmFlotaPokret.Visible) {
					frmFlotaPokret.postaviOdrediste(odabranaZvijezda);
					return;
				}
				else
					odaberiZvijezdu(odabranaZvijezda, true);
			}

			pomakPogleda = new Tocka<double>(x / (double)picMapa.Width, y / (double)picMapa.Height);
		}

		public void prikaziFlotu(Zvijezda zvijezda)
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormIgra];

			tvFlota.Nodes.Clear();
			tvFlota.ImageList.Images.Clear();
			foreach (Igrac _igrac in igra.igraci)
				tvFlota.ImageList.Images.Add(Slike.FlotaTabBoja[_igrac.boja]);

			if (igrac.floteStacionarne.ContainsKey(zvijezda)) {
				Flota flota = igrac.floteStacionarne[zvijezda];
				TreeNode nodeStacionarnaFloata = new TreeNode(jezik["flotaObrana"].tekst(null));
				nodeStacionarnaFloata.ImageIndex = igrac.id;
				nodeStacionarnaFloata.Tag = flota;
				tvFlota.Nodes.Add(nodeStacionarnaFloata);

				foreach (Brod brod in flota.brodovi.Values) {
					TreeNode node = new TreeNode(brod.dizajn.ime + " x " + Fje.PrefiksFormater(brod.kolicina));
					tvFlota.ImageList.Images.Add(brod.dizajn.trup.slika);
					node.ImageIndex = tvFlota.ImageList.Images.Count - 1;
					node.SelectedImageIndex = node.ImageIndex;
					node.Tag = brod;
					nodeStacionarnaFloata.Nodes.Add(node);
				}
			}
			tvFlota.ExpandAll();
			postaviAkcijeBroda();
		}

		private void odaberiZvijezdu(Zvijezda zvijezda, bool promjeniTab)
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormIgra];

			igrac.odabranaZvijezda = zvijezda;
			if (promjeniTab) tabCtrlDesno.SelectedTab = tabPageZvijezda;
			
			bool imaKoloniju = false;
			
				for (int i = 0; i < zvijezda.planeti.Count; i++) {
					if (igrac.posjeceneZvjezde.Contains(zvijezda))
						planetInfoi[i].PostaviPlanet(zvijezda.planeti[i], igrac);
					else
						planetInfoi[i].PostaviNeistrazeno(i);
				}

			prikaziFlotu(zvijezda);

			bool igracevSustav = (igrac.OdabranSustav != null);

			if (igracevSustav) {
				hscrZvjezdaGradnja.Value = (int)(igrac.OdabranSustav.UdioGradnje * hscrZvjezdaGradnja.Maximum);
				lblImeZvjezde.Text = zvijezda.ime + "\n" +
					jezik["lblZracenje"].tekst() + ": " + zvijezda.zracenje() + "\n" +
					jezik["lblMigracija"].tekst() + ": " + Fje.PrefiksFormater(zvijezda.efektiPoIgracu[igrac.id][Kolonija.MigracijaMax]);
			}
			else {
				lblImeZvjezde.Text = zvijezda.ime + "\n" +
					jezik["lblZracenje"].tekst() + ": " + zvijezda.zracenje();
			}

			btnVojnaGradnja.Visible = igracevSustav;
			hscrZvjezdaGradnja.Visible = igracevSustav;
			lblVojnaGradnja.Visible = igracevSustav;
			lblProcjenaVojneGradnje.Visible = igracevSustav;
			lblRazvoj.Visible = igracevSustav;

			osvjeziMapu();
			tabCtrlDesno.ImageList.Images[0] = Slike.ZvijezdaTab[zvijezda.tip];
			tabCtrlDesno.Refresh();

			btnSlijedecaKolonija.Visible = imaKoloniju;
			btnPrethodnaKolonija.Visible = imaKoloniju;
		}

		public void osvjeziMapu()
		{
			picMapa.Image = prikazMape.osvjezi();
		}

		private void odaberiPlanet(Planet planet, bool promjeniTab)
		{
			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.Kolonija];

			if (planet.tip == Planet.Tip.NIKAKAV) return;
			igrac.OdabranPlanet = planet;
			if (promjeniTab) tabCtrlDesno.SelectedTab = tabPageKolonija;
			tabCtrlDesno.ImageList.Images[1] = planet.slika;

			picSlikaPlaneta.Image = planet.slika;
			lblImePlaneta.Text = planet.ime;
			if (planet.kolonija != null) {
				groupPoStan.Visible = true;
				groupCivGradnja.Visible = true;
				lblRazvoj.Visible = true;

				lblPopulacija.Text = jezik["plPopulacija"].tekst() + ":\n" + Fje.PrefiksFormater(planet.kolonija.populacija) + " / " + Fje.PrefiksFormater(planet.kolonija.Efekti[Kolonija.PopulacijaMax]);
				hscrCivilnaIndustrija.Enabled = true;
				btnCivilnaGradnja.Enabled = true;

				hscrZvjezdaGradnja.Enabled = true;
				btnVojnaGradnja.Enabled = true;
				osvjeziPogledNaKoloniju();
			}
			else {
				lblPopulacija.Text = jezik["plNenaseljeno"].tekst();
				groupPoStan.Visible = false;
				groupCivGradnja.Visible = false;
				lblRazvoj.Visible = false;
			}
		}

		int odrediPomakScrolla(int sirinaKlijenta, int sirinaPrikaza, ScrollProperties scroll, int zeljenaPozicija, double brzina)
		{
			if (sirinaPrikaza >= sirinaKlijenta) return 0;

			double skala = (sirinaKlijenta - sirinaPrikaza) / (double)(scroll.Maximum - scroll.LargeChange);
			int pozicijaPrikaza = sirinaPrikaza / 2 + (int)(scroll.Value * skala);

			zeljenaPozicija = Fje.Ogranici(zeljenaPozicija, sirinaPrikaza / 2 + 1, sirinaKlijenta - sirinaPrikaza / 2 - 1);
			double delta = zeljenaPozicija - pozicijaPrikaza;

			if (brzina == 0)
				return (int)delta;

			if (Math.Abs(delta) < brzina + 2)
				if (Math.Abs(delta) < 2)
					return (int)(delta * skala);
				else
					return Math.Sign(delta);
			else
				return (int)(delta / (brzina * skala));
		}

		private void timerAnimacija_Tick(object sender, EventArgs e)
		{
			if (pomakPogleda == null)
				return;

			int pomakNaX = (int)(picMapa.Width * pomakPogleda.x);
			int pomakNaY = (int)(picMapa.Height * pomakPogleda.y);
			int dx = odrediPomakScrolla(picMapa.Width, pnlMapa.Width, pnlMapa.HorizontalScroll, pomakNaX, 4);
			int dy = odrediPomakScrolla(picMapa.Height, pnlMapa.Height, pnlMapa.VerticalScroll, pomakNaY, 4);

			if (dx == 0)
				dx = odrediPomakScrolla(picMapa.Width, pnlMapa.Width, pnlMapa.HorizontalScroll, pomakNaX, 4);
			
			pnlMapa.HorizontalScroll.Value += dx;
			pnlMapa.VerticalScroll.Value += dy;
			
			if (dx == 0 && dy == 0)
					pomakPogleda = null;
		}

		private void listViewPlaneti_Click(object sender, EventArgs e)
		{
			BriefPlanetItem planetInfo = sender as BriefPlanetItem;
			odaberiPlanet(planetInfo.Planet, true);
		}

		private void btnEndTurn_Click(object sender, EventArgs e)
		{
			igra.slijedeciIgrac();
			noviKrugPogled();

			bool imaPoruka = false;
			var filtriranePoruke = igrac.FiltriranePoruke();

			foreach (Poruka.Tip tip in filtriranePoruke.Keys)
				if (igrac.filtarPoruka[tip] && filtriranePoruke[tip].Count > 0)
					imaPoruka = true;

			if (imaPoruka)
				novostiMenu_Click(this, null);
		}

		private void btnPlanetInfo_Click(object sender, EventArgs e)
		{
			if (igrac.OdabranPlanet.kolonija != null)
			{
				FormPlanetInfo planetInfo = new FormPlanetInfo(igrac.OdabranPlanet.kolonija);
				planetInfo.ShowDialog();
				osvjeziPogledNaKoloniju();
			}
			else {
				FormPlanetInfo planetInfo = new FormPlanetInfo(igrac, igrac.OdabranPlanet);
				planetInfo.ShowDialog();
			}
		}

		private void osvjeziLabele()
		{
			Kolonija kolonija = igrac.OdabranPlanet.kolonija;
			ZvjezdanaUprava sustav = igrac.OdabranSustav;
			sustav.IzracunajEfekte();

			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.Kolonija];

			if (kolonija != null) {
				lblHranaPoStan.Text = jezik["HranaPoStan"].tekst() + ": " + kolonija.Efekti[Kolonija.HranaPoFarmeru].ToString("0.##");
				lblRudePoStan.Text = jezik["RudePoStan"].tekst() + ": " + kolonija.Efekti[Kolonija.RudePoRudaru].ToString("0.##");
				lblOdrzavanjePoStan.Text = jezik["OdrzavanjePoStan"].tekst() + ": " + (kolonija.Efekti[Kolonija.OdrzavanjeUkupno] / kolonija.Efekti[Kolonija.Populacija]).ToString("0.##");
				lblIndustrijaPoStan.Text = jezik["IndustrijaPoStan"].tekst() + ": " + kolonija.Efekti[Kolonija.IndPoRadnikuEfektivno].ToString("0.##");
				lblRazvojPoStan.Text = jezik["RazvojPoStan"].tekst() + ": " + kolonija.Efekti[Kolonija.RazPoRadnikuEfektivno].ToString("0.##");

				lblCivilnaIndustrija.Text = Fje.PrefiksFormater(kolonija.poeniIndustrije()) + " " + jezik["jedInd"].tekst();
				lblProcjenaCivilneGradnje.Text = kolonija.ProcjenaVremenaGradnje();
			}

			if (sustav != null) {
				lblVojnaGradnja.Text = Fje.PrefiksFormater(sustav.PoeniIndustrije) + " " + jezik["jedInd"].tekst();
				lblProcjenaVojneGradnje.Text = sustav.ProcjenaVremenaGradnje();
				lblRazvoj.Text = jezik["lblRazvoj"].tekst() + Fje.PrefiksFormater(sustav.PoeniRazvoja);
			}

			if (igrac.OdabranPlanet.kolonija.RedGradnje.Count > 0)
			{
				btnCivilnaGradnja.Image = igrac.OdabranPlanet.kolonija.RedGradnje.First.Value.slika;
				btnCivilnaGradnja.Text = "";
			}
			else
			{
				btnCivilnaGradnja.Image = null;
				btnCivilnaGradnja.Text = jezik["Civilna_Gradnja"].tekst();
			}

			if (igrac.OdabranSustav.RedGradnje.Count > 0) {
				btnVojnaGradnja.Image = igrac.OdabranSustav.RedGradnje.First.Value.slika;
				btnVojnaGradnja.Text = "";
			}
			else {
				btnVojnaGradnja.Image = null;
				btnVojnaGradnja.Text = jezik["Vojna_Gradnja"].tekst();
			}
		}

		private void osvjeziPogledNaKoloniju()
		{
			hscrCivilnaIndustrija.Value = (int)(igrac.OdabranPlanet.kolonija.CivilnaIndustrija * hscrCivilnaIndustrija.Maximum);

			osvjeziLabele();
		}

		private void postaviAkcijeBroda()
		{
			if (tvFlota.SelectedNode == null) {
				btnPrimAkcijaBroda.Visible = false;
				btnSekAkcija.Visible = false;
				btnFlotaPokret.Enabled = false;
				return;
			}

			if (tvFlota.SelectedNode.Parent == null) {
				btnPrimAkcijaBroda.Visible = false;
				btnSekAkcija.Visible = false;
				btnFlotaPokret.Enabled = true;
			}
			else if (tvFlota.SelectedNode.Tag != null) {
				Brod brod = (Brod)tvFlota.SelectedNode.Tag;

				if (brod.dizajn.primarnoOruzje != null) {
					btnPrimAkcijaBroda.Visible = (brod.dizajn.primarnaMisija == Misija.Tip.Kolonizacija);
					btnPrimAkcijaBroda.Text = Misija.Opisnici[brod.dizajn.primarnaMisija].naziv;
				}
				else
					btnPrimAkcijaBroda.Visible = false;

				if (brod.dizajn.sekundarnoOruzje != null) {
					btnSekAkcija.Visible = (brod.dizajn.sekundarnaMisija == Misija.Tip.Kolonizacija);
					btnSekAkcija.Text = Misija.Opisnici[brod.dizajn.sekundarnaMisija].naziv;
				}
				else
					btnSekAkcija.Visible = false;

				btnFlotaPokret.Enabled = brod.dizajn.MZPogon != null;

			}
		}

		private void hscrCivilnaIndustrija_Scroll(object sender, ScrollEventArgs e)
		{
			if (igrac.OdabranPlanet.kolonija != null)
			{
				igrac.OdabranPlanet.kolonija.CivilnaIndustrija = e.NewValue / (double)hscrCivilnaIndustrija.Maximum;
				int val = (int)Math.Round(igrac.OdabranPlanet.kolonija.CivilnaIndustrija * hscrCivilnaIndustrija.Maximum, MidpointRounding.AwayFromZero);
				if (hscrCivilnaIndustrija.Value != val)
					e.NewValue = val;
				else
					osvjeziLabele();
			}
		}

		private void hscrZvjezdanaGradnja_Scroll(object sender, ScrollEventArgs e)
		{
			if (igrac.OdabranPlanet.kolonija != null)
			{
				igrac.OdabranSustav.UdioGradnje = e.NewValue / (double)hscrZvjezdaGradnja.Maximum;
				int val = (int)Math.Round(igrac.OdabranSustav.UdioGradnje * hscrZvjezdaGradnja.Maximum, MidpointRounding.AwayFromZero);
				if (hscrZvjezdaGradnja.Value != val)
					e.NewValue = val;
				else
					osvjeziLabele();
			}
		}

		private void btnCivilnaGradnja_Click(object sender, EventArgs e)
		{
			IGradiliste kolonija = igrac.OdabranPlanet.kolonija;

			if (FormGradnja.JeValjanoGradiliste(kolonija, igrac)) {
				using (FormGradnja frmGradnja = new FormGradnja(kolonija)) {

					if (frmGradnja.ShowDialog() == DialogResult.OK)
						osvjeziLabele();
				}
			}
		}

		private void btnVojnaGradnja_Click(object sender, EventArgs e)
		{
			IGradiliste uprava = igrac.odabranaZvijezda.efektiPoIgracu[igrac.id];

			if (FormGradnja.JeValjanoGradiliste(uprava, igrac))
				using (FormGradnja frmGradnja = new FormGradnja(uprava)) {

					if (frmGradnja.ShowDialog() == DialogResult.OK)
						osvjeziLabele();
				}
		}

		private void tvFlota_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Tag == null) return;
			if (e.Node.Parent == null) return;

			Brod brod = (Brod)e.Node.Tag;
			
			HashSet<Misija.Tip> misije = new HashSet<Misija.Tip>();
			if (brod.dizajn.primarnoOruzje != null) misije.Add(brod.dizajn.primarnoOruzje.komponenta.misija);
			if (brod.dizajn.sekundarnoOruzje != null) misije.Add(brod.dizajn.sekundarnoOruzje.komponenta.misija);

			if (misije.Contains(Misija.Tip.Kolonizacija)) {
				FormKolonizacija formKolonizacija = new FormKolonizacija(igra, igrac, brod, igrac.odabranaZvijezda);
				formKolonizacija.ShowDialog();
			}
		}

		private void FormIgra_FormClosed(object sender, FormClosedEventArgs e)
		{
			prikazMape.Dispose();
		}

		private void tvFlota_AfterSelect(object sender, TreeViewEventArgs e)
		{
			postaviAkcijeBroda();
		}

		private void btnFlotaPokret_Click(object sender, EventArgs e)
		{
			if (tvFlota.SelectedNode == null) {
				btnFlotaPokret.Enabled = false;
				return;
			}
			
			if (frmFlotaPokret.IsDisposed) {
				this.RemoveOwnedForm(frmFlotaPokret);
				this.frmFlotaPokret = new FormFlotaPokret(this);
				this.AddOwnedForm(frmFlotaPokret);
			}

			if (tvFlota.SelectedNode.Parent == null)
				frmFlotaPokret.pomicanjeFlote((Flota)tvFlota.SelectedNode.Tag, igrac, igra);
			else if (tvFlota.SelectedNode.Tag != null) {
				Flota flota = (Flota)tvFlota.SelectedNode.Parent.Tag;
				frmFlotaPokret.pomicanjeBroda(flota, (Brod)tvFlota.SelectedNode.Tag, igrac, igra);
			}
		}

		private void btnPrimAkcijaBroda_Click(object sender, EventArgs e)
		{
			if (tvFlota.SelectedNode.Tag == null) return;
			if (tvFlota.SelectedNode.Parent == null) return;

			Brod brod = (Brod)tvFlota.SelectedNode.Tag;

			switch (brod.dizajn.primarnaMisija) {
				case Misija.Tip.Kolonizacija:
					FormKolonizacija formKolonizacija = new FormKolonizacija(igra, igrac, brod, igrac.odabranaZvijezda);
					formKolonizacija.ShowDialog();
					break;
			}
		}

		private void odaberiDruguKoloniju(int smjer)
		{
			Zvijezda zvj = igrac.OdabranPlanet.zvjezda;
			for (int planetIndeks = igrac.OdabranPlanet.pozicija + smjer; planetIndeks >= 0 && planetIndeks < zvj.planeti.Count; planetIndeks += smjer) {
				Kolonija kolonija = zvj.planeti[planetIndeks].kolonija;
				
				if (kolonija != null && kolonija.Igrac == igrac) {
					odaberiPlanet(zvj.planeti[planetIndeks], false);
					return;
				}
			}
		}

		private void btnPrethodnaKolonija_Click(object sender, EventArgs e)
		{
			odaberiDruguKoloniju(-1);
		}

		private void btnSlijedecaKolonija_Click(object sender, EventArgs e)
		{
			odaberiDruguKoloniju(1);
		}

		private void FormIgra_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ')
				novostiMenu_Click(sender, e);
		}

		private void novostiMenu_Click(object sender, EventArgs e)
		{
			using (FormPoruke formPoruke = new FormPoruke(igrac)) {
				Zvijezda zvj = null;
				Planet planet = null;

				if (formPoruke.ShowDialog() == DialogResult.OK)
					if (formPoruke.odabranaProuka != null) {
						Poruka poruka = formPoruke.odabranaProuka;
						switch (formPoruke.odabranaProuka.tip) {
							case Poruka.Tip.Brod:
								zvj = ((PorukaBrod)poruka).zvijezda;
								odaberiZvijezdu(zvj, false);
								tabCtrlDesno.SelectedTab = tabPageFlote;
								centrirajZvijezdu(zvj);
								break;
							case Poruka.Tip.Kolonija:
								planet = ((PorukaKolonija)poruka).planet;
								odaberiZvijezdu(planet.zvjezda, false);
								odaberiPlanet(planet, true);
								centrirajZvijezdu(planet.zvjezda);
								break;
							case Poruka.Tip.Tehnologija:
								tehnologijeMenu_Click(sender, e);
								break;
							case Poruka.Tip.ZgradaKolonija:
								planet = ((PorukaZgradaKolonija)poruka).planet;
								odaberiZvijezdu(planet.zvjezda, false);
								odaberiPlanet(planet, true);
								centrirajZvijezdu(planet.zvjezda);
								break;
						}
					}
			}
		}

		private void floteMenu_Click(object sender, EventArgs e)
		{
			using (FormFlote formaFlote = new FormFlote(igrac))
				formaFlote.ShowDialog();
		}

		private void tehnologijeMenu_Click(object sender, EventArgs e)
		{
			using (FormTechIzbor frmTech = new FormTechIzbor(igrac))
				frmTech.ShowDialog();
		}

		private void spremiMenu_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog dialog = new SaveFileDialog()) {
				dialog.InitialDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "pohranjeno";
				dialog.FileName = "sejv.igra";
				dialog.Filter = Postavke.Jezik[Kontekst.WindowsDijalozi, "TIP_SEJVA"].tekst(null) + " (*.igra)|*.igra";

				if (dialog.ShowDialog() == DialogResult.OK) {
					GZipStream zipStream = new GZipStream(new FileStream(dialog.FileName, FileMode.Create), CompressionMode.Compress);
					StreamWriter pisac = new StreamWriter(zipStream);
					pisac.Write(igra.spremi());
					pisac.Close();
				}
			}
		}

		private void ucitajMenu_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog dialog = new OpenFileDialog()) {
				dialog.InitialDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "pohranjeno";
				dialog.FileName = "sejv.igra";
				dialog.Filter = Postavke.Jezik[Kontekst.WindowsDijalozi, "TIP_SEJVA"].tekst(null) + " (*.igra)|*.igra";

				if (dialog.ShowDialog() == DialogResult.OK) {

					GZipStream zipStream = new GZipStream(new FileStream(dialog.FileName, FileMode.Open), CompressionMode.Decompress);
					StreamReader citac = new StreamReader(zipStream);

					string ucitanaIgra = citac.ReadToEnd();
					citac.Close();

					this.igra = IgraZvj.Ucitaj(ucitanaIgra);
					igrac = igra.trenutniIgrac();

					pomakPogleda = null;
					prikazMape = new PrikazMape(igra);
					this.picMapa.Image = prikazMape.slikaMape;
					noviKrugPogled();
				}
			}
		}

		void postaviZoom()
		{
			if (razinaUvecanja < 0) razinaUvecanja = 0;
			if (razinaUvecanja > RazineUvecanja) razinaUvecanja = RazineUvecanja;

			prikazMape.zoom(razinaUvecanja / (double)RazineUvecanja);

			picMapa.Image = prikazMape.slikaMape;
		}

		private void uvecajMenu_Click(object sender, EventArgs e)
		{
			razinaUvecanja++;
			postaviZoom();
		}

		private void umanjiMenu_Click(object sender, EventArgs e)
		{
			razinaUvecanja--;
			postaviZoom();
		}

	}
}
