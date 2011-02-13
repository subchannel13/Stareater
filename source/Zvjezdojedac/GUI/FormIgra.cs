using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using Alati;
using Prototip.Podaci;
using Prototip.Podaci.Jezici;
using Prototip.Igra.Poruke;

namespace Prototip
{
	public partial class FormIgra : Form
	{
		private PrikazMape prikazMape;
		private IgraZvj igra;
		private Igrac igrac;
		private Tocka<double> pomakPogleda;

		private FormFlotaPokret frmFlotaPokret;

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

			listViewPlaneti.LargeImageList = new ImageList();
			listViewPlaneti.LargeImageList.ImageSize = new Size(32, 32);
			Image[] planetImages = new Image[Slike.PlanetImageIndex.Count];
			foreach (Image img in Slike.PlanetImageIndex.Keys)
				planetImages[Slike.PlanetImageIndex[img]] = img;
			listViewPlaneti.LargeImageList.Images.AddRange(planetImages);

			btnCivilnaGradnja.Text = "";
            btnVojnaGradnja.Text = "";
			
			tvFlota.ImageList = new ImageList();
			tvFlota.ImageList.ImageSize = new Size(20, 20);
			postaviJezik();
			postaviAkcijeBroda();
		}

		private void postaviJezik()
		{
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormIgra];

			btnEndTurn.Text = jezik["btnEndTurn"].tekst();
			btnFlote.Text = jezik["btnFlote"].tekst();
			btnFlotaPokret.Text = jezik["btnFlotaPokret"].tekst();
			btnPlanetInfo.Text = jezik["btnPlanetInfo"].tekst();
			btnSpremi.Text = jezik["btnSpremi"].tekst();
			btnTech.Text = jezik["btnTech"].tekst();
			btnUcitaj.Text = jezik["btnUcitaj"].tekst();

