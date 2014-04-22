
namespace Stareater.GUI
{
	partial class TechnologyItem
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
			this.levelLabel = new System.Windows.Forms.Label();
			this.investmentLabel = new System.Windows.Forms.Label();
			this.costLabel = new System.Windows.Forms.Label();
			this.nameLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).BeginInit();
			this.SuspendLayout();
			// 
			// thumbnailImage
			// 
			this.thumbnailImage.Location = new System.Drawing.Point(3, 3);
			this.thumbnailImage.Name = "thumbnailImage";
			this.thumbnailImage.Size = new System.Drawing.Size(44, 44);
			this.thumbnailImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnailImage.TabIndex = 0;
			this.thumbnailImage.TabStop = false;
			this.thumbnailImage.Click += new System.EventHandler(this.thumbnailImage_Click);
			// 
			// levelLabel
			// 
			this.levelLabel.AutoSize = true;
			this.levelLabel.Location = new System.Drawing.Point(53, 17);
			this.levelLabel.Name = "levelLabel";
			this.levelLabel.Size = new System.Drawing.Size(38, 13);
			this.levelLabel.TabIndex = 8;
			this.levelLabel.Text = "level 2";
			this.levelLabel.Click += new System.EventHandler(this.levelLabel_Click);
			// 
			// investmentLabel
			// 
			this.investmentLabel.AutoSize = true;
			this.investmentLabel.ForeColor = System.Drawing.Color.DarkGreen;
			this.investmentLabel.Location = new System.Drawing.Point(179, 30);
			this.investmentLabel.Name = "investmentLabel";
			this.investmentLabel.Size = new System.Drawing.Size(45, 13);
			this.investmentLabel.TabIndex = 7;
			this.investmentLabel.Text = "+1.54 G";
			this.investmentLabel.Click += new System.EventHandler(this.investmentLabel_Click);
			// 
			// costLabel
			// 
			this.costLabel.Location = new System.Drawing.Point(53, 30);
			this.costLabel.Name = "costLabel";
			this.costLabel.Size = new System.Drawing.Size(120, 17);
			this.costLabel.TabIndex = 6;
			this.costLabel.Text = "20.52 G / 80 G";
			this.costLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.costLabel.Click += new System.EventHandler(this.costLabel_Click);
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nameLabel.Location = new System.Drawing.Point(53, 3);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(105, 13);
			this.nameLabel.TabIndex = 5;
			this.nameLabel.Text = "Hydroponic farms";
			this.nameLabel.Click += new System.EventHandler(this.nameLabel_Click);
			// 
			// TechnologyItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(196)))), ((int)(((byte)(255)))));
			this.Controls.Add(this.levelLabel);
			this.Controls.Add(this.investmentLabel);
			this.Controls.Add(this.costLabel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.thumbnailImage);
			this.Name = "TechnologyItem";
			this.Size = new System.Drawing.Size(250, 50);
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label costLabel;
		private System.Windows.Forms.Label investmentLabel;
		private System.Windows.Forms.Label levelLabel;
		private System.Windows.Forms.PictureBox thumbnailImage;
	}
}
