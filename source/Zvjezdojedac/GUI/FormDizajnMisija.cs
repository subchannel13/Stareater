using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Igra.Brodovi;

namespace Zvjezdojedac.GUI
{
	public partial class FormDizajnMisija : Form
	{
		public Oruzje OdabranaMisija { get; private set; }

		private const int VelicinaSlike = 80;
		private const int BrRedaka = 3;
		private const int BrStupaca = 3;
		private Dictionary<Misija.Tip, List<Oruzje>> misije;
		private Button pomocniGumb = new Button();

		public FormDizajnMisija(Dictionary<Misija.Tip, List<Oruzje>> misije, Oruzje trenutnaMisija)
		{
			this.misije = misije;
			this.OdabranaMisija = trenutnaMisija;

			InitializeComponent();

			this.ClientSize = new Size(
				ukupnaDuljina(BrStupaca, VelicinaSlike + pomocniGumb.Margin.Horizontal, flowPanel.Margin.Horizontal) + SystemInformation.VerticalScrollBarWidth,
				ukupnaDuljina(BrRedaka, VelicinaSlike + pomocniGumb.Margin.Vertical, flowPanel.Margin.Vertical)
				);
		}

		private void FormDizajnMisija_Load(object sender, EventArgs e)
		{
			for (int misijaId = 0; misijaId < (int)Misija.Tip.N; misijaId++) {
				Misija.Tip misijaTip = (Misija.Tip)misijaId;
				if (!misije.ContainsKey(misijaTip))
					continue;

				foreach (Oruzje oruzje in misije[misijaTip])
					flowPanel.Controls.Add(napraviGumb(oruzje));
			}
		}

		private int ukupnaDuljina(int brKomponenti, int sirinaKomponente, int margine)
		{
			return brKomponenti * (sirinaKomponente + margine) + margine;
		}

		private Control napraviGumb(Oruzje oruzje)
		{
			Button gumb = new Button();

			gumb.Size = new Size(VelicinaSlike + pomocniGumb.Margin.Horizontal, VelicinaSlike + pomocniGumb.Margin.Vertical);
			gumb.BackgroundImage = oruzje.slika;
			gumb.BackgroundImageLayout = ImageLayout.Zoom;
			gumb.ForeColor = Color.DarkGray;
			gumb.Text = oruzje.naziv;
			gumb.TextAlign = ContentAlignment.BottomCenter;

			gumb.Tag = oruzje;

			return gumb;
		}
	}
}
