/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 1.10.2015.
 * Time: 15:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormPickColonizationSource
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ComboBox sourceList;
		private System.Windows.Forms.Label sourceInfo;
		private System.Windows.Forms.Button addButton;
		
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
			this.sourceList = new System.Windows.Forms.ComboBox();
			this.sourceInfo = new System.Windows.Forms.Label();
			this.addButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// sourceList
			// 
			this.sourceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sourceList.FormattingEnabled = true;
			this.sourceList.Location = new System.Drawing.Point(12, 12);
			this.sourceList.Name = "sourceList";
			this.sourceList.Size = new System.Drawing.Size(150, 21);
			this.sourceList.TabIndex = 0;
			this.sourceList.SelectedIndexChanged += new System.EventHandler(this.sourceList_SelectedIndexChanged);
			// 
			// sourceInfo
			// 
			this.sourceInfo.AutoSize = true;
			this.sourceInfo.Location = new System.Drawing.Point(12, 36);
			this.sourceInfo.Name = "sourceInfo";
			this.sourceInfo.Size = new System.Drawing.Size(35, 13);
			this.sourceInfo.TabIndex = 1;
			this.sourceInfo.Text = "label1";
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(87, 55);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(75, 23);
			this.addButton.TabIndex = 2;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// FormPickColonizationSource
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(175, 87);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.sourceInfo);
			this.Controls.Add(this.sourceList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormPickColonizationSource";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormPickColonizationSource";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
