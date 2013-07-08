namespace Winforms_Mockups
{
	partial class FormStarSystem
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
			this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
			this.ovalShape1 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
			this.ovalShape2 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
			this.ovalShape3 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
			this.ovalShape4 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
			this.ovalShape5 = new Microsoft.VisualBasic.PowerPacks.OvalShape();
			this.panel1 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// shapeContainer1
			// 
			this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
			this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
			this.shapeContainer1.Name = "shapeContainer1";
			this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.ovalShape5,
            this.ovalShape4,
            this.ovalShape3,
            this.ovalShape2,
            this.ovalShape1});
			this.shapeContainer1.Size = new System.Drawing.Size(568, 401);
			this.shapeContainer1.TabIndex = 0;
			this.shapeContainer1.TabStop = false;
			// 
			// ovalShape1
			// 
			this.ovalShape1.FillColor = System.Drawing.Color.Yellow;
			this.ovalShape1.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
			this.ovalShape1.Location = new System.Drawing.Point(-111, 11);
			this.ovalShape1.Name = "ovalShape1";
			this.ovalShape1.Size = new System.Drawing.Size(250, 250);
			// 
			// ovalShape2
			// 
			this.ovalShape2.BorderColor = System.Drawing.Color.Lime;
			this.ovalShape2.Location = new System.Drawing.Point(-248, -128);
			this.ovalShape2.Name = "ovalShape2";
			this.ovalShape2.Size = new System.Drawing.Size(500, 500);
			// 
			// ovalShape3
			// 
			this.ovalShape3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.ovalShape3.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
			this.ovalShape3.Location = new System.Drawing.Point(214, 102);
			this.ovalShape3.Name = "ovalShape3";
			this.ovalShape3.Size = new System.Drawing.Size(80, 80);
			// 
			// ovalShape4
			// 
			this.ovalShape4.BorderColor = System.Drawing.Color.Lime;
			this.ovalShape4.Location = new System.Drawing.Point(-374, -282);
			this.ovalShape4.Name = "ovalShape4";
			this.ovalShape4.Size = new System.Drawing.Size(800, 800);
			// 
			// ovalShape5
			// 
			this.ovalShape5.FillColor = System.Drawing.Color.Teal;
			this.ovalShape5.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
			this.ovalShape5.Location = new System.Drawing.Point(380, 104);
			this.ovalShape5.Name = "ovalShape5";
			this.ovalShape5.Size = new System.Drawing.Size(80, 80);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.BackgroundImage = global::Winforms_Mockups.Properties.Resources.metalic;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.hScrollBar1);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Location = new System.Drawing.Point(122, 296);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(316, 105);
			this.panel1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(8, 11);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 80);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Location = new System.Drawing.Point(90, 13);
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(206, 17);
			this.hScrollBar1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(87, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			// 
			// FormStarSystem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(568, 401);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.shapeContainer1);
			this.Name = "FormStarSystem";
			this.Text = "FormStarSystem";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
		private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape1;
		private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape5;
		private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape4;
		private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape3;
		private Microsoft.VisualBasic.PowerPacks.OvalShape ovalShape2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.HScrollBar hScrollBar1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;

	}
}