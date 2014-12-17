/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 10.12.2014.
 * Time: 15:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class ShipGroupItem
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
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
			this.thumbnailImage = new System.Windows.Forms.PictureBox();
			this.quantityLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).BeginInit();
			this.SuspendLayout();
			// 
			// thumbnailImage
			// 
			this.thumbnailImage.Location = new System.Drawing.Point(0, 0);
			this.thumbnailImage.Name = "thumbnailImage";
			this.thumbnailImage.Size = new System.Drawing.Size(60, 30);
			this.thumbnailImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnailImage.TabIndex = 0;
			this.thumbnailImage.TabStop = false;
			// 
			// quantityLabel
			// 
			this.quantityLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.quantityLabel.Location = new System.Drawing.Point(66, 0);
			this.quantityLabel.Name = "quantityLabel";
			this.quantityLabel.Size = new System.Drawing.Size(82, 28);
			this.quantityLabel.TabIndex = 1;
			this.quantityLabel.Text = "label1";
			this.quantityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ShipGroupItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.quantityLabel);
			this.Controls.Add(this.thumbnailImage);
			this.Name = "ShipGroupItem";
			this.Size = new System.Drawing.Size(148, 28);
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label quantityLabel;
		private System.Windows.Forms.PictureBox thumbnailImage;
	}
}
