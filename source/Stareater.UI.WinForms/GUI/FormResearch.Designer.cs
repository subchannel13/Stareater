/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 29.4.2014.
 * Time: 11:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormResearch
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
			this.techDescription = new System.Windows.Forms.TextBox();
			this.techLevel = new System.Windows.Forms.Label();
			this.techName = new System.Windows.Forms.Label();
			this.techImage = new System.Windows.Forms.PictureBox();
			this.topicList = new Stareater.GUI.ControlListView();
			((System.ComponentModel.ISupportInitialize)(this.techImage)).BeginInit();
			this.SuspendLayout();
			// 
			// techDescription
			// 
			this.techDescription.Location = new System.Drawing.Point(297, 98);
			this.techDescription.Multiline = true;
			this.techDescription.Name = "techDescription";
			this.techDescription.ReadOnly = true;
			this.techDescription.Size = new System.Drawing.Size(250, 299);
			this.techDescription.TabIndex = 27;
			this.techDescription.Text = "Description here";
			// 
			// techLevel
			// 
			this.techLevel.AutoSize = true;
			this.techLevel.Location = new System.Drawing.Point(383, 26);
			this.techLevel.Name = "techLevel";
			this.techLevel.Size = new System.Drawing.Size(43, 13);
			this.techLevel.TabIndex = 26;
			this.techLevel.Text = "Level X";
			// 
			// techName
			// 
			this.techName.AutoSize = true;
			this.techName.Location = new System.Drawing.Point(383, 12);
			this.techName.Name = "techName";
			this.techName.Size = new System.Drawing.Size(61, 13);
			this.techName.TabIndex = 25;
			this.techName.Text = "Tech name";
			// 
			// techImage
			// 
			this.techImage.Location = new System.Drawing.Point(297, 12);
			this.techImage.Name = "techImage";
			this.techImage.Size = new System.Drawing.Size(80, 80);
			this.techImage.TabIndex = 24;
			this.techImage.TabStop = false;
			// 
			// topicList
			// 
			this.topicList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.topicList.Location = new System.Drawing.Point(12, 12);
			this.topicList.Name = "topicList";
			this.topicList.SelectedIndex = -1;
			this.topicList.Size = new System.Drawing.Size(277, 385);
			this.topicList.TabIndex = 28;
			this.topicList.SelectedIndexChanged += new System.EventHandler(this.topicList_SelectedIndexChanged);
			this.topicList.MouseLeave += new System.EventHandler(this.topicList_MouseLeave);
			// 
			// FormResearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(559, 409);
			this.Controls.Add(this.topicList);
			this.Controls.Add(this.techDescription);
			this.Controls.Add(this.techLevel);
			this.Controls.Add(this.techName);
			this.Controls.Add(this.techImage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormResearch";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormResearch";
			((System.ComponentModel.ISupportInitialize)(this.techImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private Stareater.GUI.ControlListView topicList;
		private System.Windows.Forms.PictureBox techImage;
		private System.Windows.Forms.Label techName;
		private System.Windows.Forms.Label techLevel;
		private System.Windows.Forms.TextBox techDescription;
	}
}
