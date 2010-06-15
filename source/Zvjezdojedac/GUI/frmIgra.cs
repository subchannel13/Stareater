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

namespace Prototip
{
	public partial class frmIgra : Form
	{
		private PrikazMape prikazMape;

		private Igra igra;

		private Igrac igrac;

		private Alati.Tocka<double> pomakPogleda;

		public frmIgra(Igra igra)
		{
			InitializeComponent();

			this.igra = igra;
			igrac = igra.trenutniIgrac();

			pomakPogleda = null;
			prikazMape = new PrikazMape(igra);
			this.picMapa.Image = prikazMape.slikaMape;

			tabCtrlDesno.ImageList = new ImageList();
			tabCtrlDesno.ImageList.ImageSize = new Size(32, 32);
			tabCtrlDesno.ImageList.Images.Add(new Bitmap(1, 1));
			tabCtrlDesno.ImageList.Images.Add(Slike.PlanetTab[Planet.Tip.ASTEROIDI][0].image);
			tabCtrlDesno.ImageList.Images.Add(Slike.FlotaTab);
			tabPageZvijezda.ImageIndex = 0;
			tabPagePlanet.ImageIndex = 1;
			tabPageFlote.ImageIndex = 2;

			listViewPlaneti.LargeImageList = new ImageList();
			listViewPlaneti.LargeImageList.ImageSize = new Size(32, 32);
			//foreach(Planet.Tip tip in Enum.GetValues(typeof(Planet.Tip)))
			//	listViewPlaneti.LargeImageList.Images.Add(Slike.PlanetTab[tip]);
			Image[] planetImages = new Image[Slike.PlanetImageIndex.Count];
			foreach (Image img in Slike.PlanetImageIndex.Keys)
				planetImages[Slike.PlanetImageIndex[img]] = img;
			listViewPlaneti.LargeImageList.Images.AddRange(planetImages);

			btnCivilnaGradnja.Text = "";
            btnVojnaGradnja.Text = "";
		}

		private void frmIgra_Load(object sender, EventArgs e)
		{
			noviKrugPogled();
		}

		private void noviKrugPogled()
		{
			lblBrojKruga.Text = igra.brKruga + ". krug";
			odaberiZvijezdu(igrac.odabranaZvijezda, false);
			int x = prikazMape.XsaMape(igrac.odabranaZvijezda.x);
			int y = prikazMape.YsaMape(igrac.odabranaZvijezda.y);
			x = odrediPomakScrolla(picMapa.Width, pnlMapa.Width, pnlMapa.HorizontalScroll, x, 0);
			y = odrediPomakScrolla(picMapa.Height, pnlMapa.Height, pnlMapa.VerticalScroll, y, 0);
			pnlMapa.HorizontalScroll.Value += x;
			pnlMapa.VerticalScroll.Value += y;

			btnPoruke.Text = "Novosti (" + igrac.poruke.Count + ")";

			odabariPlanet(igrac.odabranPlanet, true);			
		}

		private void picMapa_Click(object sender, EventArgs e)
		{
			MouseEventArgs mouseEvent = (MouseEventArgs)e;
			int x = mouseEvent.X;
			int y = mouseEvent.Y;

			Zvijezda odabranaZvijezda = igra.mapa.najblizaZvijezda(
				prikazMape.XnaMapi(x),
				prikazMape.YnaMapi(y), 0.5);

			if (odabranaZvijezda != null)
				odaberiZvijezdu(odabranaZvijezda,true);

			pomakPogleda = new Alati.Tocka<double>(x / (double)picMapa.Width, y / (double)picMapa.Height);
		}

