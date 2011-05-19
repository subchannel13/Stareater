namespace Zvjezdojedac.GUI
{
	partial class FormKolonizacija
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormKolonizacija));
			this.btnPrihvati = new System.Windows.Forms.Button();
			this.lstvPlaneti = new System.Windows.Forms.ListView();
			this.groupPoStan = new System.Windows.Forms.GroupBox();
			this.lblOdrzavanjePoStan = new System.Windows.Forms.Label();
			this.lblRazvojPoStan = new System.Windows.Forms.Label();
			this.lblIndustrijaPoStan = new System.Windows.Forms.Label();
			this.lblRudePoStan = new System.Windows.Forms.Label();
			this.lblHranaPoStan = new System.Windows.Forms.Label();
			this.hscrBrBrodova = new System.Windows.Forms.HScrollBar();
			this.lblBrBrodova = new System.Windows.Forms.Label();
			this.lblBrStanovnika = new System.Windows.Forms.Label();
			this.groupPlanet = new System.Windows.Forms.GroupBox();
			this.lblVelicina = new System.Windows.Forms.Label();
			this.lblKoefOrbitalne = new System.Windows.Forms.Label();
			this.lblZracenje = new System.Windows.Forms.Label();
			this.lblAtmoTemperatura = new System.Windows.Forms.Label();
			this.lblGravitacija = new System.Windows.Forms.Label();
			this.lblAtmosfera = new System.Windows.Forms.Label();
			this.lblAtmKvaliteta = new System.Windows.Forms.Label();
			this.lblAtmGustoca = new System.Windows.Forms.Label();
			this.lblBrRadnihMjesta = new System.Windows.Forms.Label();
			this.groupRude = new System.Windows.Forms.GroupBox();
			this.lblMinOstvareno = new System.Windows.Forms.Label();
			this.lblMinDubina = new System.Windows.Forms.Label();
			this.lblMinPovrsina = new System.Windows.Forms.Label();
			this.groupPoStan.SuspendLayout();
			this.groupPlanet.SuspendLayout();
			this.groupRude.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnPrihvati
			// 
			this.btnPrihvati.Location = new System.Drawing.Point(356, 343);
			this.btnPrihvati.Name = "btnPrihvati";
			this.btnPrihvati.Size = new System.Drawing.Size(75, 23);
			this.btnPrihvati.TabIndex = 0;
			this.btnPrihvati.Text = "Prihvati";
			this.btnPrihvati.UseVisualStyleBackColor = true;
			this.btnPrihvati.Click += new System.EventHandler(this.btnPrihvati_Click);
			// 
			// lstvPlaneti
			// 
			this.lstvPlaneti.AutoArrange = false;
			this.lstvPlaneti.BackColor = System.Drawing.Color.Black;
			this.lstvPlaneti.ForeColor = System.Drawing.Color.White;
			this.lstvPlaneti.FullRowSelect = true;
			this.lstvPlaneti.Location = new System.Drawing.Point(12, 12);
			this.lstvPlaneti.MultiSelect = false;
			this.lstvPlaneti.Name = "lstvPlaneti";
			this.lstvPlaneti.Size = new System.Drawing.Size(148, 354);
			this.lstvPlaneti.TabIndex = 1;
			this.lstvPlaneti.UseCompatibleStateImageBehavior = false;
			this.lstvPlaneti.View = System.Windows.Forms.View.Tile;
			this.lstvPlaneti.SelectedIndexChanged += new System.EventHandler(this.lstvPlaneti_SelectedIndexChanged);
			// 
			// groupPoStan
			// 
			this.groupPoStan.Controls.Add(this.lblOdrzavanjePoStan);
			this.groupPoStan.Controls.Add(this.lblRazvojPoStan);
			this.groupPoStan.Controls.Add(this.lblIndustrijaPoStan);
			this.groupPoStan.Controls.Add(this.lblRudePoStan);
			this.groupPoStan.Controls.Add(this.lblHranaPoStan);
			this.groupPoStan.Location = new System.Drawing.Point(166, 121);
			this.groupPoStan.Name = "groupPoStan";
			this.groupPoStan.Size = new System.Drawing.Size(149, 88);
			this.groupPoStan.TabIndex = 31;
			this.groupPoStan.TabStop = false;
			this.groupPoStan.Text = "Procjena (po stanovniku)";
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
			// hscrBrBrodova
			// 
			this.hscrBrBrodova.LargeChange = 1;
			this.hscrBrBrodova.Location = new System.Drawing.Point(163, 232);
			this.hscrBrBrodova.Name = "hscrBrBrodova";
			this.hscrBrBrodova.Size = new System.Drawing.Size(153, 17);
			this.hscrBrBrodova.TabIndex = 32;
			this.hscrBrBrodova.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrBrBrodova_Scroll);
			// 
			// lblBrBrodova
			// 
			this.lblBrBrodova.AutoSize = true;
			this.lblBrBrodova.Location = new System.Drawing.Point(172, 251);
			this.lblBrBrodova.Name = "lblBrBrodova";
			this.lblBrBrodova.Size = new System.Drawing.Size(101, 13);
			this.lblBrBrodova.TabIndex = 33;
			this.lblBrBrodova.Text = "Br. brodova: xx.xx X";
			// 
			// lblBrStanovnika
			// 
			this.lblBrStanovnika.AutoSize = true;
			this.lblBrStanovnika.Location = new System.Drawing.Point(172, 264);
			this.lblBrStanovnika.Name = "lblBrStanovnika";
			this.lblBrStanovnika.Size = new System.Drawing.Size(114, 13);
			this.lblBrStanovnika.TabIndex = 34;
			this.lblBrStanovnika.Text = "Br. stanovnika: xx.xx X";
			// 
			// groupPlanet
			// 
			this.groupPlanet.Controls.Add(this.lblVelicina);
			this.groupPlanet.Controls.Add(this.lblKoefOrbitalne);
			this.groupPlanet.Controls.Add(this.lblZracenje);
			this.groupPlanet.Controls.Add(this.lblAtmoTemperatura);
			this.groupPlanet.Controls.Add(this.lblGravitacija);
			this.groupPlanet.Controls.Add(this.lblAtmosfera);
			this.groupPlanet.Controls.Add(this.lblAtmKvaliteta);
			this.groupPlanet.Controls.Add(this.lblAtmGustoca);
			this.groupPlanet.Location = new System.Drawing.Point(166, 12);
			this.groupPlanet.Name = "groupPlanet";
			this.groupPlanet.Size = new System.Drawing.Size(265, 103);
			this.groupPlanet.TabIndex = 35;
			this.groupPlanet.TabStop = false;
			this.groupPlanet.Text = "Planet";
			// 
			// lblVelicina
			// 
			this.lblVelicina.AutoSize = true;
			this.lblVelicina.Location = new System.Drawing.Point(6, 16);
			this.lblVelicina.Name = "lblVelicina";
			this.lblVelicina.Size = new System.Drawing.Size(83, 13);
			this.lblVelicina.TabIndex = 29;
			this.lblVelicina.Text = "Veličina: xx.xx X";
			// 
			// lblKoefOrbitalne
			// 
			this.lblKoefOrbitalne.AutoSize = true;
			this.lblKoefOrbitalne.Location = new System.Drawing.Point(6, 81);
			this.lblKoefOrbitalne.Name = "lblKoefOrbitalne";
			this.lblKoefOrbitalne.Size = new System.Drawing.Size(151, 13);
			this.lblKoefOrbitalne.TabIndex = 28;
			this.lblKoefOrbitalne.Text = "Cijena orbitalne gradnje: xxx.xx";
			// 
			// lblZracenje
			// 
			this.lblZracenje.AutoSize = true;
			this.lblZracenje.Location = new System.Drawing.Point(6, 42);
			this.lblZracenje.Name = "lblZracenje";
			this.lblZracenje.Size = new System.Drawing.Size(73, 13);
			this.lblZracenje.TabIndex = 5;
			this.lblZracenje.Text = "Zracenje: xx.x";
			// 
			// lblAtmoTemperatura
			// 
			this.lblAtmoTemperatura.AutoSize = true;
			this.lblAtmoTemperatura.Location = new System.Drawing.Point(152, 55);
			this.lblAtmoTemperatura.Name = "lblAtmoTemperatura";
			this.lblAtmoTemperatura.Size = new System.Drawing.Size(97, 13);
			this.lblAtmoTemperatura.TabIndex = 4;
			this.lblAtmoTemperatura.Text = "Temperatura: ?xx.x";
			// 
			// lblGravitacija
			// 
			this.lblGravitacija.AutoSize = true;
			this.lblGravitacija.Location = new System.Drawing.Point(6, 29);
			this.lblGravitacija.Name = "lblGravitacija";
			this.lblGravitacija.Size = new System.Drawing.Size(81, 13);
			this.lblGravitacija.TabIndex = 3;
			this.lblGravitacija.Text = "Gravitacija: xx.x";
			// 
			// lblAtmosfera
			// 
			this.lblAtmosfera.AutoSize = true;
			this.lblAtmosfera.Location = new System.Drawing.Point(152, 16);
			this.lblAtmosfera.Name = "lblAtmosfera";
			this.lblAtmosfera.Size = new System.Drawing.Size(57, 13);
			this.lblAtmosfera.TabIndex = 2;
			this.lblAtmosfera.Text = "Atmosfera:";
			// 
			// lblAtmKvaliteta
			// 
			this.lblAtmKvaliteta.AutoSize = true;
			this.lblAtmKvaliteta.Location = new System.Drawing.Point(152, 42);
			this.lblAtmKvaliteta.Name = "lblAtmKvaliteta";
			this.lblAtmKvaliteta.Size = new System.Drawing.Size(72, 13);
			this.lblAtmKvaliteta.TabIndex = 1;
			this.lblAtmKvaliteta.Text = "Kvaliteta: xx%";
			// 
			// lblAtmGustoca
			// 
			this.lblAtmGustoca.AutoSize = true;
			this.lblAtmGustoca.Location = new System.Drawing.Point(152, 29);
			this.lblAtmGustoca.Name = "lblAtmGustoca";
			this.lblAtmGustoca.Size = new System.Drawing.Size(71, 13);
			this.lblAtmGustoca.TabIndex = 0;
			this.lblAtmGustoca.Text = "Gustoća: xx.x";
			// 
			// lblBrRadnihMjesta
			// 
			this.lblBrRadnihMjesta.AutoSize = true;
			this.lblBrRadnihMjesta.Location = new System.Drawing.Point(172, 277);
			this.lblBrRadnihMjesta.Name = "lblBrRadnihMjesta";
			this.lblBrRadnihMjesta.Size = new System.Drawing.Size(111, 13);
			this.lblBrRadnihMjesta.TabIndex = 36;
			this.lblBrRadnihMjesta.Text = "Radna mjesta: xx.xx X";
			// 
			// groupRude
			// 
			this.groupRude.Controls.Add(this.lblMinOstvareno);
			this.groupRude.Controls.Add(this.lblMinDubina);
			this.groupRude.Controls.Add(this.lblMinPovrsina);
			this.groupRude.Location = new System.Drawing.Point(321, 121);
			this.groupRude.Name = "groupRude";
			this.groupRude.Size = new System.Drawing.Size(110, 68);
			this.groupRude.TabIndex = 37;
			this.groupRude.TabStop = false;
			this.groupRude.Text = "Rude:";
			// 
			// lblMinOstvareno
			// 
			this.lblMinOstvareno.AutoSize = true;
			this.lblMinOstvareno.Location = new System.Drawing.Point(6, 42);
			this.lblMinOstvareno.Name = "lblMinOstvareno";
			this.lblMinOstvareno.Size = new System.Drawing.Size(80, 13);
			this.lblMinOstvareno.TabIndex = 6;
			this.lblMinOstvareno.Text = "Ostvareno: xx,x";
			// 
			// lblMinDubina
			// 
			this.lblMinDubina.AutoSize = true;
			this.lblMinDubina.Location = new System.Drawing.Point(6, 29);
			this.lblMinDubina.Name = "lblMinDubina";
			this.lblMinDubina.Size = new System.Drawing.Size(65, 13);
			this.lblMinDubina.TabIndex = 5;
			this.lblMinDubina.Text = "Dubina: xx.x";
			// 
			// lblMinPovrsina
			// 
			this.lblMinPovrsina.AutoSize = true;
			this.lblMinPovrsina.Location = new System.Drawing.Point(6, 16);
			this.lblMinPovrsina.Name = "lblMinPovrsina";
			this.lblMinPovrsina.Size = new System.Drawing.Size(72, 13);
			this.lblMinPovrsina.TabIndex = 4;
			this.lblMinPovrsina.Text = "Površina: xx.x";
			// 
			// FormKolonizacija
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(443, 378);
			this.Controls.Add(this.groupRude);
			this.Controls.Add(this.lblBrRadnihMjesta);
			this.Controls.Add(this.groupPlanet);
			this.Controls.Add(this.lblBrStanovnika);
			this.Controls.Add(this.lblBrBrodova);
			this.Controls.Add(this.hscrBrBrodova);
			this.Controls.Add(this.groupPoStan);
			this.Controls.Add(this.lstvPlaneti);
			this.Controls.Add(this.btnPrihvati);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormKolonizacija";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Kolonizacija";
			this.groupPoStan.ResumeLayout(false);
			this.groupPoStan.PerformLayout();
			this.groupPlanet.ResumeLayout(false);
			this.groupPlanet.PerformLayout();
			this.groupRude.ResumeLayout(false);
			this.groupRude.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnPrihvati;
		private System.Windows.Forms.ListView lstvPlaneti;
		private System.Windows.Forms.GroupBox groupPoStan;
		private System.Windows.Forms.Label lblOdrzavanjePoStan;
		private System.Windows.Forms.Label lblRazvojPoStan;
		private System.Windows.Forms.Label lblIndustrijaPoStan;
		private System.Windows.Forms.Label lblRudePoStan;
		private System.Windows.Forms.HScrollBar hscrBrBrodova;
		private System.Windows.Forms.Label lblHranaPoStan;
		private System.Windows.Forms.Label lblBrBrodova;
		private System.Windows.Forms.Label lblBrStanovnika;
		private System.Windows.Forms.GroupBox groupPlanet;
		private System.Windows.Forms.Label lblVelicina;
		private System.Windows.Forms.Label lblKoefOrbitalne;
		private System.Windows.Forms.Label lblZracenje;
		private System.Windows.Forms.Label lblAtmoTemperatura;
		private System.Windows.Forms.Label lblGravitacija;
		private System.Windows.Forms.Label lblAtmosfera;
		private System.Windows.Forms.Label lblAtmKvaliteta;
		private System.Windows.Forms.Label lblAtmGustoca;
		private System.Windows.Forms.Label lblBrRadnihMjesta;
		private System.Windows.Forms.GroupBox groupRude;
		private System.Windows.Forms.Label lblMinOstvareno;
		private System.Windows.Forms.Label lblMinDubina;
		private System.Windows.Forms.Label lblMinPovrsina;
	}
}