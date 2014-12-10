/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 10.11.2014.
 * Time: 14:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormMainFleet
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
			this.button1 = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.shipGroupItem8 = new Winforms_Mockups.ShipGroupItem();
			this.shipGroupItem9 = new Winforms_Mockups.ShipGroupItem();
			this.shipGroupItem10 = new Winforms_Mockups.ShipGroupItem();
			this.shipGroupItem11 = new Winforms_Mockups.ShipGroupItem();
			this.shipGroupItem12 = new Winforms_Mockups.ShipGroupItem();
			this.shipGroupItem13 = new Winforms_Mockups.ShipGroupItem();
			this.shipGroupItem14 = new Winforms_Mockups.ShipGroupItem();
			this.button2 = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(802, 419);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 80);
			this.button1.TabIndex = 1;
			this.button1.Text = "End turn";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.panel1.Controls.Add(this.flowLayoutPanel2);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Location = new System.Drawing.Point(241, 383);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(402, 116);
			this.panel1.TabIndex = 3;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoScroll = true;
			this.flowLayoutPanel2.Controls.Add(this.shipGroupItem8);
			this.flowLayoutPanel2.Controls.Add(this.shipGroupItem9);
			this.flowLayoutPanel2.Controls.Add(this.shipGroupItem10);
			this.flowLayoutPanel2.Controls.Add(this.shipGroupItem11);
			this.flowLayoutPanel2.Controls.Add(this.shipGroupItem12);
			this.flowLayoutPanel2.Controls.Add(this.shipGroupItem13);
			this.flowLayoutPanel2.Controls.Add(this.shipGroupItem14);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(332, 109);
			this.flowLayoutPanel2.TabIndex = 3;
			// 
			// shipGroupItem8
			// 
			this.shipGroupItem8.Location = new System.Drawing.Point(3, 3);
			this.shipGroupItem8.Name = "shipGroupItem8";
			this.shipGroupItem8.Size = new System.Drawing.Size(150, 30);
			this.shipGroupItem8.TabIndex = 0;
			// 
			// shipGroupItem9
			// 
			this.shipGroupItem9.Location = new System.Drawing.Point(159, 3);
			this.shipGroupItem9.Name = "shipGroupItem9";
			this.shipGroupItem9.Size = new System.Drawing.Size(150, 30);
			this.shipGroupItem9.TabIndex = 1;
			// 
			// shipGroupItem10
			// 
			this.shipGroupItem10.Location = new System.Drawing.Point(3, 39);
			this.shipGroupItem10.Name = "shipGroupItem10";
			this.shipGroupItem10.Size = new System.Drawing.Size(150, 30);
			this.shipGroupItem10.TabIndex = 2;
			// 
			// shipGroupItem11
			// 
			this.shipGroupItem11.Location = new System.Drawing.Point(159, 39);
			this.shipGroupItem11.Name = "shipGroupItem11";
			this.shipGroupItem11.Size = new System.Drawing.Size(150, 30);
			this.shipGroupItem11.TabIndex = 3;
			// 
			// shipGroupItem12
			// 
			this.shipGroupItem12.Location = new System.Drawing.Point(3, 75);
			this.shipGroupItem12.Name = "shipGroupItem12";
			this.shipGroupItem12.Size = new System.Drawing.Size(150, 30);
			this.shipGroupItem12.TabIndex = 6;
			// 
			// shipGroupItem13
			// 
			this.shipGroupItem13.Location = new System.Drawing.Point(159, 75);
			this.shipGroupItem13.Name = "shipGroupItem13";
			this.shipGroupItem13.Size = new System.Drawing.Size(150, 30);
			this.shipGroupItem13.TabIndex = 4;
			// 
			// shipGroupItem14
			// 
			this.shipGroupItem14.Location = new System.Drawing.Point(3, 111);
			this.shipGroupItem14.Name = "shipGroupItem14";
			this.shipGroupItem14.Size = new System.Drawing.Size(150, 30);
			this.shipGroupItem14.TabIndex = 5;
			// 
			// button2
			// 
			this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button2.Location = new System.Drawing.Point(338, 3);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(60, 23);
			this.button2.TabIndex = 0;
			this.button2.Text = "Missions";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// FormMainFleet
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::Winforms_Mockups.Properties.Resources.galaxy_map;
			this.ClientSize = new System.Drawing.Size(884, 502);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.button1);
			this.Name = "FormMainFleet";
			this.Text = "FormMainFleet";
			this.panel1.ResumeLayout(false);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private Winforms_Mockups.ShipGroupItem shipGroupItem14;
		private Winforms_Mockups.ShipGroupItem shipGroupItem13;
		private Winforms_Mockups.ShipGroupItem shipGroupItem12;
		private Winforms_Mockups.ShipGroupItem shipGroupItem11;
		private Winforms_Mockups.ShipGroupItem shipGroupItem10;
		private Winforms_Mockups.ShipGroupItem shipGroupItem9;
		private Winforms_Mockups.ShipGroupItem shipGroupItem8;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button1;
	}
}
