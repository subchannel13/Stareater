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
			this.hullThumbnail = new System.Windows.Forms.PictureBox();
			this.quantityLabel = new System.Windows.Forms.Label();
			this.primaryMissionThumbnail = new System.Windows.Forms.PictureBox();
			this.secondaryMissionThumbnail = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.hullThumbnail)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.primaryMissionThumbnail)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.secondaryMissionThumbnail)).BeginInit();
			this.SuspendLayout();
			// 
			// hullThumbnail
			// 
			this.hullThumbnail.Location = new System.Drawing.Point(0, 0);
			this.hullThumbnail.Margin = new System.Windows.Forms.Padding(0);
			this.hullThumbnail.Name = "hullThumbnail";
			this.hullThumbnail.Size = new System.Drawing.Size(30, 30);
			this.hullThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.hullThumbnail.TabIndex = 0;
			this.hullThumbnail.TabStop = false;
			this.hullThumbnail.Click += new System.EventHandler(this.hullThumbnail_Click);
			// 
			// quantityLabel
			// 
			this.quantityLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.quantityLabel.BackColor = System.Drawing.Color.Transparent;
			this.quantityLabel.Location = new System.Drawing.Point(66, 0);
			this.quantityLabel.Name = "quantityLabel";
			this.quantityLabel.Size = new System.Drawing.Size(82, 30);
			this.quantityLabel.TabIndex = 1;
			this.quantityLabel.Text = "label1";
			this.quantityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.quantityLabel.Click += new System.EventHandler(this.quantityLabel_Click);
			// 
			// primaryMissionThumbnail
			// 
			this.primaryMissionThumbnail.Location = new System.Drawing.Point(30, 0);
			this.primaryMissionThumbnail.Margin = new System.Windows.Forms.Padding(0);
			this.primaryMissionThumbnail.Name = "primaryMissionThumbnail";
			this.primaryMissionThumbnail.Size = new System.Drawing.Size(15, 15);
			this.primaryMissionThumbnail.TabIndex = 2;
			this.primaryMissionThumbnail.TabStop = false;
			this.primaryMissionThumbnail.Click += new System.EventHandler(this.primaryMissionThumbnail_Click);
			// 
			// secondaryMissionThumbnail
			// 
			this.secondaryMissionThumbnail.Location = new System.Drawing.Point(30, 15);
			this.secondaryMissionThumbnail.Margin = new System.Windows.Forms.Padding(0);
			this.secondaryMissionThumbnail.Name = "secondaryMissionThumbnail";
			this.secondaryMissionThumbnail.Size = new System.Drawing.Size(15, 15);
			this.secondaryMissionThumbnail.TabIndex = 3;
			this.secondaryMissionThumbnail.TabStop = false;
			this.secondaryMissionThumbnail.Click += new System.EventHandler(this.secondaryMissionThumbnail_Click);
			// 
			// ShipGroupItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.secondaryMissionThumbnail);
			this.Controls.Add(this.primaryMissionThumbnail);
			this.Controls.Add(this.quantityLabel);
			this.Controls.Add(this.hullThumbnail);
			this.Name = "ShipGroupItem";
			this.Size = new System.Drawing.Size(148, 30);
			this.Click += new System.EventHandler(this.shipGroupItem_Click);
			((System.ComponentModel.ISupportInitialize)(this.hullThumbnail)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.primaryMissionThumbnail)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.secondaryMissionThumbnail)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.PictureBox secondaryMissionThumbnail;
		private System.Windows.Forms.PictureBox primaryMissionThumbnail;
		private System.Windows.Forms.Label quantityLabel;
		private System.Windows.Forms.PictureBox hullThumbnail;
	}
}
