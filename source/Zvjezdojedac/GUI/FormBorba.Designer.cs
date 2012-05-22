namespace Zvjezdojedac.GUI
{
	partial class FormBorba
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
			this.pnlNapadac = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlObrana = new System.Windows.Forms.FlowLayoutPanel();
			this.picLokacija = new System.Windows.Forms.PictureBox();
			this.picSelectAll = new System.Windows.Forms.PictureBox();
			this.cpBranitelj = new Zvjezdojedac.GUI.CombatantPositions();
			this.cpNapadac = new Zvjezdojedac.GUI.CombatantPositions();
			this.listPositions = new System.Windows.Forms.ListBox();
			this.lblZapovijed = new System.Windows.Forms.Label();
			this.trackKolicina = new System.Windows.Forms.TrackBar();
			this.lblKolicina = new System.Windows.Forms.Label();
			this.cbPozicija = new System.Windows.Forms.ComboBox();
			this.btnPosalji = new System.Windows.Forms.Button();
			this.btnKrajKruga = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picLokacija)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picSelectAll)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackKolicina)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlNapadac
			// 
			this.pnlNapadac.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.pnlNapadac.Location = new System.Drawing.Point(12, 58);
			this.pnlNapadac.Name = "pnlNapadac";
			this.pnlNapadac.Size = new System.Drawing.Size(255, 325);
			this.pnlNapadac.TabIndex = 0;
			// 
			// pnlObrana
			// 
			this.pnlObrana.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlObrana.Location = new System.Drawing.Point(477, 58);
			this.pnlObrana.Name = "pnlObrana";
			this.pnlObrana.Size = new System.Drawing.Size(255, 325);
			this.pnlObrana.TabIndex = 1;
			// 
			// picLokacija
			// 
			this.picLokacija.Location = new System.Drawing.Point(12, 12);
			this.picLokacija.Name = "picLokacija";
			this.picLokacija.Size = new System.Drawing.Size(40, 40);
			this.picLokacija.TabIndex = 2;
			this.picLokacija.TabStop = false;
			// 
			// picSelectAll
			// 
			this.picSelectAll.BackColor = System.Drawing.Color.Black;
			this.picSelectAll.Location = new System.Drawing.Point(352, 12);
			this.picSelectAll.Name = "picSelectAll";
			this.picSelectAll.Size = new System.Drawing.Size(40, 40);
			this.picSelectAll.TabIndex = 5;
			this.picSelectAll.TabStop = false;
			this.picSelectAll.Click += new System.EventHandler(this.picSelectAll_Click);
			// 
			// cpBranitelj
			// 
			this.cpBranitelj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cpBranitelj.BackColor = System.Drawing.Color.Red;
			this.cpBranitelj.Interactive = false;
			this.cpBranitelj.Location = new System.Drawing.Point(477, 11);
			this.cpBranitelj.Name = "cpBranitelj";
			this.cpBranitelj.ReverseDirection = true;
			this.cpBranitelj.Size = new System.Drawing.Size(166, 40);
			this.cpBranitelj.TabIndex = 4;
			// 
			// cpNapadac
			// 
			this.cpNapadac.BackColor = System.Drawing.Color.Red;
			this.cpNapadac.Location = new System.Drawing.Point(101, 12);
			this.cpNapadac.Name = "cpNapadac";
			this.cpNapadac.Size = new System.Drawing.Size(166, 40);
			this.cpNapadac.TabIndex = 3;
			this.cpNapadac.OnPositionClick += new Zvjezdojedac.GUI.Events.ObjectEventArgs<System.Collections.Generic.ICollection<Zvjezdojedac.Igra.Bitka.Borac>>.Handler(this.cpNapadac_OnPositionClick);
			// 
			// listPositions
			// 
			this.listPositions.BackColor = System.Drawing.SystemColors.Control;
			this.listPositions.FormattingEnabled = true;
			this.listPositions.Location = new System.Drawing.Point(273, 58);
			this.listPositions.Name = "listPositions";
			this.listPositions.Size = new System.Drawing.Size(198, 121);
			this.listPositions.TabIndex = 6;
			this.listPositions.SelectedIndexChanged += new System.EventHandler(this.listPositions_SelectedIndexChanged);
			// 
			// lblZapovijed
			// 
			this.lblZapovijed.AutoSize = true;
			this.lblZapovijed.Location = new System.Drawing.Point(273, 182);
			this.lblZapovijed.Name = "lblZapovijed";
			this.lblZapovijed.Size = new System.Drawing.Size(41, 13);
			this.lblZapovijed.TabIndex = 7;
			this.lblZapovijed.Text = "Orders:";
			// 
			// trackKolicina
			// 
			this.trackKolicina.Location = new System.Drawing.Point(273, 198);
			this.trackKolicina.Minimum = 1;
			this.trackKolicina.Name = "trackKolicina";
			this.trackKolicina.Size = new System.Drawing.Size(198, 45);
			this.trackKolicina.TabIndex = 8;
			this.trackKolicina.Value = 1;
			this.trackKolicina.Scroll += new System.EventHandler(this.trackKolicina_Scroll);
			// 
			// lblKolicina
			// 
			this.lblKolicina.Location = new System.Drawing.Point(276, 230);
			this.lblKolicina.Name = "lblKolicina";
			this.lblKolicina.Size = new System.Drawing.Size(195, 23);
			this.lblKolicina.TabIndex = 9;
			this.lblKolicina.Text = "xx.xx X";
			this.lblKolicina.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// cbPozicija
			// 
			this.cbPozicija.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPozicija.FormattingEnabled = true;
			this.cbPozicija.Location = new System.Drawing.Point(273, 249);
			this.cbPozicija.Name = "cbPozicija";
			this.cbPozicija.Size = new System.Drawing.Size(198, 21);
			this.cbPozicija.TabIndex = 10;
			// 
			// btnPosalji
			// 
			this.btnPosalji.Location = new System.Drawing.Point(396, 276);
			this.btnPosalji.Name = "btnPosalji";
			this.btnPosalji.Size = new System.Drawing.Size(75, 23);
			this.btnPosalji.TabIndex = 11;
			this.btnPosalji.Text = "Send";
			this.btnPosalji.UseVisualStyleBackColor = true;
			this.btnPosalji.Click += new System.EventHandler(this.btnPosalji_Click);
			// 
			// btnKrajKruga
			// 
			this.btnKrajKruga.Location = new System.Drawing.Point(396, 360);
			this.btnKrajKruga.Name = "btnKrajKruga";
			this.btnKrajKruga.Size = new System.Drawing.Size(75, 23);
			this.btnKrajKruga.TabIndex = 12;
			this.btnKrajKruga.Text = "End turn";
			this.btnKrajKruga.UseVisualStyleBackColor = true;
			this.btnKrajKruga.Click += new System.EventHandler(this.btnKrajKruga_Click);
			// 
			// FormBorba
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(744, 395);
			this.Controls.Add(this.btnKrajKruga);
			this.Controls.Add(this.btnPosalji);
			this.Controls.Add(this.cbPozicija);
			this.Controls.Add(this.lblKolicina);
			this.Controls.Add(this.trackKolicina);
			this.Controls.Add(this.lblZapovijed);
			this.Controls.Add(this.listPositions);
			this.Controls.Add(this.picSelectAll);
			this.Controls.Add(this.cpBranitelj);
			this.Controls.Add(this.cpNapadac);
			this.Controls.Add(this.picLokacija);
			this.Controls.Add(this.pnlObrana);
			this.Controls.Add(this.pnlNapadac);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "FormBorba";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormBorba";
			((System.ComponentModel.ISupportInitialize)(this.picLokacija)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picSelectAll)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackKolicina)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel pnlNapadac;
		private System.Windows.Forms.FlowLayoutPanel pnlObrana;
		private System.Windows.Forms.PictureBox picLokacija;
		private CombatantPositions cpNapadac;
		private CombatantPositions cpBranitelj;
		private System.Windows.Forms.PictureBox picSelectAll;
		private System.Windows.Forms.ListBox listPositions;
		private System.Windows.Forms.Label lblZapovijed;
		private System.Windows.Forms.TrackBar trackKolicina;
		private System.Windows.Forms.Label lblKolicina;
		private System.Windows.Forms.ComboBox cbPozicija;
		private System.Windows.Forms.Button btnPosalji;
		private System.Windows.Forms.Button btnKrajKruga;
	}
}