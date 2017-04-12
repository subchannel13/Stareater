namespace Stareater.GUI
{
	partial class TreatyBriefView
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.PictureBox thumbnailImage;
		private System.Windows.Forms.Label nameText;
		
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
			this.nameText = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).BeginInit();
			this.SuspendLayout();
			// 
			// thumbnailImage
			// 
			this.thumbnailImage.Location = new System.Drawing.Point(16, 0);
			this.thumbnailImage.Margin = new System.Windows.Forms.Padding(0);
			this.thumbnailImage.Name = "thumbnailImage";
			this.thumbnailImage.Size = new System.Drawing.Size(20, 20);
			this.thumbnailImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnailImage.TabIndex = 0;
			this.thumbnailImage.TabStop = false;
			// 
			// nameText
			// 
			this.nameText.Location = new System.Drawing.Point(39, 0);
			this.nameText.Name = "nameText";
			this.nameText.Size = new System.Drawing.Size(93, 20);
			this.nameText.TabIndex = 1;
			this.nameText.Text = "label1";
			// 
			// TreatyInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.nameText);
			this.Controls.Add(this.thumbnailImage);
			this.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.Name = "TreatyInfo";
			this.Size = new System.Drawing.Size(135, 20);
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).EndInit();
			this.ResumeLayout(false);

		}
	}
}
