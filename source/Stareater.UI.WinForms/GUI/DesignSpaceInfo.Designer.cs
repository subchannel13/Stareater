/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 11.12.2015.
 * Time: 15:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class DesignSpaceInfo
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label infoText;
		
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
			this.infoText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// infoText
			// 
			this.infoText.BackColor = System.Drawing.Color.Transparent;
			this.infoText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.infoText.Location = new System.Drawing.Point(0, 0);
			this.infoText.Name = "infoText";
			this.infoText.Size = new System.Drawing.Size(150, 150);
			this.infoText.TabIndex = 0;
			this.infoText.Text = "x.xx X / x.xx X";
			this.infoText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DesignSpaceInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.infoText);
			this.Name = "DesignSpaceInfo";
			this.ResumeLayout(false);

		}
	}
}
