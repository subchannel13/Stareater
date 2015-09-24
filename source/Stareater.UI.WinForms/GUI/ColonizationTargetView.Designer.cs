/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 24.9.2015.
 * Time: 15:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class ColonizationTargetView
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.PictureBox thumbnailImage;
		private System.Windows.Forms.Label fromLabel;
		private System.Windows.Forms.Label targetInfo;
		private System.Windows.Forms.FlowLayoutPanel sourceList;
		
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
			this.addButton = new System.Windows.Forms.Button();
			this.thumbnailImage = new System.Windows.Forms.PictureBox();
			this.fromLabel = new System.Windows.Forms.Label();
			this.targetInfo = new System.Windows.Forms.Label();
			this.sourceList = new System.Windows.Forms.FlowLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).BeginInit();
			this.SuspendLayout();
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(20, 75);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(23, 23);
			this.addButton.TabIndex = 3;
			// 
			// thumbnailImage
			// 
			this.thumbnailImage.BackColor = System.Drawing.Color.Black;
			this.thumbnailImage.Location = new System.Drawing.Point(3, 3);
			this.thumbnailImage.Name = "thumbnailImage";
			this.thumbnailImage.Size = new System.Drawing.Size(40, 40);
			this.thumbnailImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnailImage.TabIndex = 4;
			this.thumbnailImage.TabStop = false;
			// 
			// fromLabel
			// 
			this.fromLabel.AutoSize = true;
			this.fromLabel.Location = new System.Drawing.Point(10, 46);
			this.fromLabel.Name = "fromLabel";
			this.fromLabel.Size = new System.Drawing.Size(33, 13);
			this.fromLabel.TabIndex = 6;
			this.fromLabel.Text = "From:";
			// 
			// targetInfo
			// 
			this.targetInfo.AutoSize = true;
			this.targetInfo.Location = new System.Drawing.Point(49, 3);
			this.targetInfo.Name = "targetInfo";
			this.targetInfo.Size = new System.Drawing.Size(120, 26);
			this.targetInfo.TabIndex = 5;
			this.targetInfo.Text = "Alpha Centauri III\r\n0.01 / 100 G population";
			// 
			// sourceList
			// 
			this.sourceList.AutoSize = true;
			this.sourceList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.sourceList.Location = new System.Drawing.Point(49, 46);
			this.sourceList.Name = "sourceList";
			this.sourceList.Size = new System.Drawing.Size(0, 0);
			this.sourceList.TabIndex = 7;
			// 
			// ColonizationTargetView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.sourceList);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.thumbnailImage);
			this.Controls.Add(this.fromLabel);
			this.Controls.Add(this.targetInfo);
			this.Name = "ColonizationTargetView";
			this.Size = new System.Drawing.Size(172, 101);
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
