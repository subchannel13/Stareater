
namespace Winforms_Mockups
{
	partial class FromDevelopment
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
			this.controlListView1 = new My_ListView.ControlListView();
			this.researchItem1 = new Winforms_Mockups.ResearchItem();
			this.researchItem2 = new Winforms_Mockups.ResearchItem();
			this.researchItem3 = new Winforms_Mockups.ResearchItem();
			this.researchItem4 = new Winforms_Mockups.ResearchItem();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.lable1 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.controlListView1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// controlListView1
			// 
			this.controlListView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.controlListView1.BackColor = System.Drawing.SystemColors.Window;
			this.controlListView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.controlListView1.Controls.Add(this.researchItem1);
			this.controlListView1.Controls.Add(this.researchItem2);
			this.controlListView1.Controls.Add(this.researchItem3);
			this.controlListView1.Controls.Add(this.researchItem4);
			this.controlListView1.Location = new System.Drawing.Point(12, 12);
			this.controlListView1.Name = "controlListView1";
			this.controlListView1.Size = new System.Drawing.Size(260, 402);
			this.controlListView1.TabIndex = 0;
			// 
			// researchItem1
			// 
			this.researchItem1.BackColor = System.Drawing.SystemColors.Control;
			this.researchItem1.Location = new System.Drawing.Point(3, 3);
			this.researchItem1.Name = "researchItem1";
			this.researchItem1.Size = new System.Drawing.Size(250, 50);
			this.researchItem1.TabIndex = 0;
			// 
			// researchItem2
			// 
			this.researchItem2.BackColor = System.Drawing.SystemColors.Control;
			this.researchItem2.Location = new System.Drawing.Point(3, 59);
			this.researchItem2.Name = "researchItem2";
			this.researchItem2.Size = new System.Drawing.Size(250, 50);
			this.researchItem2.TabIndex = 1;
			// 
			// researchItem3
			// 
			this.researchItem3.BackColor = System.Drawing.SystemColors.Control;
			this.researchItem3.Location = new System.Drawing.Point(3, 115);
			this.researchItem3.Name = "researchItem3";
			this.researchItem3.Size = new System.Drawing.Size(250, 50);
			this.researchItem3.TabIndex = 2;
			// 
			// researchItem4
			// 
			this.researchItem4.BackColor = System.Drawing.SystemColors.Control;
			this.researchItem4.Location = new System.Drawing.Point(3, 171);
			this.researchItem4.Name = "researchItem4";
			this.researchItem4.Size = new System.Drawing.Size(250, 50);
			this.researchItem4.TabIndex = 3;
			// 
			// button1
			// 
			this.button1.Image = global::Winforms_Mockups.Properties.Resources.arrow_first;
			this.button1.Location = new System.Drawing.Point(278, 17);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(50, 50);
			this.button1.TabIndex = 1;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Image = global::Winforms_Mockups.Properties.Resources.arrow_up;
			this.button2.Location = new System.Drawing.Point(278, 73);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(50, 50);
			this.button2.TabIndex = 2;
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Image = global::Winforms_Mockups.Properties.Resources.arrow_down;
			this.button3.Location = new System.Drawing.Point(278, 129);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(50, 50);
			this.button3.TabIndex = 3;
			this.button3.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.Image = global::Winforms_Mockups.Properties.Resources.arrow_last;
			this.button4.Location = new System.Drawing.Point(278, 185);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(50, 50);
			this.button4.TabIndex = 4;
			this.button4.UseVisualStyleBackColor = true;
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(278, 353);
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(210, 45);
			this.trackBar1.TabIndex = 5;
			this.trackBar1.Value = 5;
			// 
			// lable1
			// 
			this.lable1.AutoSize = true;
			this.lable1.Location = new System.Drawing.Point(278, 401);
			this.lable1.Name = "lable1";
			this.lable1.Size = new System.Drawing.Size(32, 13);
			this.lable1.TabIndex = 6;
			this.lable1.Text = "Even";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(440, 401);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Focused";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(278, 337);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Distribution";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(278, 302);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(139, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Development points: 6.12 G";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Winforms_Mockups.Properties.Resources.hydroponic_farms;
			this.pictureBox1.Location = new System.Drawing.Point(375, 17);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(80, 80);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 10;
			this.pictureBox1.TabStop = false;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(461, 17);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(270, 218);
			this.textBox1.TabIndex = 11;
			this.textBox1.Text = "Improved hydroponic farming. Lore ipsum\r\nDoloret reat msddo mowerr maugad sad\r\nda" +
			"od coksal. Je sorof msoerta naue usfa.\r\n\r\nFood per population: +0.25\r\nFood per f" +
			"armer: +0.5\r\nPopulation growth: +25%";
			// 
			// FromDevelopment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(741, 426);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lable1);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.controlListView1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FromDevelopment";
			this.ShowInTaskbar = false;
			this.Text = "FromDevelopment";
			this.controlListView1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lable1;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private Winforms_Mockups.ResearchItem researchItem4;
		private Winforms_Mockups.ResearchItem researchItem3;
		private Winforms_Mockups.ResearchItem researchItem2;
		private Winforms_Mockups.ResearchItem researchItem1;
		private My_ListView.ControlListView controlListView1;
	}
}
