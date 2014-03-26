/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 19.2.2014.
 * Time: 10:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormShipDesignList
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
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
			this.countHeader = new System.Windows.Forms.Label();
			this.designHeader = new System.Windows.Forms.Label();
			this.designList = new System.Windows.Forms.FlowLayoutPanel();
			this.newDesignButton = new System.Windows.Forms.Button();
			this.infoList = new System.Windows.Forms.FlowLayoutPanel();
			this.designThumbnail = new System.Windows.Forms.PictureBox();
			this.designName = new System.Windows.Forms.Label();
			this.hullName = new System.Windows.Forms.Label();
			this.infoList.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.designThumbnail)).BeginInit();
			this.SuspendLayout();
			// 
			// countHeader
			// 
			this.countHeader.AutoSize = true;
			this.countHeader.Location = new System.Drawing.Point(262, 9);
			this.countHeader.Name = "countHeader";
			this.countHeader.Size = new System.Drawing.Size(33, 13);
			this.countHeader.TabIndex = 16;
			this.countHeader.Text = "Ships";
			// 
			// designHeader
			// 
			this.designHeader.AutoSize = true;
			this.designHeader.Location = new System.Drawing.Point(12, 9);
			this.designHeader.Name = "designHeader";
			this.designHeader.Size = new System.Drawing.Size(40, 13);
			this.designHeader.TabIndex = 15;
			this.designHeader.Text = "Design";
			// 
			// designList
			// 
			this.designList.AutoScroll = true;
			this.designList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.designList.Location = new System.Drawing.Point(12, 25);
			this.designList.Name = "designList";
			this.designList.Size = new System.Drawing.Size(365, 313);
			this.designList.TabIndex = 17;
			// 
			// newDesignButton
			// 
			this.newDesignButton.Image = global::Stareater.Properties.Resources.newDesign;
			this.newDesignButton.Location = new System.Drawing.Point(12, 344);
			this.newDesignButton.Name = "newDesignButton";
			this.newDesignButton.Size = new System.Drawing.Size(56, 56);
			this.newDesignButton.TabIndex = 19;
			this.newDesignButton.Click += new System.EventHandler(this.newDesignButton_Click);
			// 
			// infoList
			// 
			this.infoList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.infoList.AutoScroll = true;
			this.infoList.Controls.Add(this.designThumbnail);
			this.infoList.Controls.Add(this.designName);
			this.infoList.Controls.Add(this.hullName);
			this.infoList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.infoList.Location = new System.Drawing.Point(383, 25);
			this.infoList.Name = "infoList";
			this.infoList.Size = new System.Drawing.Size(200, 377);
			this.infoList.TabIndex = 20;
			// 
			// designThumbnail
			// 
			this.designThumbnail.Location = new System.Drawing.Point(3, 3);
			this.designThumbnail.Name = "designThumbnail";
			this.designThumbnail.Size = new System.Drawing.Size(80, 80);
			this.designThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.designThumbnail.TabIndex = 0;
			this.designThumbnail.TabStop = false;
			// 
			// designName
			// 
			this.designName.AutoSize = true;
			this.designName.Location = new System.Drawing.Point(3, 86);
			this.designName.Name = "designName";
			this.designName.Size = new System.Drawing.Size(69, 13);
			this.designName.TabIndex = 1;
			this.designName.Text = "Design name";
			// 
			// hullName
			// 
			this.hullName.AutoSize = true;
			this.hullName.Location = new System.Drawing.Point(3, 99);
			this.hullName.Name = "hullName";
			this.hullName.Size = new System.Drawing.Size(54, 13);
			this.hullName.TabIndex = 2;
			this.hullName.Text = "Hull name";
			// 
			// FormShipDesignList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(594, 414);
			this.Controls.Add(this.infoList);
			this.Controls.Add(this.newDesignButton);
			this.Controls.Add(this.designList);
			this.Controls.Add(this.countHeader);
			this.Controls.Add(this.designHeader);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormShipDesignList";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormShipDesignList";
			this.infoList.ResumeLayout(false);
			this.infoList.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.designThumbnail)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label hullName;
		private System.Windows.Forms.Label designName;
		private System.Windows.Forms.PictureBox designThumbnail;
		private System.Windows.Forms.FlowLayoutPanel infoList;
		private System.Windows.Forms.Button newDesignButton;
		private System.Windows.Forms.FlowLayoutPanel designList;
		private System.Windows.Forms.Label designHeader;
		private System.Windows.Forms.Label countHeader;
	}
}
