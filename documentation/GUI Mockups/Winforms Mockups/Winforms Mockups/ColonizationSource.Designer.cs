/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 23.9.2015.
 * Time: 9:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class ColonizationSource
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		
		/// <summary>
		/// Disposes resources used by the control.
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
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Alpha Centauri";
			// 
			// button1
			// 
			this.button1.BackgroundImage = global::Winforms_Mockups.Properties.Resources.cancel;
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button1.Location = new System.Drawing.Point(174, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(23, 23);
			this.button1.TabIndex = 1;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 18);
			this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(113, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "11 Ships, ETA: 7 turns";
			// 
			// ColonizationSource
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.Name = "ColonizationSource";
			this.Size = new System.Drawing.Size(200, 31);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
