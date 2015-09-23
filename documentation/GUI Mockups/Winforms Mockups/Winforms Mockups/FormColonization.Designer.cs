/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 22.9.2015.
 * Time: 15:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormColonization
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private My_ListView.ControlListView controlListView1;
		private Winforms_Mockups.ColonizationTarget colonizationTarget1;
		private Winforms_Mockups.ColonizationTarget colonizationTarget2;
		
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
			this.controlListView1 = new My_ListView.ControlListView();
			this.colonizationTarget1 = new Winforms_Mockups.ColonizationTarget();
			this.colonizationTarget2 = new Winforms_Mockups.ColonizationTarget();
			this.controlListView1.SuspendLayout();
			this.SuspendLayout();
			// 
			// controlListView1
			// 
			this.controlListView1.AutoScroll = true;
			this.controlListView1.Controls.Add(this.colonizationTarget1);
			this.controlListView1.Controls.Add(this.colonizationTarget2);
			this.controlListView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.controlListView1.Location = new System.Drawing.Point(0, 0);
			this.controlListView1.Name = "controlListView1";
			this.controlListView1.Size = new System.Drawing.Size(284, 262);
			this.controlListView1.TabIndex = 0;
			// 
			// colonizationTarget1
			// 
			this.colonizationTarget1.AutoSize = true;
			this.colonizationTarget1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationTarget1.Location = new System.Drawing.Point(3, 3);
			this.colonizationTarget1.Name = "colonizationTarget1";
			this.colonizationTarget1.Size = new System.Drawing.Size(258, 129);
			this.colonizationTarget1.TabIndex = 0;
			// 
			// colonizationTarget2
			// 
			this.colonizationTarget2.AutoSize = true;
			this.colonizationTarget2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationTarget2.Location = new System.Drawing.Point(3, 138);
			this.colonizationTarget2.Name = "colonizationTarget2";
			this.colonizationTarget2.Size = new System.Drawing.Size(258, 129);
			this.colonizationTarget2.TabIndex = 1;
			// 
			// FormColonization
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.controlListView1);
			this.Name = "FormColonization";
			this.Text = "FormColonization";
			this.controlListView1.ResumeLayout(false);
			this.controlListView1.PerformLayout();
			this.ResumeLayout(false);

		}
	}
}
