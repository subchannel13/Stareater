using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zvjezdojedac.Igra.Bitka;
using Zvjezdojedac.Igra.Brodovi;
using Zvjezdojedac.Alati;
using Zvjezdojedac.GUI.Events;

namespace Zvjezdojedac.GUI
{
	public partial class CombatantItem : UserControl
	{
		readonly Color BojaOklopa = Color.Red;
		readonly Color BojaStita = Color.Blue;

		ICollection<Borac> grupaBoraca = null;
		Dizajn dizajn = null;
		Color bojaIgraca = Color.Black;
		
		bool isSelected = false;
		ICollection<CombatantItem> group = new HashSet<CombatantItem>();

		public CombatantItem()
		{
			Selectable = true;

			InitializeComponent();

			group.Add(this);
		}

		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DescriptionAttribute("Indicates whether control is interactive or readonly.")]
		[DefaultValue(true)]
		public bool Selectable { get; set; }

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public event ObjectEventArgs<ICollection<Borac>>.Handler OnSelect;

		public void SetData(IList<Borac> grupaBoraca, bool prikazPrikrivanja)
		{
			this.dizajn = grupaBoraca[0].Dizajn;
			this.bojaIgraca = grupaBoraca[0].Igrac.boja;
			this.grupaBoraca = grupaBoraca;

			picPrikrivanje.Visible = prikazPrikrivanja;

			RefreshData();
		}

		public void RefreshData()
		{
			int brojBoraca = 0;
			double izdrzljivostOklopa = 0;
			double izdrzljivostStita = 0;

			foreach (Borac borac in grupaBoraca) {
				brojBoraca++;
				izdrzljivostOklopa += borac.IzdrzljivostOklopa / borac.Dizajn.izdrzljivostOklopa;
				
				if (dizajn.stit != null)
					izdrzljivostStita += borac.IzdrzljivostStita / borac.Dizajn.izdrzljivostStita;
			}
			izdrzljivostOklopa /= brojBoraca;
			izdrzljivostStita /= brojBoraca;

			Image slikaStanje = picStanje.Image ?? new Bitmap(picStanje.Width, picStanje.Height);
			using (Graphics g = Graphics.FromImage(slikaStanje)) {
				g.Clear(Color.Black);

				int granicaOklopStit = slikaStanje.Height / 2;
				Rectangle oklopRect = new Rectangle(0, granicaOklopStit, (int)(izdrzljivostOklopa * slikaStanje.Width), slikaStanje.Height - granicaOklopStit);
				Rectangle stitRect = new Rectangle(0, 0, (int)(izdrzljivostStita * slikaStanje.Width), granicaOklopStit);

				g.DrawRectangle(new Pen(BojaOklopa), oklopRect);
				g.FillRectangle(new SolidBrush(BojaOklopa), oklopRect);

				g.DrawRectangle(new Pen(BojaStita), stitRect);
				g.FillRectangle(new SolidBrush(BojaStita), stitRect);
			}

			picIkona.Image = dizajn.ikona;
			lblNaziv.Text = Fje.PrefiksFormater(brojBoraca) + " x " + dizajn.ime;
			picStanje.Image = slikaStanje;

			Refresh();
		}

		public void GroupWith(CombatantItem otherItem)
		{
			group.Remove(this);
			otherItem.group.Add(this);
			group = otherItem.group;
		}

		public bool Selected
		{
			get { return isSelected; }
			set
			{
				isSelected = value;
				BackColor = (isSelected) ? SystemColors.Highlight : SystemColors.Control;
				
				if (isSelected)
					foreach (CombatantItem item in group)
						if (item != this)
							item.Selected = false;
			}
		}

		public void SelectThis()
		{
			if (!Selectable || Selected) return;

			Selected = true;
			OnSelect(this, new ObjectEventArgs<ICollection<Borac>>(grupaBoraca));
		}

		private void CombatantItem_Click(object sender, EventArgs e)
		{
			SelectThis();
		}

		private void picIkona_Click(object sender, EventArgs e)
		{
			CombatantItem_Click(this, e);
		}

		private void picStanje_Click(object sender, EventArgs e)
		{
			CombatantItem_Click(this, e);
		}

		private void picPrikrivanje_Click(object sender, EventArgs e)
		{
			CombatantItem_Click(this, e);
		}

		private void lblNaziv_Click(object sender, EventArgs e)
		{
			CombatantItem_Click(this, e);
		}
	}
}
