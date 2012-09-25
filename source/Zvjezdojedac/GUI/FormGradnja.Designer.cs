namespace Zvjezdojedac.GUI
{
	partial class FormGradnja
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGradnja));
			this.lstMoguceGradit = new System.Windows.Forms.ListBox();
			this.picSlikaZgrade = new System.Windows.Forms.PictureBox();
			this.lblZgradaInfo = new System.Windows.Forms.Label();
			this.lstRedGradnje = new System.Windows.Forms.ListBox();
			this.btnDodaj = new System.Windows.Forms.Button();
			this.lblMogucnosti = new System.Windows.Forms.Label();
			this.lblPopis = new System.Windows.Forms.Label();
			this.btnPrije = new System.Windows.Forms.Button();
			this.btnKasnije = new System.Windows.Forms.Button();
			this.btnUkloni = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picSlikaZgrade)).BeginInit();
			this.SuspendLayout();
			// 
			// lstMoguceGradit
			// 
			this.lstMoguceGradit.FormattingEnabled = true;
			this.lstMoguceGradit.Location = new System.Drawing.Point(12, 25);
			this.lstMoguceGradit.Name = "lstMoguceGradit";
			this.lstMoguceGradit.Size = new System.Drawing.Size(120, 95);
			this.lstMoguceGradit.TabIndex = 0;
			this.lstMoguceGradit.SelectedIndexChanged += new System.EventHandler(this.lstMoguceGradit_SelectedIndexChanged);
			this.lstMoguceGradit.DoubleClick += new System.EventHandler(this.lstMoguceGradit_DoubleClick);
			// 
			// picSlikaZgrade
			// 
			this.picSlikaZgrade.Location = new System.Drawing.Point(12, 126);
			this.picSlikaZgrade.Name = "picSlikaZgrade";
			this.picSlikaZgrade.Size = new System.Drawing.Size(80, 80);
			this.picSlikaZgrade.TabIndex = 1;
			this.picSlikaZgrade.TabStop = false;
			// 
			// lblZgradaInfo
			// 
			this.lblZgradaInfo.AutoSize = true;
			this.lblZgradaInfo.Location = new System.Drawing.Point(98, 126);
			this.lblZgradaInfo.Name = "lblZgradaInfo";
			this.lblZgradaInfo.Size = new System.Drawing.Size(35, 13);
			this.lblZgradaInfo.TabIndex = 2;
			this.lblZgradaInfo.Text = "label1";
			// 
			// lstRedGradnje
			// 
			this.lstRedGradnje.FormattingEnabled = true;
			this.lstRedGradnje.Location = new System.Drawing.Point(186, 25);
			this.lstRedGradnje.Name = "lstRedGradnje";
			this.lstRedGradnje.Size = new System.Drawing.Size(120, 95);
			this.lstRedGradnje.TabIndex = 3;
			this.lstRedGradnje.DoubleClick += new System.EventHandler(this.lstRedGradnje_DoubleClick);
			// 
			// btnDodaj
			// 
			this.btnDodaj.Location = new System.Drawing.Point(138, 62);
			this.btnDodaj.Name = "btnDodaj";
			this.btnDodaj.Size = new System.Drawing.Size(42, 23);
			this.btnDodaj.TabIndex = 4;
			this.btnDodaj.Text = ">>";
			this.btnDodaj.UseVisualStyleBackColor = true;
			this.btnDodaj.Click += new System.EventHandler(this.btnDodaj_Click);
			// 
			// lblMogucnosti
			// 
			this.lblMogucnosti.AutoSize = true;
			this.lblMogucnosti.Location = new System.Drawing.Point(9, 9);
			this.lblMogucnosti.Name = "lblMogucnosti";
			this.lblMogucnosti.Size = new System.Drawing.Size(103, 13);
			this.lblMogucnosti.TabIndex = 5;
			this.lblMogucnosti.Text = "Mogućnosti gradnje:";
			// 
			// lblPopis
			// 
			this.lblPopis.AutoSize = true;
			this.lblPopis.Location = new System.Drawing.Point(183, 9);
			this.lblPopis.Name = "lblPopis";
			this.lblPopis.Size = new System.Drawing.Size(88, 13);
			this.lblPopis.TabIndex = 6;
			this.lblPopis.Text = "Popis za gradnju:";
			// 
			// btnPrije
			// 
			this.btnPrije.Location = new System.Drawing.Point(312, 25);
			this.btnPrije.Name = "btnPrije";
			this.btnPrije.Size = new System.Drawing.Size(75, 23);
			this.btnPrije.TabIndex = 7;
			this.btnPrije.Text = "Prije";
			this.btnPrije.UseVisualStyleBackColor = true;
			this.btnPrije.Click += new System.EventHandler(this.btnPrije_Click);
			// 
			// btnKasnije
			// 
			this.btnKasnije.Location = new System.Drawing.Point(312, 54);
			this.btnKasnije.Name = "btnKasnije";
			this.btnKasnije.Size = new System.Drawing.Size(75, 23);
			this.btnKasnije.TabIndex = 8;
			this.btnKasnije.Text = "Kasnije";
			this.btnKasnije.UseVisualStyleBackColor = true;
			this.btnKasnije.Click += new System.EventHandler(this.btnKasnije_Click);
			// 
			// btnUkloni
			// 
			this.btnUkloni.Location = new System.Drawing.Point(312, 97);
			this.btnUkloni.Name = "btnUkloni";
			this.btnUkloni.Size = new System.Drawing.Size(75, 23);
			this.btnUkloni.TabIndex = 9;
			this.btnUkloni.Text = "Ukloni";
			this.btnUkloni.UseVisualStyleBackColor = true;
			this.btnUkloni.Click += new System.EventHandler(this.btnUkloni_Click);
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOk.Location = new System.Drawing.Point(119, 239);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 10;
			this.btnOk.Text = "U redu";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// FormGradnja
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOk;
			this.ClientSize = new System.Drawing.Size(399, 266);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnUkloni);
			this.Controls.Add(this.btnKasnije);
			this.Controls.Add(this.btnPrije);
			this.Controls.Add(this.lblPopis);
			this.Controls.Add(this.lblMogucnosti);
			this.Controls.Add(this.btnDodaj);
			this.Controls.Add(this.lstRedGradnje);
			this.Controls.Add(this.lblZgradaInfo);
			this.Controls.Add(this.picSlikaZgrade);
			this.Controls.Add(this.lstMoguceGradit);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormGradnja";
			this.ShowInTaskbar = false;
			this.Text = "Civilna gradnja";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGradnja_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.picSlikaZgrade)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lstMoguceGradit;
		private System.Windows.Forms.PictureBox picSlikaZgrade;
		private System.Windows.Forms.Label lblZgradaInfo;
		private System.Windows.Forms.ListBox lstRedGradnje;
		private System.Windows.Forms.Button btnDodaj;
		private System.Windows.Forms.Label lblMogucnosti;
		private System.Windows.Forms.Label lblPopis;
		private System.Windows.Forms.Button btnPrije;
		private System.Windows.Forms.Button btnKasnije;
		private System.Windows.Forms.Button btnUkloni;
		private System.Windows.Forms.Button btnOk;
	}
}