		private void odaberiZvijezdu(Zvijezda zvijezda, bool promjeniTab)
		{
			igrac.odabranaZvijezda = zvijezda;
			if (promjeniTab) tabCtrlDesno.SelectedTab = tabPageZvijezda;
			listViewPlaneti.Items.Clear();

			for (int i = 0; i < zvijezda.planeti.Count; i++)
			{
				Planet planet = zvijezda.planeti[i];
				ListViewItem item = new ListViewItem("Planet " + (i+1));
				item.ImageIndex = Slike.PlanetImageIndex[planet.slika];
				listViewPlaneti.Items.Add(item);
			}

			tvFlota.Nodes.Clear();
			if (igrac.floteStacionarne.ContainsKey(zvijezda))
			{
				TreeNode nodeStacionarnaFloata = new TreeNode("Obrana");
				tvFlota.Nodes.Add(nodeStacionarnaFloata);
				Flota flota = igrac.floteStacionarne[zvijezda];
				foreach (Dictionary<Dizajn, Brod> brodovi in flota.brodovi.Values)
					foreach (Brod brod in brodovi.Values)
						nodeStacionarnaFloata.Nodes.Add(brod.dizajn.ime + " x " + Fje.PrefiksFormater(brod.kolicina));
			}

			lblImeZvjezde.Text = zvijezda.ime + "\nZračenje: " + zvijezda.zracenje();
			picMapa.Image = prikazMape.osvjezi();
			tabCtrlDesno.ImageList.Images[0] = Slike.ZvijezdaTab[zvijezda.tip];
			tabCtrlDesno.Refresh();
		}

		private void odabariPlanet(Planet planet, bool promjeniTab)
		{
			if (planet.tip == Planet.Tip.NIKAKAV) return;
			igrac.odabranPlanet = planet;
			if (promjeniTab) tabCtrlDesno.SelectedTab = tabPagePlanet;
			tabCtrlDesno.ImageList.Images[1] = planet.slika;

			picSlikaPlaneta.Image = planet.slika;
			lblImePlaneta.Text = planet.ime;
			if (planet.kolonija != null)
			{
				lblPopulacija.Text = "Populacija: " + Fje.PrefiksFormater((long)planet.kolonija.efekti[Kolonija.Populacija]);
				hscrCivilnaIndustrija.Enabled = true;
				btnCivilnaGradnja.Enabled = true;

				hscrVojnaIndustrija.Enabled = true;
				btnVojnaGradnja.Enabled = true;
				osvjeziPogledNaKoloniju();
			}
			else
			{
				lblPopulacija.Text = "Nenaseljeno";
				hscrCivilnaIndustrija.Enabled = false;
				lblCivilnaIndustrija.Text = "";
				lblProcjenaCivilneGradnje.Text = "";
				lblRazvoj.Text = "";
				btnCivilnaGradnja.Enabled = false;
				btnCivilnaGradnja.Image = null;

				hscrVojnaIndustrija.Enabled = false;
				lblVojnaGradnja.Text = "";
				lblProcjenaVojneGradnje.Text = "";
				btnVojnaGradnja.Enabled = false;
				btnVojnaGradnja.Image = null;
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
			//if (dx != 0 || dy != 0)
			//	lstvPoruke.Items.Add(pomakPogleda.x + ", " + pomakPogleda.y + "(" + dx + ", " + dy + ")");
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
			odabariPlanet(igrac.odabranaZvijezda.planeti[listViewPlaneti.SelectedIndices[0]], true);
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
				frmPlanetInfo planetInfo = new frmPlanetInfo(igrac.odabranPlanet.kolonija);
				planetInfo.ShowDialog();
				osvjeziPogledNaKoloniju();
			}
		}

		private void osvjeziLabele()
		{
			lblCivilnaIndustrija.Text = Fje.PrefiksFormater(igrac.odabranPlanet.kolonija.poeniCivilneIndustrije()) + " ind";
			lblVojnaGradnja.Text = Fje.PrefiksFormater(igrac.odabranPlanet.kolonija.poeniVojneIndustrije()) + " ind";
			lblProcjenaCivilneGradnje.Text = igrac.odabranPlanet.kolonija.procjenaVremenaCivilneGradnje();
			lblProcjenaVojneGradnje.Text = igrac.odabranPlanet.kolonija.procjenaVremenaVojneGradnje();
			lblRazvoj.Text = "Razvoj: " + Fje.PrefiksFormater(igrac.odabranPlanet.kolonija.poeniRazvoja());

			if (igrac.odabranPlanet.kolonija.redCivilneGradnje.Count > 0)
			{
				btnCivilnaGradnja.Image = igrac.odabranPlanet.kolonija.redCivilneGradnje.First.Value.slika;
				btnCivilnaGradnja.Text = "";
			}
			else
			{
				btnCivilnaGradnja.Image = null;
				btnCivilnaGradnja.Text = "Civilna gradnja";
			}

			if (igrac.odabranPlanet.kolonija.redVojneGradnje.Count > 0)
			{
				btnVojnaGradnja.Image = igrac.odabranPlanet.kolonija.redVojneGradnje.First.Value.slika;
				btnVojnaGradnja.Text = "";
			}
			else
			{
				btnVojnaGradnja.Image = null;
				btnVojnaGradnja.Text = "Vojna gradnja";
			}
		}

