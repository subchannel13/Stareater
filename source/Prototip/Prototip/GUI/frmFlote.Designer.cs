namespace Prototip
{
	partial class frmFlote
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFlote));
			this.tabCtrlFlote = new System.Windows.Forms.TabControl();
			this.tabDizajnovi = new System.Windows.Forms.TabPage();
			this.btnUkloniDizajn = new System.Windows.Forms.Button();
			this.txtDizajnInfo = new System.Windows.Forms.TextBox();
			this.lstvDizajnovi = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.picSlikaDizajna = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabNoviDizajn = new System.Windows.Forms.TabPage();
			this.lblNDcijena = new System.Windows.Forms.Label();
			this.cbNDinfoStrana = new System.Windows.Forms.ComboBox();
			this.btnNDinfoSlijedeca = new System.Windows.Forms.Button();
			this.btnNDinfoPrethodna = new System.Windows.Forms.Button();
			this.lblNDslobodno = new System.Windows.Forms.Label();
			this.btnNDspecOpremaMinus = new System.Windows.Forms.Button();
			this.btnNDspecOpremaPlus = new System.Windows.Forms.Button();
			this.lstvNDspecOprema = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.cbNDtaktika = new System.Windows.Forms.ComboBox();
			this.lblNDnosivost = new System.Windows.Forms.Label();
			this.lblNDoklop = new System.Windows.Forms.Label();
			this.lblNDsenzori = new System.Windows.Forms.Label();
			this.lblNDpokretljivost = new System.Windows.Forms.Label();
			this.lblNDudioMisija = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbNDprimMisija = new System.Windows.Forms.ComboBox();
			this.cbNDsekMisija = new System.Windows.Forms.ComboBox();
			this.cbNDstit = new System.Windows.Forms.ComboBox();
			this.hscrUdioMisija = new System.Windows.Forms.HScrollBar();
			this.txtNDinfo = new System.Windows.Forms.TextBox();
			this.chNDMZpogon = new System.Windows.Forms.CheckBox();
			this.btnSpremi = new System.Windows.Forms.Button();
			this.cbNDvelicina = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtNDnaziv = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.picNDSlika = new System.Windows.Forms.PictureBox();
			this.tabCtrlFlote.SuspendLayout();
			this.tabDizajnovi.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSlikaDizajna)).BeginInit();
			this.tabNoviDizajn.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picNDSlika)).BeginInit();
			this.SuspendLayout();
			// 
			// tabCtrlFlote
			// 
			this.tabCtrlFlote.Controls.Add(this.tabDizajnovi);
			this.tabCtrlFlote.Controls.Add(this.tabNoviDizajn);
			this.tabCtrlFlote.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabCtrlFlote.Location = new System.Drawing.Point(0, 0);
			this.tabCtrlFlote.Name = "tabCtrlFlote";
			this.tabCtrlFlote.SelectedIndex = 0;
			this.tabCtrlFlote.Size = new System.Drawing.Size(588, 476);
			this.tabCtrlFlote.TabIndex = 0;
			// 
			// tabDizajnovi
			// 
			this.tabDizajnovi.BackColor = System.Drawing.SystemColors.Control;
			this.tabDizajnovi.Controls.Add(this.btnUkloniDizajn);
			this.tabDizajnovi.Controls.Add(this.txtDizajnInfo);
			this.tabDizajnovi.Controls.Add(this.lstvDizajnovi);
			this.tabDizajnovi.Controls.Add(this.picSlikaDizajna);
			this.tabDizajnovi.Controls.Add(this.label1);
			this.tabDizajnovi.Location = new System.Drawing.Point(4, 22);
			this.tabDizajnovi.Name = "tabDizajnovi";
			this.tabDizajnovi.Padding = new System.Windows.Forms.Padding(3);
			this.tabDizajnovi.Size = new System.Drawing.Size(580, 450);
			this.tabDizajnovi.TabIndex = 0;
			this.tabDizajnovi.Text = "Dizajnovi";
			// 
			// btnUkloniDizajn
			// 
			this.btnUkloniDizajn.Enabled = false;
			this.btnUkloniDizajn.Location = new System.Drawing.Point(8, 278);
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
            this.columnHeader4,
            this.columnHeader5});
			this.lstvDizajnovi.FullRowSelect = true;
			this.lstvDizajnovi.GridLines = true;
			this.lstvDizajnovi.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstvDizajnovi.HideSelection = false;
			this.lstvDizajnovi.Location = new System.Drawing.Point(8, 92);
			this.lstvDizajnovi.Name = "lstvDizajnovi";
			this.lstvDizajnovi.Size = new System.Drawing.Size(365, 180);
			this.lstvDizajnovi.TabIndex = 3;
			this.lstvDizajnovi.UseCompatibleStateImageBehavior = false;
			this.lstvDizajnovi.View = System.Windows.Forms.View.Details;
			this.lstvDizajnovi.SelectedIndexChanged += new System.EventHandler(this.lstvDizajnovi_SelectedIndexChanged);
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Dizajn";
			this.columnHeader4.Width = 294;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Br. brodova";
			this.columnHeader5.Width = 67;
			// 
			// picSlikaDizajna
			// 
			this.picSlikaDizajna.Location = new System.Drawing.Point(293, 6);
			this.picSlikaDizajna.Name = "picSlikaDizajna";
			this.picSlikaDizajna.Size = new System.Drawing.Size(80, 80);
			this.picSlikaDizajna.TabIndex = 2;
			this.picSlikaDizajna.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(379, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Dizajn:";
			// 
			// tabNoviDizajn
			// 
			this.tabNoviDizajn.BackColor = System.Drawing.SystemColors.Control;
			this.tabNoviDizajn.Controls.Add(this.lblNDcijena);
			this.tabNoviDizajn.Controls.Add(this.cbNDinfoStrana);
			this.tabNoviDizajn.Controls.Add(this.btnNDinfoSlijedeca);
			this.tabNoviDizajn.Controls.Add(this.btnNDinfoPrethodna);
			this.tabNoviDizajn.Controls.Add(this.lblNDslobodno);
			this.tabNoviDizajn.Controls.Add(this.btnNDspecOpremaMinus);
			this.tabNoviDizajn.Controls.Add(this.btnNDspecOpremaPlus);
			this.tabNoviDizajn.Controls.Add(this.lstvNDspecOprema);
			this.tabNoviDizajn.Controls.Add(this.label9);
			this.tabNoviDizajn.Controls.Add(this.label8);
			this.tabNoviDizajn.Controls.Add(this.cbNDtaktika);
			this.tabNoviDizajn.Controls.Add(this.lblNDnosivost);
			this.tabNoviDizajn.Controls.Add(this.lblNDoklop);
			this.tabNoviDizajn.Controls.Add(this.lblNDsenzori);
			this.tabNoviDizajn.Controls.Add(this.lblNDpokretljivost);
			this.tabNoviDizajn.Controls.Add(this.lblNDudioMisija);
			this.tabNoviDizajn.Controls.Add(this.label7);
			this.tabNoviDizajn.Controls.Add(this.label6);
			this.tabNoviDizajn.Controls.Add(this.label5);
			this.tabNoviDizajn.Controls.Add(this.label4);
			this.tabNoviDizajn.Controls.Add(this.cbNDprimMisija);
			this.tabNoviDizajn.Controls.Add(this.cbNDsekMisija);
			this.tabNoviDizajn.Controls.Add(this.cbNDstit);
			this.tabNoviDizajn.Controls.Add(this.hscrUdioMisija);
			this.tabNoviDizajn.Controls.Add(this.txtNDinfo);
			this.tabNoviDizajn.Controls.Add(this.chNDMZpogon);
			this.tabNoviDizajn.Controls.Add(this.btnSpremi);
			this.tabNoviDizajn.Controls.Add(this.cbNDvelicina);
			this.tabNoviDizajn.Controls.Add(this.label3);
			this.tabNoviDizajn.Controls.Add(this.txtNDnaziv);
			this.tabNoviDizajn.Controls.Add(this.label2);
			this.tabNoviDizajn.Controls.Add(this.picNDSlika);
			this.tabNoviDizajn.Location = new System.Drawing.Point(4, 22);
			this.tabNoviDizajn.Name = "tabNoviDizajn";
			this.tabNoviDizajn.Padding = new System.Windows.Forms.Padding(3);
			this.tabNoviDizajn.Size = new System.Drawing.Size(580, 450);
			this.tabNoviDizajn.TabIndex = 1;
			this.tabNoviDizajn.Text = "Novi dizajn";
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
			this.btnNDinfoSlijedeca.Location = new System.Drawing.Point(451, 69);
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
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.lstvNDspecOprema.FullRowSelect = true;
			this.lstvNDspecOprema.GridLines = true;
			this.lstvNDspecOprema.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstvNDspecOprema.HideSelection = false;
			this.lstvNDspecOprema.Location = new System.Drawing.Point(110, 246);
			this.lstvNDspecOprema.MultiSelect = false;
			this.lstvNDspecOprema.Name = "lstvNDspecOprema";
			this.lstvNDspecOprema.Size = new System.Drawing.Size(370, 104);
			this.lstvNDspecOprema.TabIndex = 24;
			this.lstvNDspecOprema.UseCompatibleStateImageBehavior = false;
			this.lstvNDspecOprema.View = System.Windows.Forms.View.Details;
			this.lstvNDspecOprema.ItemActivate += new System.EventHandler(this.lstvNDspecOprema_ItemActivate);
			this.lstvNDspecOprema.SelectedIndexChanged += new System.EventHandler(this.lstvNDspecOprema_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "";
			this.columnHeader1.Width = 35;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Naziv";
			this.columnHeader2.Width = 269;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Velicina";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(8, 246);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(97, 13);
			this.label9.TabIndex = 23;
			this.label9.Text = "Specijalna oprema:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(8, 222);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(46, 13);
			this.label8.TabIndex = 22;
			this.label8.Text = "Taktika:";
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
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(8, 153);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(91, 13);
			this.label7.TabIndex = 14;
			this.label7.Text = "Udio sekundarne:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(8, 172);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(25, 13);
			this.label6.TabIndex = 13;
			this.label6.Text = "Štit:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 128);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 13);
			this.label5.TabIndex = 12;
			this.label5.Text = "Sekundarna misija:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 101);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(79, 13);
			this.label4.TabIndex = 11;
			this.label4.Text = "Primarna misija:";
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
			this.txtNDinfo.Size = new System.Drawing.Size(186, 142);
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
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(94, 35);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(47, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Veličina:";
			// 
			// txtNDnaziv
			// 
			this.txtNDnaziv.Location = new System.Drawing.Point(149, 6);
			this.txtNDnaziv.Name = "txtNDnaziv";
			this.txtNDnaziv.Size = new System.Drawing.Size(139, 20);
			this.txtNDnaziv.TabIndex = 2;
			this.txtNDnaziv.TextChanged += new System.EventHandler(this.txtNDnaziv_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(94, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Naziv:";
			// 
			// picNDSlika
			// 
			this.picNDSlika.Location = new System.Drawing.Point(8, 6);
			this.picNDSlika.Name = "picNDSlika";
			this.picNDSlika.Size = new System.Drawing.Size(80, 80);
			this.picNDSlika.TabIndex = 0;
			this.picNDSlika.TabStop = false;
			// 
			// frmFlote
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(588, 476);
			this.Controls.Add(this.tabCtrlFlote);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "frmFlote";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Flote";
			this.tabCtrlFlote.ResumeLayout(false);
			this.tabDizajnovi.ResumeLayout(false);
			this.tabDizajnovi.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSlikaDizajna)).EndInit();
			this.tabNoviDizajn.ResumeLayout(false);
			this.tabNoviDizajn.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picNDSlika)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabCtrlFlote;
		private System.Windows.Forms.TabPage tabDizajnovi;
		private System.Windows.Forms.TabPage tabNoviDizajn;
		private System.Windows.Forms.PictureBox picSlikaDizajna;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox picNDSlika;
		private System.Windows.Forms.ComboBox cbNDvelicina;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtNDnaziv;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.HScrollBar hscrUdioMisija;
		private System.Windows.Forms.ComboBox cbNDstit;
		private System.Windows.Forms.ComboBox cbNDsekMisija;
		private System.Windows.Forms.Button btnSpremi;
		private System.Windows.Forms.TextBox txtNDinfo;
		private System.Windows.Forms.ComboBox cbNDprimMisija;
		private System.Windows.Forms.CheckBox chNDMZpogon;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblNDudioMisija;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblNDsenzori;
		private System.Windows.Forms.Label lblNDpokretljivost;
		private System.Windows.Forms.Label lblNDnosivost;
		private System.Windows.Forms.Label lblNDoklop;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cbNDtaktika;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ListView lstvNDspecOprema;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Button btnNDspecOpremaMinus;
		private System.Windows.Forms.Button btnNDspecOpremaPlus;
		private System.Windows.Forms.Label lblNDslobodno;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Button btnNDinfoPrethodna;
		private System.Windows.Forms.ComboBox cbNDinfoStrana;
		private System.Windows.Forms.Button btnNDinfoSlijedeca;
		private System.Windows.Forms.Label lblNDcijena;
		private System.Windows.Forms.ListView lstvDizajnovi;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.Button btnUkloniDizajn;
		private System.Windows.Forms.TextBox txtDizajnInfo;
	}
}