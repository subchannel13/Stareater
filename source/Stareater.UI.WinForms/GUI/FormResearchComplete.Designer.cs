/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 26.10.2016.
 * Time: 13:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormResearchComplete
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.PictureBox topicThumbnail;
		private System.Windows.Forms.Label levelLabel;
		private System.Windows.Forms.Label topicName;
		private System.Windows.Forms.TextBox topicDescription;
		private Zvjezdojedac.GUI.ControlListView unlockedList;
		private System.Windows.Forms.Label priorityTitle;
		
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
			this.acceptButton = new System.Windows.Forms.Button();
			this.topicThumbnail = new System.Windows.Forms.PictureBox();
			this.levelLabel = new System.Windows.Forms.Label();
			this.topicName = new System.Windows.Forms.Label();
			this.topicDescription = new System.Windows.Forms.TextBox();
			this.unlockedList = new Zvjezdojedac.GUI.ControlListView();
			this.priorityTitle = new System.Windows.Forms.Label();
			this.reorderDownAction = new System.Windows.Forms.Button();
			this.reorderUpAction = new System.Windows.Forms.Button();
			this.techDescription = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.topicThumbnail)).BeginInit();
			this.SuspendLayout();
			// 
			// acceptButton
			// 
			this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.acceptButton.AutoSize = true;
			this.acceptButton.Location = new System.Drawing.Point(531, 368);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 0;
			this.acceptButton.Text = "confirm";
			this.acceptButton.UseVisualStyleBackColor = true;
			// 
			// topicThumbnail
			// 
			this.topicThumbnail.Location = new System.Drawing.Point(12, 12);
			this.topicThumbnail.Name = "topicThumbnail";
			this.topicThumbnail.Size = new System.Drawing.Size(80, 80);
			this.topicThumbnail.TabIndex = 5;
			this.topicThumbnail.TabStop = false;
			// 
			// levelLabel
			// 
			this.levelLabel.AutoSize = true;
			this.levelLabel.Location = new System.Drawing.Point(98, 25);
			this.levelLabel.Name = "levelLabel";
			this.levelLabel.Size = new System.Drawing.Size(39, 13);
			this.levelLabel.TabIndex = 4;
			this.levelLabel.Text = "level X";
			// 
			// topicName
			// 
			this.topicName.AutoSize = true;
			this.topicName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.topicName.Location = new System.Drawing.Point(98, 12);
			this.topicName.Name = "topicName";
			this.topicName.Size = new System.Drawing.Size(73, 13);
			this.topicName.TabIndex = 3;
			this.topicName.Text = "Topic name";
			// 
			// topicDescription
			// 
			this.topicDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.topicDescription.Location = new System.Drawing.Point(12, 98);
			this.topicDescription.Multiline = true;
			this.topicDescription.Name = "topicDescription";
			this.topicDescription.ReadOnly = true;
			this.topicDescription.Size = new System.Drawing.Size(222, 293);
			this.topicDescription.TabIndex = 18;
			this.topicDescription.Text = "Lore ipsum\r\nDoloret reat msddo mowerr maugad sad\r\ndaod coksal. Je sorof msoerta n" +
				"aue usfa.\r\n";
			// 
			// unlockedList
			// 
			this.unlockedList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.unlockedList.Location = new System.Drawing.Point(240, 28);
			this.unlockedList.Name = "unlockedList";
			this.unlockedList.SelectedIndex = -1;
			this.unlockedList.Size = new System.Drawing.Size(277, 173);
			this.unlockedList.TabIndex = 19;
			// 
			// priorityTitle
			// 
			this.priorityTitle.AutoSize = true;
			this.priorityTitle.Location = new System.Drawing.Point(240, 12);
			this.priorityTitle.Name = "priorityTitle";
			this.priorityTitle.Size = new System.Drawing.Size(75, 13);
			this.priorityTitle.TabIndex = 20;
			this.priorityTitle.Text = "choose priority";
			// 
			// reorderDownAction
			// 
			this.reorderDownAction.Image = global::Stareater.Properties.Resources.arrowDown;
			this.reorderDownAction.Location = new System.Drawing.Point(523, 98);
			this.reorderDownAction.Name = "reorderDownAction";
			this.reorderDownAction.Size = new System.Drawing.Size(50, 50);
			this.reorderDownAction.TabIndex = 22;
			this.reorderDownAction.UseVisualStyleBackColor = true;
			// 
			// reorderUpAction
			// 
			this.reorderUpAction.Image = global::Stareater.Properties.Resources.arrowUp;
			this.reorderUpAction.Location = new System.Drawing.Point(523, 42);
			this.reorderUpAction.Name = "reorderUpAction";
			this.reorderUpAction.Size = new System.Drawing.Size(50, 50);
			this.reorderUpAction.TabIndex = 21;
			this.reorderUpAction.UseVisualStyleBackColor = true;
			// 
			// techDescription
			// 
			this.techDescription.Location = new System.Drawing.Point(240, 207);
			this.techDescription.Multiline = true;
			this.techDescription.Name = "techDescription";
			this.techDescription.ReadOnly = true;
			this.techDescription.Size = new System.Drawing.Size(277, 161);
			this.techDescription.TabIndex = 24;
			this.techDescription.Text = "Description here";
			// 
			// FormResearchComplete
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(618, 403);
			this.Controls.Add(this.techDescription);
			this.Controls.Add(this.reorderDownAction);
			this.Controls.Add(this.reorderUpAction);
			this.Controls.Add(this.priorityTitle);
			this.Controls.Add(this.unlockedList);
			this.Controls.Add(this.topicDescription);
			this.Controls.Add(this.topicThumbnail);
			this.Controls.Add(this.levelLabel);
			this.Controls.Add(this.topicName);
			this.Controls.Add(this.acceptButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormResearchComplete";
			this.Text = "FormResearchComplete";
			((System.ComponentModel.ISupportInitialize)(this.topicThumbnail)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.Button reorderDownAction;
		private System.Windows.Forms.Button reorderUpAction;
		private System.Windows.Forms.TextBox techDescription;
	}
}
