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
			this.techDescription = new System.Windows.Forms.TextBox();
			this.techLevel = new System.Windows.Forms.Label();
			this.techName = new System.Windows.Forms.Label();
			this.techImage = new System.Windows.Forms.PictureBox();
			this.topicList = new System.Windows.Forms.FlowLayoutPanel();
			this.focusedLabel = new System.Windows.Forms.Label();
			this.listTitle = new System.Windows.Forms.Label();
			this.focusedItem = new Stareater.GUI.ResearchItem();
			((System.ComponentModel.ISupportInitialize)(this.techImage)).BeginInit();
			this.SuspendLayout();
			// 
			// techDescription
			// 
			this.techDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.techDescription.Location = new System.Drawing.Point(297, 98);
			this.techDescription.Multiline = true;
			this.techDescription.Name = "techDescription";
			this.techDescription.ReadOnly = true;
			this.techDescription.Size = new System.Drawing.Size(250, 361);
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
			this.topicList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.topicList.AutoScroll = true;
			this.topicList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.topicList.Location = new System.Drawing.Point(12, 117);
			this.topicList.Name = "topicList";
			this.topicList.Size = new System.Drawing.Size(277, 342);
			this.topicList.TabIndex = 28;
			this.topicList.MouseLeave += new System.EventHandler(this.topicList_MouseLeave);
			// 
			// focusedLabel
			// 
			this.focusedLabel.AutoSize = true;
			this.focusedLabel.Location = new System.Drawing.Point(12, 9);
			this.focusedLabel.Name = "focusedLabel";
			this.focusedLabel.Size = new System.Drawing.Size(48, 13);
			this.focusedLabel.TabIndex = 29;
			this.focusedLabel.Text = "focused:";
			// 
			// listTitle
			// 
			this.listTitle.AutoSize = true;
			this.listTitle.Location = new System.Drawing.Point(12, 101);
			this.listTitle.Name = "listTitle";
			this.listTitle.Size = new System.Drawing.Size(62, 13);
			this.listTitle.TabIndex = 31;
			this.listTitle.Text = "other topics";
			// 
			// focusedItem
			// 
			this.focusedItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(196)))), ((int)(((byte)(255)))));
			this.focusedItem.Location = new System.Drawing.Point(15, 25);
			this.focusedItem.Name = "focusedItem";
			this.focusedItem.Size = new System.Drawing.Size(250, 50);
			this.focusedItem.TabIndex = 30;
			// 
			// FormResearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(559, 471);
			this.Controls.Add(this.listTitle);
			this.Controls.Add(this.focusedItem);
			this.Controls.Add(this.focusedLabel);
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
		private System.Windows.Forms.FlowLayoutPanel topicList;
		private System.Windows.Forms.PictureBox techImage;
		private System.Windows.Forms.Label techName;
		private System.Windows.Forms.Label techLevel;
		private System.Windows.Forms.TextBox techDescription;
		private System.Windows.Forms.Label focusedLabel;
		private ResearchItem focusedItem;
		private System.Windows.Forms.Label listTitle;
	}
}
