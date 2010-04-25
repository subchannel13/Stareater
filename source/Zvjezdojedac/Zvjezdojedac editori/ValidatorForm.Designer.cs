namespace Zvjezdojedac_editori
{
	partial class ValidatorForm
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
			if (disposing && (components != null))
			{
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
			this.components = new System.ComponentModel.Container();
			this.tmrDelayCheck = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// tmrDelayCheck
			// 
			this.tmrDelayCheck.Enabled = true;
			this.tmrDelayCheck.Tick += new System.EventHandler(this.tmrDelayCheck_Tick);
			// 
			// ValidatorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Name = "ValidatorForm";
			this.Text = "ValidatorForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer tmrDelayCheck;
	}
}