namespace Zvjezdojedac.GUI
{
	partial class CombatantItem
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.picIkona = new System.Windows.Forms.PictureBox();
			this.lblNaziv = new System.Windows.Forms.Label();
			this.picStanje = new System.Windows.Forms.PictureBox();
			this.picPrikrivanje = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picIkona)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picStanje)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picPrikrivanje)).BeginInit();
			this.SuspendLayout();
			// 
			// picIkona
			// 
			this.picIkona.Location = new System.Drawing.Point(3, 3);
			this.picIkona.Name = "picIkona";
			this.picIkona.Size = new System.Drawing.Size(60, 40);
			this.picIkona.TabIndex = 0;
			this.picIkona.TabStop = false;
			this.picIkona.Click += new System.EventHandler(this.picIkona_Click);
			// 
			// lblNaziv
			// 
			this.lblNaziv.AutoSize = true;
			this.lblNaziv.Location = new System.Drawing.Point(69, 3);
			this.lblNaziv.Name = "lblNaziv";
			this.lblNaziv.Size = new System.Drawing.Size(35, 13);
			this.lblNaziv.TabIndex = 1;
			this.lblNaziv.Text = "label1";
			this.lblNaziv.Click += new System.EventHandler(this.lblNaziv_Click);
			// 
			// picStanje
			// 
			this.picStanje.Location = new System.Drawing.Point(69, 19);
			this.picStanje.Name = "picStanje";
			this.picStanje.Size = new System.Drawing.Size(150, 24);
			this.picStanje.TabIndex = 2;
			this.picStanje.TabStop = false;
			this.picStanje.Click += new System.EventHandler(this.picStanje_Click);
			// 
			// picPrikrivanje
			// 
			this.picPrikrivanje.Location = new System.Drawing.Point(225, 3);
			this.picPrikrivanje.Name = "picPrikrivanje";
			this.picPrikrivanje.Size = new System.Drawing.Size(20, 40);
			this.picPrikrivanje.TabIndex = 3;
			this.picPrikrivanje.TabStop = false;
			this.picPrikrivanje.Click += new System.EventHandler(this.picPrikrivanje_Click);
			// 
			// CombatantItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.picPrikrivanje);
			this.Controls.Add(this.picStanje);
			this.Controls.Add(this.lblNaziv);
			this.Controls.Add(this.picIkona);
			this.Name = "CombatantItem";
			this.Size = new System.Drawing.Size(248, 46);
			this.Click += new System.EventHandler(this.CombatantItem_Click);
			((System.ComponentModel.ISupportInitialize)(this.picIkona)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picStanje)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picPrikrivanje)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picIkona;
		private System.Windows.Forms.Label lblNaziv;
		private System.Windows.Forms.PictureBox picStanje;
		private System.Windows.Forms.PictureBox picPrikrivanje;
	}
}
