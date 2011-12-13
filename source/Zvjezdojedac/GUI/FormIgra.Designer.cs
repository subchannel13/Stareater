namespace Zvjezdojedac.GUI
{
	partial class FormIgra
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIgra));
			this.tabCtrlDesno = new System.Windows.Forms.TabControl();
			this.tabPageZvijezda = new System.Windows.Forms.TabPage();
			this.planetiFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlOpisZvjezde = new System.Windows.Forms.Panel();
			this.tabPageKolonija = new System.Windows.Forms.TabPage();
			this.btnSlijedecaKolonija = new System.Windows.Forms.Button();
			this.btnPrethodnaKolonija = new System.Windows.Forms.Button();
			this.groupPoStan = new System.Windows.Forms.GroupBox();
			this.lblOdrzavanjePoStan = new System.Windows.Forms.Label();
			this.lblRazvojPoStan = new System.Windows.Forms.Label();
			this.lblIndustrijaPoStan = new System.Windows.Forms.Label();
			this.lblRudePoStan = new System.Windows.Forms.Label();
			this.lblHranaPoStan = new System.Windows.Forms.Label();
			this.groupCivGradnja = new System.Windows.Forms.GroupBox();
			this.btnCivilnaGradnja = new System.Windows.Forms.Button();
			this.hscrCivilnaIndustrija = new System.Windows.Forms.HScrollBar();
			this.lblProcjenaCivilneGradnje = new System.Windows.Forms.Label();
			this.lblCivilnaIndustrija = new System.Windows.Forms.Label();
			this.picSlikaPlaneta = new System.Windows.Forms.PictureBox();
			this.btnPlanetInfo = new System.Windows.Forms.Button();
			this.lblPopulacija = new System.Windows.Forms.Label();
			this.lblImePlaneta = new System.Windows.Forms.Label();
			this.tabPageFlote = new System.Windows.Forms.TabPage();
			this.tvFlota = new System.Windows.Forms.TreeView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnFlotaPokret = new System.Windows.Forms.Button();
			this.btnSekAkcija = new System.Windows.Forms.Button();
			this.btnPrimAkcijaBroda = new System.Windows.Forms.Button();
			this.lblRazvoj = new System.Windows.Forms.Label();
			this.btnVojnaGradnja = new System.Windows.Forms.Button();
			this.lblProcjenaVojneGradnje = new System.Windows.Forms.Label();
			this.hscrZvjezdaGradnja = new System.Windows.Forms.HScrollBar();
			this.lblVojnaGradnja = new System.Windows.Forms.Label();
			this.pnlMapa = new System.Windows.Forms.Panel();
			this.picMapa = new System.Windows.Forms.PictureBox();
			this.pnlDno = new System.Windows.Forms.Panel();
			this.dnoSredinaPanel = new System.Windows.Forms.Panel();
			this.zvijezdaPicBox = new System.Windows.Forms.PictureBox();
			this.lblImeZvjezde = new System.Windows.Forms.Label();
			this.btnEndTurn = new System.Windows.Forms.Button();
			this.timerAnimacija = new System.Windows.Forms.Timer(this.components);
			this.pnlDesno = new System.Windows.Forms.Panel();
			this.pnlDesnoGore = new System.Windows.Forms.Panel();
			this.lblBrojKruga = new System.Windows.Forms.Label();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.izbornikMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.novaIgraMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.spremiMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.ucitajMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.uvećanjeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uvecajMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.umanjiMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.izlazMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.novostiMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.kolonijeMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.floteMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.tehnologijeMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.backgroundTurnProcessor = new System.ComponentModel.BackgroundWorker();
			this.dizjnoviToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabCtrlDesno.SuspendLayout();
			this.tabPageZvijezda.SuspendLayout();
			this.tabPageKolonija.SuspendLayout();
			this.groupPoStan.SuspendLayout();
			this.groupCivGradnja.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSlikaPlaneta)).BeginInit();
			this.tabPageFlote.SuspendLayout();
			this.panel1.SuspendLayout();
			this.pnlMapa.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picMapa)).BeginInit();
			this.pnlDno.SuspendLayout();
			this.dnoSredinaPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.zvijezdaPicBox)).BeginInit();
			this.pnlDesno.SuspendLayout();
			this.pnlDesnoGore.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabCtrlDesno
			// 
			this.tabCtrlDesno.Alignment = System.Windows.Forms.TabAlignment.Right;
			this.tabCtrlDesno.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabCtrlDesno.Controls.Add(this.tabPageZvijezda);
			this.tabCtrlDesno.Controls.Add(this.tabPageKolonija);
			this.tabCtrlDesno.Controls.Add(this.tabPageFlote);
			this.tabCtrlDesno.ItemSize = new System.Drawing.Size(10, 38);
			this.tabCtrlDesno.Location = new System.Drawing.Point(0, 31);
			this.tabCtrlDesno.Multiline = true;
			this.tabCtrlDesno.Name = "tabCtrlDesno";
			this.tabCtrlDesno.SelectedIndex = 0;
			this.tabCtrlDesno.Size = new System.Drawing.Size(250, 393);
			this.tabCtrlDesno.TabIndex = 0;
			// 
			// tabPageZvijezda
			// 
			this.tabPageZvijezda.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageZvijezda.Controls.Add(this.planetiFlowPanel);
			this.tabPageZvijezda.Controls.Add(this.pnlOpisZvjezde);
			this.tabPageZvijezda.Location = new System.Drawing.Point(4, 4);
			this.tabPageZvijezda.Name = "tabPageZvijezda";
			this.tabPageZvijezda.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageZvijezda.Size = new System.Drawing.Size(204, 385);
			this.tabPageZvijezda.TabIndex = 0;
			// 
			// planetiFlowPanel
			// 
			this.planetiFlowPanel.BackColor = System.Drawing.Color.Black;
			this.planetiFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.planetiFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.planetiFlowPanel.Location = new System.Drawing.Point(3, 51);
			this.planetiFlowPanel.Name = "planetiFlowPanel";
			this.planetiFlowPanel.Size = new System.Drawing.Size(198, 331);
			this.planetiFlowPanel.TabIndex = 0;
			// 
			// pnlOpisZvjezde
			// 
			this.pnlOpisZvjezde.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlOpisZvjezde.Location = new System.Drawing.Point(3, 3);
			this.pnlOpisZvjezde.Name = "pnlOpisZvjezde";
			this.pnlOpisZvjezde.Size = new System.Drawing.Size(198, 48);
			this.pnlOpisZvjezde.TabIndex = 1;
			// 
			// tabPageKolonija
			// 
			this.tabPageKolonija.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageKolonija.Controls.Add(this.btnSlijedecaKolonija);
			this.tabPageKolonija.Controls.Add(this.btnPrethodnaKolonija);
			this.tabPageKolonija.Controls.Add(this.groupPoStan);
			this.tabPageKolonija.Controls.Add(this.groupCivGradnja);
			this.tabPageKolonija.Controls.Add(this.picSlikaPlaneta);
			this.tabPageKolonija.Controls.Add(this.btnPlanetInfo);
			this.tabPageKolonija.Controls.Add(this.lblPopulacija);
			this.tabPageKolonija.Controls.Add(this.lblImePlaneta);
			this.tabPageKolonija.Location = new System.Drawing.Point(4, 4);
			this.tabPageKolonija.Name = "tabPageKolonija";
			this.tabPageKolonija.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageKolonija.Size = new System.Drawing.Size(204, 385);
			this.tabPageKolonija.TabIndex = 1;
			// 
			// btnSlijedecaKolonija
			// 
			this.btnSlijedecaKolonija.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSlijedecaKolonija.Location = new System.Drawing.Point(167, 6);
			this.btnSlijedecaKolonija.Name = "btnSlijedecaKolonija";
			this.btnSlijedecaKolonija.Size = new System.Drawing.Size(31, 23);
			this.btnSlijedecaKolonija.TabIndex = 32;
			this.btnSlijedecaKolonija.Text = "->";
			this.btnSlijedecaKolonija.UseVisualStyleBackColor = true;
			this.btnSlijedecaKolonija.Visible = false;
			this.btnSlijedecaKolonija.Click += new System.EventHandler(this.btnSlijedecaKolonija_Click);
			// 
			// btnPrethodnaKolonija
			// 
			this.btnPrethodnaKolonija.Location = new System.Drawing.Point(6, 6);
			this.btnPrethodnaKolonija.Name = "btnPrethodnaKolonija";
			this.btnPrethodnaKolonija.Size = new System.Drawing.Size(31, 23);
			this.btnPrethodnaKolonija.TabIndex = 31;
			this.btnPrethodnaKolonija.Text = "<-";
			this.btnPrethodnaKolonija.UseVisualStyleBackColor = true;
			this.btnPrethodnaKolonija.Visible = false;
			this.btnPrethodnaKolonija.Click += new System.EventHandler(this.btnPrethodnaKolonija_Click);
			// 
			// groupPoStan
			// 
			this.groupPoStan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupPoStan.Controls.Add(this.lblOdrzavanjePoStan);
			this.groupPoStan.Controls.Add(this.lblRazvojPoStan);
			this.groupPoStan.Controls.Add(this.lblIndustrijaPoStan);
			this.groupPoStan.Controls.Add(this.lblRudePoStan);
			this.groupPoStan.Controls.Add(this.lblHranaPoStan);
			this.groupPoStan.Location = new System.Drawing.Point(6, 115);
			this.groupPoStan.Name = "groupPoStan";
			this.groupPoStan.Size = new System.Drawing.Size(187, 88);
			this.groupPoStan.TabIndex = 30;
			this.groupPoStan.TabStop = false;
			this.groupPoStan.Text = "Po stanovniku";
			// 
			// lblOdrzavanjePoStan
			// 
			this.lblOdrzavanjePoStan.AutoSize = true;
			this.lblOdrzavanjePoStan.Location = new System.Drawing.Point(6, 42);
			this.lblOdrzavanjePoStan.Name = "lblOdrzavanjePoStan";
			this.lblOdrzavanjePoStan.Size = new System.Drawing.Size(85, 13);
			this.lblOdrzavanjePoStan.TabIndex = 4;
			this.lblOdrzavanjePoStan.Text = "Održavanje: xx.x";
			// 
			// lblRazvojPoStan
			// 
			this.lblRazvojPoStan.AutoSize = true;
			this.lblRazvojPoStan.Location = new System.Drawing.Point(6, 68);
			this.lblRazvojPoStan.Name = "lblRazvojPoStan";
			this.lblRazvojPoStan.Size = new System.Drawing.Size(64, 13);
			this.lblRazvojPoStan.TabIndex = 3;
			this.lblRazvojPoStan.Text = "Razvoj: xx.x";
			// 
			// lblIndustrijaPoStan
			// 
			this.lblIndustrijaPoStan.AutoSize = true;
			this.lblIndustrijaPoStan.Location = new System.Drawing.Point(6, 55);
			this.lblIndustrijaPoStan.Name = "lblIndustrijaPoStan";
			this.lblIndustrijaPoStan.Size = new System.Drawing.Size(73, 13);
			this.lblIndustrijaPoStan.TabIndex = 2;
			this.lblIndustrijaPoStan.Text = "Industrija: xx.x";
			// 
			// lblRudePoStan
			// 
			this.lblRudePoStan.AutoSize = true;
			this.lblRudePoStan.Location = new System.Drawing.Point(6, 29);
			this.lblRudePoStan.Name = "lblRudePoStan";
			this.lblRudePoStan.Size = new System.Drawing.Size(57, 13);
			this.lblRudePoStan.TabIndex = 1;
			this.lblRudePoStan.Text = "Rude: xx.x";
			// 
			// lblHranaPoStan
			// 
			this.lblHranaPoStan.AutoSize = true;
			this.lblHranaPoStan.Location = new System.Drawing.Point(6, 16);
			this.lblHranaPoStan.Name = "lblHranaPoStan";
			this.lblHranaPoStan.Size = new System.Drawing.Size(60, 13);
			this.lblHranaPoStan.TabIndex = 0;
			this.lblHranaPoStan.Text = "Hrana: xx.x";
			// 
			// groupCivGradnja
			// 
			this.groupCivGradnja.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupCivGradnja.Controls.Add(this.btnCivilnaGradnja);
			this.groupCivGradnja.Controls.Add(this.hscrCivilnaIndustrija);
			this.groupCivGradnja.Controls.Add(this.lblProcjenaCivilneGradnje);
			this.groupCivGradnja.Controls.Add(this.lblCivilnaIndustrija);
			this.groupCivGradnja.Location = new System.Drawing.Point(6, 209);
			this.groupCivGradnja.Name = "groupCivGradnja";
			this.groupCivGradnja.Size = new System.Drawing.Size(188, 152);
			this.groupCivGradnja.TabIndex = 29;
			this.groupCivGradnja.TabStop = false;
			this.groupCivGradnja.Text = "Civilna gradnja";
			// 
			// btnCivilnaGradnja
			// 
			this.btnCivilnaGradnja.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.btnCivilnaGradnja.Location = new System.Drawing.Point(52, 19);
			this.btnCivilnaGradnja.Name = "btnCivilnaGradnja";
			this.btnCivilnaGradnja.Size = new System.Drawing.Size(84, 84);
			this.btnCivilnaGradnja.TabIndex = 22;
			this.btnCivilnaGradnja.Text = "slika civilne zgrade";
			this.btnCivilnaGradnja.UseVisualStyleBackColor = true;
			this.btnCivilnaGradnja.Click += new System.EventHandler(this.btnCivilnaGradnja_Click);
			// 
			// hscrCivilnaIndustrija
			// 
			this.hscrCivilnaIndustrija.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.hscrCivilnaIndustrija.LargeChange = 1;
			this.hscrCivilnaIndustrija.Location = new System.Drawing.Point(3, 106);
			this.hscrCivilnaIndustrija.Name = "hscrCivilnaIndustrija";
			this.hscrCivilnaIndustrija.Size = new System.Drawing.Size(182, 18);
			this.hscrCivilnaIndustrija.TabIndex = 11;
			this.hscrCivilnaIndustrija.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrCivilnaIndustrija_Scroll);
			// 
			// lblProcjenaCivilneGradnje
			// 
			this.lblProcjenaCivilneGradnje.AutoSize = true;
			this.lblProcjenaCivilneGradnje.Location = new System.Drawing.Point(6, 137);
			this.lblProcjenaCivilneGradnje.Name = "lblProcjenaCivilneGradnje";
			this.lblProcjenaCivilneGradnje.Size = new System.Drawing.Size(82, 13);
			this.lblProcjenaCivilneGradnje.TabIndex = 26;
			this.lblProcjenaCivilneGradnje.Text = "xx.xx X krugova";
			// 
			// lblCivilnaIndustrija
			// 
			this.lblCivilnaIndustrija.AutoSize = true;
			this.lblCivilnaIndustrija.Location = new System.Drawing.Point(6, 124);
			this.lblCivilnaIndustrija.Name = "lblCivilnaIndustrija";
			this.lblCivilnaIndustrija.Size = new System.Drawing.Size(40, 13);
			this.lblCivilnaIndustrija.TabIndex = 12;
			this.lblCivilnaIndustrija.Text = "xx.xx X";
			// 
			// picSlikaPlaneta
			// 
			this.picSlikaPlaneta.Location = new System.Drawing.Point(5, 42);
			this.picSlikaPlaneta.Name = "picSlikaPlaneta";
			this.picSlikaPlaneta.Size = new System.Drawing.Size(32, 32);
			this.picSlikaPlaneta.TabIndex = 7;
			this.picSlikaPlaneta.TabStop = false;
			// 
			// btnPlanetInfo
			// 
			this.btnPlanetInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPlanetInfo.Location = new System.Drawing.Point(6, 86);
			this.btnPlanetInfo.Name = "btnPlanetInfo";
			this.btnPlanetInfo.Size = new System.Drawing.Size(192, 23);
			this.btnPlanetInfo.TabIndex = 6;
			this.btnPlanetInfo.Text = "&Detaljnije";
			this.btnPlanetInfo.UseVisualStyleBackColor = true;
			this.btnPlanetInfo.Click += new System.EventHandler(this.btnPlanetInfo_Click);
			// 
			// lblPopulacija
			// 
			this.lblPopulacija.AutoSize = true;
			this.lblPopulacija.Location = new System.Drawing.Point(43, 42);
			this.lblPopulacija.Name = "lblPopulacija";
			this.lblPopulacija.Size = new System.Drawing.Size(55, 13);
			this.lblPopulacija.TabIndex = 1;
			this.lblPopulacija.Text = "populacija";
			// 
			// lblImePlaneta
			// 
			this.lblImePlaneta.AutoSize = true;
			this.lblImePlaneta.Location = new System.Drawing.Point(43, 11);
			this.lblImePlaneta.Name = "lblImePlaneta";
			this.lblImePlaneta.Size = new System.Drawing.Size(23, 13);
			this.lblImePlaneta.TabIndex = 0;
			this.lblImePlaneta.Text = "ime";
			// 
			// tabPageFlote
			// 
			this.tabPageFlote.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageFlote.Controls.Add(this.tvFlota);
			this.tabPageFlote.Controls.Add(this.panel1);
			this.tabPageFlote.Location = new System.Drawing.Point(4, 4);
			this.tabPageFlote.Name = "tabPageFlote";
			this.tabPageFlote.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageFlote.Size = new System.Drawing.Size(204, 385);
			this.tabPageFlote.TabIndex = 2;
			// 
			// tvFlota
			// 
			this.tvFlota.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvFlota.Location = new System.Drawing.Point(3, 103);
			this.tvFlota.Name = "tvFlota";
			this.tvFlota.Size = new System.Drawing.Size(198, 279);
			this.tvFlota.TabIndex = 1;
			this.tvFlota.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFlota_AfterSelect);
			this.tvFlota.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvFlota_NodeMouseDoubleClick);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnFlotaPokret);
			this.panel1.Controls.Add(this.btnSekAkcija);
			this.panel1.Controls.Add(this.btnPrimAkcijaBroda);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(198, 100);
			this.panel1.TabIndex = 0;
			// 
			// btnFlotaPokret
			// 
			this.btnFlotaPokret.Location = new System.Drawing.Point(3, 71);
			this.btnFlotaPokret.Name = "btnFlotaPokret";
			this.btnFlotaPokret.Size = new System.Drawing.Size(75, 23);
			this.btnFlotaPokret.TabIndex = 2;
			this.btnFlotaPokret.Text = "Pokret";
			this.btnFlotaPokret.UseVisualStyleBackColor = true;
			this.btnFlotaPokret.Click += new System.EventHandler(this.btnFlotaPokret_Click);
			// 
			// btnSekAkcija
			// 
			this.btnSekAkcija.Location = new System.Drawing.Point(3, 42);
			this.btnSekAkcija.Name = "btnSekAkcija";
			this.btnSekAkcija.Size = new System.Drawing.Size(75, 23);
			this.btnSekAkcija.TabIndex = 1;
			this.btnSekAkcija.Text = "Sek akcija";
			this.btnSekAkcija.UseVisualStyleBackColor = true;
			// 
			// btnPrimAkcijaBroda
			// 
			this.btnPrimAkcijaBroda.Location = new System.Drawing.Point(3, 13);
			this.btnPrimAkcijaBroda.Name = "btnPrimAkcijaBroda";
			this.btnPrimAkcijaBroda.Size = new System.Drawing.Size(75, 23);
			this.btnPrimAkcijaBroda.TabIndex = 0;
			this.btnPrimAkcijaBroda.Text = "Prim akcija";
			this.btnPrimAkcijaBroda.UseVisualStyleBackColor = true;
			this.btnPrimAkcijaBroda.Click += new System.EventHandler(this.btnPrimAkcijaBroda_Click);
			// 
			// lblRazvoj
			// 
			this.lblRazvoj.AutoSize = true;
			this.lblRazvoj.Location = new System.Drawing.Point(255, 19);
			this.lblRazvoj.Name = "lblRazvoj";
			this.lblRazvoj.Size = new System.Drawing.Size(79, 13);
			this.lblRazvoj.TabIndex = 13;
			this.lblRazvoj.Text = "Razvoj: xx.xx X";
			// 
			// btnVojnaGradnja
			// 
			this.btnVojnaGradnja.Location = new System.Drawing.Point(165, 6);
			this.btnVojnaGradnja.Name = "btnVojnaGradnja";
			this.btnVojnaGradnja.Size = new System.Drawing.Size(84, 84);
			this.btnVojnaGradnja.TabIndex = 23;
			this.btnVojnaGradnja.Text = "slika vojne gradnje";
			this.btnVojnaGradnja.UseVisualStyleBackColor = true;
			this.btnVojnaGradnja.Click += new System.EventHandler(this.btnVojnaGradnja_Click);
			// 
			// lblProcjenaVojneGradnje
			// 
			this.lblProcjenaVojneGradnje.AutoSize = true;
			this.lblProcjenaVojneGradnje.Location = new System.Drawing.Point(167, 111);
			this.lblProcjenaVojneGradnje.Name = "lblProcjenaVojneGradnje";
			this.lblProcjenaVojneGradnje.Size = new System.Drawing.Size(82, 13);
			this.lblProcjenaVojneGradnje.TabIndex = 27;
			this.lblProcjenaVojneGradnje.Text = "xx.xx X krugova";
			// 
			// hscrZvjezdaGradnja
			// 
			this.hscrZvjezdaGradnja.LargeChange = 1;
			this.hscrZvjezdaGradnja.Location = new System.Drawing.Point(165, 93);
			this.hscrZvjezdaGradnja.Name = "hscrZvjezdaGradnja";
			this.hscrZvjezdaGradnja.Size = new System.Drawing.Size(84, 18);
			this.hscrZvjezdaGradnja.TabIndex = 24;
			this.hscrZvjezdaGradnja.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrZvjezdanaGradnja_Scroll);
			// 
			// lblVojnaGradnja
			// 
			this.lblVojnaGradnja.AutoSize = true;
			this.lblVojnaGradnja.Location = new System.Drawing.Point(255, 6);
			this.lblVojnaGradnja.Name = "lblVojnaGradnja";
			this.lblVojnaGradnja.Size = new System.Drawing.Size(40, 13);
			this.lblVojnaGradnja.TabIndex = 25;
			this.lblVojnaGradnja.Text = "xx.xx X";
			// 
			// pnlMapa
			// 
			this.pnlMapa.AutoScroll = true;
			this.pnlMapa.Controls.Add(this.picMapa);
			this.pnlMapa.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMapa.Location = new System.Drawing.Point(0, 24);
			this.pnlMapa.Name = "pnlMapa";
			this.pnlMapa.Size = new System.Drawing.Size(575, 358);
			this.pnlMapa.TabIndex = 1;
			// 
			// picMapa
			// 
			this.picMapa.Location = new System.Drawing.Point(0, 0);
			this.picMapa.Name = "picMapa";
			this.picMapa.Size = new System.Drawing.Size(40, 128);
			this.picMapa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picMapa.TabIndex = 0;
			this.picMapa.TabStop = false;
			this.picMapa.Click += new System.EventHandler(this.picMapa_Click);
			// 
			// pnlDno
			// 
			this.pnlDno.Controls.Add(this.dnoSredinaPanel);
			this.pnlDno.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlDno.Location = new System.Drawing.Point(0, 382);
			this.pnlDno.Name = "pnlDno";
			this.pnlDno.Size = new System.Drawing.Size(575, 134);
			this.pnlDno.TabIndex = 1;
			// 
			// dnoSredinaPanel
			// 
			this.dnoSredinaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.dnoSredinaPanel.Controls.Add(this.lblVojnaGradnja);
			this.dnoSredinaPanel.Controls.Add(this.lblProcjenaVojneGradnje);
			this.dnoSredinaPanel.Controls.Add(this.btnVojnaGradnja);
			this.dnoSredinaPanel.Controls.Add(this.lblRazvoj);
			this.dnoSredinaPanel.Controls.Add(this.hscrZvjezdaGradnja);
			this.dnoSredinaPanel.Controls.Add(this.zvijezdaPicBox);
			this.dnoSredinaPanel.Controls.Add(this.lblImeZvjezde);
			this.dnoSredinaPanel.Location = new System.Drawing.Point(101, 0);
			this.dnoSredinaPanel.Name = "dnoSredinaPanel";
			this.dnoSredinaPanel.Size = new System.Drawing.Size(372, 134);
			this.dnoSredinaPanel.TabIndex = 2;
			// 
			// zvijezdaPicBox
			// 
			this.zvijezdaPicBox.Location = new System.Drawing.Point(3, 6);
			this.zvijezdaPicBox.Name = "zvijezdaPicBox";
			this.zvijezdaPicBox.Size = new System.Drawing.Size(40, 40);
			this.zvijezdaPicBox.TabIndex = 2;
			this.zvijezdaPicBox.TabStop = false;
			// 
			// lblImeZvjezde
			// 
			this.lblImeZvjezde.AutoSize = true;
			this.lblImeZvjezde.Location = new System.Drawing.Point(49, 6);
			this.lblImeZvjezde.Name = "lblImeZvjezde";
			this.lblImeZvjezde.Size = new System.Drawing.Size(110, 39);
			this.lblImeZvjezde.TabIndex = 1;
			this.lblImeZvjezde.Text = "Ime zvijezde\r\nZračenje\r\nMax migracija: xx.xx X";
			// 
			// btnEndTurn
			// 
			this.btnEndTurn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEndTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnEndTurn.Location = new System.Drawing.Point(140, 424);
			this.btnEndTurn.Name = "btnEndTurn";
			this.btnEndTurn.Size = new System.Drawing.Size(68, 68);
			this.btnEndTurn.TabIndex = 1;
			this.btnEndTurn.Text = "Završi &krug";
			this.btnEndTurn.UseVisualStyleBackColor = true;
			this.btnEndTurn.Click += new System.EventHandler(this.btnEndTurn_Click);
			// 
			// timerAnimacija
			// 
			this.timerAnimacija.Enabled = true;
			this.timerAnimacija.Interval = 30;
			this.timerAnimacija.Tick += new System.EventHandler(this.timerAnimacija_Tick);
			// 
			// pnlDesno
			// 
			this.pnlDesno.Controls.Add(this.btnEndTurn);
			this.pnlDesno.Controls.Add(this.tabCtrlDesno);
			this.pnlDesno.Controls.Add(this.pnlDesnoGore);
			this.pnlDesno.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlDesno.Location = new System.Drawing.Point(575, 24);
			this.pnlDesno.Name = "pnlDesno";
			this.pnlDesno.Size = new System.Drawing.Size(250, 492);
			this.pnlDesno.TabIndex = 1;
			// 
			// pnlDesnoGore
			// 
			this.pnlDesnoGore.Controls.Add(this.lblBrojKruga);
			this.pnlDesnoGore.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDesnoGore.Location = new System.Drawing.Point(0, 0);
			this.pnlDesnoGore.Name = "pnlDesnoGore";
			this.pnlDesnoGore.Size = new System.Drawing.Size(250, 31);
			this.pnlDesnoGore.TabIndex = 2;
			// 
			// lblBrojKruga
			// 
			this.lblBrojKruga.AutoSize = true;
			this.lblBrojKruga.Location = new System.Drawing.Point(68, 9);
			this.lblBrojKruga.Name = "lblBrojKruga";
			this.lblBrojKruga.Size = new System.Drawing.Size(55, 13);
			this.lblBrojKruga.TabIndex = 1;
			this.lblBrojKruga.Text = "Broj kruga";
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.izbornikMenu,
            this.novostiMenu,
            this.kolonijeMenu,
            this.floteMenu,
            this.tehnologijeMenu,
            this.dizjnoviToolStripMenuItem});
			this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(825, 24);
			this.menuStrip.TabIndex = 6;
			this.menuStrip.Text = "menuStrip1";
			// 
			// izbornikMenu
			// 
			this.izbornikMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.novaIgraMenu,
            this.spremiMenu,
            this.ucitajMenu,
            this.uvećanjeToolStripMenuItem,
            this.izlazMenu});
			this.izbornikMenu.Name = "izbornikMenu";
			this.izbornikMenu.ShortcutKeyDisplayString = "";
			this.izbornikMenu.Size = new System.Drawing.Size(61, 20);
			this.izbornikMenu.Text = "Izbornik";
			// 
			// novaIgraMenu
			// 
			this.novaIgraMenu.Name = "novaIgraMenu";
			this.novaIgraMenu.Size = new System.Drawing.Size(125, 22);
			this.novaIgraMenu.Text = "Nova igra";
			// 
			// spremiMenu
			// 
			this.spremiMenu.Name = "spremiMenu";
			this.spremiMenu.Size = new System.Drawing.Size(125, 22);
			this.spremiMenu.Text = "Spremi";
			this.spremiMenu.Click += new System.EventHandler(this.spremiMenu_Click);
			// 
			// ucitajMenu
			// 
			this.ucitajMenu.Name = "ucitajMenu";
			this.ucitajMenu.Size = new System.Drawing.Size(125, 22);
			this.ucitajMenu.Text = "Učitaj";
			this.ucitajMenu.Click += new System.EventHandler(this.ucitajMenu_Click);
			// 
			// uvećanjeToolStripMenuItem
			// 
			this.uvećanjeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uvecajMenu,
            this.umanjiMenu});
			this.uvećanjeToolStripMenuItem.Name = "uvećanjeToolStripMenuItem";
			this.uvećanjeToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
			this.uvećanjeToolStripMenuItem.Text = "Mapa";
			// 
			// uvecajMenu
			// 
			this.uvecajMenu.Name = "uvecajMenu";
			this.uvecajMenu.ShortcutKeyDisplayString = "Ctrl++";
			this.uvecajMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
			this.uvecajMenu.Size = new System.Drawing.Size(151, 22);
			this.uvecajMenu.Text = "Uvećaj";
			this.uvecajMenu.Click += new System.EventHandler(this.uvecajMenu_Click);
			// 
			// umanjiMenu
			// 
			this.umanjiMenu.Name = "umanjiMenu";
			this.umanjiMenu.ShortcutKeyDisplayString = "Ctrl--";
			this.umanjiMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
			this.umanjiMenu.Size = new System.Drawing.Size(151, 22);
			this.umanjiMenu.Text = "Umanji";
			this.umanjiMenu.Click += new System.EventHandler(this.umanjiMenu_Click);
			// 
			// izlazMenu
			// 
			this.izlazMenu.Name = "izlazMenu";
			this.izlazMenu.Size = new System.Drawing.Size(125, 22);
			this.izlazMenu.Text = "Izlaz";
			// 
			// novostiMenu
			// 
			this.novostiMenu.Name = "novostiMenu";
			this.novostiMenu.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.novostiMenu.Size = new System.Drawing.Size(60, 20);
			this.novostiMenu.Text = "Novosti";
			this.novostiMenu.Click += new System.EventHandler(this.novostiMenu_Click);
			// 
			// kolonijeMenu
			// 
			this.kolonijeMenu.Name = "kolonijeMenu";
			this.kolonijeMenu.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.kolonijeMenu.Size = new System.Drawing.Size(62, 20);
			this.kolonijeMenu.Text = "Kolonije";
			// 
			// floteMenu
			// 
			this.floteMenu.Name = "floteMenu";
			this.floteMenu.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.floteMenu.Size = new System.Drawing.Size(45, 20);
			this.floteMenu.Text = "Flote";
			this.floteMenu.Click += new System.EventHandler(this.floteMenu_Click);
			// 
			// tehnologijeMenu
			// 
			this.tehnologijeMenu.Name = "tehnologijeMenu";
			this.tehnologijeMenu.ShortcutKeys = System.Windows.Forms.Keys.F4;
			this.tehnologijeMenu.Size = new System.Drawing.Size(82, 20);
			this.tehnologijeMenu.Text = "Tehnologije";
			this.tehnologijeMenu.Click += new System.EventHandler(this.tehnologijeMenu_Click);
			// 
			// backgroundTurnProcessor
			// 
			this.backgroundTurnProcessor.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundTurnProcessor_DoWork);
			this.backgroundTurnProcessor.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundTurnProcessor_RunWorkerCompleted);
			// 
			// dizjnoviToolStripMenuItem
			// 
			this.dizjnoviToolStripMenuItem.Name = "dizjnoviToolStripMenuItem";
			this.dizjnoviToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
			this.dizjnoviToolStripMenuItem.Text = "Dizajnovi";
			this.dizjnoviToolStripMenuItem.Click += new System.EventHandler(this.dizajnoviToolStripMenuItem_Click);
			// 
			// FormIgra
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(825, 516);
			this.Controls.Add(this.pnlMapa);
			this.Controls.Add(this.pnlDno);
			this.Controls.Add(this.pnlDesno);
			this.Controls.Add(this.menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip;
			this.Name = "FormIgra";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Zvjezdojedac";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormIgra_FormClosed);
			this.Load += new System.EventHandler(this.frmIgra_Load);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormIgra_KeyPress);
			this.tabCtrlDesno.ResumeLayout(false);
			this.tabPageZvijezda.ResumeLayout(false);
			this.tabPageKolonija.ResumeLayout(false);
			this.tabPageKolonija.PerformLayout();
			this.groupPoStan.ResumeLayout(false);
			this.groupPoStan.PerformLayout();
			this.groupCivGradnja.ResumeLayout(false);
			this.groupCivGradnja.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSlikaPlaneta)).EndInit();
			this.tabPageFlote.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.pnlMapa.ResumeLayout(false);
			this.pnlMapa.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picMapa)).EndInit();
			this.pnlDno.ResumeLayout(false);
			this.dnoSredinaPanel.ResumeLayout(false);
			this.dnoSredinaPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.zvijezdaPicBox)).EndInit();
			this.pnlDesno.ResumeLayout(false);
			this.pnlDesnoGore.ResumeLayout(false);
			this.pnlDesnoGore.PerformLayout();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabCtrlDesno;
		private System.Windows.Forms.TabPage tabPageZvijezda;
		private System.Windows.Forms.Panel pnlMapa;
		private System.Windows.Forms.PictureBox picMapa;
		private System.Windows.Forms.Timer timerAnimacija;
		private System.Windows.Forms.Panel pnlOpisZvjezde;
		private System.Windows.Forms.TabPage tabPageKolonija;
		private System.Windows.Forms.Label lblImePlaneta;
		private System.Windows.Forms.Label lblPopulacija;
		private System.Windows.Forms.Button btnEndTurn;
		private System.Windows.Forms.Panel pnlDesno;
		private System.Windows.Forms.Label lblBrojKruga;
		private System.Windows.Forms.Panel pnlDesnoGore;
		private System.Windows.Forms.Button btnPlanetInfo;
		private System.Windows.Forms.PictureBox picSlikaPlaneta;
		private System.Windows.Forms.HScrollBar hscrCivilnaIndustrija;
		private System.Windows.Forms.Label lblRazvoj;
		private System.Windows.Forms.Label lblCivilnaIndustrija;
		private System.Windows.Forms.Button btnCivilnaGradnja;
		private System.Windows.Forms.HScrollBar hscrZvjezdaGradnja;
		private System.Windows.Forms.Button btnVojnaGradnja;
		private System.Windows.Forms.Label lblVojnaGradnja;
		private System.Windows.Forms.Label lblProcjenaVojneGradnje;
		private System.Windows.Forms.Label lblProcjenaCivilneGradnje;
		private System.Windows.Forms.GroupBox groupCivGradnja;
		private System.Windows.Forms.TabPage tabPageFlote;
		private System.Windows.Forms.TreeView tvFlota;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupPoStan;
		private System.Windows.Forms.Label lblHranaPoStan;
		private System.Windows.Forms.Label lblRudePoStan;
		private System.Windows.Forms.Label lblIndustrijaPoStan;
		private System.Windows.Forms.Label lblOdrzavanjePoStan;
		private System.Windows.Forms.Label lblRazvojPoStan;
		private System.Windows.Forms.Button btnPrimAkcijaBroda;
		private System.Windows.Forms.Button btnFlotaPokret;
		private System.Windows.Forms.Button btnSekAkcija;
		private System.Windows.Forms.Button btnSlijedecaKolonija;
		private System.Windows.Forms.Button btnPrethodnaKolonija;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem izbornikMenu;
		private System.Windows.Forms.ToolStripMenuItem spremiMenu;
		private System.Windows.Forms.ToolStripMenuItem novaIgraMenu;
		private System.Windows.Forms.ToolStripMenuItem ucitajMenu;
		private System.Windows.Forms.ToolStripMenuItem izlazMenu;
		private System.Windows.Forms.ToolStripMenuItem novostiMenu;
		private System.Windows.Forms.ToolStripMenuItem kolonijeMenu;
		private System.Windows.Forms.ToolStripMenuItem floteMenu;
		private System.Windows.Forms.ToolStripMenuItem tehnologijeMenu;
		private System.Windows.Forms.ToolStripMenuItem uvećanjeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uvecajMenu;
		private System.Windows.Forms.ToolStripMenuItem umanjiMenu;
		private System.Windows.Forms.FlowLayoutPanel planetiFlowPanel;
		private System.Windows.Forms.Panel pnlDno;
		private System.Windows.Forms.Label lblImeZvjezde;
		private System.Windows.Forms.Panel dnoSredinaPanel;
		private System.Windows.Forms.PictureBox zvijezdaPicBox;
		private System.ComponentModel.BackgroundWorker backgroundTurnProcessor;
		private System.Windows.Forms.ToolStripMenuItem dizjnoviToolStripMenuItem;
	}
}