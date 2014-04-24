/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 23.4.2014.
 * Time: 13:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormResearch
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
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.controlListView1 = new My_ListView.ControlListView();
			this.researchItem1 = new Winforms_Mockups.ResearchItem();
			this.researchItem2 = new Winforms_Mockups.ResearchItem();
			this.researchItem3 = new Winforms_Mockups.ResearchItem();
			this.researchItem4 = new Winforms_Mockups.ResearchItem();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.controlListView1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(381, 26);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "Level 2";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(381, 12);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(89, 13);
			this.label4.TabIndex = 17;
			this.label4.Text = "Hydroponic farms";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(295, 98);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(250, 193);
			this.textBox1.TabIndex = 16;
			this.textBox1.Text = "Improved hydroponic farming. Lore ipsum\r\nDoloret reat msddo mowerr maugad sad\r\nda" +
			"od coksal. Je sorof msoerta naue usfa.\r\n\r\nFood per population: +0.25\r\nFood per f" +
			"armer: +0.5\r\nPopulation growth: +25%";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Winforms_Mockups.Properties.Resources.hydroponic_farms;
			this.pictureBox1.Location = new System.Drawing.Point(295, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(80, 80);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 15;
			this.pictureBox1.TabStop = false;
			// 
			// controlListView1
			// 
			this.controlListView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.controlListView1.AutoScroll = true;
			this.controlListView1.BackColor = System.Drawing.SystemColors.Control;
			this.controlListView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.controlListView1.Controls.Add(this.researchItem1);
			this.controlListView1.Controls.Add(this.researchItem2);
			this.controlListView1.Controls.Add(this.researchItem3);
			this.controlListView1.Controls.Add(this.researchItem4);
			this.controlListView1.Location = new System.Drawing.Point(12, 12);
			this.controlListView1.Name = "controlListView1";
			this.controlListView1.Size = new System.Drawing.Size(277, 385);
			this.controlListView1.TabIndex = 14;
			// 
			// researchItem1
			// 
			this.researchItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(196)))), ((int)(((byte)(255)))));
			this.researchItem1.Location = new System.Drawing.Point(3, 3);
			this.researchItem1.Name = "researchItem1";
			this.researchItem1.Size = new System.Drawing.Size(250, 50);
			this.researchItem1.TabIndex = 0;
			// 
			// researchItem2
			// 
			this.researchItem2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(196)))), ((int)(((byte)(255)))));
			this.researchItem2.Location = new System.Drawing.Point(3, 59);
			this.researchItem2.Name = "researchItem2";
			this.researchItem2.Size = new System.Drawing.Size(250, 50);
			this.researchItem2.TabIndex = 1;
			// 
			// researchItem3
			// 
			this.researchItem3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(196)))), ((int)(((byte)(255)))));
			this.researchItem3.Location = new System.Drawing.Point(3, 115);
			this.researchItem3.Name = "researchItem3";
			this.researchItem3.Size = new System.Drawing.Size(250, 50);
			this.researchItem3.TabIndex = 2;
			// 
			// researchItem4
			// 
			this.researchItem4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(196)))), ((int)(((byte)(255)))));
			this.researchItem4.Location = new System.Drawing.Point(3, 171);
			this.researchItem4.Name = "researchItem4";
			this.researchItem4.Size = new System.Drawing.Size(250, 50);
			this.researchItem4.TabIndex = 3;
			// 
			// FormResearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(558, 409);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.controlListView1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormResearch";
			this.Text = "FormResearch";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.controlListView1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private Winforms_Mockups.ResearchItem researchItem4;
		private Winforms_Mockups.ResearchItem researchItem3;
		private Winforms_Mockups.ResearchItem researchItem2;
		private Winforms_Mockups.ResearchItem researchItem1;
		private My_ListView.ControlListView controlListView1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
	}
}
