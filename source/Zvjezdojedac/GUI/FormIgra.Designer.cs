namespace Prototip
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
			this.listViewPlaneti = new System.Windows.Forms.ListView();
			this.pnlOpisZvjezde = new System.Windows.Forms.Panel();
			this.lblImeZvjezde = new System.Windows.Forms.Label();
			this.tabPageKolonija = new System.Windows.Forms.TabPage();
			this.groupPoStan = new System.Windows.Forms.GroupBox();
			this.lblOdrzavanjePoStan = new System.Windows.Forms.Label();
			this.lblRazvojPoStan = new System.Windows.Forms.Label();
			this.lblIndustrijaPoStan = new System.Windows.Forms.Label();
			this.lblRudePoStan = new System.Windows.Forms.Label();
			this.lblHranaPoStan = new System.Windows.Forms.Label();
			this.lblRazvoj = new System.Windows.Forms.Label();
			this.groupCivGradnja = new System.Windows.Forms.GroupBox();
			this.btnCivilnaGradnja = new System.Windows.Forms.Button();
			this.hscrCivilnaIndustrija = new System.Windows.Forms.HScrollBar();
			this.lblProcjenaCivilneGradnje = new System.Windows.Forms.Label();
			this.lblCivilnaIndustrija = new System.Windows.Forms.Label();
			this.groupVojGradnja = new System.Windows.Forms.GroupBox();
			this.btnVojnaGradnja = new System.Windows.Forms.Button();
			this.lblProcjenaVojneGradnje = new System.Windows.Forms.Label();
			this.hscrVojnaIndustrija = new System.Windows.Forms.HScrollBar();
			this.lblVojnaGradnja = new System.Windows.Forms.Label();
			this.picSlikaPlaneta = new System.Windows.Forms.PictureBox();
			this.btnPlanetInfo = new System.Windows.Forms.Button();
			this.lblPopulacija = new System.Windows.Forms.Label();
			this.lblImePlaneta = new System.Windows.Forms.Label();
			this.tabPageFlote = new System.Windows.Forms.TabPage();
			this.tvFlota = new System.Windows.Forms.TreeView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnPrimAkcijaBroda = new System.Windows.Forms.Button();
			this.pnlMapa = new System.Windows.Forms.Panel();
			this.picMapa = new System.Windows.Forms.PictureBox();
			this.btnEndTurn = new System.Windows.Forms.Button();
			this.trackBarZoom = new System.Windows.Forms.TrackBar();
			this.timerAnimacija = new System.Windows.Forms.Timer(this.components);
			this.pnlDesno = new System.Windows.Forms.Panel();
			this.pnlDesnoGore = new System.Windows.Forms.Panel();
			this.lblBrojKruga = new System.Windows.Forms.Label();
			this.pnlKomande = new System.Windows.Forms.Panel();
			this.btnPoruke = new System.Windows.Forms.Button();
			this.btnFlote = new System.Windows.Forms.Button();
			this.pnlKomandeDesno = new System.Windows.Forms.Panel();
			this.btnUcitaj = new System.Windows.Forms.Button();
			this.btnSpremi = new System.Windows.Forms.Button();
			this.btnTech = new System.Windows.Forms.Button();
			this.btnSekAkcija = new System.Windows.Forms.Button();
			this.btnFlotaPokret = new System.Windows.Forms.Button();
			this.tabCtrlDesno.SuspendLayout();
			this.tabPageZvijezda.SuspendLayout();
			this.pnlOpisZvjezde.SuspendLayout();
			this.tabPageKolonija.SuspendLayout();
			this.groupPoStan.SuspendLayout();
			this.groupCivGradnja.SuspendLayout();
			this.groupVojGradnja.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSlikaPlaneta)).BeginInit();
			this.tabPageFlote.SuspendLayout();
			this.panel1.SuspendLayout();
			this.pnlMapa.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picMapa)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).BeginInit();
			this.pnlDesno.SuspendLayout();
			this.pnlDesnoGore.SuspendLayout();
			this.pnlKomande.SuspendLayout();
			this.pnlKomandeDesno.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabCtrlDesno
			// 
			this.tabCtrlDesno.Alignment = System.Windows.Forms.TabAlignment.Right;
			this.tabCtrlDesno.Controls.Add(this.tabPageZvijezda);
			this.tabCtrlDesno.Controls.Add(this.tabPageKolonija);
			this.tabCtrlDesno.Controls.Add(this.tabPageFlote);
			this.tabCtrlDesno.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabCtrlDesno.ItemSize = new System.Drawing.Size(10, 38);
			this.tabCtrlDesno.Location = new System.Drawing.Point(0, 31);
			this.tabCtrlDesno.Multiline = true;
			this.tabCtrlDesno.Name = "tabCtrlDesno";
			this.tabCtrlDesno.SelectedIndex = 0;
			this.tabCtrlDesno.Size = new System.Drawing.Size(200, 531);
			this.tabCtrlDesno.TabIndex = 0;
			// 
			// tabPageZvijezda
			// 
			this.tabPageZvijezda.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageZvijezda.Controls.Add(this.listViewPlaneti);
			this.tabPageZvijezda.Controls.Add(this.pnlOpisZvjezde);
			this.tabPageZvijezda.Location = new System.Drawing.Point(4, 4);
			this.tabPageZvijezda.Name = "tabPageZvijezda";
			this.tabPageZvijezda.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageZvijezda.Size = new System.Drawing.Size(154, 523);
			this.tabPageZvijezda.TabIndex = 0;
			// 
			// listViewPlaneti
			// 
			this.listViewPlaneti.AutoArrange = false;
			this.listViewPlaneti.BackColor = System.Drawing.Color.Black;
			this.listViewPlaneti.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewPlaneti.ForeColor = System.Drawing.Color.White;
			this.listViewPlaneti.FullRowSelect = true;
			this.listViewPlaneti.Location = new System.Drawing.Point(3, 40);
			this.listViewPlaneti.MultiSelect = false;
			this.listViewPlaneti.Name = "listViewPlaneti";
			this.listViewPlaneti.Size = new System.Drawing.Size(148, 480);
			this.listViewPlaneti.TabIndex = 0;
			this.listViewPlaneti.UseCompatibleStateImageBehavior = false;
			this.listViewPlaneti.View = System.Windows.Forms.View.Tile;
			this.listViewPlaneti.Click += new System.EventHandler(this.listViewPlaneti_Click);
			// 
			// pnlOpisZvjezde
			// 
			this.pnlOpisZvjezde.Controls.Add(this.lblImeZvjezde);
			this.pnlOpisZvjezde.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlOpisZvjezde.Location = new System.Drawing.Point(3, 3);
			this.pnlOpisZvjezde.Name = "pnlOpisZvjezde";
			this.pnlOpisZvjezde.Size = new System.Drawing.Size(148, 37);
			this.pnlOpisZvjezde.TabIndex = 1;
			// 
			// lblImeZvjezde
			// 
			this.lblImeZvjezde.AutoSize = true;
			this.lblImeZvjezde.Location = new System.Drawing.Point(3, 2);
			this.lblImeZvjezde.Name = "lblImeZvjezde";
			this.lblImeZvjezde.Size = new System.Drawing.Size(65, 26);
			this.lblImeZvjezde.TabIndex = 0;
			this.lblImeZvjezde.Text = "Ime zvijezde\r\nZračenje";
			// 
			// tabPageKolonija
			// 
			this.tabPageKolonija.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageKolonija.Controls.Add(this.groupPoStan);
			this.tabPageKolonija.Controls.Add(this.lblRazvoj);
			this.tabPageKolonija.Controls.Add(this.groupCivGradnja);
			this.tabPageKolonija.Controls.Add(this.groupVojGradnja);
			this.tabPageKolonija.Controls.Add(this.picSlikaPlaneta);
			this.tabPageKolonija.Controls.Add(this.btnPlanetInfo);
			this.tabPageKolonija.Controls.Add(this.lblPopulacija);
			this.tabPageKolonija.Controls.Add(this.lblImePlaneta);
			this.tabPageKolonija.Location = new System.Drawing.Point(4, 4);
			this.tabPageKolonija.Name = "tabPageKolonija";
			this.tabPageKolonija.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageKolonija.Size = new System.Drawing.Size(154, 523);
			this.tabPageKolonija.TabIndex = 1;
			// 
			// groupPoStan
			// 
			this.groupPoStan.Controls.Add(this.lblOdrzavanjePoStan);
			this.groupPoStan.Controls.Add(this.lblRazvojPoStan);
			this.groupPoStan.Controls.Add(this.lblIndustrijaPoStan);
			this.groupPoStan.Controls.Add(this.lblRudePoStan);
			this.groupPoStan.Controls.Add(this.lblHranaPoStan);
			this.groupPoStan.Location = new System.Drawing.Point(6, 70);
			this.groupPoStan.Name = "groupPoStan";
			this.groupPoStan.Size = new System.Drawing.Size(137, 88);
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
			// lblRazvoj
			// 
			this.lblRazvoj.AutoSize = true;
			this.lblRazvoj.Location = new System.Drawing.Point(6, 478);
			this.lblRazvoj.Name = "lblRazvoj";
			this.lblRazvoj.Size = new System.Drawing.Size(79, 13);
			this.lblRazvoj.TabIndex = 13;
			this.lblRazvoj.Text = "Razvoj: xx.xx X";
			// 
			// groupCivGradnja
			// 
			this.groupCivGradnja.Controls.Add(this.btnCivilnaGradnja);
			this.groupCivGradnja.Controls.Add(this.hscrCivilnaIndustrija);
			this.groupCivGradnja.Controls.Add(this.lblProcjenaCivilneGradnje);
			this.groupCivGradnja.Controls.Add(this.lblCivilnaIndustrija);
			this.groupCivGradnja.Location = new System.Drawing.Point(6, 164);
			this.groupCivGradnja.Name = "groupCivGradnja";
			this.groupCivGradnja.Size = new System.Drawing.Size(138, 152);
			this.groupCivGradnja.TabIndex = 29;
			this.groupCivGradnja.TabStop = false;
			this.groupCivGradnja.Text = "Civilna gradnja";
			// 
			// btnCivilnaGradnja
			// 
			this.btnCivilnaGradnja.Location = new System.Drawing.Point(30, 19);
			this.btnCivilnaGradnja.Name = "btnCivilnaGradnja";
			this.btnCivilnaGradnja.Size = new System.Drawing.Size(84, 84);
			this.btnCivilnaGradnja.TabIndex = 22;
			this.btnCivilnaGradnja.Text = "slika civilne zgrade";
			this.btnCivilnaGradnja.UseVisualStyleBackColor = true;
			this.btnCivilnaGradnja.Click += new System.EventHandler(this.btnCivilnaGradnja_Click);
			// 
			// hscrCivilnaIndustrija
			// 
			this.hscrCivilnaIndustrija.LargeChange = 1;
			this.hscrCivilnaIndustrija.Location = new System.Drawing.Point(3, 106);
			this.hscrCivilnaIndustrija.Name = "hscrCivilnaIndustrija";
			this.hscrCivilnaIndustrija.Size = new System.Drawing.Size(134, 18);
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
			// groupVojGradnja
			// 
			this.groupVojGradnja.Controls.Add(this.btnVojnaGradnja);
			this.groupVojGradnja.Controls.Add(this.lblProcjenaVojneGradnje);
			this.groupVojGradnja.Controls.Add(this.hscrVojnaIndustrija);
			this.groupVojGradnja.Controls.Add(this.lblVojnaGradnja);
			this.groupVojGradnja.Location = new System.Drawing.Point(6, 322);
			this.groupVojGradnja.Name = "groupVojGradnja";
			this.groupVojGradnja.Size = new System.Drawing.Size(138, 153);
			this.groupVojGradnja.TabIndex = 28;
			this.groupVojGradnja.TabStop = false;
			this.groupVojGradnja.Text = "Vojna gradnja";
			// 
			// btnVojnaGradnja
			// 
			this.btnVojnaGradnja.Location = new System.Drawing.Point(28, 15);
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
			this.lblProcjenaVojneGradnje.Location = new System.Drawing.Point(6, 138);
			this.lblProcjenaVojneGradnje.Name = "lblProcjenaVojneGradnje";
			this.lblProcjenaVojneGradnje.Size = new System.Drawing.Size(82, 13);
			this.lblProcjenaVojneGradnje.TabIndex = 27;
			this.lblProcjenaVojneGradnje.Text = "xx.xx X krugova";
			// 
			// hscrVojnaIndustrija
			// 
			this.hscrVojnaIndustrija.LargeChange = 1;
			this.hscrVojnaIndustrija.Location = new System.Drawing.Point(3, 102);
			this.hscrVojnaIndustrija.Name = "hscrVojnaIndustrija";
			this.hscrVojnaIndustrija.Size = new System.Drawing.Size(134, 18);
			this.hscrVojnaIndustrija.TabIndex = 24;
			this.hscrVojnaIndustrija.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrVojnaIndustrija_Scroll);
			// 
			// lblVojnaGradnja
			// 
			this.lblVojnaGradnja.AutoSize = true;
			this.lblVojnaGradnja.Location = new System.Drawing.Point(6, 123);
			this.lblVojnaGradnja.Name = "lblVojnaGradnja";
			this.lblVojnaGradnja.Size = new System.Drawing.Size(40, 13);
			this.lblVojnaGradnja.TabIndex = 25;
			this.lblVojnaGradnja.Text = "xx.xx X";
			// 
			// picSlikaPlaneta
			// 
			this.picSlikaPlaneta.Location = new System.Drawing.Point(6, 6);
			this.picSlikaPlaneta.Name = "picSlikaPlaneta";
			this.picSlikaPlaneta.Size = new System.Drawing.Size(32, 32);
			this.picSlikaPlaneta.TabIndex = 7;
			this.picSlikaPlaneta.TabStop = false;
			// 
			// btnPlanetInfo
			// 
			this.btnPlanetInfo.Location = new System.Drawing.Point(6, 41);
			this.btnPlanetInfo.Name = "btnPlanetInfo";
			this.btnPlanetInfo.Size = new System.Drawing.Size(142, 23);
			this.btnPlanetInfo.TabIndex = 6;
			this.btnPlanetInfo.Text = "&Detaljnije";
			this.btnPlanetInfo.UseVisualStyleBackColor = true;
			this.btnPlanetInfo.Click += new System.EventHandler(this.btnPlanetInfo_Click);
			// 
			// lblPopulacija
			// 
			this.lblPopulacija.AutoSize = true;
			this.lblPopulacija.Location = new System.Drawing.Point(44, 19);
			this.lblPopulacija.Name = "lblPopulacija";
			this.lblPopulacija.Size = new System.Drawing.Size(55, 13);
			this.lblPopulacija.TabIndex = 1;
			this.lblPopulacija.Text = "populacija";
			// 
			// lblImePlaneta
			// 
			this.lblImePlaneta.AutoSize = true;
			this.lblImePlaneta.Location = new System.Drawing.Point(44, 6);
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
			this.tabPageFlote.Size = new System.Drawing.Size(154, 523);
			this.tabPageFlote.TabIndex = 2;
			// 
			// tvFlota
			// 
			this.tvFlota.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvFlota.Location = new System.Drawing.Point(3, 103);
			this.tvFlota.Name = "tvFlota";
			this.tvFlota.Size = new System.Drawing.Size(148, 417);
			this.tvFlota.TabIndex = 1;
			this.tvFlota.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvFlota_NodeMouseDoubleClick);
			this.tvFlota.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFlota_AfterSelect);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnFlotaPokret);
			this.panel1.Controls.Add(this.btnSekAkcija);
			this.panel1.Controls.Add(this.btnPrimAkcijaBroda);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(148, 100);
			this.panel1.TabIndex = 0;
			// 
			// btnPrimAkcijaBroda
			// 
			this.btnPrimAkcijaBroda.Location = new System.Drawing.Point(3, 13);
			this.btnPrimAkcijaBroda.Name = "btnPrimAkcijaBroda";
			this.btnPrimAkcijaBroda.Size = new System.Drawing.Size(75, 23);
			this.btnPrimAkcijaBroda.TabIndex = 0;
			this.btnPrimAkcijaBroda.Text = "Prim akcija";
			this.btnPrimAkcijaBroda.UseVisualStyleBackColor = true;
			// 
			// pnlMapa
			// 
			this.pnlMapa.AutoScroll = true;
			this.pnlMapa.Controls.Add(this.picMapa);
			this.pnlMapa.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMapa.Location = new System.Drawing.Point(0, 0);
			this.pnlMapa.Name = "pnlMapa";
			this.pnlMapa.Size = new System.Drawing.Size(484, 479);
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
			// btnEndTurn
			// 
			this.btnEndTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.btnEndTurn.Location = new System.Drawing.Point(247, 19);
			this.btnEndTurn.Name = "btnEndTurn";
			this.btnEndTurn.Size = new System.Drawing.Size(60, 52);
			this.btnEndTurn.TabIndex = 1;
			this.btnEndTurn.Text = "Završi &krug";
			this.btnEndTurn.UseVisualStyleBackColor = true;
			this.btnEndTurn.Click += new System.EventHandler(this.btnEndTurn_Click);
			// 
			// trackBarZoom
			// 
			this.trackBarZoom.Location = new System.Drawing.Point(3, 6);
			this.trackBarZoom.Name = "trackBarZoom";
			this.trackBarZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.trackBarZoom.Size = new System.Drawing.Size(45, 73);
			this.trackBarZoom.TabIndex = 0;
			this.trackBarZoom.Value = 4;
			this.trackBarZoom.Scroll += new System.EventHandler(this.trackBarZoom_Scroll);
			// 
			// timerAnimacija
			// 
			this.timerAnimacija.Enabled = true;
			this.timerAnimacija.Interval = 30;
			this.timerAnimacija.Tick += new System.EventHandler(this.timerAnimacija_Tick);
			// 
			// pnlDesno
			// 
			this.pnlDesno.Controls.Add(this.tabCtrlDesno);
			this.pnlDesno.Controls.Add(this.pnlDesnoGore);
			this.pnlDesno.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlDesno.Location = new System.Drawing.Point(484, 0);
			this.pnlDesno.Name = "pnlDesno";
			this.pnlDesno.Size = new System.Drawing.Size(200, 562);
			this.pnlDesno.TabIndex = 1;
			// 
			// pnlDesnoGore
			// 
			this.pnlDesnoGore.Controls.Add(this.lblBrojKruga);
			this.pnlDesnoGore.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDesnoGore.Location = new System.Drawing.Point(0, 0);
			this.pnlDesnoGore.Name = "pnlDesnoGore";
			this.pnlDesnoGore.Size = new System.Drawing.Size(200, 31);
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
			// pnlKomande
			// 
			this.pnlKomande.Controls.Add(this.btnPoruke);
			this.pnlKomande.Controls.Add(this.btnFlote);
			this.pnlKomande.Controls.Add(this.pnlKomandeDesno);
			this.pnlKomande.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlKomande.Location = new System.Drawing.Point(0, 479);
			this.pnlKomande.Name = "pnlKomande";
			this.pnlKomande.Size = new System.Drawing.Size(484, 83);
			this.pnlKomande.TabIndex = 1;
			// 
			// btnPoruke
			// 
			this.btnPoruke.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPoruke.Location = new System.Drawing.Point(90, 6);
			this.btnPoruke.Name = "btnPoruke";
			this.btnPoruke.Size = new System.Drawing.Size(75, 23);
			this.btnPoruke.TabIndex = 3;
			this.btnPoruke.Text = "&Novosti";
			this.btnPoruke.UseVisualStyleBackColor = true;
			this.btnPoruke.Click += new System.EventHandler(this.btnPoruke_Click);
			// 
			// btnFlote
			// 
			this.btnFlote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFlote.Location = new System.Drawing.Point(90, 35);
			this.btnFlote.Name = "btnFlote";
			this.btnFlote.Size = new System.Drawing.Size(75, 23);
			this.btnFlote.TabIndex = 4;
			this.btnFlote.Text = "&Flote";
			this.btnFlote.UseVisualStyleBackColor = true;
			this.btnFlote.Click += new System.EventHandler(this.btnFlote_Click);
			// 
			// pnlKomandeDesno
			// 
			this.pnlKomandeDesno.Controls.Add(this.btnUcitaj);
			this.pnlKomandeDesno.Controls.Add(this.btnSpremi);
			this.pnlKomandeDesno.Controls.Add(this.btnTech);
			this.pnlKomandeDesno.Controls.Add(this.trackBarZoom);
			this.pnlKomandeDesno.Controls.Add(this.btnEndTurn);
			this.pnlKomandeDesno.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlKomandeDesno.Location = new System.Drawing.Point(171, 0);
			this.pnlKomandeDesno.Name = "pnlKomandeDesno";
			this.pnlKomandeDesno.Size = new System.Drawing.Size(313, 83);
			this.pnlKomandeDesno.TabIndex = 0;
			// 
			// btnUcitaj
			// 
			this.btnUcitaj.Location = new System.Drawing.Point(166, 53);
			this.btnUcitaj.Name = "btnUcitaj";
			this.btnUcitaj.Size = new System.Drawing.Size(75, 23);
			this.btnUcitaj.TabIndex = 3;
			this.btnUcitaj.Text = "Učitaj";
			this.btnUcitaj.UseVisualStyleBackColor = true;
			this.btnUcitaj.Click += new System.EventHandler(this.btnUcitaj_Click);
			// 
			// btnSpremi
			// 
			this.btnSpremi.Location = new System.Drawing.Point(166, 19);
			this.btnSpremi.Name = "btnSpremi";
			this.btnSpremi.Size = new System.Drawing.Size(75, 23);
			this.btnSpremi.TabIndex = 1;
			this.btnSpremi.Text = "Spremi";
			this.btnSpremi.UseVisualStyleBackColor = true;
			this.btnSpremi.Click += new System.EventHandler(this.btnSpremi_Click);
			// 
			// btnTech
			// 
			this.btnTech.Location = new System.Drawing.Point(54, 6);
			this.btnTech.Name = "btnTech";
			this.btnTech.Size = new System.Drawing.Size(85, 23);
			this.btnTech.TabIndex = 2;
			this.btnTech.Text = "&Tehnologije";
			this.btnTech.UseVisualStyleBackColor = true;
			this.btnTech.Click += new System.EventHandler(this.btnTech_Click);
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
			// FormIgra
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(684, 562);
			this.Controls.Add(this.pnlMapa);
			this.Controls.Add(this.pnlKomande);
			this.Controls.Add(this.pnlDesno);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormIgra";
			this.Text = "Zvjezdojedac";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.frmIgra_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormIgra_FormClosed);
			this.tabCtrlDesno.ResumeLayout(false);
			this.tabPageZvijezda.ResumeLayout(false);
			this.pnlOpisZvjezde.ResumeLayout(false);
			this.pnlOpisZvjezde.PerformLayout();
			this.tabPageKolonija.ResumeLayout(false);
			this.tabPageKolonija.PerformLayout();
			this.groupPoStan.ResumeLayout(false);
			this.groupPoStan.PerformLayout();
			this.groupCivGradnja.ResumeLayout(false);
			this.groupCivGradnja.PerformLayout();
			this.groupVojGradnja.ResumeLayout(false);
			this.groupVojGradnja.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSlikaPlaneta)).EndInit();
			this.tabPageFlote.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.pnlMapa.ResumeLayout(false);
			this.pnlMapa.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picMapa)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).EndInit();
			this.pnlDesno.ResumeLayout(false);
			this.pnlDesnoGore.ResumeLayout(false);
			this.pnlDesnoGore.PerformLayout();
			this.pnlKomande.ResumeLayout(false);
			this.pnlKomandeDesno.ResumeLayout(false);
			this.pnlKomandeDesno.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabCtrlDesno;
		private System.Windows.Forms.TabPage tabPageZvijezda;
		private System.Windows.Forms.Panel pnlMapa;
		private System.Windows.Forms.PictureBox picMapa;
		private System.Windows.Forms.Timer timerAnimacija;
		private System.Windows.Forms.TrackBar trackBarZoom;
		private System.Windows.Forms.Panel pnlOpisZvjezde;
		private System.Windows.Forms.ListView listViewPlaneti;
		private System.Windows.Forms.Label lblImeZvjezde;
		private System.Windows.Forms.TabPage tabPageKolonija;
		private System.Windows.Forms.Label lblImePlaneta;
		private System.Windows.Forms.Label lblPopulacija;
		private System.Windows.Forms.Button btnEndTurn;
		private System.Windows.Forms.Panel pnlDesno;
		private System.Windows.Forms.Label lblBrojKruga;
		private System.Windows.Forms.Panel pnlDesnoGore;
		private System.Windows.Forms.Button btnPlanetInfo;
		private System.Windows.Forms.PictureBox picSlikaPlaneta;
		private System.Windows.Forms.Panel pnlKomande;
		private System.Windows.Forms.Panel pnlKomandeDesno;
		private System.Windows.Forms.HScrollBar hscrCivilnaIndustrija;
		private System.Windows.Forms.Label lblRazvoj;
		private System.Windows.Forms.Label lblCivilnaIndustrija;
		private System.Windows.Forms.Button btnCivilnaGradnja;
        private System.Windows.Forms.Button btnTech;
		private System.Windows.Forms.Button btnPoruke;
		private System.Windows.Forms.HScrollBar hscrVojnaIndustrija;
		private System.Windows.Forms.Button btnVojnaGradnja;
		private System.Windows.Forms.Label lblVojnaGradnja;
		private System.Windows.Forms.Label lblProcjenaVojneGradnje;
		private System.Windows.Forms.Label lblProcjenaCivilneGradnje;
		private System.Windows.Forms.GroupBox groupCivGradnja;
		private System.Windows.Forms.GroupBox groupVojGradnja;
		private System.Windows.Forms.Button btnFlote;
		private System.Windows.Forms.TabPage tabPageFlote;
		private System.Windows.Forms.TreeView tvFlota;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnSpremi;
		private System.Windows.Forms.Button btnUcitaj;
		private System.Windows.Forms.GroupBox groupPoStan;
		private System.Windows.Forms.Label lblHranaPoStan;
		private System.Windows.Forms.Label lblRudePoStan;
		private System.Windows.Forms.Label lblIndustrijaPoStan;
		private System.Windows.Forms.Label lblOdrzavanjePoStan;
		private System.Windows.Forms.Label lblRazvojPoStan;
		private System.Windows.Forms.Button btnPrimAkcijaBroda;
		private System.Windows.Forms.Button btnFlotaPokret;
		private System.Windows.Forms.Button btnSekAkcija;
	}
}