namespace Stareater.GUI
{
	partial class ColorItem
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
			this.colorBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.colorBox)).BeginInit();
			this.SuspendLayout();
			// 
			// colorBox
			// 
			this.colorBox.BackColor = System.Drawing.Color.Red;
			this.colorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.colorBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.colorBox.Location = new System.Drawing.Point(3, 3);
			this.colorBox.Name = "colorBox";
			this.colorBox.Size = new System.Drawing.Size(32, 32);
			this.colorBox.TabIndex = 0;
			this.colorBox.TabStop = false;
			this.colorBox.Click += new System.EventHandler(this.colorBox_Click);
			// 
			// ColorItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.colorBox);
			this.Name = "ColorItem";
			this.Padding = new System.Windows.Forms.Padding(3);
			this.Size = new System.Drawing.Size(38, 38);
			((System.ComponentModel.ISupportInitialize)(this.colorBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox colorBox;
	}
}
