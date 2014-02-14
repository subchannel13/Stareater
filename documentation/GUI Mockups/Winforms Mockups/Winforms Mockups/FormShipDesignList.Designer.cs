/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 13.2.2014.
 * Time: 11:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormShipDesignList
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
			this.btnNoviDizajn = new System.Windows.Forms.Button();
			this.lblDizajn = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.designInfo1 = new Winforms_Mockups.DesignInfo();
			this.label1 = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.designItem1 = new Winforms_Mockups.DesignItem();
			this.designItem2 = new Winforms_Mockups.DesignItem();
			this.designItem3 = new Winforms_Mockups.DesignItem();
			this.designItem4 = new Winforms_Mockups.DesignItem();
			this.designItem5 = new Winforms_Mockups.DesignItem();
			this.designItem6 = new Winforms_Mockups.DesignItem();
			this.designItem7 = new Winforms_Mockups.DesignItem();
			this.panel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnNoviDizajn
			// 
			this.btnNoviDizajn.Image = global::Winforms_Mockups.Properties.Resources.new_design;
			this.btnNoviDizajn.Location = new System.Drawing.Point(12, 347);
			this.btnNoviDizajn.Name = "btnNoviDizajn";
			this.btnNoviDizajn.Size = new System.Drawing.Size(56, 56);
			this.btnNoviDizajn.TabIndex = 12;
			this.btnNoviDizajn.UseVisualStyleBackColor = true;
			// 
			// lblDizajn
			// 
			this.lblDizajn.AutoSize = true;
			this.lblDizajn.Location = new System.Drawing.Point(15, 9);
			this.lblDizajn.Name = "lblDizajn";
			this.lblDizajn.Size = new System.Drawing.Size(40, 13);
			this.lblDizajn.TabIndex = 7;
			this.lblDizajn.Text = "Design";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.designInfo1);
			this.panel1.Location = new System.Drawing.Point(383, 25);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(200, 378);
			this.panel1.TabIndex = 13;
			// 
			// designInfo1
			// 
			this.designInfo1.AutoSize = true;
			this.designInfo1.Location = new System.Drawing.Point(3, 3);
			this.designInfo1.Name = "designInfo1";
			this.designInfo1.Size = new System.Drawing.Size(178, 328);
			this.designInfo1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(265, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "Ships";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Controls.Add(this.designItem7);
			this.flowLayoutPanel1.Controls.Add(this.designItem1);
			this.flowLayoutPanel1.Controls.Add(this.designItem2);
			this.flowLayoutPanel1.Controls.Add(this.designItem3);
			this.flowLayoutPanel1.Controls.Add(this.designItem4);
			this.flowLayoutPanel1.Controls.Add(this.designItem5);
			this.flowLayoutPanel1.Controls.Add(this.designItem6);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 28);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(365, 313);
			this.flowLayoutPanel1.TabIndex = 15;
			// 
			// designItem1
			// 
			this.designItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.designItem1.Location = new System.Drawing.Point(3, 55);
			this.designItem1.Name = "designItem1";
			this.designItem1.Size = new System.Drawing.Size(340, 46);
			this.designItem1.TabIndex = 0;
			// 
			// designItem2
			// 
			this.designItem2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.designItem2.Location = new System.Drawing.Point(3, 107);
			this.designItem2.Name = "designItem2";
			this.designItem2.Size = new System.Drawing.Size(340, 46);
			this.designItem2.TabIndex = 1;
			// 
			// designItem3
			// 
			this.designItem3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.designItem3.Location = new System.Drawing.Point(3, 159);
			this.designItem3.Name = "designItem3";
			this.designItem3.Size = new System.Drawing.Size(340, 46);
			this.designItem3.TabIndex = 2;
			// 
			// designItem4
			// 
			this.designItem4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.designItem4.Location = new System.Drawing.Point(3, 211);
			this.designItem4.Name = "designItem4";
			this.designItem4.Size = new System.Drawing.Size(340, 46);
			this.designItem4.TabIndex = 3;
			// 
			// designItem5
			// 
			this.designItem5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.designItem5.Location = new System.Drawing.Point(3, 263);
			this.designItem5.Name = "designItem5";
			this.designItem5.Size = new System.Drawing.Size(340, 46);
			this.designItem5.TabIndex = 4;
			// 
			// designItem6
			// 
			this.designItem6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.designItem6.Location = new System.Drawing.Point(3, 315);
			this.designItem6.Name = "designItem6";
			this.designItem6.Size = new System.Drawing.Size(340, 46);
			this.designItem6.TabIndex = 5;
			// 
			// designItem7
			// 
			this.designItem7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
			this.designItem7.Location = new System.Drawing.Point(3, 3);
			this.designItem7.Name = "designItem7";
			this.designItem7.Size = new System.Drawing.Size(340, 46);
			this.designItem7.TabIndex = 6;
			// 
			// FormShipDesignList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(594, 414);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnNoviDizajn);
			this.Controls.Add(this.lblDizajn);
			this.Name = "FormShipDesignList";
			this.Text = "FormShipDesignList";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private Winforms_Mockups.DesignItem designItem6;
		private Winforms_Mockups.DesignItem designItem5;
		private Winforms_Mockups.DesignItem designItem4;
		private Winforms_Mockups.DesignItem designItem3;
		private Winforms_Mockups.DesignItem designItem7;
		private Winforms_Mockups.DesignItem designItem2;
		private Winforms_Mockups.DesignItem designItem1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private Winforms_Mockups.DesignInfo designInfo1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblDizajn;
		private System.Windows.Forms.Button btnNoviDizajn;
	}
}
