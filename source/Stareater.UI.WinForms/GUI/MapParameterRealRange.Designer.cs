namespace Stareater.GUI
{
	partial class MapParameterRealRange
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.nameLabel = new System.Windows.Forms.Label();
			this.valueSlider = new System.Windows.Forms.HScrollBar();
			this.valueLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(-3, 0);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(35, 13);
			this.nameLabel.TabIndex = 0;
			this.nameLabel.Text = "label1";
			// 
			// valueSlider
			// 
			this.valueSlider.LargeChange = 500;
			this.valueSlider.Location = new System.Drawing.Point(0, 16);
			this.valueSlider.Maximum = 10499;
			this.valueSlider.Name = "valueSlider";
			this.valueSlider.Size = new System.Drawing.Size(140, 17);
			this.valueSlider.TabIndex = 1;
			this.valueSlider.Scroll += new System.Windows.Forms.ScrollEventHandler(this.valueSlider_Scroll);
			// 
			// valueLabel
			// 
			this.valueLabel.Location = new System.Drawing.Point(3, 33);
			this.valueLabel.Name = "valueLabel";
			this.valueLabel.Size = new System.Drawing.Size(137, 13);
			this.valueLabel.TabIndex = 2;
			this.valueLabel.Text = "label1";
			this.valueLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// MapParameterRealRange
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.valueLabel);
			this.Controls.Add(this.valueSlider);
			this.Controls.Add(this.nameLabel);
			this.Name = "MapParameterRealRange";
			this.Size = new System.Drawing.Size(140, 46);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.HScrollBar valueSlider;
		private System.Windows.Forms.Label valueLabel;
	}
}
