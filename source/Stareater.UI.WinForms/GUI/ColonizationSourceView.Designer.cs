/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 24.9.2015.
 * Time: 15:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class ColonizationSourceView
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label progressText;
		private System.Windows.Forms.Button controlButton;
		private System.Windows.Forms.Label starName;
		
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
			this.progressText = new System.Windows.Forms.Label();
			this.controlButton = new System.Windows.Forms.Button();
			this.starName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressText
			// 
			this.progressText.AutoSize = true;
			this.progressText.Location = new System.Drawing.Point(6, 21);
			this.progressText.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.progressText.Name = "progressText";
			this.progressText.Size = new System.Drawing.Size(128, 13);
			this.progressText.TabIndex = 5;
			this.progressText.Text = "xx.x X Ships, ETA: x turns";
			// 
			// controlButton
			// 
			this.controlButton.Location = new System.Drawing.Point(174, 3);
			this.controlButton.Name = "controlButton";
			this.controlButton.Size = new System.Drawing.Size(23, 23);
			this.controlButton.TabIndex = 6;
			this.controlButton.Click += new System.EventHandler(this.controlButton_Click);
			// 
			// starName
			// 
			this.starName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.starName.AutoSize = true;
			this.starName.Location = new System.Drawing.Point(6, 3);
			this.starName.Name = "starName";
			this.starName.Size = new System.Drawing.Size(55, 13);
			this.starName.TabIndex = 4;
			this.starName.Text = "Star name";
			// 
			// ColonizationSourceView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.Controls.Add(this.progressText);
			this.Controls.Add(this.controlButton);
			this.Controls.Add(this.starName);
			this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.Name = "ColonizationSourceView";
			this.Size = new System.Drawing.Size(200, 37);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
