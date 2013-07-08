namespace Winforms_Mockups
{
	partial class FormNewGame
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.button4 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.newPlayerInfo1 = new Winforms_Mockups.NewPlayerInfo();
			this.newPlayerInfo2 = new Winforms_Mockups.NewPlayerInfo();
			this.newPlayerInfo3 = new Winforms_Mockups.NewPlayerInfo();
			this.newPlayerInfo4 = new Winforms_Mockups.NewPlayerInfo();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(188, 41);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.ReadOnly = true;
			this.textBox3.Size = new System.Drawing.Size(170, 81);
			this.textBox3.TabIndex = 8;
			this.textBox3.Text = "Rectangular\r\n\r\n36 stars";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(188, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(170, 23);
			this.button1.TabIndex = 9;
			this.button1.Text = "Map";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(188, 157);
			this.textBox4.Multiline = true;
			this.textBox4.Name = "textBox4";
			this.textBox4.ReadOnly = true;
			this.textBox4.Size = new System.Drawing.Size(170, 81);
			this.textBox4.TabIndex = 12;
			this.textBox4.Text = "Population: 7 G\r\nColonies: 2\r\nStar systems: 1";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(283, 244);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 13;
			this.button3.Text = "Start";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.newPlayerInfo1);
			this.flowLayoutPanel1.Controls.Add(this.newPlayerInfo2);
			this.flowLayoutPanel1.Controls.Add(this.newPlayerInfo3);
			this.flowLayoutPanel1.Controls.Add(this.newPlayerInfo4);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 41);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(170, 191);
			this.flowLayoutPanel1.TabIndex = 14;
			// 
			// button4
			// 
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button4.Location = new System.Drawing.Point(12, 12);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(170, 23);
			this.button4.TabIndex = 15;
			this.button4.Text = "Setup players";
			this.button4.UseVisualStyleBackColor = true;
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(188, 128);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(170, 21);
			this.comboBox1.TabIndex = 16;
			// 
			// newPlayerInfo1
			// 
			this.newPlayerInfo1.Location = new System.Drawing.Point(3, 3);
			this.newPlayerInfo1.Name = "newPlayerInfo1";
			this.newPlayerInfo1.Size = new System.Drawing.Size(162, 40);
			this.newPlayerInfo1.TabIndex = 0;
			// 
			// newPlayerInfo2
			// 
			this.newPlayerInfo2.Location = new System.Drawing.Point(3, 49);
			this.newPlayerInfo2.Name = "newPlayerInfo2";
			this.newPlayerInfo2.Size = new System.Drawing.Size(162, 40);
			this.newPlayerInfo2.TabIndex = 1;
			// 
			// newPlayerInfo3
			// 
			this.newPlayerInfo3.Location = new System.Drawing.Point(3, 95);
			this.newPlayerInfo3.Name = "newPlayerInfo3";
			this.newPlayerInfo3.Size = new System.Drawing.Size(162, 40);
			this.newPlayerInfo3.TabIndex = 2;
			// 
			// newPlayerInfo4
			// 
			this.newPlayerInfo4.Location = new System.Drawing.Point(3, 141);
			this.newPlayerInfo4.Name = "newPlayerInfo4";
			this.newPlayerInfo4.Size = new System.Drawing.Size(162, 40);
			this.newPlayerInfo4.TabIndex = 3;
			// 
			// FormNewGame
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(375, 279);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.textBox4);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox3);
			this.Name = "FormNewGame";
			this.Text = "FormNewGame";
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private NewPlayerInfo newPlayerInfo1;
		private NewPlayerInfo newPlayerInfo2;
		private NewPlayerInfo newPlayerInfo3;
		private NewPlayerInfo newPlayerInfo4;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.ComboBox comboBox1;
	}
}