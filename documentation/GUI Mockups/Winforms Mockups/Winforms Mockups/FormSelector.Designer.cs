
namespace Winforms_Mockups
{
	partial class FormSelector
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
			this.components = new System.ComponentModel.Container();
			this.formList = new System.Windows.Forms.ListBox();
			this.delayedRun = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// formList
			// 
			this.formList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.formList.FormattingEnabled = true;
			this.formList.Location = new System.Drawing.Point(12, 12);
			this.formList.Name = "formList";
			this.formList.Size = new System.Drawing.Size(260, 238);
			this.formList.TabIndex = 0;
			this.formList.SelectedIndexChanged += new System.EventHandler(this.FormListSelectedIndexChanged);
			// 
			// delayedRun
			// 
			this.delayedRun.Enabled = true;
			this.delayedRun.Tick += new System.EventHandler(this.DelayedRunTick);
			// 
			// FormSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.formList);
			this.Name = "FormSelector";
			this.Text = "FormSelector";
			this.Load += new System.EventHandler(this.FormSelectorLoad);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Timer delayedRun;
		private System.Windows.Forms.ListBox formList;
	}
}
