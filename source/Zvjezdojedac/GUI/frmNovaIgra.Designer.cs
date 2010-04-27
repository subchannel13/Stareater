namespace Prototip
{
	partial class frmNovaIgra
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNovaIgra));
			this.label1 = new System.Windows.Forms.Label();
			this.cbVelicinaMape = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtIme = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbOrganizacija = new System.Windows.Forms.ComboBox();
			this.lblOpisMape = new System.Windows.Forms.Label();
			this.lblOpisOrg = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbBrIgraca = new System.Windows.Forms.ComboBox();
			this.btnKreni = new System.Windows.Forms.Button();
			this.btnOdustani = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(217, 63);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Veličina mape:";
			// 
			// cbVelicinaMape
			// 
			this.cbVelicinaMape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVelicinaMape.FormattingEnabled = true;
			this.cbVelicinaMape.Location = new System.Drawing.Point(220, 79);
			this.cbVelicinaMape.Name = "cbVelicinaMape";
			this.cbVelicinaMape.Size = new System.Drawing.Size(121, 21);
			this.cbVelicinaMape.TabIndex = 6;
			this.cbVelicinaMape.SelectedIndexChanged += new System.EventHandler(this.cbVelicinaMape_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Ime igrača:";
			// 
			// txtIme
			// 
			this.txtIme.Location = new System.Drawing.Point(86, 25);
			this.txtIme.Name = "txtIme";
			this.txtIme.Size = new System.Drawing.Size(100, 20);
			this.txtIme.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 63);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(68, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Organizacija:";
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
			this.lblOpisMape.Location = new System.Drawing.Point(217, 103);
			this.lblOpisMape.Name = "lblOpisMape";
			this.lblOpisMape.Size = new System.Drawing.Size(35, 13);
			this.lblOpisMape.TabIndex = 7;
			this.lblOpisMape.Text = "label4";
			// 
			// lblOpisOrg
			// 
			this.lblOpisOrg.Location = new System.Drawing.Point(15, 84);
			this.lblOpisOrg.Name = "lblOpisOrg";
			this.lblOpisOrg.Size = new System.Drawing.Size(171, 59);
			this.lblOpisOrg.TabIndex = 4;
			this.lblOpisOrg.Text = "label4";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 149);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Broj igrača:";
			// 
			// cbBrIgraca
			// 
			this.cbBrIgraca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBrIgraca.FormattingEnabled = true;
			this.cbBrIgraca.Location = new System.Drawing.Point(86, 146);
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
			// frmNovaIgra
			// 
			this.AcceptButton = this.btnKreni;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOdustani;
			this.ClientSize = new System.Drawing.Size(380, 266);
			this.Controls.Add(this.btnOdustani);
			this.Controls.Add(this.btnKreni);
			this.Controls.Add(this.cbBrIgraca);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lblOpisOrg);
			this.Controls.Add(this.lblOpisMape);
			this.Controls.Add(this.cbOrganizacija);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtIme);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbVelicinaMape);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmNovaIgra";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Nova igra";
			this.Load += new System.EventHandler(this.frmNovaIgra_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbVelicinaMape;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtIme;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbOrganizacija;
		private System.Windows.Forms.Label lblOpisMape;
		private System.Windows.Forms.Label lblOpisOrg;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbBrIgraca;
		private System.Windows.Forms.Button btnKreni;
		private System.Windows.Forms.Button btnOdustani;
	}
}