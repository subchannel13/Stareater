/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 22.9.2015.
 * Time: 15:36
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormColonization
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private My_ListView.ControlListView controlListView1;
		private Winforms_Mockups.ColonizationTarget colonizationTarget1;
		private Winforms_Mockups.ColonizationTarget colonizationTarget2;
		
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
			this.colonizationTarget1 = new Winforms_Mockups.ColonizationTarget();
			this.colonizationTarget2 = new Winforms_Mockups.ColonizationTarget();
			this.selectColonizerAction = new System.Windows.Forms.Button();
			this.colonizerDesignText = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.controlListView2 = new My_ListView.ControlListView();
			this.colonizationSource1 = new Winforms_Mockups.ColonizationSource();
			this.colonizationSource2 = new Winforms_Mockups.ColonizationSource();
			this.colonizationSource3 = new Winforms_Mockups.ColonizationSource();
			this.colonizationSource4 = new Winforms_Mockups.ColonizationSource();
			this.colonizationSource5 = new Winforms_Mockups.ColonizationSource();
			this.colonizationSource6 = new Winforms_Mockups.ColonizationSource();
			this.colonizationSource7 = new Winforms_Mockups.ColonizationSource();
			this.controlListView1.SuspendLayout();
			this.controlListView2.SuspendLayout();
			this.SuspendLayout();
			// 
			// controlListView1
			// 
			this.controlListView1.AutoScroll = true;
			this.controlListView1.Controls.Add(this.colonizationTarget1);
			this.controlListView1.Controls.Add(this.colonizationTarget2);
			this.controlListView1.Location = new System.Drawing.Point(12, 73);
			this.controlListView1.Name = "controlListView1";
			this.controlListView1.Size = new System.Drawing.Size(281, 269);
			this.controlListView1.TabIndex = 0;
			// 
			// colonizationTarget1
			// 
			this.colonizationTarget1.AutoSize = true;
			this.colonizationTarget1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationTarget1.Location = new System.Drawing.Point(3, 3);
			this.colonizationTarget1.Name = "colonizationTarget1";
			this.colonizationTarget1.Size = new System.Drawing.Size(258, 129);
			this.colonizationTarget1.TabIndex = 0;
			// 
			// colonizationTarget2
			// 
			this.colonizationTarget2.AutoSize = true;
			this.colonizationTarget2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationTarget2.Location = new System.Drawing.Point(3, 138);
			this.colonizationTarget2.Name = "colonizationTarget2";
			this.colonizationTarget2.Size = new System.Drawing.Size(258, 129);
			this.colonizationTarget2.TabIndex = 1;
			// 
			// selectColonizerAction
			// 
			this.selectColonizerAction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.selectColonizerAction.Location = new System.Drawing.Point(148, 9);
			this.selectColonizerAction.Name = "selectColonizerAction";
			this.selectColonizerAction.Size = new System.Drawing.Size(124, 36);
			this.selectColonizerAction.TabIndex = 3;
			this.selectColonizerAction.Text = "button1";
			this.selectColonizerAction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.selectColonizerAction.UseVisualStyleBackColor = true;
			// 
			// colonizerDesignText
			// 
			this.colonizerDesignText.Location = new System.Drawing.Point(12, 9);
			this.colonizerDesignText.Name = "colonizerDesignText";
			this.colonizerDesignText.Size = new System.Drawing.Size(130, 36);
			this.colonizerDesignText.TabIndex = 2;
			this.colonizerDesignText.Text = "Colony ship design:";
			this.colonizerDesignText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(295, 9);
			this.label1.Margin = new System.Windows.Forms.Padding(20, 0, 3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 36);
			this.label1.TabIndex = 4;
			this.label1.Text = "Target transport capacoty:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(421, 18);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(80, 20);
			this.textBox1.TabIndex = 5;
			this.textBox1.Text = "820 k";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 57);
			this.label2.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(99, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Planets to colonize:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(316, 57);
			this.label3.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(92, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Colony ship yards:";
			// 
			// controlListView2
			// 
			this.controlListView2.AutoScroll = true;
			this.controlListView2.Controls.Add(this.colonizationSource1);
			this.controlListView2.Controls.Add(this.colonizationSource2);
			this.controlListView2.Controls.Add(this.colonizationSource3);
			this.controlListView2.Controls.Add(this.colonizationSource4);
			this.controlListView2.Controls.Add(this.colonizationSource5);
			this.controlListView2.Controls.Add(this.colonizationSource6);
			this.controlListView2.Controls.Add(this.colonizationSource7);
			this.controlListView2.Location = new System.Drawing.Point(319, 73);
			this.controlListView2.Name = "controlListView2";
			this.controlListView2.Size = new System.Drawing.Size(224, 269);
			this.controlListView2.TabIndex = 8;
			// 
			// colonizationSource1
			// 
			this.colonizationSource1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationSource1.Location = new System.Drawing.Point(3, 6);
			this.colonizationSource1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.colonizationSource1.Name = "colonizationSource1";
			this.colonizationSource1.Size = new System.Drawing.Size(200, 31);
			this.colonizationSource1.TabIndex = 0;
			// 
			// colonizationSource2
			// 
			this.colonizationSource2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationSource2.Location = new System.Drawing.Point(3, 46);
			this.colonizationSource2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.colonizationSource2.Name = "colonizationSource2";
			this.colonizationSource2.Size = new System.Drawing.Size(200, 31);
			this.colonizationSource2.TabIndex = 1;
			// 
			// colonizationSource3
			// 
			this.colonizationSource3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationSource3.Location = new System.Drawing.Point(3, 86);
			this.colonizationSource3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.colonizationSource3.Name = "colonizationSource3";
			this.colonizationSource3.Size = new System.Drawing.Size(200, 31);
			this.colonizationSource3.TabIndex = 2;
			// 
			// colonizationSource4
			// 
			this.colonizationSource4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationSource4.Location = new System.Drawing.Point(3, 126);
			this.colonizationSource4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.colonizationSource4.Name = "colonizationSource4";
			this.colonizationSource4.Size = new System.Drawing.Size(200, 31);
			this.colonizationSource4.TabIndex = 3;
			// 
			// colonizationSource5
			// 
			this.colonizationSource5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationSource5.Location = new System.Drawing.Point(3, 166);
			this.colonizationSource5.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.colonizationSource5.Name = "colonizationSource5";
			this.colonizationSource5.Size = new System.Drawing.Size(200, 31);
			this.colonizationSource5.TabIndex = 4;
			// 
			// colonizationSource6
			// 
			this.colonizationSource6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationSource6.Location = new System.Drawing.Point(3, 206);
			this.colonizationSource6.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.colonizationSource6.Name = "colonizationSource6";
			this.colonizationSource6.Size = new System.Drawing.Size(200, 31);
			this.colonizationSource6.TabIndex = 5;
			// 
			// colonizationSource7
			// 
			this.colonizationSource7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.colonizationSource7.Location = new System.Drawing.Point(3, 246);
			this.colonizationSource7.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.colonizationSource7.Name = "colonizationSource7";
			this.colonizationSource7.Size = new System.Drawing.Size(200, 31);
			this.colonizationSource7.TabIndex = 6;
			// 
			// FormColonization
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(554, 361);
			this.Controls.Add(this.controlListView2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.selectColonizerAction);
			this.Controls.Add(this.colonizerDesignText);
			this.Controls.Add(this.controlListView1);
			this.Name = "FormColonization";
			this.Text = "FormColonization";
			this.controlListView1.ResumeLayout(false);
			this.controlListView1.PerformLayout();
			this.controlListView2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.Button selectColonizerAction;
		private System.Windows.Forms.Label colonizerDesignText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private My_ListView.ControlListView controlListView2;
		private ColonizationSource colonizationSource1;
		private ColonizationSource colonizationSource2;
		private ColonizationSource colonizationSource3;
		private ColonizationSource colonizationSource4;
		private ColonizationSource colonizationSource5;
		private ColonizationSource colonizationSource6;
		private ColonizationSource colonizationSource7;
	}
}
