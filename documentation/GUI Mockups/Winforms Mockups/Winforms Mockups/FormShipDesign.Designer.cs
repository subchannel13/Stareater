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
			this.button6 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.specialEquipmentItem1 = new Winforms_Mockups.SpecialEquipmentItem();
			this.specialEquipmentItem2 = new Winforms_Mockups.SpecialEquipmentItem();
			this.specialEquipmentItem3 = new Winforms_Mockups.SpecialEquipmentItem();
			this.specialEquipmentItem4 = new Winforms_Mockups.SpecialEquipmentItem();
			this.specialEquipmentItem5 = new Winforms_Mockups.SpecialEquipmentItem();
			this.specialEquipmentItem6 = new Winforms_Mockups.SpecialEquipmentItem();
			this.button5 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Winforms_Mockups.Properties.Resources.small_cruiser;
			this.pictureBox1.Location = new System.Drawing.Point(26, 12);
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
			this.button1.Location = new System.Drawing.Point(9, 43);
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
			this.button2.Location = new System.Drawing.Point(107, 43);
			this.button2.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(16, 16);
			this.button2.TabIndex = 2;
			this.button2.UseVisualStyleBackColor = true;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(9, 98);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(120, 20);
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
			this.comboBox1.Location = new System.Drawing.Point(9, 124);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(120, 21);
			this.comboBox1.TabIndex = 4;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(180, 12);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(153, 24);
			this.button6.TabIndex = 13;
			this.button6.Text = "200 x Colonizer";
			this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.button6.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(180, 42);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(153, 24);
			this.button3.TabIndex = 14;
			this.button3.Text = "1.5 k x Laser";
			this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.button3.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(180, 72);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(153, 24);
			this.button4.TabIndex = 15;
			this.button4.Text = "EM shield";
			this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.button4.UseVisualStyleBackColor = true;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::Winforms_Mockups.Properties.Resources.colonizer;
			this.pictureBox2.Location = new System.Drawing.Point(150, 12);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(24, 24);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox2.TabIndex = 16;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = global::Winforms_Mockups.Properties.Resources.laser;
			this.pictureBox3.Location = new System.Drawing.Point(150, 42);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(24, 24);
			this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox3.TabIndex = 17;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox4
			// 
			this.pictureBox4.Image = global::Winforms_Mockups.Properties.Resources.radiation_shield;
			this.pictureBox4.Location = new System.Drawing.Point(150, 72);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(24, 24);
			this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox4.TabIndex = 18;
			this.pictureBox4.TabStop = false;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem1);
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem2);
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem3);
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem4);
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem5);
			this.flowLayoutPanel1.Controls.Add(this.specialEquipmentItem6);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(150, 102);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(205, 132);
			this.flowLayoutPanel1.TabIndex = 19;
			// 
			// specialEquipmentItem1
			// 
			this.specialEquipmentItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem1.Location = new System.Drawing.Point(3, 3);
			this.specialEquipmentItem1.Name = "specialEquipmentItem1";
			this.specialEquipmentItem1.Size = new System.Drawing.Size(180, 20);
			this.specialEquipmentItem1.TabIndex = 0;
			// 
			// specialEquipmentItem2
			// 
			this.specialEquipmentItem2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem2.Location = new System.Drawing.Point(3, 29);
			this.specialEquipmentItem2.Name = "specialEquipmentItem2";
			this.specialEquipmentItem2.Size = new System.Drawing.Size(180, 20);
			this.specialEquipmentItem2.TabIndex = 1;
			// 
			// specialEquipmentItem3
			// 
			this.specialEquipmentItem3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem3.Location = new System.Drawing.Point(3, 55);
			this.specialEquipmentItem3.Name = "specialEquipmentItem3";
			this.specialEquipmentItem3.Size = new System.Drawing.Size(180, 20);
			this.specialEquipmentItem3.TabIndex = 2;
			// 
			// specialEquipmentItem4
			// 
			this.specialEquipmentItem4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem4.Location = new System.Drawing.Point(3, 81);
			this.specialEquipmentItem4.Name = "specialEquipmentItem4";
			this.specialEquipmentItem4.Size = new System.Drawing.Size(180, 20);
			this.specialEquipmentItem4.TabIndex = 3;
			// 
			// specialEquipmentItem5
			// 
			this.specialEquipmentItem5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem5.Location = new System.Drawing.Point(3, 107);
			this.specialEquipmentItem5.Name = "specialEquipmentItem5";
			this.specialEquipmentItem5.Size = new System.Drawing.Size(180, 20);
			this.specialEquipmentItem5.TabIndex = 4;
			// 
			// specialEquipmentItem6
			// 
			this.specialEquipmentItem6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.specialEquipmentItem6.Location = new System.Drawing.Point(3, 133);
			this.specialEquipmentItem6.Name = "specialEquipmentItem6";
			this.specialEquipmentItem6.Size = new System.Drawing.Size(180, 20);
			this.specialEquipmentItem6.TabIndex = 5;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(361, 102);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(23, 23);
			this.button5.TabIndex = 20;
			this.button5.Text = "+";
			this.button5.UseVisualStyleBackColor = true;
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(361, 128);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(23, 23);
			this.button7.TabIndex = 21;
			this.button7.Text = "-";
			this.button7.UseVisualStyleBackColor = true;
			// 
			// button8
			// 
			this.button8.BackgroundImage = global::Winforms_Mockups.Properties.Resources.cancel;
			this.button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button8.Location = new System.Drawing.Point(361, 154);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(23, 23);
			this.button8.TabIndex = 22;
			this.button8.UseVisualStyleBackColor = true;
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(272, 278);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(75, 23);
			this.button9.TabIndex = 23;
			this.button9.Text = "Build";
			this.button9.UseVisualStyleBackColor = true;
			// 
			// FormShipDesign
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(415, 307);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.pictureBox4);
			this.Controls.Add(this.pictureBox3);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormShipDesign";
			this.Text = "FormShipDesign";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button button9;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem6;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem5;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem4;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem3;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem2;
		private Winforms_Mockups.SpecialEquipmentItem specialEquipmentItem1;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}