		private void osvjeziPogledNaKoloniju()
		{
			hscrCivilnaIndustrija.Value = (int)(igrac.odabranPlanet.kolonija.civilnaIndustrija * hscrCivilnaIndustrija.Maximum);
			hscrVojnaIndustrija.Value = (int)(igrac.odabranPlanet.kolonija.vojnaIndustrija * hscrVojnaIndustrija.Maximum);

			osvjeziLabele();
		}

		private void hscrCivilnaIndustrija_Scroll(object sender, ScrollEventArgs e)
		{
			if (igrac.odabranPlanet.kolonija != null)
			{
				igrac.odabranPlanet.kolonija.civilnaIndustrija = hscrCivilnaIndustrija.Value / (double)hscrCivilnaIndustrija.Maximum;
				int val = (int)(igrac.odabranPlanet.kolonija.civilnaIndustrija * hscrCivilnaIndustrija.Maximum);
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
				igrac.odabranPlanet.kolonija.vojnaIndustrija = hscrVojnaIndustrija.Value / (double)hscrVojnaIndustrija.Maximum;
				int val = (int)(igrac.odabranPlanet.kolonija.vojnaIndustrija * hscrVojnaIndustrija.Maximum);
				if (hscrVojnaIndustrija.Value != val)
					e.NewValue = val;
				else
					osvjeziLabele();
			}
		}

		private void btnCivilnaGradnja_Click(object sender, EventArgs e)
		{
			frmGradnja frmGradnja = new frmGradnja(igrac.odabranPlanet.kolonija, true);

			if (frmGradnja.ShowDialog() == DialogResult.OK)
				osvjeziLabele();
		}

		private void btnVojnaGradnja_Click(object sender, EventArgs e)
		{
			frmGradnja frmGradnja = new frmGradnja(igrac.odabranPlanet.kolonija, false);

			if (frmGradnja.ShowDialog() == DialogResult.OK)
				osvjeziLabele();
		}

		private void btnTech_Click(object sender, EventArgs e)
		{
			frmTechIzbor frmTech = new frmTechIzbor(igrac);
			frmTech.ShowDialog();
		}

		private void btnPoruke_Click(object sender, EventArgs e)
		{
			frmPoruke poruke = new frmPoruke(igrac);
			if (poruke.ShowDialog() == DialogResult.OK)
				if (poruke.odabranaProuka != null)
					if (poruke.odabranaProuka.tip == Poruka.Tip.Tehnologija)
						btnTech_Click(sender, e);
		}

		private void btnFlote_Click(object sender, EventArgs e)
		{
			frmFlote formaFlote = new frmFlote(igrac);
			formaFlote.ShowDialog();
		}

		private void btnSpremi_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.InitialDirectory = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "pohranjeno";
			dialog.FileName = "sejv.igra";
			dialog.Filter = "Zvjezdojedac igra (*.igra)|*.igra";

			if (dialog.ShowDialog() == DialogResult.OK) {
				FileStream pisac = new FileStream(dialog.FileName, FileMode.Create);

				MemoryStream zipMemory = new MemoryStream();
				GZipStream zipStream = new GZipStream(zipMemory, CompressionMode.Compress);
				byte[] toZip = Encoding.UTF8.GetBytes(igra.spremi());
				zipStream.Write(toZip, 0, toZip.Length);

				pisac.Write(zipMemory.ToArray(), 0, (int)zipMemory.Length);
				//pisac.Write(toZip, 0, toZip.Length);
				
				pisac.Close();
			}
		}
	}
}
