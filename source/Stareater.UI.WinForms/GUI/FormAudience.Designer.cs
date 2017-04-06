/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 6.4.2017.
 * Time: 12:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormAudience
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private Stareater.GUI.RelationsPlayerInfo contactInfo;
		
		/// <summary>
		/// Disposes resources used by the form.
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
			this.contactInfo = new Stareater.GUI.RelationsPlayerInfo();
			this.SuspendLayout();
			// 
			// contactInfo
			// 
			this.contactInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.contactInfo.Location = new System.Drawing.Point(12, 12);
			this.contactInfo.Name = "contactInfo";
			this.contactInfo.Size = new System.Drawing.Size(160, 120);
			this.contactInfo.TabIndex = 0;
			// 
			// FormAudience
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(434, 262);
			this.Controls.Add(this.contactInfo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormAudience";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormAudience";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formAudience_FormClosed);
			this.ResumeLayout(false);

		}
	}
}
