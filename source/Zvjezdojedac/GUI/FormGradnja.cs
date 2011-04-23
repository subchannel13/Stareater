﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Prototip.Podaci.Jezici;
using Prototip.Podaci;
using Alati;

namespace Prototip
{
	public partial class FormGradnja : Form
	{
		private Kolonija kolonija;
        private LinkedList<Zgrada.ZgradaInfo> redGradnje;
		private Dictionary<Zgrada.ZgradaInfo, int> redoslijedPonuda = new Dictionary<Zgrada.ZgradaInfo, int>();
        private bool civilnaGradnja;

		public FormGradnja(Kolonija kolonija, bool civilnaGradnja)
		{
			InitializeComponent();
			lblZgradaInfo.Text = "";
            if (civilnaGradnja)
                redGradnje = kolonija.redCivilneGradnje;
            else
                redGradnje = kolonija.redVojneGradnje;

            this.civilnaGradnja = civilnaGradnja;
			this.kolonija = kolonija;
			
			HashSet<Zgrada.ZgradaInfo> uRedu = new HashSet<Zgrada.ZgradaInfo>(redGradnje);
			List<Zgrada.ZgradaInfo> izgradivo = kolonija.moguceGraditi(civilnaGradnja);
			for (int i = 0; i < izgradivo.Count; i++) {
				redoslijedPonuda.Add(izgradivo[i], i);
				if (!uRedu.Contains(izgradivo[i]))
					lstMoguceGradit.Items.Add(izgradivo[i]);
			}

            foreach (Zgrada.ZgradaInfo z in redGradnje)
				lstRedGradnje.Items.Add(z);

			Dictionary<string, ITekst> jezik = Postavke.jezik[Kontekst.FormGradnja];
			btnKasnije.Text = jezik["btnKasnije"].tekst();
			btnOk.Text = jezik["btnOk"].tekst();
			btnPrije.Text = jezik["btnPrije"].tekst();
			btnUkloni.Text = jezik["btnUkloni"].tekst();
			lblMogucnosti.Text = jezik["lblMogucnosti"].tekst() + ":";
			lblPopis.Text = jezik["lblPopis"].tekst() + ":";

			if (civilnaGradnja)
				this.Text = jezik["naslovCivGradnja"].tekst();
			else
				this.Text = jezik["naslovVojGradnja"].tekst();
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
			lblZgradaInfo.Text = zgrada.opis;
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
