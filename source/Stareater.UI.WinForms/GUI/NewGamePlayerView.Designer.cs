namespace Stareater.GUI
{
	partial class NewGamePlayerView
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
			this.flagImage = new System.Windows.Forms.PictureBox();
			this.nameLabel = new System.Windows.Forms.Label();
			this.organizationLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.flagImage)).BeginInit();
			this.SuspendLayout();
			// 
			// flagImage
			// 
			this.flagImage.Location = new System.Drawing.Point(3, 3);
			this.flagImage.Name = "flagImage";
			this.flagImage.Size = new System.Drawing.Size(33, 33);
			this.flagImage.TabIndex = 0;
			this.flagImage.TabStop = false;
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(42, 3);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(35, 13);
			this.nameLabel.TabIndex = 1;
			this.nameLabel.Text = "label1";
			// 
			// organizationLabel
			// 
			this.organizationLabel.AutoSize = true;
			this.organizationLabel.Location = new System.Drawing.Point(42, 16);
			this.organizationLabel.Name = "organizationLabel";
			this.organizationLabel.Size = new System.Drawing.Size(35, 13);
			this.organizationLabel.TabIndex = 2;
			this.organizationLabel.Text = "label1";
			// 
			// NewGamePlayerInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.organizationLabel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.flagImage);
			this.Name = "NewGamePlayerInfo";
			this.Size = new System.Drawing.Size(162, 40);
			((System.ComponentModel.ISupportInitialize)(this.flagImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox flagImage;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label organizationLabel;
	}
}
