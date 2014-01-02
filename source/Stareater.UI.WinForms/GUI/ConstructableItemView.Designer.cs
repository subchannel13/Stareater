
namespace Stareater.GUI
{
	partial class ConstructableItemView
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
			this.thumbnail = new System.Windows.Forms.PictureBox();
			this.nameLabel = new System.Windows.Forms.Label();
			this.costLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).BeginInit();
			this.SuspendLayout();
			// 
			// thumbnail
			// 
			this.thumbnail.Location = new System.Drawing.Point(1, 1);
			this.thumbnail.Name = "thumbnail";
			this.thumbnail.Size = new System.Drawing.Size(36, 36);
			this.thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnail.TabIndex = 0;
			this.thumbnail.TabStop = false;
			this.thumbnail.EnabledChanged += new System.EventHandler(this.thumbnail_EnabledChanged);
			this.thumbnail.Click += new System.EventHandler(this.thumbnail_Click);
			this.thumbnail.MouseEnter += new System.EventHandler(this.thumbnail_MouseEnter);
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(42, 3);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(35, 13);
			this.nameLabel.TabIndex = 1;
			this.nameLabel.Text = "Name";
			this.nameLabel.Click += new System.EventHandler(this.nameLabel_Click);
			this.nameLabel.MouseEnter += new System.EventHandler(this.nameLabel_MouseEnter);
			// 
			// costLabel
			// 
			this.costLabel.Location = new System.Drawing.Point(41, 20);
			this.costLabel.Name = "costLabel";
			this.costLabel.Size = new System.Drawing.Size(106, 13);
			this.costLabel.TabIndex = 2;
			this.costLabel.Text = "label1";
			this.costLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.costLabel.Click += new System.EventHandler(this.costLabel_Click);
			this.costLabel.MouseEnter += new System.EventHandler(this.costLabel_MouseEnter);
			// 
			// ConstructableItemView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Silver;
			this.Controls.Add(this.costLabel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.thumbnail);
			this.Name = "ConstructableItemView";
			this.Size = new System.Drawing.Size(150, 38);
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label costLabel;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.PictureBox thumbnail;
	}
}
