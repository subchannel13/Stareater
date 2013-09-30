
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).BeginInit();
			this.SuspendLayout();
			// 
			// optionList
			// 
			this.optionList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.optionList.AutoScroll = true;
			this.optionList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.optionList.Location = new System.Drawing.Point(12, 12);
			this.optionList.Name = "optionList";
			this.optionList.Size = new System.Drawing.Size(214, 402);
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
			this.queueList.Location = new System.Drawing.Point(232, 215);
			this.queueList.Name = "queueList";
			this.queueList.SelectedIndex = -1;
			this.queueList.Size = new System.Drawing.Size(246, 199);
			this.queueList.TabIndex = 3;
			// 
			// button1
			// 
			this.button1.Image = global::Stareater.Properties.Resources.arrowUp;
			this.button1.Location = new System.Drawing.Point(484, 272);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(40, 40);
			this.button1.TabIndex = 4;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Image = global::Stareater.Properties.Resources.arrowDown;
			this.button2.Location = new System.Drawing.Point(484, 318);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(40, 40);
			this.button2.TabIndex = 5;
			this.button2.UseVisualStyleBackColor = true;
			// 
			// FormBuildingQueue
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(535, 426);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.queueList);
			this.Controls.Add(this.descriptionLabel);
			this.Controls.Add(this.thumbnailImage);
			this.Controls.Add(this.optionList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormBuildingQueue";
			this.Text = "FormBuildingQueue";
			((System.ComponentModel.ISupportInitialize)(this.thumbnailImage)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private Stareater.GUI.ControlListView queueList;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.PictureBox thumbnailImage;
		private System.Windows.Forms.FlowLayoutPanel optionList;
	}
}
