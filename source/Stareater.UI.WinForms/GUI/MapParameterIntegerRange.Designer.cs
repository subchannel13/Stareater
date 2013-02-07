namespace Stareater.GUI
{
	partial class MapParameterIntegerRange
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
			this.valueLabel = new System.Windows.Forms.Label();
			this.valueSlider = new System.Windows.Forms.HScrollBar();
			this.nameLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// valueLabel
			// 
			this.valueLabel.Location = new System.Drawing.Point(5, 33);
			this.valueLabel.Name = "valueLabel";
			this.valueLabel.Size = new System.Drawing.Size(137, 13);
			this.valueLabel.TabIndex = 28;
			this.valueLabel.Text = "label1";
			this.valueLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// valueSlider
			// 
			this.valueSlider.LargeChange = 1;
			this.valueSlider.Location = new System.Drawing.Point(2, 16);
			this.valueSlider.Name = "valueSlider";
			this.valueSlider.Size = new System.Drawing.Size(140, 17);
			this.valueSlider.TabIndex = 27;
			this.valueSlider.Scroll += new System.Windows.Forms.ScrollEventHandler(this.valueSlider_Scroll);
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(-1, 0);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(35, 13);
			this.nameLabel.TabIndex = 26;
			this.nameLabel.Text = "label1";
			// 
			// MapParameterIntegerRange
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.valueLabel);
			this.Controls.Add(this.valueSlider);
			this.Controls.Add(this.nameLabel);
			this.Name = "MapParameterIntegerRange";
			this.Size = new System.Drawing.Size(140, 46);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label valueLabel;
		private System.Windows.Forms.HScrollBar valueSlider;
		private System.Windows.Forms.Label nameLabel;
	}
}
