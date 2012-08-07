namespace Zvjezdojedac.GUI
{
	partial class FormPlaneti
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.planetList = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			// 
			// planetList
			// 
			this.planetList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.planetList.Location = new System.Drawing.Point(0, 0);
			this.planetList.Name = "planetList";
			this.planetList.Size = new System.Drawing.Size(594, 372);
			this.planetList.TabIndex = 0;
			// 
			// FormPlaneti
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(594, 372);
			this.Controls.Add(this.planetList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MinimizeBox = false;
			this.Name = "FormPlaneti";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel planetList;
	}
}