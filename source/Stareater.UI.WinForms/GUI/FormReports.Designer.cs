/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 12.9.2014.
 * Time: 11:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormReports
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
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
			this.reportList = new Stareater.GUI.ControlListView();
			this.filterButton = new System.Windows.Forms.Button();
			this.openButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// reportList
			// 
			this.reportList.Location = new System.Drawing.Point(12, 12);
			this.reportList.Name = "reportList";
			this.reportList.SelectedIndex = -1;
			this.reportList.Size = new System.Drawing.Size(276, 393);
			this.reportList.TabIndex = 0;
			// 
			// filterButton
			// 
			this.filterButton.Image = global::Stareater.Properties.Resources.filter;
			this.filterButton.Location = new System.Drawing.Point(294, 355);
			this.filterButton.Name = "filterButton";
			this.filterButton.Size = new System.Drawing.Size(50, 50);
			this.filterButton.TabIndex = 1;
			this.filterButton.UseVisualStyleBackColor = true;
			this.filterButton.Click += new System.EventHandler(this.filterButton_Click);
			// 
			// openButton
			// 
			this.openButton.Image = global::Stareater.Properties.Resources.gotoArrow;
			this.openButton.Location = new System.Drawing.Point(294, 127);
			this.openButton.Name = "openButton";
			this.openButton.Size = new System.Drawing.Size(50, 50);
			this.openButton.TabIndex = 2;
			this.openButton.UseVisualStyleBackColor = true;
			this.openButton.Click += new System.EventHandler(this.openButton_Click);
			// 
			// FormReports
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(356, 417);
			this.Controls.Add(this.openButton);
			this.Controls.Add(this.filterButton);
			this.Controls.Add(this.reportList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormReports";
			this.ShowInTaskbar = false;
			this.Text = "FormReports";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button openButton;
		private System.Windows.Forms.Button filterButton;
		private Stareater.GUI.ControlListView reportList;
	}
}
