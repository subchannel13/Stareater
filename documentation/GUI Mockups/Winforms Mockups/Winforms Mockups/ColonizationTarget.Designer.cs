/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 22.9.2015.
 * Time: 15:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class ColonizationTarget
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private Winforms_Mockups.ColonizationSource colonizationSource2;
		private Winforms_Mockups.ColonizationSource colonizationSource1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button1;
		
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
			this.label2 = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.colonizationSource2 = new Winforms_Mockups.ColonizationSource();
			this.colonizationSource1 = new Winforms_Mockups.ColonizationSource();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(49, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(206, 40);
			this.label1.TabIndex = 1;
			this.label1.Text = "Alpha Centauri III\r\n0.01 / 100 G population";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "From:";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.colonizationSource2);
			this.flowLayoutPanel1.Controls.Add(this.colonizationSource1);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(49, 46);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(206, 80);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// colonizationSource2
			// 
			this.colonizationSource2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationSource2.Location = new System.Drawing.Point(3, 6);
			this.colonizationSource2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.colonizationSource2.Name = "colonizationSource2";
			this.colonizationSource2.Size = new System.Drawing.Size(200, 31);
			this.colonizationSource2.TabIndex = 4;
			// 
			// colonizationSource1
			// 
			this.colonizationSource1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationSource1.Location = new System.Drawing.Point(3, 46);
			this.colonizationSource1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.colonizationSource1.Name = "colonizationSource1";
			this.colonizationSource1.Size = new System.Drawing.Size(200, 31);
			this.colonizationSource1.TabIndex = 3;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Black;
			this.pictureBox1.Image = global::Winforms_Mockups.Properties.Resources.earthlike_planet;
			this.pictureBox1.Location = new System.Drawing.Point(3, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(40, 40);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// button1
			// 
			this.button1.BackgroundImage = global::Winforms_Mockups.Properties.Resources._goto;
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button1.Location = new System.Drawing.Point(20, 75);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(23, 23);
			this.button1.TabIndex = 5;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// ColonizationTarget
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "ColonizationTarget";
			this.Size = new System.Drawing.Size(258, 129);
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
