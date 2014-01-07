/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 7.1.2014.
 * Time: 8:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormColony
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.controlListView1 = new System.Windows.Forms.FlowLayoutPanel();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.pictureBox5 = new System.Windows.Forms.PictureBox();
			this.pictureBox6 = new System.Windows.Forms.PictureBox();
			this.pictureBox7 = new System.Windows.Forms.PictureBox();
			this.pictureBox8 = new System.Windows.Forms.PictureBox();
			this.pictureBox9 = new System.Windows.Forms.PictureBox();
			this.pictureBox10 = new System.Windows.Forms.PictureBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.buildingItem1 = new Winforms_Mockups.BuildingItem();
			this.buildingItem2 = new Winforms_Mockups.BuildingItem();
			this.buildingItem3 = new Winforms_Mockups.BuildingItem();
			this.buildingItem4 = new Winforms_Mockups.BuildingItem();
			this.buildingItem5 = new Winforms_Mockups.BuildingItem();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.controlListView1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Winforms_Mockups.Properties.Resources.earthlike_planet;
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(64, 64);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "3,67 G / 8,205 G";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Growth: +0,146 G";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(101, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Infrastructure: 8,9 %";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(82, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(116, 64);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Population";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.controlListView1);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Location = new System.Drawing.Point(204, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(185, 131);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Planet";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 29);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(92, 13);
			this.label5.TabIndex = 1;
			this.label5.Text = "Environment: 65%";
			// 
			// controlListView1
			// 
			this.controlListView1.AutoScroll = true;
			this.controlListView1.Controls.Add(this.pictureBox2);
			this.controlListView1.Controls.Add(this.pictureBox3);
			this.controlListView1.Controls.Add(this.pictureBox4);
			this.controlListView1.Controls.Add(this.pictureBox5);
			this.controlListView1.Controls.Add(this.pictureBox6);
			this.controlListView1.Controls.Add(this.pictureBox7);
			this.controlListView1.Controls.Add(this.pictureBox8);
			this.controlListView1.Controls.Add(this.pictureBox9);
			this.controlListView1.Controls.Add(this.pictureBox10);
			this.controlListView1.Location = new System.Drawing.Point(6, 45);
			this.controlListView1.Name = "controlListView1";
			this.controlListView1.Size = new System.Drawing.Size(172, 76);
			this.controlListView1.TabIndex = 6;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::Winforms_Mockups.Properties.Resources.sun_rays;
			this.pictureBox2.Location = new System.Drawing.Point(3, 3);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(32, 32);
			this.pictureBox2.TabIndex = 2;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = global::Winforms_Mockups.Properties.Resources.sun_rays;
			this.pictureBox3.Location = new System.Drawing.Point(41, 3);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(32, 32);
			this.pictureBox3.TabIndex = 3;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox4
			// 
			this.pictureBox4.Image = global::Winforms_Mockups.Properties.Resources.sun_rays;
			this.pictureBox4.Location = new System.Drawing.Point(79, 3);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(32, 32);
			this.pictureBox4.TabIndex = 4;
			this.pictureBox4.TabStop = false;
			// 
			// pictureBox5
			// 
			this.pictureBox5.Image = global::Winforms_Mockups.Properties.Resources.sun_rays;
			this.pictureBox5.Location = new System.Drawing.Point(117, 3);
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.Size = new System.Drawing.Size(32, 32);
			this.pictureBox5.TabIndex = 5;
			this.pictureBox5.TabStop = false;
			// 
			// pictureBox6
			// 
			this.pictureBox6.Image = global::Winforms_Mockups.Properties.Resources.sun_rays;
			this.pictureBox6.Location = new System.Drawing.Point(3, 41);
			this.pictureBox6.Name = "pictureBox6";
			this.pictureBox6.Size = new System.Drawing.Size(32, 32);
			this.pictureBox6.TabIndex = 6;
			this.pictureBox6.TabStop = false;
			// 
			// pictureBox7
			// 
			this.pictureBox7.Image = global::Winforms_Mockups.Properties.Resources.sun_rays;
			this.pictureBox7.Location = new System.Drawing.Point(41, 41);
			this.pictureBox7.Name = "pictureBox7";
			this.pictureBox7.Size = new System.Drawing.Size(32, 32);
			this.pictureBox7.TabIndex = 7;
			this.pictureBox7.TabStop = false;
			// 
			// pictureBox8
			// 
			this.pictureBox8.Image = global::Winforms_Mockups.Properties.Resources.sun_rays;
			this.pictureBox8.Location = new System.Drawing.Point(79, 41);
			this.pictureBox8.Name = "pictureBox8";
			this.pictureBox8.Size = new System.Drawing.Size(32, 32);
			this.pictureBox8.TabIndex = 8;
			this.pictureBox8.TabStop = false;
			// 
			// pictureBox9
			// 
			this.pictureBox9.Image = global::Winforms_Mockups.Properties.Resources.sun_rays;
			this.pictureBox9.Location = new System.Drawing.Point(117, 41);
			this.pictureBox9.Name = "pictureBox9";
			this.pictureBox9.Size = new System.Drawing.Size(32, 32);
			this.pictureBox9.TabIndex = 9;
			this.pictureBox9.TabStop = false;
			// 
			// pictureBox10
			// 
			this.pictureBox10.Image = global::Winforms_Mockups.Properties.Resources.sun_rays;
			this.pictureBox10.Location = new System.Drawing.Point(3, 79);
			this.pictureBox10.Name = "pictureBox10";
			this.pictureBox10.Size = new System.Drawing.Size(32, 32);
			this.pictureBox10.TabIndex = 10;
			this.pictureBox10.TabStop = false;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(51, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Size: 120";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.flowLayoutPanel1);
			this.groupBox3.Location = new System.Drawing.Point(12, 82);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(186, 201);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Buildings";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Controls.Add(this.buildingItem1);
			this.flowLayoutPanel1.Controls.Add(this.buildingItem2);
			this.flowLayoutPanel1.Controls.Add(this.buildingItem3);
			this.flowLayoutPanel1.Controls.Add(this.buildingItem4);
			this.flowLayoutPanel1.Controls.Add(this.buildingItem5);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 19);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(174, 176);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// buildingItem1
			// 
			this.buildingItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.buildingItem1.Location = new System.Drawing.Point(3, 3);
			this.buildingItem1.Name = "buildingItem1";
			this.buildingItem1.Size = new System.Drawing.Size(150, 38);
			this.buildingItem1.TabIndex = 0;
			// 
			// buildingItem2
			// 
			this.buildingItem2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.buildingItem2.Location = new System.Drawing.Point(3, 47);
			this.buildingItem2.Name = "buildingItem2";
			this.buildingItem2.Size = new System.Drawing.Size(150, 38);
			this.buildingItem2.TabIndex = 1;
			// 
			// buildingItem3
			// 
			this.buildingItem3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.buildingItem3.Location = new System.Drawing.Point(3, 91);
			this.buildingItem3.Name = "buildingItem3";
			this.buildingItem3.Size = new System.Drawing.Size(150, 38);
			this.buildingItem3.TabIndex = 2;
			// 
			// buildingItem4
			// 
			this.buildingItem4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.buildingItem4.Location = new System.Drawing.Point(3, 135);
			this.buildingItem4.Name = "buildingItem4";
			this.buildingItem4.Size = new System.Drawing.Size(150, 38);
			this.buildingItem4.TabIndex = 3;
			// 
			// buildingItem5
			// 
			this.buildingItem5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.buildingItem5.Location = new System.Drawing.Point(3, 179);
			this.buildingItem5.Name = "buildingItem5";
			this.buildingItem5.Size = new System.Drawing.Size(150, 38);
			this.buildingItem5.TabIndex = 4;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label11);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Controls.Add(this.label8);
			this.groupBox4.Controls.Add(this.label7);
			this.groupBox4.Controls.Add(this.label6);
			this.groupBox4.Location = new System.Drawing.Point(204, 149);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(185, 134);
			this.groupBox4.TabIndex = 7;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Productivity";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 90);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(114, 13);
			this.label9.TabIndex = 3;
			this.label9.Text = "Development: 1,627 G";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 77);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(88, 13);
			this.label8.TabIndex = 2;
			this.label8.Text = "Industry: 4,403 G";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 29);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(88, 13);
			this.label7.TabIndex = 1;
			this.label7.Text = "Mining: 4,4 / pop";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(81, 13);
			this.label6.TabIndex = 0;
			this.label6.Text = "Food: 3,2 / pop";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(6, 43);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(94, 13);
			this.label10.TabIndex = 4;
			this.label10.Text = "Industry: 2,6 / pop";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(6, 56);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(149, 13);
			this.label11.TabIndex = 5;
			this.label11.Text = "Tech. development: 2,8 / pop";
			// 
			// FormColony
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(398, 295);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.groupBox3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormColony";
			this.Text = "Alpha Centaury IV";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.controlListView1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.GroupBox groupBox4;
		private Winforms_Mockups.BuildingItem buildingItem5;
		private Winforms_Mockups.BuildingItem buildingItem4;
		private Winforms_Mockups.BuildingItem buildingItem3;
		private Winforms_Mockups.BuildingItem buildingItem2;
		private Winforms_Mockups.BuildingItem buildingItem1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.PictureBox pictureBox10;
		private System.Windows.Forms.PictureBox pictureBox9;
		private System.Windows.Forms.PictureBox pictureBox8;
		private System.Windows.Forms.PictureBox pictureBox7;
		private System.Windows.Forms.PictureBox pictureBox6;
		private System.Windows.Forms.PictureBox pictureBox5;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.FlowLayoutPanel controlListView1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}
