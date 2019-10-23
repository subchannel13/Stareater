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
			if (disposing)
			{
				if (components != null)
				{
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
			this.fieldDescription = new System.Windows.Forms.TextBox();
			this.techImage = new System.Windows.Forms.PictureBox();
			this.topicList = new Stareater.GUI.ControlListView();
			this.techDescription = new System.Windows.Forms.TextBox();
			this.reorderDownAction = new System.Windows.Forms.Button();
			this.reorderUpAction = new System.Windows.Forms.Button();
			this.unlocksList = new Stareater.GUI.ControlListView();
			this.priorityTitle = new System.Windows.Forms.Label();
			this.focusAction = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.techImage)).BeginInit();
			this.SuspendLayout();
			// 
			// fieldDescription
			// 
			this.fieldDescription.Location = new System.Drawing.Point(383, 12);
			this.fieldDescription.Multiline = true;
			this.fieldDescription.Name = "fieldDescription";
			this.fieldDescription.ReadOnly = true;
			this.fieldDescription.Size = new System.Drawing.Size(191, 86);
			this.fieldDescription.TabIndex = 6;
			this.fieldDescription.Text = "Description here\r\nSpace\r\nLine 1\r\nLine 2\r\nLine 3\r\nLine 4\r\nLine 5";
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
			this.topicList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.topicList.AutoScroll = true;
			this.topicList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.topicList.Location = new System.Drawing.Point(12, 12);
			this.topicList.Name = "topicList";
			this.topicList.SelectedIndex = -1;
			this.topicList.Size = new System.Drawing.Size(277, 477);
			this.topicList.TabIndex = 3;
			// 
			// techDescription
			// 
			this.techDescription.Location = new System.Drawing.Point(297, 329);
			this.techDescription.Multiline = true;
			this.techDescription.Name = "techDescription";
			this.techDescription.ReadOnly = true;
			this.techDescription.Size = new System.Drawing.Size(277, 160);
			this.techDescription.TabIndex = 28;
			this.techDescription.Text = "Description here";
			// 
			// reorderDownAction
			// 
			this.reorderDownAction.Image = global::Stareater.Properties.Resources.arrowDown;
			this.reorderDownAction.Location = new System.Drawing.Point(580, 236);
			this.reorderDownAction.Name = "reorderDownAction";
			this.reorderDownAction.Size = new System.Drawing.Size(50, 50);
			this.reorderDownAction.TabIndex = 27;
			this.reorderDownAction.UseVisualStyleBackColor = true;
			this.reorderDownAction.Click += new System.EventHandler(this.reorderDownAction_Click);
			// 
			// reorderUpAction
			// 
			this.reorderUpAction.Image = global::Stareater.Properties.Resources.arrowUp;
			this.reorderUpAction.Location = new System.Drawing.Point(580, 180);
			this.reorderUpAction.Name = "reorderUpAction";
			this.reorderUpAction.Size = new System.Drawing.Size(50, 50);
			this.reorderUpAction.TabIndex = 26;
			this.reorderUpAction.UseVisualStyleBackColor = true;
			this.reorderUpAction.Click += new System.EventHandler(this.reorderUpAction_Click);
			// 
			// unlocksList
			// 
			this.unlocksList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.unlocksList.Location = new System.Drawing.Point(297, 150);
			this.unlocksList.Name = "unlocksList";
			this.unlocksList.SelectedIndex = -1;
			this.unlocksList.Size = new System.Drawing.Size(277, 173);
			this.unlocksList.TabIndex = 25;
			this.unlocksList.SelectedIndexChanged += new System.EventHandler(this.unlocksList_SelectedIndexChanged);
			// 
			// priorityTitle
			// 
			this.priorityTitle.AutoSize = true;
			this.priorityTitle.Location = new System.Drawing.Point(295, 134);
			this.priorityTitle.Name = "priorityTitle";
			this.priorityTitle.Size = new System.Drawing.Size(75, 13);
			this.priorityTitle.TabIndex = 29;
			this.priorityTitle.Text = "choose priority";
			// 
			// focusAction
			// 
			this.focusAction.Location = new System.Drawing.Point(499, 104);
			this.focusAction.Name = "focusAction";
			this.focusAction.Size = new System.Drawing.Size(75, 23);
			this.focusAction.TabIndex = 30;
			this.focusAction.Text = "button1";
			this.focusAction.UseVisualStyleBackColor = true;
			this.focusAction.Click += new System.EventHandler(this.focusAction_Click);
			// 
			// FormResearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(640, 501);
			this.Controls.Add(this.focusAction);
			this.Controls.Add(this.priorityTitle);
			this.Controls.Add(this.techDescription);
			this.Controls.Add(this.reorderDownAction);
			this.Controls.Add(this.reorderUpAction);
			this.Controls.Add(this.unlocksList);
			this.Controls.Add(this.topicList);
			this.Controls.Add(this.fieldDescription);
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
		private ControlListView topicList;
		private System.Windows.Forms.PictureBox techImage;
		private System.Windows.Forms.TextBox fieldDescription;
		private System.Windows.Forms.TextBox techDescription;
		private System.Windows.Forms.Button reorderDownAction;
		private System.Windows.Forms.Button reorderUpAction;
		private ControlListView unlocksList;
		private System.Windows.Forms.Label priorityTitle;
		private System.Windows.Forms.Button focusAction;
	}
}
