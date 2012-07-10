using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Igra.Brodovi;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Podaci.Jezici;

namespace Zvjezdojedac.GUI
{
	public partial class FormDizajnKomponenta<T> : Form where T : class, IKomponenta
	{
		public T OdabranaKomponenta { get; private set; }

		private const int VelicinaSlike = 80;
		private const int NevidljiviPadding = 2;
		private const int MarginaGumba = 1;
		private const int NekiGlupiOffset = 10;
		
		private const int BrRedaka = 3;
		private const int BrStupaca = 3;

		private Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormFlote];
		private List<T> naOdabir;
		
		public FormDizajnKomponenta(List<T> naOdabir, T trenutnaKomponenta, string naslov, Control kontrolaDo)
		{
			this.naOdabir = naOdabir;
			this.OdabranaKomponenta = trenutnaKomponenta;

			InitializeComponent();

			this.Text = naslov;

			this.ClientSize = new Size(
				ukupnaDuljina(BrStupaca, VelicinaSlike + NevidljiviPadding + MarginaGumba * 2, MarginaGumba) + SystemInformation.VerticalScrollBarWidth,
				ukupnaDuljina(BrRedaka, VelicinaSlike + NevidljiviPadding + MarginaGumba * 2, MarginaGumba)
				);

			Point origin = kontrolaDo.PointToScreen(new Point(kontrolaDo.Width, 0));
			this.Location = new Point(
				origin.X + NekiGlupiOffset,
				origin.Y - this.Size.Height / 2);
		}

		public FormDizajnKomponenta(Dictionary<Misija.Tip, List<T>> misije, T trenutnaMisija, string naslov, Control kontrolaDo)
			: this(new List<T>(), trenutnaMisija, naslov, kontrolaDo)
		{
			for (int misijaId = 0; misijaId < (int)Misija.Tip.N; misijaId++) {
				Misija.Tip misijaTip = (Misija.Tip)misijaId;
				if (!misije.ContainsKey(misijaTip))
					continue;

				foreach (T oruzje in misije[misijaTip])
					naOdabir.Add(oruzje);
			}
		}

		private void FormDizajnMisija_Load(object sender, EventArgs e)
		{
			flowPanel.Controls.Add(napraviGumb(null));

			foreach (T oruzje in naOdabir)
				flowPanel.Controls.Add(napraviGumb(oruzje));
		}

		private int ukupnaDuljina(int brKomponenti, int sirinaKomponente, int margine)
		{
			return brKomponenti * (sirinaKomponente + margine * 2);
		}

		private Control napraviGumb(T komponenta)
		{
			Button gumb = new Button();

			gumb.Size = new Size(
				VelicinaSlike + NevidljiviPadding + MarginaGumba,
				VelicinaSlike + NevidljiviPadding + MarginaGumba);
			gumb.Margin = new Padding(MarginaGumba);

			gumb.BackgroundImageLayout = ImageLayout.Zoom;
			gumb.ForeColor = Color.Black;
			gumb.TextAlign = ContentAlignment.BottomCenter;

			gumb.Click += buttonMisija_Click;


			gumb.Tag = komponenta;
			if (komponenta != null) {
				gumb.Text = komponenta.naziv;
				gumb.BackgroundImage = komponenta.slika;
			}
			else {
				gumb.Text = jezik["bezMisije"].tekst();
				gumb.TextAlign = ContentAlignment.MiddleCenter;
			}
			
			return gumb;
		}

		private void buttonMisija_Click(object sender, EventArgs e)
		{
			Button gumbSender = sender as Button;

			OdabranaKomponenta = gumbSender.Tag as T;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void FormDizajnMisija_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape) {
				DialogResult = DialogResult.Cancel;
				Close();
			}
		}
	}
}
