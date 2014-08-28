/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 13.6.2014.
 * Time: 10:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
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
			this.gameList = new Stareater.GUI.ControlListView();
			this.saveButton = new System.Windows.Forms.Button();
			this.loadButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// gameList
			// 
			this.gameList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.gameList.Location = new System.Drawing.Point(12, 12);
			this.gameList.Name = "gameList";
			this.gameList.SelectedIndex = -1;
			this.gameList.Size = new System.Drawing.Size(323, 325);
			this.gameList.TabIndex = 0;
			this.gameList.SelectedIndexChanged += new System.EventHandler(this.gameList_SelectedIndexChanged);
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.BackgroundImage = global::Stareater.Properties.Resources.save;
			this.saveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.saveButton.Enabled = false;
			this.saveButton.Location = new System.Drawing.Point(341, 93);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(80, 80);
			this.saveButton.TabIndex = 1;
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// loadButton
			// 
			this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.loadButton.BackgroundImage = global::Stareater.Properties.Resources.load;
			this.loadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.loadButton.Enabled = false;
			this.loadButton.Location = new System.Drawing.Point(341, 179);
			this.loadButton.Name = "loadButton";
			this.loadButton.Size = new System.Drawing.Size(80, 80);
			this.loadButton.TabIndex = 2;
			this.loadButton.UseVisualStyleBackColor = true;
			this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
			// 
			// FormSaveLoad
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(433, 349);
			this.Controls.Add(this.loadButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.gameList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormSaveLoad";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormSaveLoad";
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button loadButton;
		private System.Windows.Forms.Button saveButton;
		private Stareater.GUI.ControlListView gameList;
	}
}
