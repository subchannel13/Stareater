namespace Zvjezdojedac.GUI
{
	partial class BriefPlanetItem
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
			this.planetImage = new System.Windows.Forms.PictureBox();
			this.planetInfo1 = new System.Windows.Forms.Label();
			this.planetInfo2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.planetImage)).BeginInit();
			this.SuspendLayout();
			// 
			// planetImage
			// 
			this.planetImage.Location = new System.Drawing.Point(0, 0);
			this.planetImage.Name = "planetImage";
			this.planetImage.Size = new System.Drawing.Size(48, 32);
			this.planetImage.TabIndex = 0;
			this.planetImage.TabStop = false;
			this.planetImage.Click += new System.EventHandler(this.ChildControl_Click);
			// 
			// planetInfo1
			// 
			this.planetInfo1.AutoSize = true;
			this.planetInfo1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.planetInfo1.Location = new System.Drawing.Point(54, 0);
			this.planetInfo1.Name = "planetInfo1";
			this.planetInfo1.Size = new System.Drawing.Size(35, 13);
			this.planetInfo1.TabIndex = 1;
			this.planetInfo1.Text = "label1";
			this.planetInfo1.Click += new System.EventHandler(this.ChildControl_Click);
			// 
			// planetInfo2
			// 
			this.planetInfo2.AutoSize = true;
			this.planetInfo2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.planetInfo2.Location = new System.Drawing.Point(54, 13);
			this.planetInfo2.Name = "planetInfo2";
			this.planetInfo2.Size = new System.Drawing.Size(35, 13);
			this.planetInfo2.TabIndex = 2;
			this.planetInfo2.Text = "label2";
			this.planetInfo2.Click += new System.EventHandler(this.ChildControl_Click);
			// 
			// BriefPlanetItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.planetInfo2);
			this.Controls.Add(this.planetInfo1);
			this.Controls.Add(this.planetImage);
			this.Name = "BriefPlanetItem";
			this.Size = new System.Drawing.Size(189, 35);
			((System.ComponentModel.ISupportInitialize)(this.planetImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox planetImage;
		private System.Windows.Forms.Label planetInfo1;
		private System.Windows.Forms.Label planetInfo2;
	}
}
