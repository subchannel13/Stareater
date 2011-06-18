namespace Zvjezdojedac.GUI
{
	partial class FormNovaIgra
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
			if (disposing && (components != null))
			{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNovaIgra));
			this.lblVelicinaMape = new System.Windows.Forms.Label();
			this.cbVelicinaMape = new System.Windows.Forms.ComboBox();
			this.lblImeIgraca = new System.Windows.Forms.Label();
			this.txtIme = new System.Windows.Forms.TextBox();
			this.lblOrganizacija = new System.Windows.Forms.Label();
			this.cbOrganizacija = new System.Windows.Forms.ComboBox();
			this.lblOpisMape = new System.Windows.Forms.Label();
			this.lblOpisOrg = new System.Windows.Forms.Label();
			this.lblBrojIgraca = new System.Windows.Forms.Label();
			this.cbBrIgraca = new System.Windows.Forms.ComboBox();
			this.btnKreni = new System.Windows.Forms.Button();
			this.btnOdustani = new System.Windows.Forms.Button();
			this.cbPocetnaPop = new System.Windows.Forms.ComboBox();
			this.lblPocetnaPop = new System.Windows.Forms.Label();
			this.lblPopulacija = new System.Windows.Forms.Label();
			this.lblBrKolonija = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblVelicinaMape
			// 
			this.lblVelicinaMape.AutoSize = true;
			this.lblVelicinaMape.Location = new System.Drawing.Point(213, 28);
			this.lblVelicinaMape.Name = "lblVelicinaMape";
			this.lblVelicinaMape.Size = new System.Drawing.Size(76, 13);
			this.lblVelicinaMape.TabIndex = 5;
			this.lblVelicinaMape.Text = "Veličina mape:";
			// 
			// cbVelicinaMape
			// 
			this.cbVelicinaMape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVelicinaMape.FormattingEnabled = true;
			this.cbVelicinaMape.Location = new System.Drawing.Point(216, 44);
			this.cbVelicinaMape.Name = "cbVelicinaMape";
			this.cbVelicinaMape.Size = new System.Drawing.Size(121, 21);
			this.cbVelicinaMape.TabIndex = 6;
			this.cbVelicinaMape.SelectedIndexChanged += new System.EventHandler(this.cbVelicinaMape_SelectedIndexChanged);
			// 
			// lblImeIgraca
			// 
			this.lblImeIgraca.AutoSize = true;
			this.lblImeIgraca.Location = new System.Drawing.Point(12, 28);
			this.lblImeIgraca.Name = "lblImeIgraca";
			this.lblImeIgraca.Size = new System.Drawing.Size(59, 13);
			this.lblImeIgraca.TabIndex = 0;
			this.lblImeIgraca.Text = "Ime igrača:";
			// 
			// txtIme
			// 
			this.txtIme.Location = new System.Drawing.Point(86, 25);
			this.txtIme.Name = "txtIme";
			this.txtIme.Size = new System.Drawing.Size(100, 20);
			this.txtIme.TabIndex = 1;
			// 
			// lblOrganizacija
			// 
			this.lblOrganizacija.AutoSize = true;
			this.lblOrganizacija.Location = new System.Drawing.Point(12, 63);
			this.lblOrganizacija.Name = "lblOrganizacija";
			this.lblOrganizacija.Size = new System.Drawing.Size(68, 13);
			this.lblOrganizacija.TabIndex = 2;
			this.lblOrganizacija.Text = "Organizacija:";
			// 
			// cbOrganizacija
			// 
			this.cbOrganizacija.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbOrganizacija.FormattingEnabled = true;
			this.cbOrganizacija.Location = new System.Drawing.Point(86, 60);
			this.cbOrganizacija.Name = "cbOrganizacija";
			this.cbOrganizacija.Size = new System.Drawing.Size(100, 21);
			this.cbOrganizacija.TabIndex = 3;
			this.cbOrganizacija.SelectedIndexChanged += new System.EventHandler(this.cbOrganizacija_SelectedIndexChanged);
			// 
			// lblOpisMape
			// 
			this.lblOpisMape.AutoSize = true;
			this.lblOpisMape.Location = new System.Drawing.Point(213, 68);
			this.lblOpisMape.Name = "lblOpisMape";
			this.lblOpisMape.Size = new System.Drawing.Size(64, 26);
			this.lblOpisMape.TabIndex = 7;
			this.lblOpisMape.Text = "Br. zvijezda:\r\ndrugi red";
			// 
			// lblOpisOrg
			// 
			this.lblOpisOrg.Location = new System.Drawing.Point(15, 84);
			this.lblOpisOrg.Name = "lblOpisOrg";
			this.lblOpisOrg.Size = new System.Drawing.Size(171, 59);
			this.lblOpisOrg.TabIndex = 4;
			this.lblOpisOrg.Text = "label4";
			// 
			// lblBrojIgraca
			// 
			this.lblBrojIgraca.AutoSize = true;
			this.lblBrojIgraca.Location = new System.Drawing.Point(12, 149);
			this.lblBrojIgraca.Name = "lblBrojIgraca";
			this.lblBrojIgraca.Size = new System.Drawing.Size(60, 13);
			this.lblBrojIgraca.TabIndex = 8;
			this.lblBrojIgraca.Text = "Broj igrača:";
			// 
			// cbBrIgraca
			// 
			this.cbBrIgraca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBrIgraca.FormattingEnabled = true;
			this.cbBrIgraca.Location = new System.Drawing.Point(130, 146);
			this.cbBrIgraca.Name = "cbBrIgraca";
			this.cbBrIgraca.Size = new System.Drawing.Size(56, 21);
			this.cbBrIgraca.TabIndex = 9;
			// 
			// btnKreni
			// 
			this.btnKreni.Location = new System.Drawing.Point(112, 229);
			this.btnKreni.Name = "btnKreni";
			this.btnKreni.Size = new System.Drawing.Size(75, 23);
			this.btnKreni.TabIndex = 10;
			this.btnKreni.Text = "Kreni";
			this.btnKreni.UseVisualStyleBackColor = true;
			this.btnKreni.Click += new System.EventHandler(this.btnKreni_Click);
			// 
			// btnOdustani
			// 
			this.btnOdustani.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOdustani.Location = new System.Drawing.Point(193, 229);
			this.btnOdustani.Name = "btnOdustani";
			this.btnOdustani.Size = new System.Drawing.Size(75, 23);
			this.btnOdustani.TabIndex = 11;
			this.btnOdustani.Text = "Odustani";
			this.btnOdustani.UseVisualStyleBackColor = true;
			this.btnOdustani.Click += new System.EventHandler(this.btnOdustani_Click);
			// 
			// cbPocetnaPop
			// 
			this.cbPocetnaPop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPocetnaPop.FormattingEnabled = true;
			this.cbPocetnaPop.Location = new System.Drawing.Point(216, 110);
			this.cbPocetnaPop.Name = "cbPocetnaPop";
			this.cbPocetnaPop.Size = new System.Drawing.Size(121, 21);
			this.cbPocetnaPop.TabIndex = 12;
			this.cbPocetnaPop.SelectedIndexChanged += new System.EventHandler(this.cbPocetnaPop_SelectedIndexChanged);
			// 
			// lblPocetnaPop
			// 
			this.lblPocetnaPop.AutoSize = true;
			this.lblPocetnaPop.Location = new System.Drawing.Point(213, 94);
			this.lblPocetnaPop.Name = "lblPocetnaPop";
			this.lblPocetnaPop.Size = new System.Drawing.Size(101, 13);
			this.lblPocetnaPop.TabIndex = 13;
			this.lblPocetnaPop.Text = "Početna populacija:";
			// 
			// lblPopulacija
			// 
			this.lblPopulacija.AutoSize = true;
			this.lblPopulacija.Location = new System.Drawing.Point(213, 134);
			this.lblPopulacija.Name = "lblPopulacija";
			this.lblPopulacija.Size = new System.Drawing.Size(95, 13);
			this.lblPopulacija.TabIndex = 14;
			this.lblPopulacija.Text = "Populacija: xx.xx X";
			// 
			// lblBrKolonija
			// 
			this.lblBrKolonija.AutoSize = true;
			this.lblBrKolonija.Location = new System.Drawing.Point(213, 147);
			this.lblBrKolonija.Name = "lblBrKolonija";
			this.lblBrKolonija.Size = new System.Drawing.Size(70, 13);
			this.lblBrKolonija.TabIndex = 15;
			this.lblBrKolonija.Text = "Br. kolonija: x";
			// 
			// FormNovaIgra
			// 
			this.AcceptButton = this.btnKreni;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOdustani;
			this.ClientSize = new System.Drawing.Size(380, 266);
			this.Controls.Add(this.lblBrKolonija);
			this.Controls.Add(this.lblPopulacija);
			this.Controls.Add(this.lblPocetnaPop);
			this.Controls.Add(this.cbPocetnaPop);
			this.Controls.Add(this.btnOdustani);
			this.Controls.Add(this.btnKreni);
			this.Controls.Add(this.cbBrIgraca);
			this.Controls.Add(this.lblBrojIgraca);
			this.Controls.Add(this.lblOpisOrg);
			this.Controls.Add(this.lblOpisMape);
			this.Controls.Add(this.cbOrganizacija);
			this.Controls.Add(this.lblOrganizacija);
			this.Controls.Add(this.txtIme);
			this.Controls.Add(this.lblImeIgraca);
			this.Controls.Add(this.cbVelicinaMape);
			this.Controls.Add(this.lblVelicinaMape);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormNovaIgra";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Nova igra";
			this.Load += new System.EventHandler(this.frmNovaIgra_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblVelicinaMape;
		private System.Windows.Forms.ComboBox cbVelicinaMape;
		private System.Windows.Forms.Label lblImeIgraca;
		private System.Windows.Forms.TextBox txtIme;
		private System.Windows.Forms.Label lblOrganizacija;
		private System.Windows.Forms.ComboBox cbOrganizacija;
		private System.Windows.Forms.Label lblOpisMape;
		private System.Windows.Forms.Label lblOpisOrg;
		private System.Windows.Forms.Label lblBrojIgraca;
		private System.Windows.Forms.ComboBox cbBrIgraca;
		private System.Windows.Forms.Button btnKreni;
		private System.Windows.Forms.Button btnOdustani;
		private System.Windows.Forms.ComboBox cbPocetnaPop;
		private System.Windows.Forms.Label lblPocetnaPop;
		private System.Windows.Forms.Label lblPopulacija;
		private System.Windows.Forms.Label lblBrKolonija;
	}
}