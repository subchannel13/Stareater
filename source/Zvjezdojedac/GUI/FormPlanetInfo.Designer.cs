namespace Prototip
{
	partial class FormPlanetInfo
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlanetInfo));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblZracenje = new System.Windows.Forms.Label();
			this.lblAtmoTemperatura = new System.Windows.Forms.Label();
			this.lblGravitacija = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblAtmKvaliteta = new System.Windows.Forms.Label();
			this.lblAtmGustoca = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblRadnaMjesta = new System.Windows.Forms.Label();
			this.lblPopMax = new System.Windows.Forms.Label();
			this.lblPopDelta = new System.Windows.Forms.Label();
			this.lblPopBr = new System.Windows.Forms.Label();
			this.btnZatvori = new System.Windows.Forms.Button();
			this.lblProcjenaVojneGradnje = new System.Windows.Forms.Label();
			this.lblProcjenaCivilneGradnje = new System.Windows.Forms.Label();
			this.lblVojnaIndustrija = new System.Windows.Forms.Label();
			this.hscrVojnaIndustrija = new System.Windows.Forms.HScrollBar();
			this.btnVojnaGradnja = new System.Windows.Forms.Button();
			this.btnCivilnaGradnja = new System.Windows.Forms.Button();
			this.lblRazvoj = new System.Windows.Forms.Label();
			this.lblCivilnaIndustrija = new System.Windows.Forms.Label();
			this.hscrCivilnaIndustrija = new System.Windows.Forms.HScrollBar();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.lblPoFarmeru = new System.Windows.Forms.Label();
			this.lblBrFarmera = new System.Windows.Forms.Label();
			this.lblBrRudara = new System.Windows.Forms.Label();
			this.picSlika = new System.Windows.Forms.PictureBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.lblMinDubina = new System.Windows.Forms.Label();
			this.lblMinPovrsina = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblPoRudaru = new System.Windows.Forms.Label();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.lblOdrzavanjeZgrada = new System.Windows.Forms.Label();
			this.lblOdrzavanjeUkupno = new System.Windows.Forms.Label();
			this.lblOdrzavanjeTempAtm = new System.Windows.Forms.Label();
			this.lblOdrzavanjeKvalAtm = new System.Windows.Forms.Label();
			this.lblOdrzavanjeGustAtm = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblOdrzavanjeZrac = new System.Windows.Forms.Label();
			this.lblOdrzavanjeGrav = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.radKolicina = new System.Windows.Forms.RadioButton();
			this.radPostotak = new System.Windows.Forms.RadioButton();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageProizvodnja = new System.Windows.Forms.TabPage();
			this.lblKoefOrbitalne = new System.Windows.Forms.Label();
			this.tabPageZgrade = new System.Windows.Forms.TabPage();
			this.lblZgradaInfo = new System.Windows.Forms.Label();
			this.picZgrada = new System.Windows.Forms.PictureBox();
			this.lstZgrade = new System.Windows.Forms.ListBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSlika)).BeginInit();
			this.groupBox5.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabPageProizvodnja.SuspendLayout();
			this.tabPageZgrade.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picZgrada)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblZracenje);
			this.groupBox1.Controls.Add(this.lblAtmoTemperatura);
			this.groupBox1.Controls.Add(this.lblGravitacija);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.lblAtmKvaliteta);
			this.groupBox1.Controls.Add(this.lblAtmGustoca);
			this.groupBox1.Location = new System.Drawing.Point(227, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(110, 109);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Uvjeti";
			// 
			// lblZracenje
			// 
			this.lblZracenje.AutoSize = true;
			this.lblZracenje.Location = new System.Drawing.Point(6, 29);
			this.lblZracenje.Name = "lblZracenje";
			this.lblZracenje.Size = new System.Drawing.Size(73, 13);
			this.lblZracenje.TabIndex = 5;
			this.lblZracenje.Text = "Zracenje: xx.x";
			// 
			// lblAtmoTemperatura
			// 
			this.lblAtmoTemperatura.AutoSize = true;
			this.lblAtmoTemperatura.Location = new System.Drawing.Point(6, 94);
			this.lblAtmoTemperatura.Name = "lblAtmoTemperatura";
			this.lblAtmoTemperatura.Size = new System.Drawing.Size(97, 13);
			this.lblAtmoTemperatura.TabIndex = 4;
			this.lblAtmoTemperatura.Text = "Temperatura: ?xx.x";
			// 
			// lblGravitacija
			// 
			this.lblGravitacija.AutoSize = true;
			this.lblGravitacija.Location = new System.Drawing.Point(6, 16);
			this.lblGravitacija.Name = "lblGravitacija";
			this.lblGravitacija.Size = new System.Drawing.Size(81, 13);
			this.lblGravitacija.TabIndex = 3;
			this.lblGravitacija.Text = "Gravitacija: xx.x";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 55);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(57, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Atmosfera:";
			// 
			// lblAtmKvaliteta
			// 
			this.lblAtmKvaliteta.AutoSize = true;
			this.lblAtmKvaliteta.Location = new System.Drawing.Point(6, 81);
			this.lblAtmKvaliteta.Name = "lblAtmKvaliteta";
			this.lblAtmKvaliteta.Size = new System.Drawing.Size(72, 13);
			this.lblAtmKvaliteta.TabIndex = 1;
			this.lblAtmKvaliteta.Text = "Kvaliteta: xx%";
			// 
			// lblAtmGustoca
			// 
			this.lblAtmGustoca.AutoSize = true;
			this.lblAtmGustoca.Location = new System.Drawing.Point(6, 68);
			this.lblAtmGustoca.Name = "lblAtmGustoca";
			this.lblAtmGustoca.Size = new System.Drawing.Size(71, 13);
			this.lblAtmGustoca.TabIndex = 0;
			this.lblAtmGustoca.Text = "Gustoća: xx.x";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lblRadnaMjesta);
			this.groupBox2.Controls.Add(this.lblPopMax);
			this.groupBox2.Controls.Add(this.lblPopDelta);
			this.groupBox2.Controls.Add(this.lblPopBr);
			this.groupBox2.Location = new System.Drawing.Point(82, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(136, 88);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Populacija";
			// 
			// lblRadnaMjesta
			// 
			this.lblRadnaMjesta.AutoSize = true;
			this.lblRadnaMjesta.Location = new System.Drawing.Point(6, 64);
			this.lblRadnaMjesta.Name = "lblRadnaMjesta";
			this.lblRadnaMjesta.Size = new System.Drawing.Size(124, 13);
			this.lblRadnaMjesta.TabIndex = 3;
			this.lblRadnaMjesta.Text = "Br. radnih mjesta: xx.xx X";
			// 
			// lblPopMax
			// 
			this.lblPopMax.AutoSize = true;
			this.lblPopMax.Location = new System.Drawing.Point(6, 42);
			this.lblPopMax.Name = "lblPopMax";
			this.lblPopMax.Size = new System.Drawing.Size(96, 13);
			this.lblPopMax.TabIndex = 2;
			this.lblPopMax.Text = "Maksimum: xx.xx X";
			// 
			// lblPopDelta
			// 
			this.lblPopDelta.AutoSize = true;
			this.lblPopDelta.Location = new System.Drawing.Point(6, 29);
			this.lblPopDelta.Name = "lblPopDelta";
			this.lblPopDelta.Size = new System.Drawing.Size(96, 13);
			this.lblPopDelta.TabIndex = 1;
			this.lblPopDelta.Text = "Promjena: ?xx.xx X";
			// 
			// lblPopBr
			// 
			this.lblPopBr.AutoSize = true;
			this.lblPopBr.Location = new System.Drawing.Point(6, 16);
			this.lblPopBr.Name = "lblPopBr";
			this.lblPopBr.Size = new System.Drawing.Size(114, 13);
			this.lblPopBr.TabIndex = 0;
			this.lblPopBr.Text = "Br. stanovnika: xx.xx X";
			// 
			// btnZatvori
			// 
			this.btnZatvori.Location = new System.Drawing.Point(47, 359);
			this.btnZatvori.Name = "btnZatvori";
			this.btnZatvori.Size = new System.Drawing.Size(75, 23);
			this.btnZatvori.TabIndex = 2;
			this.btnZatvori.Text = "&Zatvori";
			this.btnZatvori.UseVisualStyleBackColor = true;
			this.btnZatvori.Click += new System.EventHandler(this.btnZatvori_Click);
			// 
			// lblProcjenaVojneGradnje
			// 
			this.lblProcjenaVojneGradnje.AutoSize = true;
			this.lblProcjenaVojneGradnje.Location = new System.Drawing.Point(153, 140);
			this.lblProcjenaVojneGradnje.Name = "lblProcjenaVojneGradnje";
			this.lblProcjenaVojneGradnje.Size = new System.Drawing.Size(82, 13);
			this.lblProcjenaVojneGradnje.TabIndex = 27;
			this.lblProcjenaVojneGradnje.Text = "xx.xx X krugova";
			// 
			// lblProcjenaCivilneGradnje
			// 
			this.lblProcjenaCivilneGradnje.AutoSize = true;
			this.lblProcjenaCivilneGradnje.Location = new System.Drawing.Point(31, 137);
			this.lblProcjenaCivilneGradnje.Name = "lblProcjenaCivilneGradnje";
			this.lblProcjenaCivilneGradnje.Size = new System.Drawing.Size(82, 13);
			this.lblProcjenaCivilneGradnje.TabIndex = 26;
			this.lblProcjenaCivilneGradnje.Text = "xx.xx X krugova";
			// 
			// lblVojnaIndustrija
			// 
			this.lblVojnaIndustrija.AutoSize = true;
			this.lblVojnaIndustrija.Location = new System.Drawing.Point(153, 127);
			this.lblVojnaIndustrija.Name = "lblVojnaIndustrija";
			this.lblVojnaIndustrija.Size = new System.Drawing.Size(40, 13);
			this.lblVojnaIndustrija.TabIndex = 25;
			this.lblVojnaIndustrija.Text = "xx.xx X";
			// 
			// hscrVojnaIndustrija
			// 
			this.hscrVojnaIndustrija.LargeChange = 1;
			this.hscrVojnaIndustrija.Location = new System.Drawing.Point(156, 106);
			this.hscrVojnaIndustrija.Name = "hscrVojnaIndustrija";
			this.hscrVojnaIndustrija.Size = new System.Drawing.Size(109, 18);
			this.hscrVojnaIndustrija.TabIndex = 23;
			this.hscrVojnaIndustrija.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrVojnaIndustrija_Scroll);
			// 
			// btnVojnaGradnja
			// 
			this.btnVojnaGradnja.Location = new System.Drawing.Point(168, 19);
			this.btnVojnaGradnja.Name = "btnVojnaGradnja";
			this.btnVojnaGradnja.Size = new System.Drawing.Size(84, 84);
			this.btnVojnaGradnja.TabIndex = 22;
			this.btnVojnaGradnja.Text = "slika vojne zgrade";
			this.btnVojnaGradnja.UseVisualStyleBackColor = true;
			this.btnVojnaGradnja.Click += new System.EventHandler(this.btnVojnaGradnja_Click);
			// 
			// btnCivilnaGradnja
			// 
			this.btnCivilnaGradnja.Location = new System.Drawing.Point(46, 19);
			this.btnCivilnaGradnja.Name = "btnCivilnaGradnja";
			this.btnCivilnaGradnja.Size = new System.Drawing.Size(84, 84);
			this.btnCivilnaGradnja.TabIndex = 21;
			this.btnCivilnaGradnja.Text = "slika civilne zgrade";
			this.btnCivilnaGradnja.UseVisualStyleBackColor = true;
			this.btnCivilnaGradnja.Click += new System.EventHandler(this.btnCivilnaGradnja_Click);
			// 
			// lblRazvoj
			// 
			this.lblRazvoj.AutoSize = true;
			this.lblRazvoj.Location = new System.Drawing.Point(31, 186);
			this.lblRazvoj.Name = "lblRazvoj";
			this.lblRazvoj.Size = new System.Drawing.Size(79, 13);
			this.lblRazvoj.TabIndex = 18;
			this.lblRazvoj.Text = "Razvoj: xx.xx X";
			// 
			// lblCivilnaIndustrija
			// 
			this.lblCivilnaIndustrija.AutoSize = true;
			this.lblCivilnaIndustrija.Location = new System.Drawing.Point(31, 124);
			this.lblCivilnaIndustrija.Name = "lblCivilnaIndustrija";
			this.lblCivilnaIndustrija.Size = new System.Drawing.Size(40, 13);
			this.lblCivilnaIndustrija.TabIndex = 17;
			this.lblCivilnaIndustrija.Text = "xx.xx X";
			// 
			// hscrCivilnaIndustrija
			// 
			this.hscrCivilnaIndustrija.LargeChange = 1;
			this.hscrCivilnaIndustrija.Location = new System.Drawing.Point(34, 106);
			this.hscrCivilnaIndustrija.Name = "hscrCivilnaIndustrija";
			this.hscrCivilnaIndustrija.Size = new System.Drawing.Size(109, 18);
			this.hscrCivilnaIndustrija.TabIndex = 16;
			this.hscrCivilnaIndustrija.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrIndustrijaRazvoj_Scroll);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.lblPoFarmeru);
			this.groupBox4.Controls.Add(this.lblBrFarmera);
			this.groupBox4.Location = new System.Drawing.Point(12, 141);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(110, 54);
			this.groupBox4.TabIndex = 4;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Hrana:";
			// 
			// lblPoFarmeru
			// 
			this.lblPoFarmeru.AutoSize = true;
			this.lblPoFarmeru.Location = new System.Drawing.Point(6, 17);
			this.lblPoFarmeru.Name = "lblPoFarmeru";
			this.lblPoFarmeru.Size = new System.Drawing.Size(82, 13);
			this.lblPoFarmeru.TabIndex = 1;
			this.lblPoFarmeru.Text = "Po farmeru: xx.x";
			// 
			// lblBrFarmera
			// 
			this.lblBrFarmera.AutoSize = true;
			this.lblBrFarmera.Location = new System.Drawing.Point(6, 30);
			this.lblBrFarmera.Name = "lblBrFarmera";
			this.lblBrFarmera.Size = new System.Drawing.Size(97, 13);
			this.lblBrFarmera.TabIndex = 0;
			this.lblBrFarmera.Text = "Br. farmera: xx.xx X";
			// 
			// lblBrRudara
			// 
			this.lblBrRudara.AutoSize = true;
			this.lblBrRudara.Location = new System.Drawing.Point(6, 29);
			this.lblBrRudara.Name = "lblBrRudara";
			this.lblBrRudara.Size = new System.Drawing.Size(92, 13);
			this.lblBrRudara.TabIndex = 1;
			this.lblBrRudara.Text = "Br. rudara: xx.xx X";
			// 
			// picSlika
			// 
			this.picSlika.Location = new System.Drawing.Point(12, 12);
			this.picSlika.Name = "picSlika";
			this.picSlika.Size = new System.Drawing.Size(64, 64);
			this.picSlika.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picSlika.TabIndex = 5;
			this.picSlika.TabStop = false;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.lblMinDubina);
			this.groupBox5.Controls.Add(this.lblMinPovrsina);
			this.groupBox5.Controls.Add(this.label2);
			this.groupBox5.Controls.Add(this.lblPoRudaru);
			this.groupBox5.Controls.Add(this.lblBrRudara);
			this.groupBox5.Location = new System.Drawing.Point(12, 201);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(110, 98);
			this.groupBox5.TabIndex = 6;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Rude:";
			// 
			// lblMinDubina
			// 
			this.lblMinDubina.AutoSize = true;
			this.lblMinDubina.Location = new System.Drawing.Point(6, 79);
			this.lblMinDubina.Name = "lblMinDubina";
			this.lblMinDubina.Size = new System.Drawing.Size(65, 13);
			this.lblMinDubina.TabIndex = 5;
			this.lblMinDubina.Text = "Dubina: xx.x";
			// 
			// lblMinPovrsina
			// 
			this.lblMinPovrsina.AutoSize = true;
			this.lblMinPovrsina.Location = new System.Drawing.Point(6, 66);
			this.lblMinPovrsina.Name = "lblMinPovrsina";
			this.lblMinPovrsina.Size = new System.Drawing.Size(72, 13);
			this.lblMinPovrsina.TabIndex = 4;
			this.lblMinPovrsina.Text = "Površina: xx.x";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 53);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Potencijal:";
			// 
			// lblPoRudaru
			// 
			this.lblPoRudaru.AutoSize = true;
			this.lblPoRudaru.Location = new System.Drawing.Point(6, 16);
			this.lblPoRudaru.Name = "lblPoRudaru";
			this.lblPoRudaru.Size = new System.Drawing.Size(77, 13);
			this.lblPoRudaru.TabIndex = 2;
			this.lblPoRudaru.Text = "Po rudaru: xx.x";
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.lblOdrzavanjeZgrada);
			this.groupBox6.Controls.Add(this.lblOdrzavanjeUkupno);
			this.groupBox6.Controls.Add(this.lblOdrzavanjeTempAtm);
			this.groupBox6.Controls.Add(this.lblOdrzavanjeKvalAtm);
			this.groupBox6.Controls.Add(this.lblOdrzavanjeGustAtm);
			this.groupBox6.Controls.Add(this.label5);
			this.groupBox6.Controls.Add(this.lblOdrzavanjeZrac);
			this.groupBox6.Controls.Add(this.lblOdrzavanjeGrav);
			this.groupBox6.Location = new System.Drawing.Point(343, 12);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(216, 109);
			this.groupBox6.TabIndex = 7;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Održavanje";
			// 
			// lblOdrzavanjeZgrada
			// 
			this.lblOdrzavanjeZgrada.AutoSize = true;
			this.lblOdrzavanjeZgrada.Location = new System.Drawing.Point(109, 16);
			this.lblOdrzavanjeZgrada.Name = "lblOdrzavanjeZgrada";
			this.lblOdrzavanjeZgrada.Size = new System.Drawing.Size(80, 13);
			this.lblOdrzavanjeZgrada.TabIndex = 7;
			this.lblOdrzavanjeZgrada.Text = "Zgrade: xx.xx X";
			// 
			// lblOdrzavanjeUkupno
			// 
			this.lblOdrzavanjeUkupno.AutoSize = true;
			this.lblOdrzavanjeUkupno.Location = new System.Drawing.Point(109, 55);
			this.lblOdrzavanjeUkupno.Name = "lblOdrzavanjeUkupno";
			this.lblOdrzavanjeUkupno.Size = new System.Drawing.Size(101, 13);
			this.lblOdrzavanjeUkupno.TabIndex = 6;
			this.lblOdrzavanjeUkupno.Text = "Ukupno: xx.xx X ind";
			// 
			// lblOdrzavanjeTempAtm
			// 
			this.lblOdrzavanjeTempAtm.AutoSize = true;
			this.lblOdrzavanjeTempAtm.Location = new System.Drawing.Point(6, 94);
			this.lblOdrzavanjeTempAtm.Name = "lblOdrzavanjeTempAtm";
			this.lblOdrzavanjeTempAtm.Size = new System.Drawing.Size(106, 13);
			this.lblOdrzavanjeTempAtm.TabIndex = 5;
			this.lblOdrzavanjeTempAtm.Text = "Temperatura: xx.xx X";
			// 
			// lblOdrzavanjeKvalAtm
			// 
			this.lblOdrzavanjeKvalAtm.AutoSize = true;
			this.lblOdrzavanjeKvalAtm.Location = new System.Drawing.Point(6, 81);
			this.lblOdrzavanjeKvalAtm.Name = "lblOdrzavanjeKvalAtm";
			this.lblOdrzavanjeKvalAtm.Size = new System.Drawing.Size(87, 13);
			this.lblOdrzavanjeKvalAtm.TabIndex = 4;
			this.lblOdrzavanjeKvalAtm.Text = "Kvaliteta: xx.xx X";
			// 
			// lblOdrzavanjeGustAtm
			// 
			this.lblOdrzavanjeGustAtm.AutoSize = true;
			this.lblOdrzavanjeGustAtm.Location = new System.Drawing.Point(6, 68);
			this.lblOdrzavanjeGustAtm.Name = "lblOdrzavanjeGustAtm";
			this.lblOdrzavanjeGustAtm.Size = new System.Drawing.Size(86, 13);
			this.lblOdrzavanjeGustAtm.TabIndex = 3;
			this.lblOdrzavanjeGustAtm.Text = "Gustoća: xx.xx X";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 55);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(57, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "Atmosfera:";
			// 
			// lblOdrzavanjeZrac
			// 
			this.lblOdrzavanjeZrac.AutoSize = true;
			this.lblOdrzavanjeZrac.Location = new System.Drawing.Point(6, 29);
			this.lblOdrzavanjeZrac.Name = "lblOdrzavanjeZrac";
			this.lblOdrzavanjeZrac.Size = new System.Drawing.Size(88, 13);
			this.lblOdrzavanjeZrac.TabIndex = 1;
			this.lblOdrzavanjeZrac.Text = "Zračenje: xx.xx X";
			// 
			// lblOdrzavanjeGrav
			// 
			this.lblOdrzavanjeGrav.AutoSize = true;
			this.lblOdrzavanjeGrav.Location = new System.Drawing.Point(6, 16);
			this.lblOdrzavanjeGrav.Name = "lblOdrzavanjeGrav";
			this.lblOdrzavanjeGrav.Size = new System.Drawing.Size(96, 13);
			this.lblOdrzavanjeGrav.TabIndex = 0;
			this.lblOdrzavanjeGrav.Text = "Gravitacija: xx.xx X";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 79);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(39, 13);
			this.label7.TabIndex = 8;
			this.label7.Text = "Prikaz:";
			// 
			// radKolicina
			// 
			this.radKolicina.AutoSize = true;
			this.radKolicina.Location = new System.Drawing.Point(12, 95);
			this.radKolicina.Name = "radKolicina";
			this.radKolicina.Size = new System.Drawing.Size(62, 17);
			this.radKolicina.TabIndex = 9;
			this.radKolicina.TabStop = true;
			this.radKolicina.Text = "Količina";
			this.radKolicina.UseVisualStyleBackColor = true;
			this.radKolicina.CheckedChanged += new System.EventHandler(this.radKolicina_CheckedChanged);
			// 
			// radPostotak
			// 
			this.radPostotak.AutoSize = true;
			this.radPostotak.Location = new System.Drawing.Point(12, 118);
			this.radPostotak.Name = "radPostotak";
			this.radPostotak.Size = new System.Drawing.Size(67, 17);
			this.radPostotak.TabIndex = 10;
			this.radPostotak.TabStop = true;
			this.radPostotak.Text = "Postotak";
			this.radPostotak.UseVisualStyleBackColor = true;
			this.radPostotak.CheckedChanged += new System.EventHandler(this.radPostotak_CheckedChanged);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPageProizvodnja);
			this.tabControl.Controls.Add(this.tabPageZgrade);
			this.tabControl.Location = new System.Drawing.Point(128, 127);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(305, 228);
			this.tabControl.TabIndex = 11;
			// 
			// tabPageProizvodnja
			// 
			this.tabPageProizvodnja.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageProizvodnja.Controls.Add(this.lblKoefOrbitalne);
			this.tabPageProizvodnja.Controls.Add(this.lblProcjenaVojneGradnje);
			this.tabPageProizvodnja.Controls.Add(this.btnCivilnaGradnja);
			this.tabPageProizvodnja.Controls.Add(this.lblProcjenaCivilneGradnje);
			this.tabPageProizvodnja.Controls.Add(this.hscrCivilnaIndustrija);
			this.tabPageProizvodnja.Controls.Add(this.lblVojnaIndustrija);
			this.tabPageProizvodnja.Controls.Add(this.lblCivilnaIndustrija);
			this.tabPageProizvodnja.Controls.Add(this.hscrVojnaIndustrija);
			this.tabPageProizvodnja.Controls.Add(this.lblRazvoj);
			this.tabPageProizvodnja.Controls.Add(this.btnVojnaGradnja);
			this.tabPageProizvodnja.Location = new System.Drawing.Point(4, 22);
			this.tabPageProizvodnja.Name = "tabPageProizvodnja";
			this.tabPageProizvodnja.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageProizvodnja.Size = new System.Drawing.Size(297, 202);
			this.tabPageProizvodnja.TabIndex = 0;
			this.tabPageProizvodnja.Text = "Proizvodnja";
			// 
			// lblKoefOrbitalne
			// 
			this.lblKoefOrbitalne.AutoSize = true;
			this.lblKoefOrbitalne.Location = new System.Drawing.Point(31, 153);
			this.lblKoefOrbitalne.Name = "lblKoefOrbitalne";
			this.lblKoefOrbitalne.Size = new System.Drawing.Size(151, 13);
			this.lblKoefOrbitalne.TabIndex = 28;
			this.lblKoefOrbitalne.Text = "Cijena orbitalne gradnje: xxx.xx";
			// 
			// tabPageZgrade
			// 
			this.tabPageZgrade.BackColor = System.Drawing.SystemColors.Control;
			this.tabPageZgrade.Controls.Add(this.lblZgradaInfo);
			this.tabPageZgrade.Controls.Add(this.picZgrada);
			this.tabPageZgrade.Controls.Add(this.lstZgrade);
			this.tabPageZgrade.Location = new System.Drawing.Point(4, 22);
			this.tabPageZgrade.Name = "tabPageZgrade";
			this.tabPageZgrade.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageZgrade.Size = new System.Drawing.Size(297, 202);
			this.tabPageZgrade.TabIndex = 1;
			this.tabPageZgrade.Text = "Zgrade";
			// 
			// lblZgradaInfo
			// 
			this.lblZgradaInfo.AutoSize = true;
			this.lblZgradaInfo.Location = new System.Drawing.Point(132, 89);
			this.lblZgradaInfo.Name = "lblZgradaInfo";
			this.lblZgradaInfo.Size = new System.Drawing.Size(35, 13);
			this.lblZgradaInfo.TabIndex = 2;
			this.lblZgradaInfo.Text = "label3";
			// 
			// picZgrada
			// 
			this.picZgrada.Location = new System.Drawing.Point(132, 6);
			this.picZgrada.Name = "picZgrada";
			this.picZgrada.Size = new System.Drawing.Size(80, 80);
			this.picZgrada.TabIndex = 1;
			this.picZgrada.TabStop = false;
			// 
			// lstZgrade
			// 
			this.lstZgrade.FormattingEnabled = true;
			this.lstZgrade.Location = new System.Drawing.Point(6, 6);
			this.lstZgrade.Name = "lstZgrade";
			this.lstZgrade.Size = new System.Drawing.Size(120, 186);
			this.lstZgrade.Sorted = true;
			this.lstZgrade.TabIndex = 0;
			this.lstZgrade.SelectedIndexChanged += new System.EventHandler(this.lstZgrade_SelectedIndexChanged);
			// 
			// FormPlanetInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(564, 388);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.radPostotak);
			this.Controls.Add(this.radKolicina);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.picSlika);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.btnZatvori);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormPlanetInfo";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmPlanetInfo";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picSlika)).EndInit();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.tabControl.ResumeLayout(false);
			this.tabPageProizvodnja.ResumeLayout(false);
			this.tabPageProizvodnja.PerformLayout();
			this.tabPageZgrade.ResumeLayout(false);
			this.tabPageZgrade.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picZgrada)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label lblAtmGustoca;
		private System.Windows.Forms.Label lblAtmKvaliteta;
		private System.Windows.Forms.Label lblPopMax;
		private System.Windows.Forms.Label lblPopDelta;
		private System.Windows.Forms.Label lblPopBr;
		private System.Windows.Forms.Button btnZatvori;
		private System.Windows.Forms.Label lblGravitacija;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblRadnaMjesta;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.PictureBox picSlika;
		private System.Windows.Forms.Label lblBrRudara;
		private System.Windows.Forms.Label lblBrFarmera;
		private System.Windows.Forms.Label lblZracenje;
		private System.Windows.Forms.Label lblAtmoTemperatura;
		private System.Windows.Forms.Label lblPoFarmeru;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label lblPoRudaru;
		private System.Windows.Forms.Label lblMinDubina;
		private System.Windows.Forms.Label lblMinPovrsina;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblOdrzavanjeZrac;
		private System.Windows.Forms.Label lblOdrzavanjeGrav;
		private System.Windows.Forms.Label lblOdrzavanjeTempAtm;
		private System.Windows.Forms.Label lblOdrzavanjeKvalAtm;
		private System.Windows.Forms.Label lblOdrzavanjeGustAtm;
		private System.Windows.Forms.Label lblOdrzavanjeUkupno;
		private System.Windows.Forms.Label lblRazvoj;
		private System.Windows.Forms.Label lblCivilnaIndustrija;
		private System.Windows.Forms.HScrollBar hscrCivilnaIndustrija;
		private System.Windows.Forms.Button btnCivilnaGradnja;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.RadioButton radKolicina;
		private System.Windows.Forms.RadioButton radPostotak;
		private System.Windows.Forms.Label lblVojnaIndustrija;
        private System.Windows.Forms.HScrollBar hscrVojnaIndustrija;
        private System.Windows.Forms.Button btnVojnaGradnja;
		private System.Windows.Forms.Label lblProcjenaVojneGradnje;
		private System.Windows.Forms.Label lblProcjenaCivilneGradnje;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageProizvodnja;
		private System.Windows.Forms.TabPage tabPageZgrade;
		private System.Windows.Forms.ListBox lstZgrade;
		private System.Windows.Forms.Label lblZgradaInfo;
		private System.Windows.Forms.PictureBox picZgrada;
		private System.Windows.Forms.Label lblOdrzavanjeZgrada;
		private System.Windows.Forms.Label lblKoefOrbitalne;
	}
}