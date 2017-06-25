
namespace Stareater.GUI
{
	partial class QueuedConstructionView
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
			this.costLabel = new System.Windows.Forms.Label();
			this.nameLabel = new System.Windows.Forms.Label();
			this.investmentLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).BeginInit();
			this.SuspendLayout();
			// 
			// thumbnailImage
			// 
			this.thumbnailImage.Location = new System.Drawing.Point(1, 1);
			this.thumbnailImage.Name = "thumbnailImage";
			this.thumbnailImage.Size = new System.Drawing.Size(36, 36);
			this.thumbnailImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnailImage.TabIndex = 0;
			this.thumbnailImage.TabStop = false;
			this.thumbnailImage.Click += new System.EventHandler(this.thumbnailImage_Click);
			// 
			// costLabel
			// 
			this.costLabel.Location = new System.Drawing.Point(41, 20);
			this.costLabel.Name = "costLabel";
			this.costLabel.Size = new System.Drawing.Size(106, 13);
			this.costLabel.TabIndex = 1;
			this.costLabel.Text = "label1";
			this.costLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.costLabel.Click += new System.EventHandler(this.costLabel_Click);
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(42, 3);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(35, 13);
			this.nameLabel.TabIndex = 0;
			this.nameLabel.Text = "Name";
			this.nameLabel.Click += new System.EventHandler(this.nameLabel_Click);
			// 
			// investmentLabel
			// 
			this.investmentLabel.Location = new System.Drawing.Point(143, 20);
			this.investmentLabel.Name = "investmentLabel";
			this.investmentLabel.Size = new System.Drawing.Size(54, 13);
			this.investmentLabel.TabIndex = 2;
			this.investmentLabel.Text = "label1";
			this.investmentLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.investmentLabel.Click += new System.EventHandler(this.investmentLabel_Click);
			// 
			// QueuedConstructionView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Silver;
			this.Controls.Add(this.investmentLabel);
			this.Controls.Add(this.costLabel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.thumbnailImage);
			this.Name = "QueuedConstructionView";
			this.Size = new System.Drawing.Size(200, 38);
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Label investmentLabel;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label costLabel;
		private System.Windows.Forms.PictureBox thumbnailImage;
	}
}
