namespace Zvjezdojedac.GUI
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
			this.tabRazvoj = new System.Windows.Forms.TabPage();
			this.txtRazOpis = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.lblFokusirano = new System.Windows.Forms.Label();
			this.lblRavnomjerno = new System.Windows.Forms.Label();
			this.lblRaspodjela = new System.Windows.Forms.Label();
			this.trkRazKoncentracija = new System.Windows.Forms.TrackBar();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.picRazSlika = new System.Windows.Forms.PictureBox();
			this.btnRazDno = new System.Windows.Forms.Button();
			this.btnRazDolje = new System.Windows.Forms.Button();
			this.btnRazGore = new System.Windows.Forms.Button();
			this.btnRazVrh = new System.Windows.Forms.Button();
			this.lblURazvoju = new System.Windows.Forms.Label();
			this.lstRazvoj = new System.Windows.Forms.ListView();
			this.chRazNaziv = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chRazNivo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chRazPoeni = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chRazUlaganje = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tabIstrazivanje = new System.Windows.Forms.TabPage();
			this.txtIstOpis = new System.Windows.Forms.TextBox();
			this.btnIstDno = new System.Windows.Forms.Button();
			this.btnIstDolje = new System.Windows.Forms.Button();
			this.btnIstGore = new System.Windows.Forms.Button();
			this.btnIstVrh = new System.Windows.Forms.Button();
			this.lblIstSustav = new System.Windows.Forms.Label();
			this.lblIstPoeni = new System.Windows.Forms.Label();
			this.picIstSlika = new System.Windows.Forms.PictureBox();
			this.lstIstrazivanje = new System.Windows.Forms.ListView();
			this.chIstNaziv = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chIstNivo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chIstPoeni = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chIstPrioritet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblUIstrazivanju = new System.Windows.Forms.Label();
			this.tabKnjiznica = new System.Windows.Forms.TabPage();
			this.lblKnjizNaziv = new System.Windows.Forms.Label();
			this.txtKnjizOpis = new System.Windows.Forms.TextBox();
			this.picKnjizSlika = new System.Windows.Forms.PictureBox();
			this.lstKnjiznica = new System.Windows.Forms.ListView();
			this.chKnjizNaziv = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chKnjizNivo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tabOprema = new System.Windows.Forms.TabPage();
			this.cbOpVelicine = new System.Windows.Forms.ComboBox();
			this.cbOpKategorija = new System.Windows.Forms.ComboBox();
			this.lblOpNaziv = new System.Windows.Forms.Label();
			this.txtOpOpis = new System.Windows.Forms.TextBox();
			this.picOpSlika = new System.Windows.Forms.PictureBox();
			this.lstOprema = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblRazPoeni = new System.Windows.Forms.Label();
			this.tabControlTech.SuspendLayout();
			this.tabRazvoj.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkRazKoncentracija)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picRazSlika)).BeginInit();
			this.tabIstrazivanje.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picIstSlika)).BeginInit();
			this.tabKnjiznica.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picKnjizSlika)).BeginInit();
			this.tabOprema.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picOpSlika)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControlTech
			// 
			this.tabControlTech.Controls.Add(this.tabRazvoj);
			this.tabControlTech.Controls.Add(this.tabIstrazivanje);
			this.tabControlTech.Controls.Add(this.tabKnjiznica);
			this.tabControlTech.Controls.Add(this.tabOprema);
			this.tabControlTech.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlTech.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.tabControlTech.Location = new System.Drawing.Point(0, 0);
			this.tabControlTech.Name = "tabControlTech";
			this.tabControlTech.SelectedIndex = 0;
			this.tabControlTech.Size = new System.Drawing.Size(594, 448);
			this.tabControlTech.TabIndex = 0;
			// 
			// tabRazvoj
			// 
			this.tabRazvoj.BackColor = System.Drawing.SystemColors.Control;
			this.tabRazvoj.Controls.Add(this.lblRazPoeni);
			this.tabRazvoj.Controls.Add(this.txtRazOpis);
			this.tabRazvoj.Controls.Add(this.btnOk);
			this.tabRazvoj.Controls.Add(this.lblFokusirano);
			this.tabRazvoj.Controls.Add(this.lblRavnomjerno);
			this.tabRazvoj.Controls.Add(this.lblRaspodjela);
			this.tabRazvoj.Controls.Add(this.trkRazKoncentracija);
			this.tabRazvoj.Controls.Add(this.trackBar1);
			this.tabRazvoj.Controls.Add(this.picRazSlika);
			this.tabRazvoj.Controls.Add(this.btnRazDno);
			this.tabRazvoj.Controls.Add(this.btnRazDolje);
			this.tabRazvoj.Controls.Add(this.btnRazGore);
			this.tabRazvoj.Controls.Add(this.btnRazVrh);
			this.tabRazvoj.Controls.Add(this.lblURazvoju);
			this.tabRazvoj.Controls.Add(this.lstRazvoj);
			this.tabRazvoj.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.tabRazvoj.Location = new System.Drawing.Point(4, 22);
			this.tabRazvoj.Name = "tabRazvoj";
			this.tabRazvoj.Padding = new System.Windows.Forms.Padding(3);
			this.tabRazvoj.Size = new System.Drawing.Size(586, 422);
			this.tabRazvoj.TabIndex = 0;
			this.tabRazvoj.Text = "Razvoj";
			// 
			// txtRazOpis
			// 
			this.txtRazOpis.BackColor = System.Drawing.SystemColors.Control;
			this.txtRazOpis.Location = new System.Drawing.Point(94, 275);
			this.txtRazOpis.Multiline = true;
			this.txtRazOpis.Name = "txtRazOpis";
			this.txtRazOpis.ReadOnly = true;
			this.txtRazOpis.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtRazOpis.Size = new System.Drawing.Size(294, 105);
			this.txtRazOpis.TabIndex = 20;
			this.txtRazOpis.Text = "Opis tehnologije\r\n...";
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
			this.btnOk.Visible = false;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// lblFokusirano
			// 
			this.lblFokusirano.AutoSize = true;
			this.lblFokusirano.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lblFokusirano.Location = new System.Drawing.Point(519, 248);
			this.lblFokusirano.Name = "lblFokusirano";
			this.lblFokusirano.Size = new System.Drawing.Size(59, 13);
			this.lblFokusirano.TabIndex = 18;
			this.lblFokusirano.Text = "Fokusirano";
			// 
			// lblRavnomjerno
			// 
			this.lblRavnomjerno.AutoSize = true;
			this.lblRavnomjerno.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lblRavnomjerno.Location = new System.Drawing.Point(394, 248);
			this.lblRavnomjerno.Name = "lblRavnomjerno";
			this.lblRavnomjerno.Size = new System.Drawing.Size(70, 13);
			this.lblRavnomjerno.TabIndex = 17;
			this.lblRavnomjerno.Text = "Ravnomjerno";
			// 
			// lblRaspodjela
			// 
			this.lblRaspodjela.AutoSize = true;
			this.lblRaspodjela.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lblRaspodjela.Location = new System.Drawing.Point(394, 184);
			this.lblRaspodjela.Name = "lblRaspodjela";
			this.lblRaspodjela.Size = new System.Drawing.Size(96, 13);
			this.lblRaspodjela.TabIndex = 16;
			this.lblRaspodjela.Text = "Raspodjela poena:";
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
			// lblURazvoju
			// 
			this.lblURazvoju.AutoSize = true;
			this.lblURazvoju.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lblURazvoju.Location = new System.Drawing.Point(8, 3);
			this.lblURazvoju.Name = "lblURazvoju";
			this.lblURazvoju.Size = new System.Drawing.Size(55, 13);
			this.lblURazvoju.TabIndex = 7;
			this.lblURazvoju.Text = "U razvoju:";
			// 
			// lstRazvoj
			// 
			this.lstRazvoj.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chRazNaziv,
            this.chRazNivo,
            this.chRazPoeni,
            this.chRazUlaganje});
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
			// chRazNaziv
			// 
			this.chRazNaziv.Text = "Naziv";
			this.chRazNaziv.Width = 150;
			// 
			// chRazNivo
			// 
			this.chRazNivo.Text = "Nivo";
			this.chRazNivo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.chRazNivo.Width = 45;
			// 
			// chRazPoeni
			// 
			this.chRazPoeni.Text = "Poeni";
			this.chRazPoeni.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.chRazPoeni.Width = 115;
			// 
			// chRazUlaganje
			// 
			this.chRazUlaganje.Text = "Ulaganje";
			this.chRazUlaganje.Width = 65;
			// 
			// tabIstrazivanje
			// 
			this.tabIstrazivanje.BackColor = System.Drawing.SystemColors.Control;
			this.tabIstrazivanje.Controls.Add(this.txtIstOpis);
			this.tabIstrazivanje.Controls.Add(this.btnIstDno);
			this.tabIstrazivanje.Controls.Add(this.btnIstDolje);
			this.tabIstrazivanje.Controls.Add(this.btnIstGore);
			this.tabIstrazivanje.Controls.Add(this.btnIstVrh);
			this.tabIstrazivanje.Controls.Add(this.lblIstSustav);
			this.tabIstrazivanje.Controls.Add(this.lblIstPoeni);
			this.tabIstrazivanje.Controls.Add(this.picIstSlika);
			this.tabIstrazivanje.Controls.Add(this.lstIstrazivanje);
			this.tabIstrazivanje.Controls.Add(this.lblUIstrazivanju);
			this.tabIstrazivanje.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.tabIstrazivanje.Location = new System.Drawing.Point(4, 22);
			this.tabIstrazivanje.Name = "tabIstrazivanje";
			this.tabIstrazivanje.Padding = new System.Windows.Forms.Padding(3);
			this.tabIstrazivanje.Size = new System.Drawing.Size(586, 422);
			this.tabIstrazivanje.TabIndex = 1;
			this.tabIstrazivanje.Text = "Istraživanje";
			// 
			// txtIstOpis
			// 
			this.txtIstOpis.BackColor = System.Drawing.SystemColors.Control;
			this.txtIstOpis.Location = new System.Drawing.Point(94, 275);
			this.txtIstOpis.Multiline = true;
			this.txtIstOpis.Name = "txtIstOpis";
			this.txtIstOpis.ReadOnly = true;
			this.txtIstOpis.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtIstOpis.Size = new System.Drawing.Size(278, 105);
			this.txtIstOpis.TabIndex = 21;
			this.txtIstOpis.Text = "Opis tehnologije\r\n...";
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
            this.chIstNaziv,
            this.chIstNivo,
            this.chIstPoeni,
            this.chIstPrioritet});
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
			// chIstNaziv
			// 
			this.chIstNaziv.Text = "Naziv";
			this.chIstNaziv.Width = 150;
			// 
			// chIstNivo
			// 
			this.chIstNivo.Text = "Nivo";
			this.chIstNivo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.chIstNivo.Width = 45;
			// 
			// chIstPoeni
			// 
			this.chIstPoeni.Text = "Poeni";
			this.chIstPoeni.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.chIstPoeni.Width = 115;
			// 
			// chIstPrioritet
			// 
			this.chIstPrioritet.Text = "Prioritet";
			this.chIstPrioritet.Width = 50;
			// 
			// lblUIstrazivanju
			// 
			this.lblUIstrazivanju.AutoSize = true;
			this.lblUIstrazivanju.Location = new System.Drawing.Point(8, 3);
			this.lblUIstrazivanju.Name = "lblUIstrazivanju";
			this.lblUIstrazivanju.Size = new System.Drawing.Size(73, 13);
			this.lblUIstrazivanju.TabIndex = 0;
			this.lblUIstrazivanju.Text = "U istraživajnu:";
			// 
			// tabKnjiznica
			// 
			this.tabKnjiznica.BackColor = System.Drawing.SystemColors.Control;
			this.tabKnjiznica.Controls.Add(this.lblKnjizNaziv);
			this.tabKnjiznica.Controls.Add(this.txtKnjizOpis);
			this.tabKnjiznica.Controls.Add(this.picKnjizSlika);
			this.tabKnjiznica.Controls.Add(this.lstKnjiznica);
			this.tabKnjiznica.Location = new System.Drawing.Point(4, 22);
			this.tabKnjiznica.Name = "tabKnjiznica";
			this.tabKnjiznica.Padding = new System.Windows.Forms.Padding(3);
			this.tabKnjiznica.Size = new System.Drawing.Size(586, 422);
			this.tabKnjiznica.TabIndex = 2;
			this.tabKnjiznica.Text = "Knjižnica";
			// 
			// lblKnjizNaziv
			// 
			this.lblKnjizNaziv.AutoSize = true;
			this.lblKnjizNaziv.Location = new System.Drawing.Point(316, 18);
			this.lblKnjizNaziv.Name = "lblKnjizNaziv";
			this.lblKnjizNaziv.Size = new System.Drawing.Size(35, 13);
			this.lblKnjizNaziv.TabIndex = 6;
			this.lblKnjizNaziv.Text = "label1";
			// 
			// txtKnjizOpis
			// 
			this.txtKnjizOpis.BackColor = System.Drawing.SystemColors.Control;
			this.txtKnjizOpis.Location = new System.Drawing.Point(230, 92);
			this.txtKnjizOpis.Multiline = true;
			this.txtKnjizOpis.Name = "txtKnjizOpis";
			this.txtKnjizOpis.ReadOnly = true;
			this.txtKnjizOpis.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtKnjizOpis.Size = new System.Drawing.Size(348, 322);
			this.txtKnjizOpis.TabIndex = 5;
			this.txtKnjizOpis.Text = "Opis tehnologije\r\n...";
			// 
			// picKnjizSlika
			// 
			this.picKnjizSlika.Location = new System.Drawing.Point(230, 6);
			this.picKnjizSlika.Name = "picKnjizSlika";
			this.picKnjizSlika.Size = new System.Drawing.Size(80, 80);
			this.picKnjizSlika.TabIndex = 4;
			this.picKnjizSlika.TabStop = false;
			// 
			// lstKnjiznica
			// 
			this.lstKnjiznica.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chKnjizNaziv,
            this.chKnjizNivo});
			this.lstKnjiznica.FullRowSelect = true;
			this.lstKnjiznica.Location = new System.Drawing.Point(8, 6);
			this.lstKnjiznica.MultiSelect = false;
			this.lstKnjiznica.Name = "lstKnjiznica";
			this.lstKnjiznica.Size = new System.Drawing.Size(216, 408);
			this.lstKnjiznica.TabIndex = 3;
			this.lstKnjiznica.UseCompatibleStateImageBehavior = false;
			this.lstKnjiznica.View = System.Windows.Forms.View.Details;
			this.lstKnjiznica.SelectedIndexChanged += new System.EventHandler(this.lstKnjiznica_SelectedIndexChanged);
			// 
			// chKnjizNaziv
			// 
			this.chKnjizNaziv.Text = "Naziv";
			this.chKnjizNaziv.Width = 150;
			// 
			// chKnjizNivo
			// 
			this.chKnjizNivo.Text = "Nivo";
			this.chKnjizNivo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.chKnjizNivo.Width = 45;
			// 
			// tabOprema
			// 
			this.tabOprema.BackColor = System.Drawing.SystemColors.Control;
			this.tabOprema.Controls.Add(this.cbOpVelicine);
			this.tabOprema.Controls.Add(this.cbOpKategorija);
			this.tabOprema.Controls.Add(this.lblOpNaziv);
			this.tabOprema.Controls.Add(this.txtOpOpis);
			this.tabOprema.Controls.Add(this.picOpSlika);
			this.tabOprema.Controls.Add(this.lstOprema);
			this.tabOprema.Location = new System.Drawing.Point(4, 22);
			this.tabOprema.Name = "tabOprema";
			this.tabOprema.Padding = new System.Windows.Forms.Padding(3);
			this.tabOprema.Size = new System.Drawing.Size(586, 422);
			this.tabOprema.TabIndex = 3;
			this.tabOprema.Text = "Komponente brodova";
			// 
			// cbOpVelicine
			// 
			this.cbOpVelicine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbOpVelicine.FormattingEnabled = true;
			this.cbOpVelicine.Location = new System.Drawing.Point(443, 66);
			this.cbOpVelicine.Name = "cbOpVelicine";
			this.cbOpVelicine.Size = new System.Drawing.Size(135, 21);
			this.cbOpVelicine.TabIndex = 12;
			this.cbOpVelicine.Visible = false;
			this.cbOpVelicine.SelectedIndexChanged += new System.EventHandler(this.cbOpVelicine_SelectedIndexChanged);
			// 
			// cbOpKategorija
			// 
			this.cbOpKategorija.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbOpKategorija.FormattingEnabled = true;
			this.cbOpKategorija.Location = new System.Drawing.Point(8, 6);
			this.cbOpKategorija.Name = "cbOpKategorija";
			this.cbOpKategorija.Size = new System.Drawing.Size(216, 21);
			this.cbOpKategorija.TabIndex = 11;
			this.cbOpKategorija.SelectedIndexChanged += new System.EventHandler(this.cbOpKategorija_SelectedIndexChanged);
			// 
			// lblOpNaziv
			// 
			this.lblOpNaziv.AutoSize = true;
			this.lblOpNaziv.Location = new System.Drawing.Point(316, 19);
			this.lblOpNaziv.Name = "lblOpNaziv";
			this.lblOpNaziv.Size = new System.Drawing.Size(35, 13);
			this.lblOpNaziv.TabIndex = 10;
			this.lblOpNaziv.Text = "label1";
			// 
			// txtOpOpis
			// 
			this.txtOpOpis.BackColor = System.Drawing.SystemColors.Control;
			this.txtOpOpis.Location = new System.Drawing.Point(230, 93);
			this.txtOpOpis.Multiline = true;
			this.txtOpOpis.Name = "txtOpOpis";
			this.txtOpOpis.ReadOnly = true;
			this.txtOpOpis.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.txtOpOpis.Size = new System.Drawing.Size(348, 322);
			this.txtOpOpis.TabIndex = 9;
			this.txtOpOpis.Text = "Opis tehnologije\r\n...";
			// 
			// picOpSlika
			// 
			this.picOpSlika.Location = new System.Drawing.Point(230, 7);
			this.picOpSlika.Name = "picOpSlika";
			this.picOpSlika.Size = new System.Drawing.Size(80, 80);
			this.picOpSlika.TabIndex = 8;
			this.picOpSlika.TabStop = false;
			// 
			// lstOprema
			// 
			this.lstOprema.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.lstOprema.FullRowSelect = true;
			this.lstOprema.Location = new System.Drawing.Point(8, 33);
			this.lstOprema.MultiSelect = false;
			this.lstOprema.Name = "lstOprema";
			this.lstOprema.Size = new System.Drawing.Size(216, 382);
			this.lstOprema.TabIndex = 7;
			this.lstOprema.UseCompatibleStateImageBehavior = false;
			this.lstOprema.View = System.Windows.Forms.View.Details;
			this.lstOprema.SelectedIndexChanged += new System.EventHandler(this.lstOprema_SelectedIndexChanged);
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
			// lblRazPoeni
			// 
			this.lblRazPoeni.AutoSize = true;
			this.lblRazPoeni.Location = new System.Drawing.Point(394, 275);
			this.lblRazPoeni.Name = "lblRazPoeni";
			this.lblRazPoeni.Size = new System.Drawing.Size(35, 13);
			this.lblRazPoeni.TabIndex = 21;
			this.lblRazPoeni.Text = "label1";
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
			this.tabRazvoj.ResumeLayout(false);
			this.tabRazvoj.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkRazKoncentracija)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picRazSlika)).EndInit();
			this.tabIstrazivanje.ResumeLayout(false);
			this.tabIstrazivanje.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picIstSlika)).EndInit();
			this.tabKnjiznica.ResumeLayout(false);
			this.tabKnjiznica.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picKnjizSlika)).EndInit();
			this.tabOprema.ResumeLayout(false);
			this.tabOprema.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picOpSlika)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControlTech;
		private System.Windows.Forms.TabPage tabRazvoj;
		private System.Windows.Forms.TabPage tabIstrazivanje;
		private System.Windows.Forms.Button btnRazDno;
		private System.Windows.Forms.Button btnRazDolje;
		private System.Windows.Forms.Button btnRazGore;
		private System.Windows.Forms.Button btnRazVrh;
		private System.Windows.Forms.Label lblURazvoju;
		private System.Windows.Forms.ListView lstRazvoj;
		private System.Windows.Forms.ColumnHeader chRazNaziv;
		private System.Windows.Forms.ColumnHeader chRazNivo;
		private System.Windows.Forms.ColumnHeader chRazPoeni;
		private System.Windows.Forms.ColumnHeader chRazUlaganje;
		private System.Windows.Forms.PictureBox picRazSlika;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.TrackBar trkRazKoncentracija;
		private System.Windows.Forms.Label lblRavnomjerno;
		private System.Windows.Forms.Label lblRaspodjela;
		private System.Windows.Forms.Label lblFokusirano;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.ListView lstIstrazivanje;
		private System.Windows.Forms.ColumnHeader chIstNaziv;
		private System.Windows.Forms.ColumnHeader chIstNivo;
		private System.Windows.Forms.ColumnHeader chIstPoeni;
		private System.Windows.Forms.Label lblUIstrazivanju;
		private System.Windows.Forms.PictureBox picIstSlika;
		private System.Windows.Forms.Label lblIstPoeni;
		private System.Windows.Forms.Label lblIstSustav;
		private System.Windows.Forms.ColumnHeader chIstPrioritet;
		private System.Windows.Forms.Button btnIstDno;
		private System.Windows.Forms.Button btnIstDolje;
		private System.Windows.Forms.Button btnIstGore;
		private System.Windows.Forms.Button btnIstVrh;
		private System.Windows.Forms.TabPage tabKnjiznica;
		private System.Windows.Forms.PictureBox picKnjizSlika;
		private System.Windows.Forms.ListView lstKnjiznica;
		private System.Windows.Forms.ColumnHeader chKnjizNaziv;
		private System.Windows.Forms.ColumnHeader chKnjizNivo;
		private System.Windows.Forms.TextBox txtKnjizOpis;
		private System.Windows.Forms.Label lblKnjizNaziv;
		private System.Windows.Forms.TabPage tabOprema;
		private System.Windows.Forms.ComboBox cbOpKategorija;
		private System.Windows.Forms.Label lblOpNaziv;
		private System.Windows.Forms.TextBox txtOpOpis;
		private System.Windows.Forms.PictureBox picOpSlika;
		private System.Windows.Forms.ListView lstOprema;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ComboBox cbOpVelicine;
		private System.Windows.Forms.TextBox txtRazOpis;
		private System.Windows.Forms.TextBox txtIstOpis;
		private System.Windows.Forms.Label lblRazPoeni;
	}
}