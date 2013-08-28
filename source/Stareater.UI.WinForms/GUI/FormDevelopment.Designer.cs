
namespace Stareater.GUI
{
	partial class FormDevelopment
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
			this.controlListView1 = new Zvjezdojedac.GUI.ControlListView();
			this.SuspendLayout();
			// 
			// controlListView1
			// 
			this.controlListView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.controlListView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.controlListView1.Location = new System.Drawing.Point(12, 12);
			this.controlListView1.Name = "controlListView1";
			this.controlListView1.Size = new System.Drawing.Size(72, 238);
			this.controlListView1.TabIndex = 1;
			// 
			// FormDevelopment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.controlListView1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormDevelopment";
			this.ShowInTaskbar = false;
			this.Text = "Development topics";
			this.ResumeLayout(false);
		}
		private Zvjezdojedac.GUI.ControlListView controlListView1;
	}
}
