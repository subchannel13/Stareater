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
			this.lblNDslobodno = new System.Windows.Forms.Label();
			this.btnNDspecOpremaMinus = new System.Windows.Forms.Button();
			this.btnNDspecOpremaPlus = new System.Windows.Forms.Button();
			this.lstvNDspecOprema = new System.Windows.Forms.ListView();
			this.chSpecOpKolicina = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chSpecOpNaziv = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chSpecOpVelicina = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblSpecOprema = new System.Windows.Forms.Label();
			this.lblTaktika = new System.Windows.Forms.Label();
			this.cbNDtaktika = new System.Windows.Forms.ComboBox();
			this.lblNDoklop = new System.Windows.Forms.Label();
			this.lblNDsenzori = new System.Windows.Forms.Label();
			this.lblNDpokretljivost = new System.Windows.Forms.Label();
			this.lblNDudioMisija = new System.Windows.Forms.Label();
			this.lblUdioSek = new System.Windows.Forms.Label();
			this.lblStit = new System.Windows.Forms.Label();
			this.lblSekMisija = new System.Windows.Forms.Label();
			this.lblPrimMisija = new System.Windows.Forms.Label();
			this.cbNDsekMisija = new System.Windows.Forms.ComboBox();
			this.cbNDstit = new System.Windows.Forms.ComboBox();
			this.hscrUdioMisija = new System.Windows.Forms.HScrollBar();
			this.chMZpogon = new System.Windows.Forms.CheckBox();
			this.btnSpremi = new System.Windows.Forms.Button();
			this.cbVelicina = new System.Windows.Forms.ComboBox();
			this.lblVelicina = new System.Windows.Forms.Label();
			this.txtNaziv = new System.Windows.Forms.TextBox();
			this.lblNaziv = new System.Windows.Forms.Label();
			this.picSlika = new System.Windows.Forms.PictureBox();
			this.btnPrimMisija = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picSlika)).BeginInit();
			this.SuspendLayout();
			// 
			// lblCijena
			// 
			this.lblCijena.AutoSize = true;
			this.lblCijena.Location = new System.Drawing.Point(297, 367);
			this.lblCijena.Name = "lblCijena";
			this.lblCijena.Size = new System.Drawing.Size(75, 13);
			this.lblCijena.TabIndex = 64;
			this.lblCijena.Text = "Cijena: xx.xx X";
			// 
			// lblNDslobodno
			// 
			this.lblNDslobodno.AutoSize = true;
			this.lblNDslobodno.Location = new System.Drawing.Point(298, 380);
			this.lblNDslobodno.Name = "lblNDslobodno";
			this.lblNDslobodno.Size = new System.Drawing.Size(170, 13);
			this.lblNDslobodno.TabIndex = 60;
			this.lblNDslobodno.Text = "Slobodan prostor: xx.xx X / xx.xx X";
			// 
			// btnNDspecOpremaMinus
			// 
			this.btnNDspecOpremaMinus.Location = new System.Drawing.Point(258, 362);
			this.btnNDspecOpremaMinus.Name = "btnNDspecOpremaMinus";
			this.btnNDspecOpremaMinus.Size = new System.Drawing.Size(33, 23);
			this.btnNDspecOpremaMinus.TabIndex = 59;
			this.btnNDspecOpremaMinus.Text = "-";
			this.btnNDspecOpremaMinus.UseVisualStyleBackColor = true;
			// 
			// btnNDspecOpremaPlus
			// 
			this.btnNDspecOpremaPlus.Location = new System.Drawing.Point(219, 362);
			this.btnNDspecOpremaPlus.Name = "btnNDspecOpremaPlus";
			this.btnNDspecOpremaPlus.Size = new System.Drawing.Size(33, 23);
			this.btnNDspecOpremaPlus.TabIndex = 58;
			this.btnNDspecOpremaPlus.Text = "+";
			this.btnNDspecOpremaPlus.UseVisualStyleBackColor = true;
			// 
			// lstvNDspecOprema
			// 
			this.lstvNDspecOprema.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSpecOpKolicina,
            this.chSpecOpNaziv,
            this.chSpecOpVelicina});
			this.lstvNDspecOprema.FullRowSelect = true;
			this.lstvNDspecOprema.GridLines = true;
			this.lstvNDspecOprema.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstvNDspecOprema.HideSelection = false;
			this.lstvNDspecOprema.Location = new System.Drawing.Point(114, 252);
			this.lstvNDspecOprema.MultiSelect = false;
			this.lstvNDspecOprema.Name = "lstvNDspecOprema";
			this.lstvNDspecOprema.Size = new System.Drawing.Size(405, 104);
			this.lstvNDspecOprema.TabIndex = 57;
			this.lstvNDspecOprema.UseCompatibleStateImageBehavior = false;
			this.lstvNDspecOprema.View = System.Windows.Forms.View.Details;
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
			// chSpecOpVelicina
			// 
			this.chSpecOpVelicina.Text = "Velicina";
			// 
			// lblSpecOprema
			// 
			this.lblSpecOprema.AutoSize = true;
			this.lblSpecOprema.Location = new System.Drawing.Point(12, 252);
			this.lblSpecOprema.Name = "lblSpecOprema";
			this.lblSpecOprema.Size = new System.Drawing.Size(97, 13);
			this.lblSpecOprema.TabIndex = 56;
			this.lblSpecOprema.Text = "Specijalna oprema:";
			// 
			// lblTaktika
			// 
			this.lblTaktika.AutoSize = true;
			this.lblTaktika.Location = new System.Drawing.Point(12, 228);
			this.lblTaktika.Name = "lblTaktika";
			this.lblTaktika.Size = new System.Drawing.Size(46, 13);
			this.lblTaktika.TabIndex = 55;
			this.lblTaktika.Text = "Taktika:";
			// 
			// cbNDtaktika
			// 
			this.cbNDtaktika.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbNDtaktika.FormattingEnabled = true;
			this.cbNDtaktika.Location = new System.Drawing.Point(114, 225);
			this.cbNDtaktika.Name = "cbNDtaktika";
			this.cbNDtaktika.Size = new System.Drawing.Size(178, 21);
			this.cbNDtaktika.TabIndex = 54;
			// 
			// lblNDoklop
			// 
			this.lblNDoklop.AutoSize = true;
			this.lblNDoklop.Location = new System.Drawing.Point(321, 41);
			this.lblNDoklop.Name = "lblNDoklop";
			this.lblNDoklop.Size = new System.Drawing.Size(135, 13);
			this.lblNDoklop.TabIndex = 52;
			this.lblNDoklop.Text = "Izdržljivost okolopa: xx.xx X";
			// 
			// lblNDsenzori
			// 
			this.lblNDsenzori.AutoSize = true;
			this.lblNDsenzori.Location = new System.Drawing.Point(321, 15);
			this.lblNDsenzori.Name = "lblNDsenzori";
			this.lblNDsenzori.Size = new System.Drawing.Size(117, 13);
			this.lblNDsenzori.TabIndex = 51;
			this.lblNDsenzori.Text = "Snaga senzora: xx.xx X";
			// 
			// lblNDpokretljivost
			// 
			this.lblNDpokretljivost.AutoSize = true;
			this.lblNDpokretljivost.Location = new System.Drawing.Point(321, 28);
			this.lblNDpokretljivost.Name = "lblNDpokretljivost";
			this.lblNDpokretljivost.Size = new System.Drawing.Size(103, 13);
			this.lblNDpokretljivost.TabIndex = 50;
			this.lblNDpokretljivost.Text = "Pokretljivost: xx.xx X";
			// 
			// lblNDudioMisija
			// 
			this.lblNDudioMisija.AutoSize = true;
			this.lblNDudioMisija.Location = new System.Drawing.Point(255, 159);
			this.lblNDudioMisija.Name = "lblNDudioMisija";
			this.lblNDudioMisija.Size = new System.Drawing.Size(28, 13);
			this.lblNDudioMisija.TabIndex = 49;
			this.lblNDudioMisija.Text = "xx %";
			// 
			// lblUdioSek
			// 
			this.lblUdioSek.AutoSize = true;
			this.lblUdioSek.Location = new System.Drawing.Point(12, 159);
			this.lblUdioSek.Name = "lblUdioSek";
			this.lblUdioSek.Size = new System.Drawing.Size(91, 13);
			this.lblUdioSek.TabIndex = 48;
			this.lblUdioSek.Text = "Udio sekundarne:";
			// 
			// lblStit
			// 
			this.lblStit.AutoSize = true;
			this.lblStit.Location = new System.Drawing.Point(12, 178);
			this.lblStit.Name = "lblStit";
			this.lblStit.Size = new System.Drawing.Size(25, 13);
			this.lblStit.TabIndex = 47;
			this.lblStit.Text = "Štit:";
			// 
			// lblSekMisija
			// 
			this.lblSekMisija.AutoSize = true;
			this.lblSekMisija.Location = new System.Drawing.Point(12, 134);
			this.lblSekMisija.Name = "lblSekMisija";
			this.lblSekMisija.Size = new System.Drawing.Size(96, 13);
			this.lblSekMisija.TabIndex = 46;
			this.lblSekMisija.Text = "Sekundarna misija:";
			// 
			// lblPrimMisija
			// 
			this.lblPrimMisija.AutoSize = true;
			this.lblPrimMisija.Location = new System.Drawing.Point(12, 107);
			this.lblPrimMisija.Name = "lblPrimMisija";
			this.lblPrimMisija.Size = new System.Drawing.Size(79, 13);
			this.lblPrimMisija.TabIndex = 45;
			this.lblPrimMisija.Text = "Primarna misija:";
			// 
			// cbNDsekMisija
			// 
			this.cbNDsekMisija.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbNDsekMisija.FormattingEnabled = true;
			this.cbNDsekMisija.Location = new System.Drawing.Point(114, 131);
			this.cbNDsekMisija.Name = "cbNDsekMisija";
			this.cbNDsekMisija.Size = new System.Drawing.Size(178, 21);
			this.cbNDsekMisija.TabIndex = 33;
			// 
			// cbNDstit
			// 
			this.cbNDstit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbNDstit.FormattingEnabled = true;
			this.cbNDstit.Location = new System.Drawing.Point(114, 175);
			this.cbNDstit.Name = "cbNDstit";
			this.cbNDstit.Size = new System.Drawing.Size(178, 21);
			this.cbNDstit.TabIndex = 37;
			// 
			// hscrUdioMisija
			// 
			this.hscrUdioMisija.LargeChange = 1;
			this.hscrUdioMisija.Location = new System.Drawing.Point(114, 155);
			this.hscrUdioMisija.Maximum = 50;
			this.hscrUdioMisija.Name = "hscrUdioMisija";
			this.hscrUdioMisija.Size = new System.Drawing.Size(138, 17);
			this.hscrUdioMisija.TabIndex = 39;
			// 
			// chMZpogon
			// 
			this.chMZpogon.AutoSize = true;
			this.chMZpogon.Location = new System.Drawing.Point(114, 202);
			this.chMZpogon.Name = "chMZpogon";
			this.chMZpogon.Size = new System.Drawing.Size(75, 17);
			this.chMZpogon.TabIndex = 44;
			this.chMZpogon.Text = "MZ pogon";
			this.chMZpogon.UseVisualStyleBackColor = true;
			// 
			// btnSpremi
			// 
			this.btnSpremi.Location = new System.Drawing.Point(114, 425);
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
			this.btnPrimMisija.Location = new System.Drawing.Point(114, 102);
			this.btnPrimMisija.Name = "btnPrimMisija";
			this.btnPrimMisija.Size = new System.Drawing.Size(178, 23);
			this.btnPrimMisija.TabIndex = 65;
			this.btnPrimMisija.Text = "999 k x Tachyon Megablaster";
			this.btnPrimMisija.UseVisualStyleBackColor = true;
			this.btnPrimMisija.Click += new System.EventHandler(this.btnPrimMisija_Click);
			// 
			// FormDizajn
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(712, 485);
			this.Controls.Add(this.btnPrimMisija);
			this.Controls.Add(this.lblCijena);
			this.Controls.Add(this.lblNDslobodno);
			this.Controls.Add(this.btnNDspecOpremaMinus);
			this.Controls.Add(this.btnNDspecOpremaPlus);
			this.Controls.Add(this.lstvNDspecOprema);
			this.Controls.Add(this.lblSpecOprema);
			this.Controls.Add(this.lblTaktika);
			this.Controls.Add(this.cbNDtaktika);
			this.Controls.Add(this.lblNDoklop);
			this.Controls.Add(this.lblNDsenzori);
			this.Controls.Add(this.lblNDpokretljivost);
			this.Controls.Add(this.lblNDudioMisija);
			this.Controls.Add(this.lblUdioSek);
			this.Controls.Add(this.lblStit);
			this.Controls.Add(this.lblSekMisija);
			this.Controls.Add(this.lblPrimMisija);
			this.Controls.Add(this.cbNDsekMisija);
			this.Controls.Add(this.cbNDstit);
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
		private System.Windows.Forms.Label lblNDslobodno;
		private System.Windows.Forms.Button btnNDspecOpremaMinus;
		private System.Windows.Forms.Button btnNDspecOpremaPlus;
		private System.Windows.Forms.ListView lstvNDspecOprema;
		private System.Windows.Forms.ColumnHeader chSpecOpKolicina;
		private System.Windows.Forms.ColumnHeader chSpecOpNaziv;
		private System.Windows.Forms.ColumnHeader chSpecOpVelicina;
		private System.Windows.Forms.Label lblSpecOprema;
		private System.Windows.Forms.Label lblTaktika;
		private System.Windows.Forms.ComboBox cbNDtaktika;
		private System.Windows.Forms.Label lblNDoklop;
		private System.Windows.Forms.Label lblNDsenzori;
		private System.Windows.Forms.Label lblNDpokretljivost;
		private System.Windows.Forms.Label lblNDudioMisija;
		private System.Windows.Forms.Label lblUdioSek;
		private System.Windows.Forms.Label lblStit;
		private System.Windows.Forms.Label lblSekMisija;
		private System.Windows.Forms.Label lblPrimMisija;
		private System.Windows.Forms.ComboBox cbNDsekMisija;
		private System.Windows.Forms.ComboBox cbNDstit;
		private System.Windows.Forms.HScrollBar hscrUdioMisija;
		private System.Windows.Forms.CheckBox chMZpogon;
		private System.Windows.Forms.Button btnSpremi;
		private System.Windows.Forms.ComboBox cbVelicina;
		private System.Windows.Forms.Label lblVelicina;
		private System.Windows.Forms.TextBox txtNaziv;
		private System.Windows.Forms.Label lblNaziv;
		private System.Windows.Forms.PictureBox picSlika;
		private System.Windows.Forms.Button btnPrimMisija;
	}
}