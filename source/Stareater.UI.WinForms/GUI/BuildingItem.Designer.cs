/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 20.1.2014.
 * Time: 13:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class BuildingItem
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
			this.countLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).BeginInit();
			this.SuspendLayout();
			// 
			// thumbnail
			// 
			this.thumbnail.Location = new System.Drawing.Point(3, 3);
			this.thumbnail.Name = "thumbnail";
			this.thumbnail.Size = new System.Drawing.Size(32, 32);
			this.thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnail.TabIndex = 0;
			this.thumbnail.TabStop = false;
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nameLabel.Location = new System.Drawing.Point(41, 3);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(37, 13);
			this.nameLabel.TabIndex = 0;
			this.nameLabel.Text = "name";
			// 
			// countLabel
			// 
			this.countLabel.AutoSize = true;
			this.countLabel.Location = new System.Drawing.Point(41, 18);
			this.countLabel.Name = "countLabel";
			this.countLabel.Size = new System.Drawing.Size(22, 13);
			this.countLabel.TabIndex = 1;
			this.countLabel.Text = "xxx";
			// 
			// BuildingItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.Controls.Add(this.countLabel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.thumbnail);
			this.Name = "BuildingItem";
			this.Size = new System.Drawing.Size(150, 38);
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Label countLabel;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.PictureBox thumbnail;
	}
}
