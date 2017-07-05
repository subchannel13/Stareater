/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 1.12.2015.
 * Time: 13:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormPickComponent
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.FlowLayoutPanel componentPanel;
		private System.Windows.Forms.ImageList thumbnailList;
		
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
			this.components = new System.ComponentModel.Container();
			this.componentPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.thumbnailList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// componentPanel
			// 
			this.componentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.componentPanel.Location = new System.Drawing.Point(0, 0);
			this.componentPanel.Name = "componentPanel";
			this.componentPanel.Size = new System.Drawing.Size(284, 262);
			this.componentPanel.TabIndex = 0;
			// 
			// thumbnailList
			// 
			this.thumbnailList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.thumbnailList.ImageSize = new System.Drawing.Size(48, 48);
			this.thumbnailList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// FormPickComponent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.componentPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormPickComponent";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormPickComponent";
			this.ResumeLayout(false);

		}
	}
}
