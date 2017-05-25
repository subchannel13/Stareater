/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 25.5.2017.
 * Time: 9:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormBombardment
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private Winforms_Mockups.SystemMockup systemMockup1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label1;
		
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
			this.systemMockup1 = new Winforms_Mockups.SystemMockup();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// systemMockup1
			// 
			this.systemMockup1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.systemMockup1.Location = new System.Drawing.Point(0, 0);
			this.systemMockup1.Name = "systemMockup1";
			this.systemMockup1.Size = new System.Drawing.Size(568, 401);
			this.systemMockup1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(162, 222);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Bomb";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(162, 251);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Invade";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(486, 12);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(70, 70);
			this.button3.TabIndex = 3;
			this.button3.Text = "Done";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Black;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(162, 193);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 26);
			this.label1.TabIndex = 4;
			this.label1.Text = "Population: 1G\r\nTroops: 2M";
			// 
			// FormBombardment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(568, 401);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.systemMockup1);
			this.Name = "FormBombardment";
			this.Text = "FormBombardment";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
