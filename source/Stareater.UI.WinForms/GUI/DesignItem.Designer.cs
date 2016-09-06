/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 25.2.2014.
 * Time: 13:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class DesignItem
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
			this.nameLabel = new System.Windows.Forms.Label();
			this.thumbnail = new System.Windows.Forms.PictureBox();
			this.countLabel = new System.Windows.Forms.Label();
			this.actionButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).BeginInit();
			this.SuspendLayout();
			// 
			// nameLabel
			// 
			this.nameLabel.Location = new System.Drawing.Point(69, 3);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(175, 40);
			this.nameLabel.TabIndex = 3;
			this.nameLabel.Text = "name";
			this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.nameLabel.MouseEnter += new System.EventHandler(this.nameLabel_MouseEnter);
			// 
			// thumbnail
			// 
			this.thumbnail.Location = new System.Drawing.Point(3, 3);
			this.thumbnail.Name = "thumbnail";
			this.thumbnail.Size = new System.Drawing.Size(60, 40);
			this.thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnail.TabIndex = 4;
			this.thumbnail.TabStop = false;
			this.thumbnail.MouseEnter += new System.EventHandler(this.thumbnail_MouseEnter);
			// 
			// countLabel
			// 
			this.countLabel.Location = new System.Drawing.Point(250, 3);
			this.countLabel.Name = "countLabel";
			this.countLabel.Size = new System.Drawing.Size(49, 40);
			this.countLabel.TabIndex = 5;
			this.countLabel.Text = "x.xx X";
			this.countLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.countLabel.MouseEnter += new System.EventHandler(this.countLabel_MouseEnter);
			// 
			// actionButton
			// 
			this.actionButton.Image = global::Stareater.Properties.Resources.cancel;
			this.actionButton.Location = new System.Drawing.Point(305, 7);
			this.actionButton.Name = "actionButton";
			this.actionButton.Size = new System.Drawing.Size(32, 32);
			this.actionButton.TabIndex = 6;
			this.actionButton.UseVisualStyleBackColor = true;
			this.actionButton.Click += new System.EventHandler(this.actionButton_Click);
			// 
			// DesignItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.Controls.Add(this.actionButton);
			this.Controls.Add(this.countLabel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.thumbnail);
			this.Name = "DesignItem";
			this.Size = new System.Drawing.Size(340, 46);
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).EndInit();
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button actionButton;
		private System.Windows.Forms.Label countLabel;
		private System.Windows.Forms.PictureBox thumbnail;
		private System.Windows.Forms.Label nameLabel;
	}
}
