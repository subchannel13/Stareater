namespace Prototip
{
	partial class FormTechIzbor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTechIzbor));
			this.tabControlTech = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.btnOk = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.trkRazKoncentracija = new System.Windows.Forms.TrackBar();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.lblRazOpis = new System.Windows.Forms.Label();
			this.picRazSlika = new System.Windows.Forms.PictureBox();
			this.btnRazDno = new System.Windows.Forms.Button();
			this.btnRazDolje = new System.Windows.Forms.Button();
			this.btnRazGore = new System.Windows.Forms.Button();
			this.btnRazVrh = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lstRazvoj = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.tabIstrazivanje = new System.Windows.Forms.TabPage();
			this.btnIstDno = new System.Windows.Forms.Button();
			this.btnIstDolje = new System.Windows.Forms.Button();
			this.btnIstGore = new System.Windows.Forms.Button();
			this.btnIstVrh = new System.Windows.Forms.Button();
			this.lblIstSustav = new System.Windows.Forms.Label();
			this.lblIstPoeni = new System.Windows.Forms.Label();
			this.lblIstOpis = new System.Windows.Forms.Label();
			this.picIstSlika = new System.Windows.Forms.PictureBox();
			this.lstIstrazivanje = new System.Windows.Forms.ListView();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
			this.label5 = new System.Windows.Forms.Label();
			this.chNazivIstr = new System.Windows.Forms.ColumnHeader();
			this.chNivoIstr = new System.Windows.Forms.ColumnHeader();
			this.chPoeniIstr = new System.Windows.Forms.ColumnHeader();
			this.chPoeniPlus = new System.Windows.Forms.ColumnHeader();
			this.tabControlTech.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkRazKoncentracija)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picRazSlika)).BeginInit();
			this.tabIstrazivanje.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picIstSlika)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControlTech
			// 
			this.tabControlTech.Controls.Add(this.tabPage1);
			this.tabControlTech.Controls.Add(this.tabIstrazivanje);
			this.tabControlTech.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlTech.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.tabControlTech.Location = new System.Drawing.Point(0, 0);
			this.tabControlTech.Name = "tabControlTech";
			this.tabControlTech.SelectedIndex = 0;
			this.tabControlTech.Size = new System.Drawing.Size(594, 448);
			this.tabControlTech.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.btnOk);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.trkRazKoncentracija);
			this.tabPage1.Controls.Add(this.trackBar1);
			this.tabPage1.Controls.Add(this.lblRazOpis);
			this.tabPage1.Controls.Add(this.picRazSlika);
			this.tabPage1.Controls.Add(this.btnRazDno);
			this.tabPage1.Controls.Add(this.btnRazDolje);
			this.tabPage1.Controls.Add(this.btnRazGore);
			this.tabPage1.Controls.Add(this.btnRazVrh);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.lstRazvoj);
			this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.tabPage1.Location = new System.Drawing.Point(4, 29);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(586, 415);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Razvoj";
			// 
			// btnOk
			// 
			this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnOk.Location = new System.Drawing.Point(390, 386);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 19;
			this.btnOk.Text = "U redu";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label4.Location = new System.Drawing.Point(519, 248);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(59, 13);
			this.label4.TabIndex = 18;
			this.label4.Text = "Fokusirano";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label3.Location = new System.Drawing.Point(394, 248);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(70, 13);
			this.label3.TabIndex = 17;
			this.label3.Text = "Ravnomjerno";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label2.Location = new System.Drawing.Point(394, 184);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 13);
			this.label2.TabIndex = 16;
			this.label2.Text = "Raspodjela poena:";
			// 
			// trkRazKoncentracija
			// 
			this.trkRazKoncentracija.Location = new System.Drawing.Point(394, 200);
			this.trkRazKoncentracija.Name = "trkRazKoncentracija";
			this.trkRazKoncentracija.Size = new System.Drawing.Size(184, 45);
			this.trkRazKoncentracija.TabIndex = 15;
			this.trkRazKoncentracija.Scroll += new System.EventHandler(this.trkRazKoncentracija_Scroll);
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(549, 567);
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(104, 45);
			this.trackBar1.TabIndex = 14;
			// 
			// lblRazOpis
			// 
			this.lblRazOpis.AutoSize = true;
			this.lblRazOpis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lblRazOpis.Location = new System.Drawing.Point(94, 275);
			this.lblRazOpis.Name = "lblRazOpis";
			this.lblRazOpis.Size = new System.Drawing.Size(80, 13);
			this.lblRazOpis.TabIndex = 13;
			this.lblRazOpis.Text = "opis tehnologije";
			// 
			// picRazSlika
			// 
			this.picRazSlika.Location = new System.Drawing.Point(8, 275);
			this.picRazSlika.Name = "picRazSlika";
			this.picRazSlika.Size = new System.Drawing.Size(80, 80);
			this.picRazSlika.TabIndex = 12;
			this.picRazSlika.TabStop = false;
			// 
			// btnRazDno
			// 
			this.btnRazDno.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnRazDno.Location = new System.Drawing.Point(394, 137);
			this.btnRazDno.Name = "btnRazDno";
			this.btnRazDno.Size = new System.Drawing.Size(75, 23);
			this.btnRazDno.TabIndex = 11;
			this.btnRazDno.Text = "Na dno";
			this.btnRazDno.UseVisualStyleBackColor = true;
			this.btnRazDno.Click += new System.EventHandler(this.btnRazDno_Click);
			// 
			// btnRazDolje
			// 
			this.btnRazDolje.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnRazDolje.Location = new System.Drawing.Point(394, 92);
			this.btnRazDolje.Name = "btnRazDolje";
			this.btnRazDolje.Size = new System.Drawing.Size(75, 23);
			this.btnRazDolje.TabIndex = 10;
			this.btnRazDolje.Text = "Dolje";
			this.btnRazDolje.UseVisualStyleBackColor = true;
			this.btnRazDolje.Click += new System.EventHandler(this.btnRazDolje_Click);
			// 
			// btnRazGore
			// 
			this.btnRazGore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnRazGore.Location = new System.Drawing.Point(394, 63);
			this.btnRazGore.Name = "btnRazGore";
			this.btnRazGore.Size = new System.Drawing.Size(75, 23);
			this.btnRazGore.TabIndex = 9;
			this.btnRazGore.Text = "Gore";
			this.btnRazGore.UseVisualStyleBackColor = true;
			this.btnRazGore.Click += new System.EventHandler(this.btnRazGore_Click);
			// 
			// btnRazVrh
			// 
			this.btnRazVrh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnRazVrh.Location = new System.Drawing.Point(394, 19);
			this.btnRazVrh.Name = "btnRazVrh";
			this.btnRazVrh.Size = new System.Drawing.Size(75, 23);
			this.btnRazVrh.TabIndex = 8;
			this.btnRazVrh.Text = "Na vrh";
			this.btnRazVrh.UseVisualStyleBackColor = true;
			this.btnRazVrh.Click += new System.EventHandler(this.btnRazVrh_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label1.Location = new System.Drawing.Point(8, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "U razvoju:";
			// 
			// lstRazvoj
			// 
			this.lstRazvoj.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
			this.lstRazvoj.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lstRazvoj.FullRowSelect = true;
			this.lstRazvoj.HideSelection = false;
			this.lstRazvoj.Location = new System.Drawing.Point(8, 19);
			this.lstRazvoj.MultiSelect = false;
			this.lstRazvoj.Name = "lstRazvoj";
			this.lstRazvoj.Size = new System.Drawing.Size(380, 250);
			this.lstRazvoj.TabIndex = 6;
			this.lstRazvoj.UseCompatibleStateImageBehavior = false;
			this.lstRazvoj.View = System.Windows.Forms.View.Details;
			this.lstRazvoj.SelectedIndexChanged += new System.EventHandler(this.lstRazvoj_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Naziv";
			this.columnHeader1.Width = 150;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Nivo";
			this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader2.Width = 45;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Poeni";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader3.Width = 115;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Ulaganje";
			this.columnHeader4.Width = 65;
			// 
			// tabIstrazivanje
			// 
			this.tabIstrazivanje.BackColor = System.Drawing.SystemColors.Control;
			this.tabIstrazivanje.Controls.Add(this.btnIstDno);
			this.tabIstrazivanje.Controls.Add(this.btnIstDolje);
			this.tabIstrazivanje.Controls.Add(this.btnIstGore);
			this.tabIstrazivanje.Controls.Add(this.btnIstVrh);
			this.tabIstrazivanje.Controls.Add(this.lblIstSustav);
			this.tabIstrazivanje.Controls.Add(this.lblIstPoeni);
			this.tabIstrazivanje.Controls.Add(this.lblIstOpis);
			this.tabIstrazivanje.Controls.Add(this.picIstSlika);
			this.tabIstrazivanje.Controls.Add(this.lstIstrazivanje);
			this.tabIstrazivanje.Controls.Add(this.label5);
			this.tabIstrazivanje.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.tabIstrazivanje.Location = new System.Drawing.Point(4, 29);
			this.tabIstrazivanje.Name = "tabIstrazivanje";
			this.tabIstrazivanje.Padding = new System.Windows.Forms.Padding(3);
			this.tabIstrazivanje.Size = new System.Drawing.Size(586, 415);
			this.tabIstrazivanje.TabIndex = 1;
			this.tabIstrazivanje.Text = "Istraživanje";
			// 
			// btnIstDno
			// 
			this.btnIstDno.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnIstDno.Location = new System.Drawing.Point(378, 137);
			this.btnIstDno.Name = "btnIstDno";
			this.btnIstDno.Size = new System.Drawing.Size(75, 23);
			this.btnIstDno.TabIndex = 15;
			this.btnIstDno.Text = "Na dno";
			this.btnIstDno.UseVisualStyleBackColor = true;
			this.btnIstDno.Click += new System.EventHandler(this.btnIstDno_Click);
			// 
			// btnIstDolje
			// 
			this.btnIstDolje.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnIstDolje.Location = new System.Drawing.Point(378, 92);
			this.btnIstDolje.Name = "btnIstDolje";
			this.btnIstDolje.Size = new System.Drawing.Size(75, 23);
			this.btnIstDolje.TabIndex = 14;
			this.btnIstDolje.Text = "Dolje";
			this.btnIstDolje.UseVisualStyleBackColor = true;
			this.btnIstDolje.Click += new System.EventHandler(this.btnIstDolje_Click);
			// 
			// btnIstGore
			// 
			this.btnIstGore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnIstGore.Location = new System.Drawing.Point(378, 63);
			this.btnIstGore.Name = "btnIstGore";
			this.btnIstGore.Size = new System.Drawing.Size(75, 23);
			this.btnIstGore.TabIndex = 13;
			this.btnIstGore.Text = "Gore";
			this.btnIstGore.UseVisualStyleBackColor = true;
			this.btnIstGore.Click += new System.EventHandler(this.btnIstGore_Click);
			// 
			// btnIstVrh
			// 
			this.btnIstVrh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnIstVrh.Location = new System.Drawing.Point(378, 19);
			this.btnIstVrh.Name = "btnIstVrh";
			this.btnIstVrh.Size = new System.Drawing.Size(75, 23);
			this.btnIstVrh.TabIndex = 12;
			this.btnIstVrh.Text = "Na vrh";
			this.btnIstVrh.UseVisualStyleBackColor = true;
			this.btnIstVrh.Click += new System.EventHandler(this.btnIstVrh_Click);
			// 
			// lblIstSustav
			// 
			this.lblIstSustav.AutoSize = true;
			this.lblIstSustav.Location = new System.Drawing.Point(378, 213);
			this.lblIstSustav.Name = "lblIstSustav";
			this.lblIstSustav.Size = new System.Drawing.Size(44, 13);
			this.lblIstSustav.TabIndex = 5;
			this.lblIstSustav.Text = "(sustav)";
			// 
			// lblIstPoeni
			// 
			this.lblIstPoeni.AutoSize = true;
			this.lblIstPoeni.Location = new System.Drawing.Point(378, 200);
			this.lblIstPoeni.Name = "lblIstPoeni";
			this.lblIstPoeni.Size = new System.Drawing.Size(89, 13);
			this.lblIstPoeni.TabIndex = 4;
			this.lblIstPoeni.Text = "Poeni istraživanja";
			// 
			// lblIstOpis
			// 
			this.lblIstOpis.AutoSize = true;
			this.lblIstOpis.Location = new System.Drawing.Point(94, 275);
			this.lblIstOpis.Name = "lblIstOpis";
			this.lblIstOpis.Size = new System.Drawing.Size(80, 13);
			this.lblIstOpis.TabIndex = 3;
			this.lblIstOpis.Text = "opis tehnologije";
			// 
			// picIstSlika
			// 
			this.picIstSlika.Location = new System.Drawing.Point(8, 275);
			this.picIstSlika.Name = "picIstSlika";
			this.picIstSlika.Size = new System.Drawing.Size(80, 80);
			this.picIstSlika.TabIndex = 2;
			this.picIstSlika.TabStop = false;
			// 
			// lstIstrazivanje
			// 
			this.lstIstrazivanje.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
			this.lstIstrazivanje.FullRowSelect = true;
			this.lstIstrazivanje.Location = new System.Drawing.Point(8, 19);
			this.lstIstrazivanje.MultiSelect = false;
			this.lstIstrazivanje.Name = "lstIstrazivanje";
			this.lstIstrazivanje.Size = new System.Drawing.Size(364, 250);
			this.lstIstrazivanje.TabIndex = 1;
			this.lstIstrazivanje.UseCompatibleStateImageBehavior = false;
			this.lstIstrazivanje.View = System.Windows.Forms.View.Details;
			this.lstIstrazivanje.SelectedIndexChanged += new System.EventHandler(this.lstIstrazivanje_SelectedIndexChanged);
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Naziv";
			this.columnHeader5.Width = 150;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "Nivo";
			this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader6.Width = 45;
			// 
			// columnHeader7
			// 
			this.columnHeader7.Text = "Poeni";
			this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader7.Width = 115;
			// 
			// columnHeader8
			// 
			this.columnHeader8.Text = "Prioritet";
			this.columnHeader8.Width = 50;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 3);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(73, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "U istraživajnu:";
			// 
			// chNazivIstr
			// 
			this.chNazivIstr.Text = "Naziv";
			this.chNazivIstr.Width = 99;
			// 
			// chNivoIstr
			// 
			this.chNivoIstr.Text = "Nivo";
			// 
			// chPoeniIstr
			// 
			this.chPoeniIstr.Text = "Poeni";
			this.chPoeniIstr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.chPoeniIstr.Width = 89;
			// 
			// chPoeniPlus
			// 
			this.chPoeniPlus.Text = "Ulaganje";
			// 
			// FormTechIzbor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(594, 448);
			this.Controls.Add(this.tabControlTech);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormTechIzbor";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Istraživanje i razovj";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTechIzbor_FormClosing);
			this.tabControlTech.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkRazKoncentracija)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picRazSlika)).EndInit();
			this.tabIstrazivanje.ResumeLayout(false);
			this.tabIstrazivanje.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picIstSlika)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControlTech;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabIstrazivanje;
		private System.Windows.Forms.Button btnRazDno;
		private System.Windows.Forms.Button btnRazDolje;
		private System.Windows.Forms.Button btnRazGore;
		private System.Windows.Forms.Button btnRazVrh;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView lstRazvoj;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader chNazivIstr;
		private System.Windows.Forms.ColumnHeader chNivoIstr;
		private System.Windows.Forms.ColumnHeader chPoeniIstr;
		private System.Windows.Forms.ColumnHeader chPoeniPlus;
		private System.Windows.Forms.Label lblRazOpis;
		private System.Windows.Forms.PictureBox picRazSlika;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.TrackBar trkRazKoncentracija;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.ListView lstIstrazivanje;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblIstOpis;
		private System.Windows.Forms.PictureBox picIstSlika;
		private System.Windows.Forms.Label lblIstPoeni;
		private System.Windows.Forms.Label lblIstSustav;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.Button btnIstDno;
		private System.Windows.Forms.Button btnIstDolje;
		private System.Windows.Forms.Button btnIstGore;
		private System.Windows.Forms.Button btnIstVrh;
	}
}