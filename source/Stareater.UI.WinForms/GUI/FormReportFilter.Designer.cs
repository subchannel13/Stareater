/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 15.6.2016.
 * Time: 15:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormReportFilter
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.CheckBox checkTechs;
		private System.Windows.Forms.Button applyAction;
		
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
			this.checkTechs = new System.Windows.Forms.CheckBox();
			this.applyAction = new System.Windows.Forms.Button();
			this.checkContacts = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// checkTechs
			// 
			this.checkTechs.AutoSize = true;
			this.checkTechs.Location = new System.Drawing.Point(12, 35);
			this.checkTechs.Name = "checkTechs";
			this.checkTechs.Size = new System.Drawing.Size(52, 17);
			this.checkTechs.TabIndex = 1;
			this.checkTechs.Text = "techs";
			this.checkTechs.UseVisualStyleBackColor = true;
			this.checkTechs.CheckedChanged += new System.EventHandler(this.checkTechs_CheckedChanged);
			// 
			// applyAction
			// 
			this.applyAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.applyAction.Location = new System.Drawing.Point(142, 75);
			this.applyAction.Name = "applyAction";
			this.applyAction.Size = new System.Drawing.Size(75, 23);
			this.applyAction.TabIndex = 2;
			this.applyAction.Text = "apply";
			this.applyAction.UseVisualStyleBackColor = true;
			this.applyAction.Click += new System.EventHandler(this.applyAction_Click);
			// 
			// checkContacts
			// 
			this.checkContacts.AutoSize = true;
			this.checkContacts.Location = new System.Drawing.Point(12, 12);
			this.checkContacts.Name = "checkContacts";
			this.checkContacts.Size = new System.Drawing.Size(67, 17);
			this.checkContacts.TabIndex = 0;
			this.checkContacts.Text = "contacts";
			this.checkContacts.UseVisualStyleBackColor = true;
			this.checkContacts.CheckedChanged += new System.EventHandler(this.checkContacts_CheckedChanged);
			// 
			// FormReportFilter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(229, 110);
			this.Controls.Add(this.checkContacts);
			this.Controls.Add(this.applyAction);
			this.Controls.Add(this.checkTechs);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormReportFilter";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormReportFilter";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.CheckBox checkContacts;
	}
}
