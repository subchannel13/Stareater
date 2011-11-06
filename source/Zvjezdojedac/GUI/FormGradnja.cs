using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Podaci.Jezici;
using Zvjezdojedac.Podaci;
using Zvjezdojedac.Alati;
using Zvjezdojedac.Igra;

namespace Zvjezdojedac.GUI
{
	public partial class FormGradnja : Form
	{

		private AGradiliste gradiliste;
        private LinkedList<Zgrada.ZgradaInfo> redGradnje;
		private Dictionary<Zgrada.ZgradaInfo, int> redoslijedPonuda = new Dictionary<Zgrada.ZgradaInfo, int>();

		public FormGradnja(AGradiliste gradiliste)
		{
			InitializeComponent();
			lblZgradaInfo.Text = "";
            
            redGradnje = gradiliste.RedGradnje;
			this.gradiliste = gradiliste;
			
			HashSet<Zgrada.ZgradaInfo> uRedu = new HashSet<Zgrada.ZgradaInfo>(redGradnje);
			List<Zgrada.ZgradaInfo> izgradivo = gradiliste.MoguceGraditi();
			for (int i = 0; i < izgradivo.Count; i++) {
				redoslijedPonuda.Add(izgradivo[i], i);
				if (!uRedu.Contains(izgradivo[i]))
					lstMoguceGradit.Items.Add(izgradivo[i]);
			}

            foreach (Zgrada.ZgradaInfo z in redGradnje)
				lstRedGradnje.Items.Add(z);

			Dictionary<string, ITekst> jezik = Postavke.Jezik[Kontekst.FormGradnja];
			btnKasnije.Text = jezik["btnKasnije"].tekst();
			btnOk.Text = jezik["btnOk"].tekst();
			btnPrije.Text = jezik["btnPrije"].tekst();
			btnUkloni.Text = jezik["btnUkloni"].tekst();
			lblMogucnosti.Text = jezik["lblMogucnosti"].tekst() + ":";
			lblPopis.Text = jezik["lblPopis"].tekst() + ":";

			//if (civilnaGradnja)
				this.Text = jezik["naslovCivGradnja"].tekst();
			/*else
				this.Text = jezik["naslovVojGradnja"].tekst();*/

			this.Font = Postavke.FontSucelja(this.Font);
		}

		public static bool JeValjanoGradiliste(AGradiliste gradiliste, Igrac igrac)
		{
			return (gradiliste != null) && (gradiliste.Igrac == igrac);
		}

		private int sorterPonuda(object lijeva, object desna)
		{
			return redoslijedPonuda[(Zgrada.ZgradaInfo)lijeva].CompareTo(redoslijedPonuda[(Zgrada.ZgradaInfo)desna]);
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnDodaj_Click(object sender, EventArgs e)
		{
			if (lstMoguceGradit.SelectedItem == null)
				return;

			lstRedGradnje.Items.Add(lstMoguceGradit.SelectedItem);
			lstMoguceGradit.Items.Remove(lstMoguceGradit.SelectedItem);
		}

		private void btnUkloni_Click(object sender, EventArgs e)
		{
			if (lstRedGradnje.SelectedItem == null)
				return;

			object zInfo = lstRedGradnje.SelectedItem;
			int pozicija = Fje.BinarySearch(lstMoguceGradit.Items, zInfo, sorterPonuda);
			if (pozicija < lstMoguceGradit.Items.Count)
				if (sorterPonuda(lstMoguceGradit.Items[pozicija], zInfo) < 0)
					pozicija++;

			lstMoguceGradit.Items.Insert(pozicija, lstRedGradnje.SelectedItem);
			lstRedGradnje.Items.Remove(lstRedGradnje.SelectedItem);
		}

		private void lstMoguceGradit_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lstMoguceGradit.SelectedItem == null)
				return;

			Zgrada.ZgradaInfo zgrada = (Zgrada.ZgradaInfo)lstMoguceGradit.SelectedItem;
			picSlikaZgrade.Image = zgrada.slika;
			lblZgradaInfo.Text = zgrada.Opis;
		}

		private void lstMoguceGradit_DoubleClick(object sender, EventArgs e)
		{
			btnDodaj_Click(sender, e);
		}

		private void btnPrije_Click(object sender, EventArgs e)
		{
			if (lstRedGradnje.SelectedItem == null || lstRedGradnje.SelectedIndex <= 0)
				return;

			object tmp = lstRedGradnje.SelectedItem;
			lstRedGradnje.Items[lstRedGradnje.SelectedIndex] = lstRedGradnje.Items[lstRedGradnje.SelectedIndex - 1];
			lstRedGradnje.Items[lstRedGradnje.SelectedIndex - 1] = tmp;
			lstRedGradnje.SelectedIndex--;
		}

		private void btnKasnije_Click(object sender, EventArgs e)
		{
			if (lstRedGradnje.SelectedItem == null || lstRedGradnje.SelectedIndex >= lstRedGradnje.Items.Count - 1)
				return;

			object tmp = lstRedGradnje.SelectedItem;
			lstRedGradnje.Items[lstRedGradnje.SelectedIndex] = lstRedGradnje.Items[lstRedGradnje.SelectedIndex + 1];
			lstRedGradnje.Items[lstRedGradnje.SelectedIndex + 1] = tmp;
			lstRedGradnje.SelectedIndex++;
		}

		private void lstRedGradnje_DoubleClick(object sender, EventArgs e)
		{
			btnUkloni_Click(sender, e);
		}

		private void FormGradnja_FormClosing(object sender, FormClosingEventArgs e)
		{
			List<Zgrada.ZgradaInfo> ret = new List<Zgrada.ZgradaInfo>();
			foreach (object o in lstRedGradnje.Items)
				ret.Add((Zgrada.ZgradaInfo)o);

			redGradnje.Clear();
			foreach (Zgrada.ZgradaInfo z in ret)
				redGradnje.AddLast(z);

			DialogResult = DialogResult.OK;
		}

	}
}