			jezik = Postavke.jezik[Kontekst.Kolonija];
			groupPoStan.Text = jezik["groupPoStan"].tekst();
			groupCivGradnja.Text = jezik["Civilna_Gradnja"].tekst();
			groupVojGradnja.Text = jezik["Vojna_Gradnja"].tekst();
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
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormIgra];

			lblBrojKruga.Text = jezik["lblBrojKruga"].tekst(vars);
			odaberiZvijezdu(igrac.odabranaZvijezda, false);
			centrirajZvijezdu(igrac.odabranaZvijezda);

			btnPoruke.Text = jezik["btnPoruke"].tekst(vars);

			odaberiPlanet(igrac.odabranPlanet, true);			
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

			if (frmFlotaPokret != null)
				if (frmFlotaPokret.Visible) {
					frmFlotaPokret.postaviOdrediste(odabranaZvijezda);
					return;
				}

			if (odabranaZvijezda != null)
				odaberiZvijezdu(odabranaZvijezda,true);

			pomakPogleda = new Tocka<double>(x / (double)picMapa.Width, y / (double)picMapa.Height);
		}

		public void prikaziFlotu(Zvijezda zvijezda)
		{
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormIgra];

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
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormIgra];

			igrac.odabranaZvijezda = zvijezda;
			if (promjeniTab) tabCtrlDesno.SelectedTab = tabPageZvijezda;
			listViewPlaneti.Items.Clear();

			if (igrac.posjeceneZvjezde.Contains(zvijezda))
				for (int i = 0; i < zvijezda.planeti.Count; i++) {
					Planet planet = zvijezda.planeti[i];
					ListViewItem item = new ListViewItem();
					item.ImageIndex = Slike.PlanetImageIndex[planet.slika];

					if (planet.tip != Planet.Tip.NIKAKAV) {
						item.Text = planet.ime;
						if (planet.kolonija != null)
							item.ForeColor = planet.kolonija.igrac.boja;
					}
					listViewPlaneti.Items.Add(item);
				}
			else
				listViewPlaneti.Items.Add(jezik["zvjNeistrazeno"].tekst(null));

			prikaziFlotu(zvijezda);			

			lblImeZvjezde.Text = zvijezda.ime + "\nZračenje: " + zvijezda.zracenje();
			osvjeziMapu();
			tabCtrlDesno.ImageList.Images[0] = Slike.ZvijezdaTab[zvijezda.tip];
			tabCtrlDesno.Refresh();			
		}

		public void osvjeziMapu()
		{
			picMapa.Image = prikazMape.osvjezi();
		}

		private void odaberiPlanet(Planet planet, bool promjeniTab)
		{
			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.Kolonija];

			if (planet.tip == Planet.Tip.NIKAKAV) return;
			igrac.odabranPlanet = planet;
			if (promjeniTab) tabCtrlDesno.SelectedTab = tabPageKolonija;
			tabCtrlDesno.ImageList.Images[1] = planet.slika;

			picSlikaPlaneta.Image = planet.slika;
			lblImePlaneta.Text = planet.ime;
			if (planet.kolonija != null) {
				groupPoStan.Visible = true;
				groupCivGradnja.Visible = true;
				groupVojGradnja.Visible = true;
				lblRazvoj.Visible = true;

				lblPopulacija.Text = jezik["plPopulacija"].tekst() + ": " + Fje.PrefiksFormater(planet.kolonija.efekti[Kolonija.Populacija]);
				hscrCivilnaIndustrija.Enabled = true;
				btnCivilnaGradnja.Enabled = true;

				hscrVojnaIndustrija.Enabled = true;
				btnVojnaGradnja.Enabled = true;
				osvjeziPogledNaKoloniju();
			}
			else {
				lblPopulacija.Text = jezik["plNenaseljeno"].tekst();
				groupPoStan.Visible = false;
				groupCivGradnja.Visible = false;
				groupVojGradnja.Visible = false;
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

		private void trackBarZoom_Scroll(object sender, EventArgs e)
		{
			prikazMape.zoom(
				(trackBarZoom.Value - trackBarZoom.Minimum) / 
				(double)(trackBarZoom.Maximum - trackBarZoom.Minimum));

			picMapa.Image = prikazMape.slikaMape;
		}

		private void listViewPlaneti_Click(object sender, EventArgs e)
		{
			odaberiPlanet(igrac.odabranaZvijezda.planeti[listViewPlaneti.SelectedIndices[0]], true);
		}

		private void btnEndTurn_Click(object sender, EventArgs e)
		{
			igra.slijedeciIgrac();
			noviKrugPogled();

			if (igrac.poruke.Count > 0)
				btnPoruke_Click(this, null);
		}

		private void btnPlanetInfo_Click(object sender, EventArgs e)
		{
			if (igrac.odabranPlanet.kolonija != null)
			{
				FormPlanetInfo planetInfo = new FormPlanetInfo(igrac.odabranPlanet.kolonija);
				planetInfo.ShowDialog();
				osvjeziPogledNaKoloniju();
			}
			else {
				FormPlanetInfo planetInfo = new FormPlanetInfo(igrac, igrac.odabranPlanet);
				planetInfo.ShowDialog();
			}
		}

		private void osvjeziLabele()
		{
			Kolonija kolonija = igrac.odabranPlanet.kolonija;

			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.Kolonija];

			lblHranaPoStan.Text = jezik["HranaPoStan"].tekst() + ": " + kolonija.efekti[Kolonija.HranaPoFarmeru].ToString("0.##");
			lblRudePoStan.Text = jezik["RudePoStan"].tekst() + ": " + kolonija.efekti[Kolonija.RudePoRudaru].ToString("0.##");
			lblOdrzavanjePoStan.Text = jezik["OdrzavanjePoStan"].tekst() + ": " + (kolonija.efekti[Kolonija.OdrzavanjeUkupno] / kolonija.efekti[Kolonija.Populacija]).ToString("0.##");
			lblIndustrijaPoStan.Text = jezik["IndustrijaPoStan"].tekst() + ": " + kolonija.efekti[Kolonija.IndustrijaPoRadniku].ToString("0.##");
			lblRazvojPoStan.Text = jezik["RazvojPoStan"].tekst() + ": " + kolonija.efekti[Kolonija.RazvojPoRadniku].ToString("0.##");

			lblCivilnaIndustrija.Text = Fje.PrefiksFormater(kolonija.poeniCivilneIndustrije()) + " " + jezik["jedInd"].tekst();
			lblVojnaGradnja.Text = Fje.PrefiksFormater(kolonija.poeniVojneIndustrije()) + " " + jezik["jedInd"].tekst();
			lblProcjenaCivilneGradnje.Text = kolonija.procjenaVremenaCivilneGradnje();
			lblProcjenaVojneGradnje.Text = kolonija.procjenaVremenaVojneGradnje();
			lblRazvoj.Text = jezik["lblRazvoj"].tekst() + Fje.PrefiksFormater(kolonija.poeniRazvoja());

			if (igrac.odabranPlanet.kolonija.redCivilneGradnje.Count > 0)
			{
				btnCivilnaGradnja.Image = igrac.odabranPlanet.kolonija.redCivilneGradnje.First.Value.slika;
				btnCivilnaGradnja.Text = "";
			}
			else
			{
				btnCivilnaGradnja.Image = null;
				btnCivilnaGradnja.Text = jezik["Civilna_Gradnja"].tekst();
			}

			if (igrac.odabranPlanet.kolonija.redVojneGradnje.Count > 0)
			{
				btnVojnaGradnja.Image = igrac.odabranPlanet.kolonija.redVojneGradnje.First.Value.slika;
				btnVojnaGradnja.Text = "";
			}
			else
			{
				btnVojnaGradnja.Image = null;
				btnVojnaGradnja.Text = jezik["Vojna_Gradnja"].tekst();
			}
		}

		private void osvjeziPogledNaKoloniju()
		{
			hscrCivilnaIndustrija.Value = (int)(igrac.odabranPlanet.kolonija.civilnaIndustrija * hscrCivilnaIndustrija.Maximum);
			hscrVojnaIndustrija.Value = (int)(igrac.odabranPlanet.kolonija.vojnaIndustrija * hscrVojnaIndustrija.Maximum);

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
				btnPrimAkcijaBroda.Visible = brod.dizajn.primarnoOruzje != null;
				btnSekAkcija.Visible = brod.dizajn.sekundarnoOruzje != null;
				btnFlotaPokret.Enabled = brod.dizajn.MZPogon != null;
			}
		}

		private void hscrCivilnaIndustrija_Scroll(object sender, ScrollEventArgs e)
		{
			if (igrac.odabranPlanet.kolonija != null)
			{
				igrac.odabranPlanet.kolonija.civilnaIndustrija = e.NewValue / (double)hscrCivilnaIndustrija.Maximum;
				int val = (int)Math.Round(igrac.odabranPlanet.kolonija.civilnaIndustrija * hscrCivilnaIndustrija.Maximum, MidpointRounding.AwayFromZero);
				if (hscrCivilnaIndustrija.Value != val)
					e.NewValue = val;
				else
					osvjeziLabele();
			}
		}

		private void hscrVojnaIndustrija_Scroll(object sender, ScrollEventArgs e)
		{
			if (igrac.odabranPlanet.kolonija != null)
			{
				igrac.odabranPlanet.kolonija.vojnaIndustrija = e.NewValue / (double)hscrVojnaIndustrija.Maximum;
				int val = (int)Math.Round(igrac.odabranPlanet.kolonija.vojnaIndustrija * hscrVojnaIndustrija.Maximum, MidpointRounding.AwayFromZero);
				if (hscrVojnaIndustrija.Value != val)
					e.NewValue = val;
				else
					osvjeziLabele();
			}
		}

		private void btnCivilnaGradnja_Click(object sender, EventArgs e)
		{
			FormGradnja frmGradnja = new FormGradnja(igrac.odabranPlanet.kolonija, true);

			if (frmGradnja.ShowDialog() == DialogResult.OK)
				osvjeziLabele();
		}

		private void btnVojnaGradnja_Click(object sender, EventArgs e)
		{
			FormGradnja frmGradnja = new FormGradnja(igrac.odabranPlanet.kolonija, false);

			if (frmGradnja.ShowDialog() == DialogResult.OK)
				osvjeziLabele();
		}

		private void btnTech_Click(object sender, EventArgs e)
		{
			FormTechIzbor frmTech = new FormTechIzbor(igrac);
			frmTech.ShowDialog();
		}

		private void btnPoruke_Click(object sender, EventArgs e)
		{
			FormPoruke formPoruke = new FormPoruke(igrac);
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
							btnTech_Click(sender, e);
							break;
						case Poruka.Tip.Zgrada:
							planet = ((PorukaZgrada)poruka).planet;
							odaberiZvijezdu(planet.zvjezda, false);
							odaberiPlanet(planet, true);
							centrirajZvijezdu(planet.zvjezda);
							break;
					}
				}
		}

		private void btnFlote_Click(object sender, EventArgs e)
		{
			FormFlote formaFlote = new FormFlote(igrac);
			formaFlote.ShowDialog();
		}

		private void btnSpremi_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.InitialDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "pohranjeno";
			dialog.FileName = "sejv.igra";
			dialog.Filter = Postavke.jezik[Kontekst.WindowsDijalozi, "TIP_SEJVA"].tekst(null) + " (*.igra)|*.igra";

			if (dialog.ShowDialog() == DialogResult.OK) {
				GZipStream zipStream = new GZipStream(new FileStream(dialog.FileName, FileMode.Create), CompressionMode.Compress);
				StreamWriter pisac = new StreamWriter(zipStream);
				pisac.Write(igra.spremi());
				pisac.Close();
			}
		}

		private void btnUcitaj_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.InitialDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "pohranjeno";
			dialog.FileName = "sejv.igra";
			dialog.Filter = Postavke.jezik[Kontekst.WindowsDijalozi, "TIP_SEJVA"].tekst(null) + " (*.igra)|*.igra";

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
	}
}
