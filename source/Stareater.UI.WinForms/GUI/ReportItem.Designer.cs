/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 30.9.2014.
 * Time: 15:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class ReportItem
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
			this.messageLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).BeginInit();
			this.SuspendLayout();
			// 
			// thumbnail
			// 
			this.thumbnail.Location = new System.Drawing.Point(3, 3);
			this.thumbnail.Name = "thumbnail";
			this.thumbnail.Size = new System.Drawing.Size(44, 44);
			this.thumbnail.TabIndex = 0;
			this.thumbnail.TabStop = false;
			this.thumbnail.Click += new System.EventHandler(this.thumbnail_Click);
			// 
			// messageLabel
			// 
			this.messageLabel.Location = new System.Drawing.Point(53, 3);
			this.messageLabel.Name = "messageLabel";
			this.messageLabel.Size = new System.Drawing.Size(194, 44);
			this.messageLabel.TabIndex = 1;
			this.messageLabel.Text = "label1";
			this.messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.messageLabel.Click += new System.EventHandler(this.messageLabel_Click);
			// 
			// ReportItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(196)))), ((int)(((byte)(255)))));
			this.Controls.Add(this.messageLabel);
			this.Controls.Add(this.thumbnail);
			this.Name = "ReportItem";
			this.Size = new System.Drawing.Size(250, 50);
			((System.ComponentModel.ISupportInitialize)(this.thumbnail)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label messageLabel;
		private System.Windows.Forms.PictureBox thumbnail;
	}
}
