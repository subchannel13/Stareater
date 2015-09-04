/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 2.9.2015.
 * Time: 15:59
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormStarSystemUncolonized
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private Winforms_Mockups.SystemMockup systemMockup1;
		private System.Windows.Forms.Panel managementPanel;
		private System.Windows.Forms.Button detailsButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		
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
			this.systemMockup1 = new Winforms_Mockups.SystemMockup();
			this.managementPanel = new System.Windows.Forms.Panel();
			this.detailsButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.managementPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// systemMockup1
			// 
			this.systemMockup1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.systemMockup1.Location = new System.Drawing.Point(0, 0);
			this.systemMockup1.Name = "systemMockup1";
			this.systemMockup1.Size = new System.Drawing.Size(568, 401);
			this.systemMockup1.TabIndex = 0;
			// 
			// managementPanel
			// 
			this.managementPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.managementPanel.BackColor = System.Drawing.SystemColors.Control;
			this.managementPanel.BackgroundImage = global::Winforms_Mockups.Properties.Resources.metalic;
			this.managementPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.managementPanel.Controls.Add(this.detailsButton);
			this.managementPanel.Controls.Add(this.label1);
			this.managementPanel.Controls.Add(this.button1);
			this.managementPanel.Location = new System.Drawing.Point(126, 296);
			this.managementPanel.Name = "managementPanel";
			this.managementPanel.Size = new System.Drawing.Size(316, 105);
			this.managementPanel.TabIndex = 2;
			// 
			// detailsButton
			// 
			this.detailsButton.Location = new System.Drawing.Point(221, 68);
			this.detailsButton.Name = "detailsButton";
			this.detailsButton.Size = new System.Drawing.Size(75, 23);
			this.detailsButton.TabIndex = 5;
			this.detailsButton.Text = "Details";
			this.detailsButton.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(87, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "7 turns";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(8, 11);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 80);
			this.button1.TabIndex = 0;
			this.button1.Text = "Colonize";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// FormStarSystemUncolonized
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(568, 401);
			this.Controls.Add(this.managementPanel);
			this.Controls.Add(this.systemMockup1);
			this.Name = "FormStarSystemUncolonized";
			this.Text = "FormStarSystemUncolonized";
			this.managementPanel.ResumeLayout(false);
			this.managementPanel.PerformLayout();
			this.ResumeLayout(false);

		}
	}
}
