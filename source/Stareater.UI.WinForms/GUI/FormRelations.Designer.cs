/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 23.3.2017.
 * Time: 15:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormRelations
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private Stareater.GUI.ControlListView playerList;
		private System.Windows.Forms.Button audienceAction;
		
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
			this.playerList = new Stareater.GUI.ControlListView();
			this.audienceAction = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// playerList
			// 
			this.playerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.playerList.Location = new System.Drawing.Point(12, 12);
			this.playerList.Name = "playerList";
			this.playerList.SelectedIndex = -1;
			this.playerList.Size = new System.Drawing.Size(350, 267);
			this.playerList.TabIndex = 0;
			this.playerList.SelectedIndexChanged += new System.EventHandler(this.playerList_SelectedIndexChanged);
			// 
			// audienceAction
			// 
			this.audienceAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.audienceAction.Image = global::Stareater.Properties.Resources.message;
			this.audienceAction.Location = new System.Drawing.Point(400, 234);
			this.audienceAction.Name = "audienceAction";
			this.audienceAction.Size = new System.Drawing.Size(95, 45);
			this.audienceAction.TabIndex = 1;
			this.audienceAction.Text = "button1";
			this.audienceAction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.audienceAction.UseVisualStyleBackColor = true;
			// 
			// FormRelations
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(507, 291);
			this.Controls.Add(this.audienceAction);
			this.Controls.Add(this.playerList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormRelations";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormRelations";
			this.ResumeLayout(false);

		}
	}
}
