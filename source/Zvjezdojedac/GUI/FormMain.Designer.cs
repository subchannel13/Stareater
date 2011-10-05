namespace Zvjezdojedac.GUI
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.btnNovaIgra = new System.Windows.Forms.Button();
			this.btnUcitaj = new System.Windows.Forms.Button();
			this.btnUgasi = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnPostavke = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnNovaIgra
			// 
			this.btnNovaIgra.Location = new System.Drawing.Point(109, 64);
			this.btnNovaIgra.Name = "btnNovaIgra";
			this.btnNovaIgra.Size = new System.Drawing.Size(75, 30);
			this.btnNovaIgra.TabIndex = 1;
			this.btnNovaIgra.Text = "Nova igra";
			this.btnNovaIgra.UseVisualStyleBackColor = true;
			this.btnNovaIgra.Click += new System.EventHandler(this.btnNovaIgra_Click);
			// 
			// btnUcitaj
			// 
			this.btnUcitaj.Location = new System.Drawing.Point(109, 100);
			this.btnUcitaj.Name = "btnUcitaj";
			this.btnUcitaj.Size = new System.Drawing.Size(75, 30);
			this.btnUcitaj.TabIndex = 2;
			this.btnUcitaj.Text = "Učitaj igru";
			this.btnUcitaj.UseVisualStyleBackColor = true;
			this.btnUcitaj.Click += new System.EventHandler(this.btnUcitaj_Click);
			// 
			// btnUgasi
			// 
			this.btnUgasi.Location = new System.Drawing.Point(109, 172);
			this.btnUgasi.Name = "btnUgasi";
			this.btnUgasi.Size = new System.Drawing.Size(75, 30);
			this.btnUgasi.TabIndex = 4;
			this.btnUgasi.Text = "Ugasi";
			this.btnUgasi.UseVisualStyleBackColor = true;
			this.btnUgasi.Click += new System.EventHandler(this.btnUgasi_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label1.Location = new System.Drawing.Point(79, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(135, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Zvjezdojedac";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(165, 244);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(115, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Ivan Kravarščan 2011.";
			// 
			// btnPostavke
			// 
			this.btnPostavke.Location = new System.Drawing.Point(109, 136);
			this.btnPostavke.Name = "btnPostavke";
			this.btnPostavke.Size = new System.Drawing.Size(75, 30);
			this.btnPostavke.TabIndex = 3;
			this.btnPostavke.Text = "Postavke";
			this.btnPostavke.UseVisualStyleBackColor = true;
			this.btnPostavke.Click += new System.EventHandler(this.btnPostavke_Click);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.btnPostavke);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnUgasi);
			this.Controls.Add(this.btnUcitaj);
			this.Controls.Add(this.btnNovaIgra);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Zvjezdojedac";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnNovaIgra;
		private System.Windows.Forms.Button btnUcitaj;
		private System.Windows.Forms.Button btnUgasi;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnPostavke;
	}
}

