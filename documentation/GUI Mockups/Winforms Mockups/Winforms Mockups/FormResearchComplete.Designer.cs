/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 25.10.2016.
 * Time: 14:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormResearchComplete
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox1;
		private My_ListView.ControlListView controlListView1;
		private Winforms_Mockups.ResearchItem researchItem1;
		private Winforms_Mockups.ResearchItem researchItem2;
		private Winforms_Mockups.ResearchItem researchItem3;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button button9;
		
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.controlListView1 = new My_ListView.ControlListView();
			this.researchItem1 = new Winforms_Mockups.ResearchItem();
			this.researchItem2 = new Winforms_Mockups.ResearchItem();
			this.researchItem3 = new Winforms_Mockups.ResearchItem();
			this.label3 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button9 = new System.Windows.Forms.Button();
			this.controlListView1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Biology";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Level 2";
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
			this.textBox1.Location = new System.Drawing.Point(12, 57);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(222, 376);
			this.textBox1.TabIndex = 17;
			this.textBox1.Text = "Lore ipsum\r\nDoloret reat msddo mowerr maugad sad\r\ndaod coksal. Je sorof msoerta n" +
	"aue usfa.\r\n";
			// 
			// controlListView1
			// 
			this.controlListView1.AutoScroll = true;
			this.controlListView1.BackColor = System.Drawing.SystemColors.Control;
			this.controlListView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.controlListView1.Controls.Add(this.researchItem1);
			this.controlListView1.Controls.Add(this.researchItem2);
			this.controlListView1.Controls.Add(this.researchItem3);
			this.controlListView1.Location = new System.Drawing.Point(240, 25);
			this.controlListView1.Name = "controlListView1";
			this.controlListView1.Size = new System.Drawing.Size(277, 173);
			this.controlListView1.TabIndex = 18;
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
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(240, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(87, 13);
			this.label3.TabIndex = 19;
			this.label3.Text = "Choose priorities:";
			// 
			// button3
			// 
			this.button3.Image = global::Winforms_Mockups.Properties.Resources.arrow_down;
			this.button3.Location = new System.Drawing.Point(523, 113);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(50, 50);
			this.button3.TabIndex = 21;
			this.button3.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Image = global::Winforms_Mockups.Properties.Resources.arrow_up;
			this.button2.Location = new System.Drawing.Point(523, 57);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(50, 50);
			this.button2.TabIndex = 20;
			this.button2.UseVisualStyleBackColor = true;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(240, 204);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(250, 193);
			this.textBox2.TabIndex = 22;
			this.textBox2.Text = "Improved hydroponic farming. Lore ipsum\r\nDoloret reat msddo mowerr maugad sad\r\nda" +
	"od coksal. Je sorof msoerta naue usfa.\r\n\r\nFood per population: +0.25\r\nFood per f" +
	"armer: +0.5\r\nPopulation growth: +25%";
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(498, 410);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(75, 23);
			this.button9.TabIndex = 24;
			this.button9.Text = "Confirm";
			this.button9.UseVisualStyleBackColor = true;
			// 
			// FormResearchComplete
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(587, 445);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.controlListView1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormResearchComplete";
			this.Text = "Research complete!";
			this.controlListView1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
