namespace Zvjezdojedac.GUI
{
	partial class FormDizajn
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblCijena = new System.Windows.Forms.Label();
			this.lblSlobodno = new System.Windows.Forms.Label();
			this.btnSpecOpremaMinus = new System.Windows.Forms.Button();
			this.btnSpecOpremaPlus = new System.Windows.Forms.Button();
			this.lstvSpecOprema = new System.Windows.Forms.ListView();
			this.chSpecOpKolicina = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chSpecOpNaziv = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblSpecOprema = new System.Windows.Forms.Label();
			this.lblTaktika = new System.Windows.Forms.Label();
			this.cbTaktika = new System.Windows.Forms.ComboBox();
			this.lblOklop = new System.Windows.Forms.Label();
			this.lblSenzori = new System.Windows.Forms.Label();
			this.lblPokretljivost = new System.Windows.Forms.Label();
			this.lblUdioMisija = new System.Windows.Forms.Label();
			this.lblUdioSek = new System.Windows.Forms.Label();
			this.lblStit = new System.Windows.Forms.Label();
			this.lblSekMisija = new System.Windows.Forms.Label();
			this.lblPrimMisija = new System.Windows.Forms.Label();
			this.hscrUdioMisija = new System.Windows.Forms.HScrollBar();
			this.chMZpogon = new System.Windows.Forms.CheckBox();
			this.btnSpremi = new System.Windows.Forms.Button();
			this.cbVelicina = new System.Windows.Forms.ComboBox();
			this.lblVelicina = new System.Windows.Forms.Label();
			this.txtNaziv = new System.Windows.Forms.TextBox();
			this.lblNaziv = new System.Windows.Forms.Label();
			this.picSlika = new System.Windows.Forms.PictureBox();
			this.btnPrimMisija = new System.Windows.Forms.Button();
			this.btnSekMisija = new System.Windows.Forms.Button();
			this.btnStit = new System.Windows.Forms.Button();
			this.lblReaktor = new System.Windows.Forms.Label();
			this.lblPrikrivanje = new System.Windows.Forms.Label();
			this.lblOmetanje = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picSlika)).BeginInit();
			this.SuspendLayout();
			// 
			// lblCijena
			// 
			this.lblCijena.AutoSize = true;
			this.lblCijena.Location = new System.Drawing.Point(111, 249);
			this.lblCijena.Name = "lblCijena";
			this.lblCijena.Size = new System.Drawing.Size(75, 13);
			this.lblCijena.TabIndex = 64;
			this.lblCijena.Text = "Cijena: xx.xx X";
			// 
			// lblSlobodno
			// 
			this.lblSlobodno.AutoSize = true;
			this.lblSlobodno.Location = new System.Drawing.Point(111, 262);
			this.lblSlobodno.Name = "lblSlobodno";
			this.lblSlobodno.Size = new System.Drawing.Size(170, 13);
			this.lblSlobodno.TabIndex = 60;
			this.lblSlobodno.Text = "Slobodan prostor: xx.xx X / xx.xx X";
			// 
			// btnSpecOpremaMinus
			// 
			this.btnSpecOpremaMinus.Location = new System.Drawing.Point(375, 225);
			this.btnSpecOpremaMinus.Name = "btnSpecOpremaMinus";
			this.btnSpecOpremaMinus.Size = new System.Drawing.Size(33, 23);
			this.btnSpecOpremaMinus.TabIndex = 59;
			this.btnSpecOpremaMinus.Text = "-";
			this.btnSpecOpremaMinus.UseVisualStyleBackColor = true;
			this.btnSpecOpremaMinus.Click += new System.EventHandler(this.btnSpecOpremaMinus_Click);
			// 
			// btnSpecOpremaPlus
			// 
			this.btnSpecOpremaPlus.Location = new System.Drawing.Point(336, 225);
			this.btnSpecOpremaPlus.Name = "btnSpecOpremaPlus";
			this.btnSpecOpremaPlus.Size = new System.Drawing.Size(33, 23);
			this.btnSpecOpremaPlus.TabIndex = 58;
			this.btnSpecOpremaPlus.Text = "+";
			this.btnSpecOpremaPlus.UseVisualStyleBackColor = true;
			this.btnSpecOpremaPlus.Click += new System.EventHandler(this.btnSpecOpremaPlus_Click);
			// 
			// lstvSpecOprema
			// 
			this.lstvSpecOprema.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSpecOpKolicina,
            this.chSpecOpNaziv});
			this.lstvSpecOprema.FullRowSelect = true;
			this.lstvSpecOprema.GridLines = true;
			this.lstvSpecOprema.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstvSpecOprema.HideSelection = false;
			this.lstvSpecOprema.Location = new System.Drawing.Point(298, 115);
			this.lstvSpecOprema.MultiSelect = false;
			this.lstvSpecOprema.Name = "lstvSpecOprema";
			this.lstvSpecOprema.Size = new System.Drawing.Size(324, 104);
			this.lstvSpecOprema.TabIndex = 57;
			this.lstvSpecOprema.UseCompatibleStateImageBehavior = false;
			this.lstvSpecOprema.View = System.Windows.Forms.View.Details;
			// 
			// chSpecOpKolicina
			// 
			this.chSpecOpKolicina.Text = "";
			this.chSpecOpKolicina.Width = 35;
			// 
			// chSpecOpNaziv
			// 
			this.chSpecOpNaziv.Text = "Naziv";
			this.chSpecOpNaziv.Width = 285;
			// 
			// lblSpecOprema
			// 
			this.lblSpecOprema.AutoSize = true;
			this.lblSpecOprema.Location = new System.Drawing.Point(295, 99);
			this.lblSpecOprema.Name = "lblSpecOprema";
			this.lblSpecOprema.Size = new System.Drawing.Size(97, 13);
			this.lblSpecOprema.TabIndex = 56;
			this.lblSpecOprema.Text = "Specijalna oprema:";
			// 
			// lblTaktika
			// 
			this.lblTaktika.AutoSize = true;
			this.lblTaktika.Location = new System.Drawing.Point(98, 68);
			this.lblTaktika.Name = "lblTaktika";
			this.lblTaktika.Size = new System.Drawing.Size(46, 13);
			this.lblTaktika.TabIndex = 55;
			this.lblTaktika.Text = "Taktika:";
			// 
			// cbTaktika
			// 
			this.cbTaktika.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTaktika.FormattingEnabled = true;
			this.cbTaktika.Location = new System.Drawing.Point(153, 65);
			this.cbTaktika.Name = "cbTaktika";
			this.cbTaktika.Size = new System.Drawing.Size(139, 21);
			this.cbTaktika.TabIndex = 54;
			this.cbTaktika.SelectedIndexChanged += new System.EventHandler(this.cbTaktika_SelectedIndexChanged);
			// 
			// lblOklop
			// 
			this.lblOklop.AutoSize = true;
			this.lblOklop.Location = new System.Drawing.Point(320, 25);
			this.lblOklop.Name = "lblOklop";
			this.lblOklop.Size = new System.Drawing.Size(135, 13);
			this.lblOklop.TabIndex = 52;
			this.lblOklop.Text = "Izdržljivost okolopa: xx.xx X";
			// 
			// lblSenzori
			// 
			this.lblSenzori.AutoSize = true;
			this.lblSenzori.Location = new System.Drawing.Point(320, 51);
			this.lblSenzori.Name = "lblSenzori";
			this.lblSenzori.Size = new System.Drawing.Size(94, 13);
			this.lblSenzori.TabIndex = 51;
			this.lblSenzori.Text = "Snaga senzora: xx";
			// 
			// lblPokretljivost
			// 
			this.lblPokretljivost.AutoSize = true;
			this.lblPokretljivost.Location = new System.Drawing.Point(320, 12);
			this.lblPokretljivost.Name = "lblPokretljivost";
			this.lblPokretljivost.Size = new System.Drawing.Size(80, 13);
			this.lblPokretljivost.TabIndex = 50;
			this.lblPokretljivost.Text = "Pokretljivost: xx";
			// 
			// lblUdioMisija
			// 
			this.lblUdioMisija.AutoSize = true;
			this.lblUdioMisija.Location = new System.Drawing.Point(255, 174);
			this.lblUdioMisija.Name = "lblUdioMisija";
			this.lblUdioMisija.Size = new System.Drawing.Size(28, 13);
			this.lblUdioMisija.TabIndex = 49;
			this.lblUdioMisija.Text = "xx %";
			// 
			// lblUdioSek
			// 
			this.lblUdioSek.AutoSize = true;
			this.lblUdioSek.Location = new System.Drawing.Point(12, 174);
			this.lblUdioSek.Name = "lblUdioSek";
			this.lblUdioSek.Size = new System.Drawing.Size(91, 13);
			this.lblUdioSek.TabIndex = 48;
			this.lblUdioSek.Text = "Udio sekundarne:";
			// 
			// lblStit
			// 
			this.lblStit.AutoSize = true;
			this.lblStit.Location = new System.Drawing.Point(12, 195);
			this.lblStit.Name = "lblStit";
			this.lblStit.Size = new System.Drawing.Size(25, 13);
			this.lblStit.TabIndex = 47;
			this.lblStit.Text = "Štit:";
			// 
			// lblSekMisija
			// 
			this.lblSekMisija.AutoSize = true;
			this.lblSekMisija.Location = new System.Drawing.Point(12, 147);
			this.lblSekMisija.Name = "lblSekMisija";
			this.lblSekMisija.Size = new System.Drawing.Size(96, 13);
			this.lblSekMisija.TabIndex = 46;
			this.lblSekMisija.Text = "Sekundarna misija:";
			// 
			// lblPrimMisija
			// 
			this.lblPrimMisija.AutoSize = true;
			this.lblPrimMisija.Location = new System.Drawing.Point(12, 120);
			this.lblPrimMisija.Name = "lblPrimMisija";
			this.lblPrimMisija.Size = new System.Drawing.Size(79, 13);
			this.lblPrimMisija.TabIndex = 45;
			this.lblPrimMisija.Text = "Primarna misija:";
			// 
			// hscrUdioMisija
			// 
			this.hscrUdioMisija.LargeChange = 1;
			this.hscrUdioMisija.Location = new System.Drawing.Point(114, 170);
			this.hscrUdioMisija.Maximum = 50;
			this.hscrUdioMisija.Name = "hscrUdioMisija";
			this.hscrUdioMisija.Size = new System.Drawing.Size(138, 17);
			this.hscrUdioMisija.TabIndex = 39;
			this.hscrUdioMisija.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrUdioMisija_Scroll);
			// 
			// chMZpogon
			// 
			this.chMZpogon.AutoSize = true;
			this.chMZpogon.Location = new System.Drawing.Point(114, 219);
			this.chMZpogon.Name = "chMZpogon";
			this.chMZpogon.Size = new System.Drawing.Size(75, 17);
			this.chMZpogon.TabIndex = 44;
			this.chMZpogon.Text = "MZ pogon";
			this.chMZpogon.UseVisualStyleBackColor = true;
			this.chMZpogon.CheckedChanged += new System.EventHandler(this.chMZpogon_CheckedChanged);
			// 
			// btnSpremi
			// 
			this.btnSpremi.Location = new System.Drawing.Point(114, 278);
			this.btnSpremi.Name = "btnSpremi";
			this.btnSpremi.Size = new System.Drawing.Size(75, 23);
			this.btnSpremi.TabIndex = 43;
			this.btnSpremi.Text = "Spremi";
			this.btnSpremi.UseVisualStyleBackColor = true;
			// 
			// cbVelicina
			// 
			this.cbVelicina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVelicina.FormattingEnabled = true;
			this.cbVelicina.Location = new System.Drawing.Point(153, 38);
			this.cbVelicina.Name = "cbVelicina";
			this.cbVelicina.Size = new System.Drawing.Size(139, 21);
			this.cbVelicina.TabIndex = 42;
			this.cbVelicina.SelectedIndexChanged += new System.EventHandler(this.cbVelicina_SelectedIndexChanged);
			// 
			// lblVelicina
			// 
			this.lblVelicina.AutoSize = true;
			this.lblVelicina.Location = new System.Drawing.Point(98, 41);
			this.lblVelicina.Name = "lblVelicina";
			this.lblVelicina.Size = new System.Drawing.Size(47, 13);
			this.lblVelicina.TabIndex = 41;
			this.lblVelicina.Text = "Veličina:";
			// 
			// txtNaziv
			// 
			this.txtNaziv.Location = new System.Drawing.Point(153, 12);
			this.txtNaziv.Name = "txtNaziv";
			this.txtNaziv.Size = new System.Drawing.Size(139, 20);
			this.txtNaziv.TabIndex = 40;
			// 
			// lblNaziv
			// 
			this.lblNaziv.AutoSize = true;
			this.lblNaziv.Location = new System.Drawing.Point(98, 15);
			this.lblNaziv.Name = "lblNaziv";
			this.lblNaziv.Size = new System.Drawing.Size(37, 13);
			this.lblNaziv.TabIndex = 38;
			this.lblNaziv.Text = "Naziv:";
			// 
			// picSlika
			// 
			this.picSlika.Location = new System.Drawing.Point(12, 12);
			this.picSlika.Name = "picSlika";
			this.picSlika.Size = new System.Drawing.Size(80, 80);
			this.picSlika.TabIndex = 35;
			this.picSlika.TabStop = false;
			// 
			// btnPrimMisija
			// 
			this.btnPrimMisija.Location = new System.Drawing.Point(114, 115);
			this.btnPrimMisija.Name = "btnPrimMisija";
			this.btnPrimMisija.Size = new System.Drawing.Size(178, 23);
			this.btnPrimMisija.TabIndex = 65;
			this.btnPrimMisija.Text = "999 k x Tachyon Megablaster";
			this.btnPrimMisija.UseVisualStyleBackColor = true;
			this.btnPrimMisija.Click += new System.EventHandler(this.btnPrimMisija_Click);
			// 
			// btnSekMisija
			// 
			this.btnSekMisija.Location = new System.Drawing.Point(114, 144);
			this.btnSekMisija.Name = "btnSekMisija";
			this.btnSekMisija.Size = new System.Drawing.Size(178, 23);
			this.btnSekMisija.TabIndex = 66;
			this.btnSekMisija.Text = "999 k x Tachyon Megablaster";
			this.btnSekMisija.UseVisualStyleBackColor = true;
			this.btnSekMisija.Click += new System.EventHandler(this.btnSekMisija_Click);
			// 
			// btnStit
			// 
			this.btnStit.Location = new System.Drawing.Point(114, 190);
			this.btnStit.Name = "btnStit";
			this.btnStit.Size = new System.Drawing.Size(178, 23);
			this.btnStit.TabIndex = 67;
			this.btnStit.Text = "Absolute Mirror Field";
			this.btnStit.UseVisualStyleBackColor = true;
			this.btnStit.Click += new System.EventHandler(this.btnStit_Click);
			// 
			// lblReaktor
			// 
			this.lblReaktor.AutoSize = true;
			this.lblReaktor.Location = new System.Drawing.Point(320, 38);
			this.lblReaktor.Name = "lblReaktor";
			this.lblReaktor.Size = new System.Drawing.Size(72, 13);
			this.lblReaktor.TabIndex = 68;
			this.lblReaktor.Text = "Reaktor: xx %";
			// 
			// lblPrikrivanje
			// 
			this.lblPrikrivanje.AutoSize = true;
			this.lblPrikrivanje.Location = new System.Drawing.Point(320, 78);
			this.lblPrikrivanje.Name = "lblPrikrivanje";
			this.lblPrikrivanje.Size = new System.Drawing.Size(72, 13);
			this.lblPrikrivanje.TabIndex = 69;
			this.lblPrikrivanje.Text = "Prikrivanje: xx";
			// 
			// lblOmetanje
			// 
			this.lblOmetanje.AutoSize = true;
			this.lblOmetanje.Location = new System.Drawing.Point(320, 65);
			this.lblOmetanje.Name = "lblOmetanje";
			this.lblOmetanje.Size = new System.Drawing.Size(68, 13);
			this.lblOmetanje.TabIndex = 70;
			this.lblOmetanje.Text = "Ometanje: xx";
			// 
			// FormDizajn
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(631, 307);
			this.Controls.Add(this.lblOmetanje);
			this.Controls.Add(this.lblPrikrivanje);
			this.Controls.Add(this.lblReaktor);
			this.Controls.Add(this.btnStit);
			this.Controls.Add(this.btnSekMisija);
			this.Controls.Add(this.btnPrimMisija);
			this.Controls.Add(this.lblCijena);
			this.Controls.Add(this.lblSlobodno);
			this.Controls.Add(this.btnSpecOpremaMinus);
			this.Controls.Add(this.btnSpecOpremaPlus);
			this.Controls.Add(this.lstvSpecOprema);
			this.Controls.Add(this.lblSpecOprema);
			this.Controls.Add(this.lblTaktika);
			this.Controls.Add(this.cbTaktika);
			this.Controls.Add(this.lblOklop);
			this.Controls.Add(this.lblSenzori);
			this.Controls.Add(this.lblPokretljivost);
			this.Controls.Add(this.lblUdioMisija);
			this.Controls.Add(this.lblUdioSek);
			this.Controls.Add(this.lblStit);
			this.Controls.Add(this.lblSekMisija);
			this.Controls.Add(this.lblPrimMisija);
			this.Controls.Add(this.hscrUdioMisija);
			this.Controls.Add(this.chMZpogon);
			this.Controls.Add(this.btnSpremi);
			this.Controls.Add(this.cbVelicina);
			this.Controls.Add(this.lblVelicina);
			this.Controls.Add(this.txtNaziv);
			this.Controls.Add(this.lblNaziv);
			this.Controls.Add(this.picSlika);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDizajn";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "FormDizajn";
			this.Load += new System.EventHandler(this.FormDizajn_Load);
			((System.ComponentModel.ISupportInitialize)(this.picSlika)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblCijena;
		private System.Windows.Forms.Label lblSlobodno;
		private System.Windows.Forms.Button btnSpecOpremaMinus;
		private System.Windows.Forms.Button btnSpecOpremaPlus;
		private System.Windows.Forms.ListView lstvSpecOprema;
		private System.Windows.Forms.ColumnHeader chSpecOpKolicina;
		private System.Windows.Forms.ColumnHeader chSpecOpNaziv;
		private System.Windows.Forms.Label lblSpecOprema;
		private System.Windows.Forms.Label lblTaktika;
		private System.Windows.Forms.ComboBox cbTaktika;
		private System.Windows.Forms.Label lblOklop;
		private System.Windows.Forms.Label lblSenzori;
		private System.Windows.Forms.Label lblPokretljivost;
		private System.Windows.Forms.Label lblUdioMisija;
		private System.Windows.Forms.Label lblUdioSek;
		private System.Windows.Forms.Label lblStit;
		private System.Windows.Forms.Label lblSekMisija;
		private System.Windows.Forms.Label lblPrimMisija;
		private System.Windows.Forms.HScrollBar hscrUdioMisija;
		private System.Windows.Forms.CheckBox chMZpogon;
		private System.Windows.Forms.Button btnSpremi;
		private System.Windows.Forms.ComboBox cbVelicina;
		private System.Windows.Forms.Label lblVelicina;
		private System.Windows.Forms.TextBox txtNaziv;
		private System.Windows.Forms.Label lblNaziv;
		private System.Windows.Forms.PictureBox picSlika;
		private System.Windows.Forms.Button btnPrimMisija;
		private System.Windows.Forms.Button btnSekMisija;
		private System.Windows.Forms.Button btnStit;
		private System.Windows.Forms.Label lblReaktor;
		private System.Windows.Forms.Label lblPrikrivanje;
		private System.Windows.Forms.Label lblOmetanje;
	}
}