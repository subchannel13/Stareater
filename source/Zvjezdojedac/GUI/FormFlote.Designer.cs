namespace Zvjezdojedac.GUI
{
	partial class FormFlote
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFlote));
			this.tabvCtrlFlote = new System.Windows.Forms.TabControl();
			this.tabDizajnovi = new System.Windows.Forms.TabPage();
			this.btnNoviDizajn = new System.Windows.Forms.Button();
			this.btnUkloniDizajn = new System.Windows.Forms.Button();
			this.txtDizajnInfo = new System.Windows.Forms.TextBox();
			this.lstvDizajnovi = new System.Windows.Forms.ListView();
			this.chDizajnNaziv = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chBrojBrodova = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.picSlikaDizajna = new System.Windows.Forms.PictureBox();
			this.lblDizajn = new System.Windows.Forms.Label();
			this.tabNoviDizajn = new System.Windows.Forms.TabPage();
			this.btnNDZadrziInfo = new System.Windows.Forms.Button();
			this.lblNDcijena = new System.Windows.Forms.Label();
			this.cbNDinfoStrana = new System.Windows.Forms.ComboBox();
			this.btnNDinfoSlijedeca = new System.Windows.Forms.Button();
			this.btnNDinfoPrethodna = new System.Windows.Forms.Button();
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
			this.lblNDnosivost = new System.Windows.Forms.Label();
			this.lblNDoklop = new System.Windows.Forms.Label();
			this.lblNDsenzori = new System.Windows.Forms.Label();
			this.lblNDpokretljivost = new System.Windows.Forms.Label();
			this.lblNDudioMisija = new System.Windows.Forms.Label();
			this.lblUdioSek = new System.Windows.Forms.Label();
			this.lblStit = new System.Windows.Forms.Label();
			this.lblSekMisija = new System.Windows.Forms.Label();
			this.lblPrimMisija = new System.Windows.Forms.Label();
			this.cbNDprimMisija = new System.Windows.Forms.ComboBox();
			this.cbNDsekMisija = new System.Windows.Forms.ComboBox();
			this.cbNDstit = new System.Windows.Forms.ComboBox();
			this.hscrUdioMisija = new System.Windows.Forms.HScrollBar();
			this.txtNDinfo = new System.Windows.Forms.TextBox();
			this.chNDMZpogon = new System.Windows.Forms.CheckBox();
			this.btnSpremi = new System.Windows.Forms.Button();
			this.cbNDvelicina = new System.Windows.Forms.ComboBox();
			this.lblVelicina = new System.Windows.Forms.Label();
			this.txtNDnaziv = new System.Windows.Forms.TextBox();
			this.lblNaziv = new System.Windows.Forms.Label();
			this.picNDSlika = new System.Windows.Forms.PictureBox();
			this.tabvCtrlFlote.SuspendLayout();
			this.tabDizajnovi.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSlikaDizajna)).BeginInit();
			this.tabNoviDizajn.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picNDSlika)).BeginInit();
			this.SuspendLayout();
			// 
			// tabvCtrlFlote
			// 
			this.tabvCtrlFlote.Controls.Add(this.tabDizajnovi);
			this.tabvCtrlFlote.Controls.Add(this.tabNoviDizajn);
			this.tabvCtrlFlote.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabvCtrlFlote.Location = new System.Drawing.Point(0, 0);
			this.tabvCtrlFlote.Name = "tabvCtrlFlote";
			this.tabvCtrlFlote.SelectedIndex = 0;
			this.tabvCtrlFlote.Size = new System.Drawing.Size(588, 476);
			this.tabvCtrlFlote.TabIndex = 0;
			// 
			// tabDizajnovi
			// 
			this.tabDizajnovi.BackColor = System.Drawing.SystemColors.Control;
			this.tabDizajnovi.Controls.Add(this.btnNoviDizajn);
			this.tabDizajnovi.Controls.Add(this.btnUkloniDizajn);
			this.tabDizajnovi.Controls.Add(this.txtDizajnInfo);
			this.tabDizajnovi.Controls.Add(this.lstvDizajnovi);
			this.tabDizajnovi.Controls.Add(this.picSlikaDizajna);
			this.tabDizajnovi.Controls.Add(this.lblDizajn);
			this.tabDizajnovi.Location = new System.Drawing.Point(4, 22);
			this.tabDizajnovi.Name = "tabDizajnovi";
			this.tabDizajnovi.Padding = new System.Windows.Forms.Padding(3);
			this.tabDizajnovi.Size = new System.Drawing.Size(580, 450);
			this.tabDizajnovi.TabIndex = 0;
			this.tabDizajnovi.Text = "Dizajnovi";
			// 
			// btnNoviDizajn
			// 
			this.btnNoviDizajn.Location = new System.Drawing.Point(8, 341);
			this.btnNoviDizajn.Name = "btnNoviDizajn";
			this.btnNoviDizajn.Size = new System.Drawing.Size(75, 23);
			this.btnNoviDizajn.TabIndex = 6;
			this.btnNoviDizajn.Text = "Novi dizajn";
			this.btnNoviDizajn.UseVisualStyleBackColor = true;
			this.btnNoviDizajn.Click += new System.EventHandler(this.btnNoviDizajn_Click);
			// 
			// btnUkloniDizajn
			// 
			this.btnUkloniDizajn.Enabled = false;
			this.btnUkloniDizajn.Location = new System.Drawing.Point(298, 341);
			this.btnUkloniDizajn.Name = "btnUkloniDizajn";
			this.btnUkloniDizajn.Size = new System.Drawing.Size(75, 23);
			this.btnUkloniDizajn.TabIndex = 5;
			this.btnUkloniDizajn.Text = "Ukloni";
			this.btnUkloniDizajn.UseVisualStyleBackColor = true;
			this.btnUkloniDizajn.Click += new System.EventHandler(this.btnUkloniDizajn_Click);
			// 
			// txtDizajnInfo
			// 
			this.txtDizajnInfo.Location = new System.Drawing.Point(382, 22);
			this.txtDizajnInfo.Multiline = true;
			this.txtDizajnInfo.Name = "txtDizajnInfo";
			this.txtDizajnInfo.ReadOnly = true;
			this.txtDizajnInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDizajnInfo.Size = new System.Drawing.Size(190, 420);
			this.txtDizajnInfo.TabIndex = 4;
			// 
			// lstvDizajnovi
			// 
			this.lstvDizajnovi.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDizajnNaziv,
            this.chBrojBrodova});
			this.lstvDizajnovi.FullRowSelect = true;
			this.lstvDizajnovi.GridLines = true;
			this.lstvDizajnovi.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstvDizajnovi.HideSelection = false;
			this.lstvDizajnovi.Location = new System.Drawing.Point(8, 92);
			this.lstvDizajnovi.Name = "lstvDizajnovi";
			this.lstvDizajnovi.Size = new System.Drawing.Size(365, 243);
			this.lstvDizajnovi.TabIndex = 3;
			this.lstvDizajnovi.UseCompatibleStateImageBehavior = false;
			this.lstvDizajnovi.View = System.Windows.Forms.View.Details;
			this.lstvDizajnovi.SelectedIndexChanged += new System.EventHandler(this.lstvDizajnovi_SelectedIndexChanged);
			// 
			// chDizajnNaziv
			// 
			this.chDizajnNaziv.Text = "Dizajn";
			this.chDizajnNaziv.Width = 271;
			// 
			// chBrojBrodova
			// 
			this.chBrojBrodova.Text = "Br. brodova";
			this.chBrojBrodova.Width = 67;
			// 
			// picSlikaDizajna
			// 
			this.picSlikaDizajna.Location = new System.Drawing.Point(293, 6);
			this.picSlikaDizajna.Name = "picSlikaDizajna";
			this.picSlikaDizajna.Size = new System.Drawing.Size(80, 80);
			this.picSlikaDizajna.TabIndex = 2;
			this.picSlikaDizajna.TabStop = false;
			// 
			// lblDizajn
			// 
			this.lblDizajn.AutoSize = true;
			this.lblDizajn.Location = new System.Drawing.Point(379, 6);
			this.lblDizajn.Name = "lblDizajn";
			this.lblDizajn.Size = new System.Drawing.Size(39, 13);
			this.lblDizajn.TabIndex = 0;
			this.lblDizajn.Text = "Dizajn:";
			// 
			// tabNoviDizajn
			// 
			this.tabNoviDizajn.BackColor = System.Drawing.SystemColors.Control;
			this.tabNoviDizajn.Controls.Add(this.btnNDZadrziInfo);
			this.tabNoviDizajn.Controls.Add(this.lblNDcijena);
			this.tabNoviDizajn.Controls.Add(this.cbNDinfoStrana);
			this.tabNoviDizajn.Controls.Add(this.btnNDinfoSlijedeca);
			this.tabNoviDizajn.Controls.Add(this.btnNDinfoPrethodna);
			this.tabNoviDizajn.Controls.Add(this.lblNDslobodno);
			this.tabNoviDizajn.Controls.Add(this.btnNDspecOpremaMinus);
			this.tabNoviDizajn.Controls.Add(this.btnNDspecOpremaPlus);
			this.tabNoviDizajn.Controls.Add(this.lstvNDspecOprema);
			this.tabNoviDizajn.Controls.Add(this.lblSpecOprema);
			this.tabNoviDizajn.Controls.Add(this.lblTaktika);
			this.tabNoviDizajn.Controls.Add(this.cbNDtaktika);
			this.tabNoviDizajn.Controls.Add(this.lblNDnosivost);
			this.tabNoviDizajn.Controls.Add(this.lblNDoklop);
			this.tabNoviDizajn.Controls.Add(this.lblNDsenzori);
			this.tabNoviDizajn.Controls.Add(this.lblNDpokretljivost);
			this.tabNoviDizajn.Controls.Add(this.lblNDudioMisija);
			this.tabNoviDizajn.Controls.Add(this.lblUdioSek);
			this.tabNoviDizajn.Controls.Add(this.lblStit);
			this.tabNoviDizajn.Controls.Add(this.lblSekMisija);
			this.tabNoviDizajn.Controls.Add(this.lblPrimMisija);
			this.tabNoviDizajn.Controls.Add(this.cbNDprimMisija);
			this.tabNoviDizajn.Controls.Add(this.cbNDsekMisija);
			this.tabNoviDizajn.Controls.Add(this.cbNDstit);
			this.tabNoviDizajn.Controls.Add(this.hscrUdioMisija);
			this.tabNoviDizajn.Controls.Add(this.txtNDinfo);
			this.tabNoviDizajn.Controls.Add(this.chNDMZpogon);
			this.tabNoviDizajn.Controls.Add(this.btnSpremi);
			this.tabNoviDizajn.Controls.Add(this.cbNDvelicina);
			this.tabNoviDizajn.Controls.Add(this.lblVelicina);
			this.tabNoviDizajn.Controls.Add(this.txtNDnaziv);
			this.tabNoviDizajn.Controls.Add(this.lblNaziv);
			this.tabNoviDizajn.Controls.Add(this.picNDSlika);
			this.tabNoviDizajn.Location = new System.Drawing.Point(4, 22);
			this.tabNoviDizajn.Name = "tabNoviDizajn";
			this.tabNoviDizajn.Padding = new System.Windows.Forms.Padding(3);
			this.tabNoviDizajn.Size = new System.Drawing.Size(580, 450);
			this.tabNoviDizajn.TabIndex = 1;
			this.tabNoviDizajn.Text = "Novi dizajn";
			// 
			// btnNDZadrziInfo
			// 
			this.btnNDZadrziInfo.Location = new System.Drawing.Point(451, 69);
			this.btnNDZadrziInfo.Name = "btnNDZadrziInfo";
			this.btnNDZadrziInfo.Size = new System.Drawing.Size(29, 23);
			this.btnNDZadrziInfo.TabIndex = 32;
			this.btnNDZadrziInfo.Text = "*";
			this.btnNDZadrziInfo.UseVisualStyleBackColor = true;
			this.btnNDZadrziInfo.Click += new System.EventHandler(this.bntNDZadrziInfo_Click);
			// 
			// lblNDcijena
			// 
			this.lblNDcijena.AutoSize = true;
			this.lblNDcijena.Location = new System.Drawing.Point(94, 57);
			this.lblNDcijena.Name = "lblNDcijena";
			this.lblNDcijena.Size = new System.Drawing.Size(75, 13);
			this.lblNDcijena.TabIndex = 31;
			this.lblNDcijena.Text = "Cijena: xx.xx X";
			// 
			// cbNDinfoStrana
			// 
			this.cbNDinfoStrana.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbNDinfoStrana.FormattingEnabled = true;
			this.cbNDinfoStrana.Location = new System.Drawing.Point(329, 71);
			this.cbNDinfoStrana.Name = "cbNDinfoStrana";
			this.cbNDinfoStrana.Size = new System.Drawing.Size(116, 21);
			this.cbNDinfoStrana.TabIndex = 30;
			this.cbNDinfoStrana.SelectedIndexChanged += new System.EventHandler(this.cbNDinfoStrana_SelectedIndexChanged);
			// 
			// btnNDinfoSlijedeca
			// 
			this.btnNDinfoSlijedeca.Location = new System.Drawing.Point(486, 69);
			this.btnNDinfoSlijedeca.Name = "btnNDinfoSlijedeca";
			this.btnNDinfoSlijedeca.Size = new System.Drawing.Size(29, 23);
			this.btnNDinfoSlijedeca.TabIndex = 29;
			this.btnNDinfoSlijedeca.Text = "->";
			this.btnNDinfoSlijedeca.UseVisualStyleBackColor = true;
			this.btnNDinfoSlijedeca.Click += new System.EventHandler(this.btnNDinfoSlijedeca_Click);
			// 
			// btnNDinfoPrethodna
			// 
			this.btnNDinfoPrethodna.Location = new System.Drawing.Point(294, 69);
			this.btnNDinfoPrethodna.Name = "btnNDinfoPrethodna";
			this.btnNDinfoPrethodna.Size = new System.Drawing.Size(29, 23);
			this.btnNDinfoPrethodna.TabIndex = 28;
			this.btnNDinfoPrethodna.Text = "<-";
			this.btnNDinfoPrethodna.UseVisualStyleBackColor = true;
			this.btnNDinfoPrethodna.Click += new System.EventHandler(this.btnNDinfoPrethodna_Click);
			// 
			// lblNDslobodno
			// 
			this.lblNDslobodno.AutoSize = true;
			this.lblNDslobodno.Location = new System.Drawing.Point(291, 361);
			this.lblNDslobodno.Name = "lblNDslobodno";
			this.lblNDslobodno.Size = new System.Drawing.Size(126, 13);
			this.lblNDslobodno.TabIndex = 27;
			this.lblNDslobodno.Text = "Slobodan prostor: xx.xx X";
			// 
			// btnNDspecOpremaMinus
			// 
			this.btnNDspecOpremaMinus.Location = new System.Drawing.Point(254, 356);
			this.btnNDspecOpremaMinus.Name = "btnNDspecOpremaMinus";
			this.btnNDspecOpremaMinus.Size = new System.Drawing.Size(33, 23);
			this.btnNDspecOpremaMinus.TabIndex = 26;
			this.btnNDspecOpremaMinus.Text = "-";
			this.btnNDspecOpremaMinus.UseVisualStyleBackColor = true;
			this.btnNDspecOpremaMinus.Click += new System.EventHandler(this.btnNDspecOpremaMinus_Click);
			// 
			// btnNDspecOpremaPlus
			// 
			this.btnNDspecOpremaPlus.Location = new System.Drawing.Point(215, 356);
			this.btnNDspecOpremaPlus.Name = "btnNDspecOpremaPlus";
			this.btnNDspecOpremaPlus.Size = new System.Drawing.Size(33, 23);
			this.btnNDspecOpremaPlus.TabIndex = 25;
			this.btnNDspecOpremaPlus.Text = "+";
			this.btnNDspecOpremaPlus.UseVisualStyleBackColor = true;
			this.btnNDspecOpremaPlus.Click += new System.EventHandler(this.btnNDspecOpremaPlus_Click);
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
			this.lstvNDspecOprema.Location = new System.Drawing.Point(110, 246);
			this.lstvNDspecOprema.MultiSelect = false;
			this.lstvNDspecOprema.Name = "lstvNDspecOprema";
			this.lstvNDspecOprema.Size = new System.Drawing.Size(405, 104);
			this.lstvNDspecOprema.TabIndex = 24;
			this.lstvNDspecOprema.UseCompatibleStateImageBehavior = false;
			this.lstvNDspecOprema.View = System.Windows.Forms.View.Details;
			this.lstvNDspecOprema.ItemActivate += new System.EventHandler(this.lstvNDspecOprema_ItemActivate);
			this.lstvNDspecOprema.SelectedIndexChanged += new System.EventHandler(this.lstvNDspecOprema_SelectedIndexChanged);
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
			this.lblSpecOprema.Location = new System.Drawing.Point(8, 246);
			this.lblSpecOprema.Name = "lblSpecOprema";
			this.lblSpecOprema.Size = new System.Drawing.Size(97, 13);
			this.lblSpecOprema.TabIndex = 23;
			this.lblSpecOprema.Text = "Specijalna oprema:";
			// 
			// lblTaktika
			// 
			this.lblTaktika.AutoSize = true;
			this.lblTaktika.Location = new System.Drawing.Point(8, 222);
			this.lblTaktika.Name = "lblTaktika";
			this.lblTaktika.Size = new System.Drawing.Size(46, 13);
			this.lblTaktika.TabIndex = 22;
			this.lblTaktika.Text = "Taktika:";
			// 
			// cbNDtaktika
			// 
			this.cbNDtaktika.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbNDtaktika.FormattingEnabled = true;
			this.cbNDtaktika.Location = new System.Drawing.Point(110, 219);
			this.cbNDtaktika.Name = "cbNDtaktika";
			this.cbNDtaktika.Size = new System.Drawing.Size(178, 21);
			this.cbNDtaktika.TabIndex = 21;
			this.cbNDtaktika.SelectedIndexChanged += new System.EventHandler(this.cbNDtaktika_SelectedIndexChanged);
			// 
			// lblNDnosivost
			// 
			this.lblNDnosivost.AutoSize = true;
			this.lblNDnosivost.Location = new System.Drawing.Point(317, 48);
			this.lblNDnosivost.Name = "lblNDnosivost";
			this.lblNDnosivost.Size = new System.Drawing.Size(87, 13);
			this.lblNDnosivost.TabIndex = 19;
			this.lblNDnosivost.Text = "Nosivost: xx.xx X";
			// 
			// lblNDoklop
			// 
			this.lblNDoklop.AutoSize = true;
			this.lblNDoklop.Location = new System.Drawing.Point(317, 35);
			this.lblNDoklop.Name = "lblNDoklop";
			this.lblNDoklop.Size = new System.Drawing.Size(135, 13);
			this.lblNDoklop.TabIndex = 18;
			this.lblNDoklop.Text = "Izdržljivost okolopa: xx.xx X";
			// 
			// lblNDsenzori
			// 
			this.lblNDsenzori.AutoSize = true;
			this.lblNDsenzori.Location = new System.Drawing.Point(317, 9);
			this.lblNDsenzori.Name = "lblNDsenzori";
			this.lblNDsenzori.Size = new System.Drawing.Size(117, 13);
			this.lblNDsenzori.TabIndex = 17;
			this.lblNDsenzori.Text = "Snaga senzora: xx.xx X";
			// 
			// lblNDpokretljivost
			// 
			this.lblNDpokretljivost.AutoSize = true;
			this.lblNDpokretljivost.Location = new System.Drawing.Point(317, 22);
			this.lblNDpokretljivost.Name = "lblNDpokretljivost";
			this.lblNDpokretljivost.Size = new System.Drawing.Size(103, 13);
			this.lblNDpokretljivost.TabIndex = 16;
			this.lblNDpokretljivost.Text = "Pokretljivost: xx.xx X";
			// 
			// lblNDudioMisija
			// 
			this.lblNDudioMisija.AutoSize = true;
			this.lblNDudioMisija.Location = new System.Drawing.Point(251, 153);
			this.lblNDudioMisija.Name = "lblNDudioMisija";
			this.lblNDudioMisija.Size = new System.Drawing.Size(28, 13);
			this.lblNDudioMisija.TabIndex = 15;
			this.lblNDudioMisija.Text = "xx %";
			// 
			// lblUdioSek
			// 
			this.lblUdioSek.AutoSize = true;
			this.lblUdioSek.Location = new System.Drawing.Point(8, 153);
			this.lblUdioSek.Name = "lblUdioSek";
			this.lblUdioSek.Size = new System.Drawing.Size(91, 13);
			this.lblUdioSek.TabIndex = 14;
			this.lblUdioSek.Text = "Udio sekundarne:";
			// 
			// lblStit
			// 
			this.lblStit.AutoSize = true;
			this.lblStit.Location = new System.Drawing.Point(8, 172);
			this.lblStit.Name = "lblStit";
			this.lblStit.Size = new System.Drawing.Size(25, 13);
			this.lblStit.TabIndex = 13;
			this.lblStit.Text = "Štit:";
			// 
			// lblSekMisija
			// 
			this.lblSekMisija.AutoSize = true;
			this.lblSekMisija.Location = new System.Drawing.Point(8, 128);
			this.lblSekMisija.Name = "lblSekMisija";
			this.lblSekMisija.Size = new System.Drawing.Size(96, 13);
			this.lblSekMisija.TabIndex = 12;
			this.lblSekMisija.Text = "Sekundarna misija:";
			// 
			// lblPrimMisija
			// 
			this.lblPrimMisija.AutoSize = true;
			this.lblPrimMisija.Location = new System.Drawing.Point(8, 101);
			this.lblPrimMisija.Name = "lblPrimMisija";
			this.lblPrimMisija.Size = new System.Drawing.Size(79, 13);
			this.lblPrimMisija.TabIndex = 11;
			this.lblPrimMisija.Text = "Primarna misija:";
			// 
			// cbNDprimMisija
			// 
			this.cbNDprimMisija.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbNDprimMisija.FormattingEnabled = true;
			this.cbNDprimMisija.Location = new System.Drawing.Point(110, 98);
			this.cbNDprimMisija.Name = "cbNDprimMisija";
			this.cbNDprimMisija.Size = new System.Drawing.Size(178, 21);
			this.cbNDprimMisija.TabIndex = 0;
			this.cbNDprimMisija.SelectedIndexChanged += new System.EventHandler(this.cbNDprimMisija_SelectedIndexChanged);
			// 
			// cbNDsekMisija
			// 
			this.cbNDsekMisija.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbNDsekMisija.FormattingEnabled = true;
			this.cbNDsekMisija.Location = new System.Drawing.Point(110, 125);
			this.cbNDsekMisija.Name = "cbNDsekMisija";
			this.cbNDsekMisija.Size = new System.Drawing.Size(178, 21);
			this.cbNDsekMisija.TabIndex = 0;
			this.cbNDsekMisija.SelectedIndexChanged += new System.EventHandler(this.cbNDsekMisija_SelectedIndexChanged);
			// 
			// cbNDstit
			// 
			this.cbNDstit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbNDstit.FormattingEnabled = true;
			this.cbNDstit.Location = new System.Drawing.Point(110, 169);
			this.cbNDstit.Name = "cbNDstit";
			this.cbNDstit.Size = new System.Drawing.Size(178, 21);
			this.cbNDstit.TabIndex = 1;
			this.cbNDstit.SelectedIndexChanged += new System.EventHandler(this.cbNDstit_SelectedIndexChanged);
			// 
			// hscrUdioMisija
			// 
			this.hscrUdioMisija.LargeChange = 1;
			this.hscrUdioMisija.Location = new System.Drawing.Point(110, 149);
			this.hscrUdioMisija.Maximum = 50;
			this.hscrUdioMisija.Name = "hscrUdioMisija";
			this.hscrUdioMisija.Size = new System.Drawing.Size(138, 17);
			this.hscrUdioMisija.TabIndex = 2;
			this.hscrUdioMisija.ValueChanged += new System.EventHandler(this.hscrUdioMisija_ValueChanged);
			// 
			// txtNDinfo
			// 
			this.txtNDinfo.Location = new System.Drawing.Point(294, 98);
			this.txtNDinfo.Multiline = true;
			this.txtNDinfo.Name = "txtNDinfo";
			this.txtNDinfo.ReadOnly = true;
			this.txtNDinfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtNDinfo.Size = new System.Drawing.Size(221, 142);
			this.txtNDinfo.TabIndex = 1;
			this.txtNDinfo.Text = "1 red\r\n2 reda\r\n3 reda\r\n4 reda\r\n5 redova\r\n6 redova\r\n7 redova\r\n8 redova\r\n9 redova\r\n" +
    "10 redova";
			// 
			// chNDMZpogon
			// 
			this.chNDMZpogon.AutoSize = true;
			this.chNDMZpogon.Location = new System.Drawing.Point(110, 196);
			this.chNDMZpogon.Name = "chNDMZpogon";
			this.chNDMZpogon.Size = new System.Drawing.Size(75, 17);
			this.chNDMZpogon.TabIndex = 9;
			this.chNDMZpogon.Text = "MZ pogon";
			this.chNDMZpogon.UseVisualStyleBackColor = true;
			this.chNDMZpogon.CheckedChanged += new System.EventHandler(this.chNDMZpogon_CheckedChanged);
			// 
			// btnSpremi
			// 
			this.btnSpremi.Location = new System.Drawing.Point(110, 419);
			this.btnSpremi.Name = "btnSpremi";
			this.btnSpremi.Size = new System.Drawing.Size(75, 23);
			this.btnSpremi.TabIndex = 7;
			this.btnSpremi.Text = "Spremi";
			this.btnSpremi.UseVisualStyleBackColor = true;
			this.btnSpremi.Click += new System.EventHandler(this.btnSpremi_Click);
			// 
			// cbNDvelicina
			// 
			this.cbNDvelicina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbNDvelicina.FormattingEnabled = true;
			this.cbNDvelicina.Location = new System.Drawing.Point(149, 32);
			this.cbNDvelicina.Name = "cbNDvelicina";
			this.cbNDvelicina.Size = new System.Drawing.Size(139, 21);
			this.cbNDvelicina.TabIndex = 4;
			this.cbNDvelicina.SelectedIndexChanged += new System.EventHandler(this.cbNDvelicina_SelectedIndexChanged);
			// 
			// lblVelicina
			// 
			this.lblVelicina.AutoSize = true;
			this.lblVelicina.Location = new System.Drawing.Point(94, 35);
			this.lblVelicina.Name = "lblVelicina";
			this.lblVelicina.Size = new System.Drawing.Size(47, 13);
			this.lblVelicina.TabIndex = 3;
			this.lblVelicina.Text = "Veličina:";
			// 
			// txtNDnaziv
			// 
			this.txtNDnaziv.Location = new System.Drawing.Point(149, 6);
			this.txtNDnaziv.Name = "txtNDnaziv";
			this.txtNDnaziv.Size = new System.Drawing.Size(139, 20);
			this.txtNDnaziv.TabIndex = 2;
			this.txtNDnaziv.TextChanged += new System.EventHandler(this.txtNDnaziv_TextChanged);
			// 
			// lblNaziv
			// 
			this.lblNaziv.AutoSize = true;
			this.lblNaziv.Location = new System.Drawing.Point(94, 9);
			this.lblNaziv.Name = "lblNaziv";
			this.lblNaziv.Size = new System.Drawing.Size(37, 13);
			this.lblNaziv.TabIndex = 1;
			this.lblNaziv.Text = "Naziv:";
			// 
			// picNDSlika
			// 
			this.picNDSlika.Location = new System.Drawing.Point(8, 6);
			this.picNDSlika.Name = "picNDSlika";
			this.picNDSlika.Size = new System.Drawing.Size(80, 80);
			this.picNDSlika.TabIndex = 0;
			this.picNDSlika.TabStop = false;
			// 
			// FormFlote
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(588, 476);
			this.Controls.Add(this.tabvCtrlFlote);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormFlote";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Flote";
			this.tabvCtrlFlote.ResumeLayout(false);
			this.tabDizajnovi.ResumeLayout(false);
			this.tabDizajnovi.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSlikaDizajna)).EndInit();
			this.tabNoviDizajn.ResumeLayout(false);
			this.tabNoviDizajn.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picNDSlika)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabvCtrlFlote;
		private System.Windows.Forms.TabPage tabDizajnovi;
		private System.Windows.Forms.TabPage tabNoviDizajn;
		private System.Windows.Forms.PictureBox picSlikaDizajna;
		private System.Windows.Forms.Label lblDizajn;
		private System.Windows.Forms.PictureBox picNDSlika;
		private System.Windows.Forms.ComboBox cbNDvelicina;
		private System.Windows.Forms.Label lblVelicina;
		private System.Windows.Forms.TextBox txtNDnaziv;
		private System.Windows.Forms.Label lblNaziv;
		private System.Windows.Forms.HScrollBar hscrUdioMisija;
		private System.Windows.Forms.ComboBox cbNDstit;
		private System.Windows.Forms.ComboBox cbNDsekMisija;
		private System.Windows.Forms.Button btnSpremi;
		private System.Windows.Forms.TextBox txtNDinfo;
		private System.Windows.Forms.ComboBox cbNDprimMisija;
		private System.Windows.Forms.CheckBox chNDMZpogon;
		private System.Windows.Forms.Label lblPrimMisija;
		private System.Windows.Forms.Label lblNDudioMisija;
		private System.Windows.Forms.Label lblUdioSek;
		private System.Windows.Forms.Label lblStit;
		private System.Windows.Forms.Label lblSekMisija;
		private System.Windows.Forms.Label lblNDsenzori;
		private System.Windows.Forms.Label lblNDpokretljivost;
		private System.Windows.Forms.Label lblNDnosivost;
		private System.Windows.Forms.Label lblNDoklop;
		private System.Windows.Forms.Label lblTaktika;
		private System.Windows.Forms.ComboBox cbNDtaktika;
		private System.Windows.Forms.Label lblSpecOprema;
		private System.Windows.Forms.ListView lstvNDspecOprema;
		private System.Windows.Forms.ColumnHeader chSpecOpKolicina;
		private System.Windows.Forms.ColumnHeader chSpecOpNaziv;
		private System.Windows.Forms.Button btnNDspecOpremaMinus;
		private System.Windows.Forms.Button btnNDspecOpremaPlus;
		private System.Windows.Forms.Label lblNDslobodno;
		private System.Windows.Forms.ColumnHeader chSpecOpVelicina;
		private System.Windows.Forms.Button btnNDinfoPrethodna;
		private System.Windows.Forms.ComboBox cbNDinfoStrana;
		private System.Windows.Forms.Button btnNDinfoSlijedeca;
		private System.Windows.Forms.Label lblNDcijena;
		private System.Windows.Forms.ListView lstvDizajnovi;
		private System.Windows.Forms.ColumnHeader chDizajnNaziv;
		private System.Windows.Forms.ColumnHeader chBrojBrodova;
		private System.Windows.Forms.Button btnUkloniDizajn;
		private System.Windows.Forms.TextBox txtDizajnInfo;
		private System.Windows.Forms.Button btnNDZadrziInfo;
		private System.Windows.Forms.Button btnNoviDizajn;
	}
}