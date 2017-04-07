/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 7.4.2017.
 * Time: 9:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class PlayerView
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label organizationLabel;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.PictureBox flagImage;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.organizationLabel = new System.Windows.Forms.Label();
			this.nameLabel = new System.Windows.Forms.Label();
			this.flagImage = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.flagImage)).BeginInit();
			this.SuspendLayout();
			// 
			// organizationLabel
			// 
			this.organizationLabel.AutoSize = true;
			this.organizationLabel.Location = new System.Drawing.Point(42, 16);
			this.organizationLabel.Name = "organizationLabel";
			this.organizationLabel.Size = new System.Drawing.Size(35, 13);
			this.organizationLabel.TabIndex = 5;
			this.organizationLabel.Text = "label1";
			this.organizationLabel.Click += new System.EventHandler(this.organizationLabel_Click);
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(42, 3);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(35, 13);
			this.nameLabel.TabIndex = 4;
			this.nameLabel.Text = "label1";
			this.nameLabel.Click += new System.EventHandler(this.nameLabel_Click);
			// 
			// flagImage
			// 
			this.flagImage.Location = new System.Drawing.Point(3, 3);
			this.flagImage.Name = "flagImage";
			this.flagImage.Size = new System.Drawing.Size(33, 33);
			this.flagImage.TabIndex = 3;
			this.flagImage.TabStop = false;
			this.flagImage.Click += new System.EventHandler(this.flagImage_Click);
			// 
			// PlayerView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.organizationLabel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.flagImage);
			this.Name = "PlayerView";
			this.Size = new System.Drawing.Size(162, 40);
			((System.ComponentModel.ISupportInitialize)(this.flagImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
