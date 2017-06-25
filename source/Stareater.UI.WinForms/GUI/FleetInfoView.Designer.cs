/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 24.5.2016.
 * Time: 12:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FleetInfoView
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.PictureBox hullThumbnail;
		private System.Windows.Forms.Label quantityLabel;
		
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
			this.hullThumbnail = new System.Windows.Forms.PictureBox();
			this.quantityLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.hullThumbnail)).BeginInit();
			this.SuspendLayout();
			// 
			// hullThumbnail
			// 
			this.hullThumbnail.Location = new System.Drawing.Point(0, 0);
			this.hullThumbnail.Margin = new System.Windows.Forms.Padding(0);
			this.hullThumbnail.Name = "hullThumbnail";
			this.hullThumbnail.Size = new System.Drawing.Size(30, 30);
			this.hullThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.hullThumbnail.TabIndex = 1;
			this.hullThumbnail.TabStop = false;
			this.hullThumbnail.Click += new System.EventHandler(this.hullThumbnail_Click);
			// 
			// quantityLabel
			// 
			this.quantityLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.quantityLabel.BackColor = System.Drawing.Color.Transparent;
			this.quantityLabel.Location = new System.Drawing.Point(48, 0);
			this.quantityLabel.Name = "quantityLabel";
			this.quantityLabel.Size = new System.Drawing.Size(99, 31);
			this.quantityLabel.TabIndex = 0;
			this.quantityLabel.Text = "label1";
			this.quantityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.quantityLabel.Click += new System.EventHandler(this.quantityLabel_Click);
			// 
			// FleetInfoView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.quantityLabel);
			this.Controls.Add(this.hullThumbnail);
			this.Name = "FleetInfoView";
			this.Size = new System.Drawing.Size(146, 30);
			this.Click += new System.EventHandler(this.fleetInfoView_Click);
			((System.ComponentModel.ISupportInitialize)(this.hullThumbnail)).EndInit();
			this.ResumeLayout(false);

		}
	}
}
