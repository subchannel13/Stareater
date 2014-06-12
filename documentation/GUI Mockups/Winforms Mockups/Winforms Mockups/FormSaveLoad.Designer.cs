/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 11.6.2014.
 * Time: 10:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Winforms_Mockups
{
	partial class FormSaveLoad
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
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.savedGame1 = new Winforms_Mockups.SavedGame();
			this.savedGame2 = new Winforms_Mockups.SavedGame();
			this.savedGame3 = new Winforms_Mockups.SavedGame();
			this.savedGame4 = new Winforms_Mockups.SavedGame();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Controls.Add(this.savedGame1);
			this.flowLayoutPanel1.Controls.Add(this.savedGame2);
			this.flowLayoutPanel1.Controls.Add(this.savedGame3);
			this.flowLayoutPanel1.Controls.Add(this.savedGame4);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(323, 325);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// savedGame1
			// 
			this.savedGame1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.savedGame1.Location = new System.Drawing.Point(3, 3);
			this.savedGame1.Name = "savedGame1";
			this.savedGame1.Size = new System.Drawing.Size(300, 75);
			this.savedGame1.TabIndex = 0;
			// 
			// savedGame2
			// 
			this.savedGame2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.savedGame2.Location = new System.Drawing.Point(3, 84);
			this.savedGame2.Name = "savedGame2";
			this.savedGame2.Size = new System.Drawing.Size(300, 75);
			this.savedGame2.TabIndex = 1;
			// 
			// savedGame3
			// 
			this.savedGame3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.savedGame3.Location = new System.Drawing.Point(3, 165);
			this.savedGame3.Name = "savedGame3";
			this.savedGame3.Size = new System.Drawing.Size(300, 75);
			this.savedGame3.TabIndex = 2;
			// 
			// savedGame4
			// 
			this.savedGame4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.savedGame4.Location = new System.Drawing.Point(3, 246);
			this.savedGame4.Name = "savedGame4";
			this.savedGame4.Size = new System.Drawing.Size(300, 75);
			this.savedGame4.TabIndex = 3;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.BackgroundImage = global::Winforms_Mockups.Properties.Resources.arrow_last;
			this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button1.Location = new System.Drawing.Point(341, 96);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 75);
			this.button1.TabIndex = 1;
			this.button1.Text = "Save";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.BackgroundImage = global::Winforms_Mockups.Properties.Resources.arrow_up;
			this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.button2.Location = new System.Drawing.Point(341, 180);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 75);
			this.button2.TabIndex = 2;
			this.button2.Text = "Save";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// FormSaveLoad
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(428, 349);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormSaveLoad";
			this.Text = "FormSaveLoad";
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private Winforms_Mockups.SavedGame savedGame4;
		private Winforms_Mockups.SavedGame savedGame3;
		private Winforms_Mockups.SavedGame savedGame2;
		private Winforms_Mockups.SavedGame savedGame1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
	}
}
