namespace Stareater.GUI
{
	partial class FormShipDesigner
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
			this.hullImage = new System.Windows.Forms.PictureBox();
			this.imageLeftButton = new System.Windows.Forms.Button();
			this.imageRightButton = new System.Windows.Forms.Button();
			this.hullPicker = new System.Windows.Forms.ComboBox();
			this.nameInput = new System.Windows.Forms.TextBox();
			this.acceptButton = new System.Windows.Forms.Button();
			this.isDriveImage = new System.Windows.Forms.PictureBox();
			this.hasIsDrive = new System.Windows.Forms.CheckBox();
			this.powerInfo = new System.Windows.Forms.Label();
			this.armorInfo = new System.Windows.Forms.Label();
			this.mobilityInfo = new System.Windows.Forms.Label();
			this.sensorInfo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.hullImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.isDriveImage)).BeginInit();
			this.SuspendLayout();
			// 
			// hullImage
			// 
			this.hullImage.BackColor = System.Drawing.Color.Black;
			this.hullImage.Location = new System.Drawing.Point(26, 20);
			this.hullImage.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.hullImage.Name = "hullImage";
			this.hullImage.Size = new System.Drawing.Size(80, 80);
			this.hullImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.hullImage.TabIndex = 0;
			this.hullImage.TabStop = false;
			// 
			// imageLeftButton
			// 
			this.imageLeftButton.BackgroundImage = global::Stareater.Properties.Resources.arrowLeft;
			this.imageLeftButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.imageLeftButton.Location = new System.Drawing.Point(9, 52);
			this.imageLeftButton.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
			this.imageLeftButton.Name = "imageLeftButton";
			this.imageLeftButton.Size = new System.Drawing.Size(16, 16);
			this.imageLeftButton.TabIndex = 1;
			this.imageLeftButton.UseVisualStyleBackColor = true;
			this.imageLeftButton.Click += new System.EventHandler(this.imageLeft_ButtonClick);
			// 
			// imageRightButton
			// 
			this.imageRightButton.BackgroundImage = global::Stareater.Properties.Resources.arrowRight;
			this.imageRightButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.imageRightButton.Location = new System.Drawing.Point(107, 52);
			this.imageRightButton.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
			this.imageRightButton.Name = "imageRightButton";
			this.imageRightButton.Size = new System.Drawing.Size(16, 16);
			this.imageRightButton.TabIndex = 2;
			this.imageRightButton.UseVisualStyleBackColor = true;
			this.imageRightButton.Click += new System.EventHandler(this.imageRight_ButtonClick);
			// 
			// hullPicker
			// 
			this.hullPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.hullPicker.FormattingEnabled = true;
			this.hullPicker.Location = new System.Drawing.Point(129, 38);
			this.hullPicker.Name = "hullPicker";
			this.hullPicker.Size = new System.Drawing.Size(170, 21);
			this.hullPicker.TabIndex = 6;
			this.hullPicker.SelectedIndexChanged += new System.EventHandler(this.hullSelector_SelectedIndexChanged);
			// 
			// nameInput
			// 
			this.nameInput.Location = new System.Drawing.Point(129, 12);
			this.nameInput.Name = "nameInput";
			this.nameInput.Size = new System.Drawing.Size(170, 20);
			this.nameInput.TabIndex = 5;
			this.nameInput.TextChanged += new System.EventHandler(this.nameInput_TextChanged);
			this.nameInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nameInput_KeyPress);
			// 
			// acceptButton
			// 
			this.acceptButton.Location = new System.Drawing.Point(337, 377);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 24;
			this.acceptButton.Text = "Build";
			this.acceptButton.UseVisualStyleBackColor = true;
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// isDriveImage
			// 
			this.isDriveImage.BackColor = System.Drawing.Color.Black;
			this.isDriveImage.Location = new System.Drawing.Point(129, 65);
			this.isDriveImage.Name = "isDriveImage";
			this.isDriveImage.Size = new System.Drawing.Size(24, 24);
			this.isDriveImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.isDriveImage.TabIndex = 28;
			this.isDriveImage.TabStop = false;
			// 
			// hasIsDrive
			// 
			this.hasIsDrive.AutoSize = true;
			this.hasIsDrive.Location = new System.Drawing.Point(159, 70);
			this.hasIsDrive.Name = "hasIsDrive";
			this.hasIsDrive.Size = new System.Drawing.Size(128, 17);
			this.hasIsDrive.TabIndex = 27;
			this.hasIsDrive.Text = "IS drive (X.XX ly/turn)";
			this.hasIsDrive.UseVisualStyleBackColor = true;
			this.hasIsDrive.CheckedChanged += new System.EventHandler(this.hasIsDrive_CheckedChanged);
			// 
			// powerInfo
			// 
			this.powerInfo.AutoSize = true;
			this.powerInfo.Location = new System.Drawing.Point(322, 41);
			this.powerInfo.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
			this.powerInfo.Name = "powerInfo";
			this.powerInfo.Size = new System.Drawing.Size(58, 13);
			this.powerInfo.TabIndex = 35;
			this.powerInfo.Text = "Power: X%";
			// 
			// armorInfo
			// 
			this.armorInfo.AutoSize = true;
			this.armorInfo.Location = new System.Drawing.Point(322, 15);
			this.armorInfo.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
			this.armorInfo.Name = "armorInfo";
			this.armorInfo.Size = new System.Drawing.Size(65, 13);
			this.armorInfo.TabIndex = 36;
			this.armorInfo.Text = "Armor: xxx X";
			// 
			// mobilityInfo
			// 
			this.mobilityInfo.AutoSize = true;
			this.mobilityInfo.Location = new System.Drawing.Point(322, 28);
			this.mobilityInfo.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
			this.mobilityInfo.Name = "mobilityInfo";
			this.mobilityInfo.Size = new System.Drawing.Size(64, 13);
			this.mobilityInfo.TabIndex = 37;
			this.mobilityInfo.Text = "Mobility: +xx";
			// 
			// sensorInfo
			// 
			this.sensorInfo.AutoSize = true;
			this.sensorInfo.Location = new System.Drawing.Point(322, 65);
			this.sensorInfo.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
			this.sensorInfo.Name = "sensorInfo";
			this.sensorInfo.Size = new System.Drawing.Size(58, 13);
			this.sensorInfo.TabIndex = 38;
			this.sensorInfo.Text = "Sensors: X";
			// 
			// FormShipDesigner
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(425, 410);
			this.Controls.Add(this.sensorInfo);
			this.Controls.Add(this.mobilityInfo);
			this.Controls.Add(this.armorInfo);
			this.Controls.Add(this.powerInfo);
			this.Controls.Add(this.isDriveImage);
			this.Controls.Add(this.hasIsDrive);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.hullPicker);
			this.Controls.Add(this.nameInput);
			this.Controls.Add(this.imageRightButton);
			this.Controls.Add(this.imageLeftButton);
			this.Controls.Add(this.hullImage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormShipDesigner";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormShipDesigner";
			((System.ComponentModel.ISupportInitialize)(this.hullImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.isDriveImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Button acceptButton;
		private System.Windows.Forms.TextBox nameInput;
		private System.Windows.Forms.ComboBox hullPicker;
		private System.Windows.Forms.Button imageRightButton;
		private System.Windows.Forms.Button imageLeftButton;
		private System.Windows.Forms.PictureBox hullImage;
		private System.Windows.Forms.PictureBox isDriveImage;
		private System.Windows.Forms.CheckBox hasIsDrive;
		private System.Windows.Forms.Label powerInfo;
		private System.Windows.Forms.Label armorInfo;
		private System.Windows.Forms.Label mobilityInfo;
		private System.Windows.Forms.Label sensorInfo;
	}
}
