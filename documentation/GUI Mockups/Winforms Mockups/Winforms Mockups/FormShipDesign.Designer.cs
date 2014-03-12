/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 13.2.2014.
 * Time: 11:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormShipDesign
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.button4 = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.specialEquipmentItem6 = new Winforms_Mockups.SpecialEquipmentItem();
			this.specialEquipmentItem2 = new Winforms_Mockups.SpecialEquipmentItem();
			this.label1 = new System.Windows.Forms.Label();
			this.specialEquipmentItem3 = new Winforms_Mockups.SpecialEquipmentItem();
			this.specialEquipmentItem4 = new Winforms_Mockups.SpecialEquipmentItem();
			this.specialEquipmentItem5 = new Winforms_Mockups.SpecialEquipmentItem();
			this.specialEquipmentItem1 = new Winforms_Mockups.SpecialEquipmentItem();
			this.button5 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.pictureBox5 = new System.Windows.Forms.PictureBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.spaceInfo1 = new Winforms_Mockups.SpaceInfo();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Winforms_Mockups.Properties.Resources.small_cruiser;
			this.pictureBox1.Location = new System.Drawing.Point(26, 20);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(80, 80);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// button1
			// 
			this.button1.BackgroundImage = global::Winforms_Mockups.Properties.Resources.arrowLeft;
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button1.Location = new System.Drawing.Point(9, 50);
			this.button1.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(16, 16);
			this.button1.TabIndex = 1;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.BackgroundImage = global::Winforms_Mockups.Properties.Resources.arrowRight;
			this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button2.Location = new System.Drawing.Point(107, 50);
			this.button2.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(16, 16);
			this.button2.TabIndex = 2;
			this.button2.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(129, 12);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(170, 20);
			this.textBox1.TabIndex = 3;
			this.textBox1.Text = "Colonizer";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
									"Cruiser",
									"Fighter"});
			this.comboBox1.Location = new System.Drawing.Point(129, 38);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(170, 21);
			this.comboBox1.TabIndex = 4;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(159, 95);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(140, 24);
			this.button4.TabIndex = 15;
			this.button4.Text = "EM shield";
			this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.button4.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem6);
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem2);
			this.flowLayoutPanel1.Controls.Add(this.label1);
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem3);
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem4);
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem5);
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem1);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 125);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(400, 203);
			this.flowLayoutPanel1.TabIndex = 19;
			// 
			// specialEquipmentItem6
			// 
			this.specialEquipmentItem6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem6.Location = new System.Drawing.Point(3, 3);
			this.specialEquipmentItem6.Name = "specialEquipmentItem6";
			this.specialEquipmentItem6.Size = new System.Drawing.Size(375, 30);
			this.specialEquipmentItem6.TabIndex = 8;
			// 
			// specialEquipmentItem2
			// 
			this.specialEquipmentItem2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem2.Location = new System.Drawing.Point(3, 39);
			this.specialEquipmentItem2.Name = "specialEquipmentItem2";
			this.specialEquipmentItem2.Size = new System.Drawing.Size(375, 30);
			this.specialEquipmentItem2.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(3, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(375, 25);
			this.label1.TabIndex = 2;
			this.label1.Text = "Special equipment";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// specialEquipmentItem3
			// 
			this.specialEquipmentItem3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem3.Location = new System.Drawing.Point(3, 100);
			this.specialEquipmentItem3.Name = "specialEquipmentItem3";
			this.specialEquipmentItem3.Size = new System.Drawing.Size(375, 30);
			this.specialEquipmentItem3.TabIndex = 5;
			// 
			// specialEquipmentItem4
			// 
			this.specialEquipmentItem4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem4.Location = new System.Drawing.Point(3, 136);
			this.specialEquipmentItem4.Name = "specialEquipmentItem4";
			this.specialEquipmentItem4.Size = new System.Drawing.Size(375, 30);
			this.specialEquipmentItem4.TabIndex = 6;
			// 
			// specialEquipmentItem5
			// 
			this.specialEquipmentItem5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem5.Location = new System.Drawing.Point(3, 172);
			this.specialEquipmentItem5.Name = "specialEquipmentItem5";
			this.specialEquipmentItem5.Size = new System.Drawing.Size(375, 30);
			this.specialEquipmentItem5.TabIndex = 7;
			// 
			// specialEquipmentItem1
			// 
			this.specialEquipmentItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem1.Location = new System.Drawing.Point(3, 208);
			this.specialEquipmentItem1.Name = "specialEquipmentItem1";
			this.specialEquipmentItem1.Size = new System.Drawing.Size(375, 30);
			this.specialEquipmentItem1.TabIndex = 3;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(159, 334);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(30, 30);
			this.button5.TabIndex = 20;
			this.button5.Text = "+";
			this.button5.UseVisualStyleBackColor = true;
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(195, 334);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(30, 30);
			this.button7.TabIndex = 21;
			this.button7.Text = "-";
			this.button7.UseVisualStyleBackColor = true;
			// 
			// button8
			// 
			this.button8.BackgroundImage = global::Winforms_Mockups.Properties.Resources.cancel;
			this.button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button8.Location = new System.Drawing.Point(382, 331);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(30, 30);
			this.button8.TabIndex = 22;
			this.button8.UseVisualStyleBackColor = true;
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(337, 377);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(75, 23);
			this.button9.TabIndex = 23;
			this.button9.Text = "Build";
			this.button9.UseVisualStyleBackColor = true;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(159, 70);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(110, 17);
			this.checkBox1.TabIndex = 24;
			this.checkBox1.Text = "IS drive (2 ly/turn)";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// pictureBox4
			// 
			this.pictureBox4.Image = global::Winforms_Mockups.Properties.Resources.radiation_shield;
			this.pictureBox4.Location = new System.Drawing.Point(129, 95);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(24, 24);
			this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox4.TabIndex = 25;
			this.pictureBox4.TabStop = false;
			// 
			// pictureBox5
			// 
			this.pictureBox5.BackColor = System.Drawing.Color.Black;
			this.pictureBox5.Location = new System.Drawing.Point(129, 65);
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.Size = new System.Drawing.Size(24, 24);
			this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox5.TabIndex = 26;
			this.pictureBox5.TabStop = false;
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button3.ForeColor = System.Drawing.Color.DarkGreen;
			this.button3.Location = new System.Drawing.Point(12, 334);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(30, 30);
			this.button3.TabIndex = 27;
			this.button3.Text = "+";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// button6
			// 
			this.button6.Image = global::Winforms_Mockups.Properties.Resources.arrow_up;
			this.button6.Location = new System.Drawing.Point(231, 334);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(30, 30);
			this.button6.TabIndex = 29;
			this.button6.Text = "-";
			this.button6.UseVisualStyleBackColor = true;
			// 
			// spaceInfo1
			// 
			this.spaceInfo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.spaceInfo1.Location = new System.Drawing.Point(12, 377);
			this.spaceInfo1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
			this.spaceInfo1.Name = "spaceInfo1";
			this.spaceInfo1.Size = new System.Drawing.Size(319, 23);
			this.spaceInfo1.TabIndex = 30;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(322, 15);
			this.label2.Margin = new System.Windows.Forms.Padding(20, 0, 3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 31;
			this.label2.Text = "Armor: 800";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(322, 28);
			this.label3.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(54, 13);
			this.label3.TabIndex = 32;
			this.label3.Text = "Mobility: 2";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(323, 62);
			this.label4.Margin = new System.Windows.Forms.Padding(15, 8, 3, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(57, 13);
			this.label4.TabIndex = 33;
			this.label4.Text = "Sensors: 2";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(322, 41);
			this.label5.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(69, 13);
			this.label5.TabIndex = 34;
			this.label5.Text = "Power: 100%";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(323, 75);
			this.label6.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 13);
			this.label6.TabIndex = 35;
			this.label6.Text = "Stealth: 5, 8";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(323, 96);
			this.label7.Margin = new System.Windows.Forms.Padding(15, 8, 3, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(66, 13);
			this.label7.TabIndex = 36;
			this.label7.Text = "Cost: 2,52 G";
			// 
			// FormShipDesign
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(425, 410);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.spaceInfo1);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.pictureBox5);
			this.Controls.Add(this.pictureBox4);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormShipDesign";
			this.Text = "FormShipDesign";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private Winforms_Mockups.SpaceInfo spaceInfo1;
		private System.Windows.Forms.Button button6;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem1;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem5;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem4;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem3;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem2;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem6;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox5;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}
