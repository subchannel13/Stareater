/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 21.3.2017.
 * Time: 10:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormDiplomacyAudiency
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private Winforms_Mockups.NewPlayerInfo newPlayerInfo1;
		private Winforms_Mockups.NewPlayerInfo newPlayerInfo2;
		
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
			this.newPlayerInfo1 = new Winforms_Mockups.NewPlayerInfo();
			this.newPlayerInfo2 = new Winforms_Mockups.NewPlayerInfo();
			this.SuspendLayout();
			// 
			// newPlayerInfo1
			// 
			this.newPlayerInfo1.Location = new System.Drawing.Point(12, 12);
			this.newPlayerInfo1.Name = "newPlayerInfo1";
			this.newPlayerInfo1.Size = new System.Drawing.Size(162, 40);
			this.newPlayerInfo1.TabIndex = 0;
			// 
			// newPlayerInfo2
			// 
			this.newPlayerInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.newPlayerInfo2.Location = new System.Drawing.Point(232, 12);
			this.newPlayerInfo2.Name = "newPlayerInfo2";
			this.newPlayerInfo2.Size = new System.Drawing.Size(162, 40);
			this.newPlayerInfo2.TabIndex = 1;
			// 
			// FormDiplomacy
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(406, 262);
			this.Controls.Add(this.newPlayerInfo2);
			this.Controls.Add(this.newPlayerInfo1);
			this.Name = "FormDiplomacy";
			this.Text = "FormDiplomacy";
			this.ResumeLayout(false);

		}
	}
}
