/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 13.2.2014.
 * Time: 15:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class DesignInfo
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
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
			this.picSlikaDizajna = new System.Windows.Forms.PictureBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picSlikaDizajna)).BeginInit();
			this.SuspendLayout();
			// 
			// picSlikaDizajna
			// 
			this.picSlikaDizajna.Image = global::Winforms_Mockups.Properties.Resources.small_cruiser;
			this.picSlikaDizajna.Location = new System.Drawing.Point(3, 3);
			this.picSlikaDizajna.Name = "picSlikaDizajna";
			this.picSlikaDizajna.Size = new System.Drawing.Size(80, 80);
			this.picSlikaDizajna.TabIndex = 9;
			this.picSlikaDizajna.TabStop = false;
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(8, 127);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(139, 198);
			this.textBox1.TabIndex = 10;
			this.textBox1.Text = "2 x Colonizer\r\n10 x Laser\r\n\r\nTitanium armor\r\nParticle shield\r\n\r\nNuclear reactor \r" +
			"\n86% power\r\n\r\nEM sensors\r\nChemical jet\r\nSubspace drive\r\n\r\n4 x Cargo hold";
			this.textBox1.TextChanged += new System.EventHandler(this.TextBox1TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 86);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 13);
			this.label1.TabIndex = 11;
			this.label1.Text = "System colonizer";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 99);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 13);
			this.label2.TabIndex = 12;
			this.label2.Text = "Cruiser";
			// 
			// DesignInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.picSlikaDizajna);
			this.Name = "DesignInfo";
			this.Size = new System.Drawing.Size(150, 345);
			((System.ComponentModel.ISupportInitialize)(this.picSlikaDizajna)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.PictureBox picSlikaDizajna;
	}
}
