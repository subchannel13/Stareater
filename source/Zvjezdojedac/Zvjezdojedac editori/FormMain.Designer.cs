namespace Zvjezdojedac_editori
{
	partial class FormMain
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
			this.btnTehnologije = new System.Windows.Forms.Button();
			this.txtPutanja = new System.Windows.Forms.TextBox();
			this.btnPutanja = new System.Windows.Forms.Button();
			this.lblDostupniPodaci = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnTehnologije
			// 
			this.btnTehnologije.Location = new System.Drawing.Point(12, 12);
			this.btnTehnologije.Name = "btnTehnologije";
			this.btnTehnologije.Size = new System.Drawing.Size(75, 23);
			this.btnTehnologije.TabIndex = 0;
			this.btnTehnologije.Text = "&Tehnologije";
			this.btnTehnologije.UseVisualStyleBackColor = true;
			this.btnTehnologije.Click += new System.EventHandler(this.btnTehnologije_Click);
			// 
			// txtPutanja
			// 
			this.txtPutanja.Location = new System.Drawing.Point(12, 217);
			this.txtPutanja.Name = "txtPutanja";
			this.txtPutanja.ReadOnly = true;
			this.txtPutanja.Size = new System.Drawing.Size(179, 20);
			this.txtPutanja.TabIndex = 1;
			// 
			// btnPutanja
			// 
			this.btnPutanja.Location = new System.Drawing.Point(197, 217);
			this.btnPutanja.Name = "btnPutanja";
			this.btnPutanja.Size = new System.Drawing.Size(75, 23);
			this.btnPutanja.TabIndex = 2;
			this.btnPutanja.Text = "Promjeni";
			this.btnPutanja.UseVisualStyleBackColor = true;
			this.btnPutanja.Click += new System.EventHandler(this.btnPutanja_Click);
			// 
			// lblDostupniPodaci
			// 
			this.lblDostupniPodaci.AutoSize = true;
			this.lblDostupniPodaci.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lblDostupniPodaci.ForeColor = System.Drawing.Color.Red;
			this.lblDostupniPodaci.Location = new System.Drawing.Point(12, 240);
			this.lblDostupniPodaci.Name = "lblDostupniPodaci";
			this.lblDostupniPodaci.Size = new System.Drawing.Size(41, 13);
			this.lblDostupniPodaci.TabIndex = 3;
			this.lblDostupniPodaci.Text = "label1";
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.lblDostupniPodaci);
			this.Controls.Add(this.btnPutanja);
			this.Controls.Add(this.txtPutanja);
			this.Controls.Add(this.btnTehnologije);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "FormMain";
			this.Text = "Editori za Zvjezdojeca";
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnTehnologije;
		private System.Windows.Forms.TextBox txtPutanja;
		private System.Windows.Forms.Button btnPutanja;
		private System.Windows.Forms.Label lblDostupniPodaci;
	}
}

