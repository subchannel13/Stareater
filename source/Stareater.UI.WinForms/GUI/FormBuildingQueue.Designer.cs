
namespace Stareater.GUI
{
	partial class FormBuildingQueue
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
			this.optionList = new System.Windows.Forms.FlowLayoutPanel();
			this.thumbnailImage = new System.Windows.Forms.PictureBox();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.queueList = new Stareater.GUI.ControlListView();
			this.moveUpButton = new System.Windows.Forms.Button();
			this.moveDownButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).BeginInit();
			this.SuspendLayout();
			// 
			// optionList
			// 
			this.optionList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.optionList.AutoScroll = true;
			this.optionList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.optionList.Location = new System.Drawing.Point(12, 25);
			this.optionList.Name = "optionList";
			this.optionList.Size = new System.Drawing.Size(214, 389);
			this.optionList.TabIndex = 0;
			// 
			// thumbnailImage
			// 
			this.thumbnailImage.Location = new System.Drawing.Point(232, 12);
			this.thumbnailImage.Name = "thumbnailImage";
			this.thumbnailImage.Size = new System.Drawing.Size(40, 40);
			this.thumbnailImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.thumbnailImage.TabIndex = 1;
			this.thumbnailImage.TabStop = false;
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.descriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.descriptionLabel.Location = new System.Drawing.Point(278, 12);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(200, 198);
			this.descriptionLabel.TabIndex = 2;
			this.descriptionLabel.Text = "Description";
			// 
			// queueList
			// 
			this.queueList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.queueList.AutoScroll = true;
			this.queueList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.queueList.Location = new System.Drawing.Point(232, 215);
			this.queueList.Name = "queueList";
			this.queueList.SelectedIndex = -1;
			this.queueList.Size = new System.Drawing.Size(246, 199);
			this.queueList.TabIndex = 3;
			// 
			// moveUpButton
			// 
			this.moveUpButton.Image = global::Stareater.Properties.Resources.arrowUp;
			this.moveUpButton.Location = new System.Drawing.Point(484, 248);
			this.moveUpButton.Name = "moveUpButton";
			this.moveUpButton.Size = new System.Drawing.Size(40, 40);
			this.moveUpButton.TabIndex = 4;
			this.moveUpButton.UseVisualStyleBackColor = true;
			this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
			// 
			// moveDownButton
			// 
			this.moveDownButton.Image = global::Stareater.Properties.Resources.arrowDown;
			this.moveDownButton.Location = new System.Drawing.Point(484, 294);
			this.moveDownButton.Name = "moveDownButton";
			this.moveDownButton.Size = new System.Drawing.Size(40, 40);
			this.moveDownButton.TabIndex = 5;
			this.moveDownButton.UseVisualStyleBackColor = true;
			this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
			// 
			// removeButton
			// 
			this.removeButton.Image = global::Stareater.Properties.Resources.cancel;
			this.removeButton.Location = new System.Drawing.Point(483, 340);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(40, 40);
			this.removeButton.TabIndex = 6;
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Options:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(232, 199);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 8;
			this.label2.Text = "Queue:";
			// 
			// FormBuildingQueue
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(535, 426);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.moveDownButton);
			this.Controls.Add(this.moveUpButton);
			this.Controls.Add(this.queueList);
			this.Controls.Add(this.descriptionLabel);
			this.Controls.Add(this.thumbnailImage);
			this.Controls.Add(this.optionList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormBuildingQueue";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Building Queue";
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button moveDownButton;
		private System.Windows.Forms.Button moveUpButton;
		private Stareater.GUI.ControlListView queueList;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.PictureBox thumbnailImage;
		private System.Windows.Forms.FlowLayoutPanel optionList;
	}
}
