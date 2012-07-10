namespace Zvjezdojedac.GUI
{
	partial class PlanetItem
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.planetImage = new System.Windows.Forms.PictureBox();
			this.planetName = new System.Windows.Forms.Label();
			this.populationIcon = new System.Windows.Forms.PictureBox();
			this.populationText = new System.Windows.Forms.Label();
			this.industryText = new System.Windows.Forms.Label();
			this.industryIcon = new System.Windows.Forms.PictureBox();
			this.buildingButton = new System.Windows.Forms.Button();
			this.buildingInfo = new System.Windows.Forms.Label();
			this.resourceSlider = new System.Windows.Forms.HScrollBar();
			((System.ComponentModel.ISupportInitialize)(this.planetImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.populationIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.industryIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// starImage
			// 
			this.planetImage.Location = new System.Drawing.Point(3, 3);
			this.planetImage.Name = "starImage";
			this.planetImage.Size = new System.Drawing.Size(40, 40);
			this.planetImage.TabIndex = 0;
			this.planetImage.TabStop = false;
			// 
			// starName
			// 
			this.planetName.AutoEllipsis = true;
			this.planetName.AutoSize = true;
			this.planetName.Location = new System.Drawing.Point(49, 17);
			this.planetName.MaximumSize = new System.Drawing.Size(160, 16);
			this.planetName.Name = "starName";
			this.planetName.Size = new System.Drawing.Size(92, 13);
			this.planetName.TabIndex = 1;
			this.planetName.Text = "Omikron Kentaura";
			// 
			// populationIcon
			// 
			this.populationIcon.Location = new System.Drawing.Point(220, 3);
			this.populationIcon.Name = "populationIcon";
			this.populationIcon.Size = new System.Drawing.Size(30, 40);
			this.populationIcon.TabIndex = 2;
			this.populationIcon.TabStop = false;
			// 
			// populationText
			// 
			this.populationText.AutoSize = true;
			this.populationText.Location = new System.Drawing.Point(257, 17);
			this.populationText.Name = "populationText";
			this.populationText.Size = new System.Drawing.Size(40, 13);
			this.populationText.TabIndex = 3;
			this.populationText.Text = "xx.xx X";
			// 
			// industryText
			// 
			this.industryText.AutoSize = true;
			this.industryText.Location = new System.Drawing.Point(340, 17);
			this.industryText.Name = "industryText";
			this.industryText.Size = new System.Drawing.Size(40, 13);
			this.industryText.TabIndex = 5;
			this.industryText.Text = "xx.xx X";
			// 
			// industryIcon
			// 
			this.industryIcon.Location = new System.Drawing.Point(303, 3);
			this.industryIcon.Name = "industryIcon";
			this.industryIcon.Size = new System.Drawing.Size(30, 40);
			this.industryIcon.TabIndex = 4;
			this.industryIcon.TabStop = false;
			// 
			// buildingButton
			// 
			this.buildingButton.Location = new System.Drawing.Point(395, 3);
			this.buildingButton.Name = "buildingButton";
			this.buildingButton.Size = new System.Drawing.Size(40, 40);
			this.buildingButton.TabIndex = 6;
			this.buildingButton.UseVisualStyleBackColor = true;
			// 
			// buildingInfo
			// 
			this.buildingInfo.AutoSize = true;
			this.buildingInfo.Location = new System.Drawing.Point(441, 24);
			this.buildingInfo.Name = "buildingInfo";
			this.buildingInfo.Size = new System.Drawing.Size(35, 13);
			this.buildingInfo.TabIndex = 7;
			this.buildingInfo.Text = "label1";
			// 
			// resourceSlider
			// 
			this.resourceSlider.Location = new System.Drawing.Point(442, 4);
			this.resourceSlider.Name = "resourceSlider";
			this.resourceSlider.Size = new System.Drawing.Size(127, 17);
			this.resourceSlider.TabIndex = 8;
			// 
			// StarItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.resourceSlider);
			this.Controls.Add(this.buildingInfo);
			this.Controls.Add(this.buildingButton);
			this.Controls.Add(this.industryText);
			this.Controls.Add(this.industryIcon);
			this.Controls.Add(this.populationText);
			this.Controls.Add(this.populationIcon);
			this.Controls.Add(this.planetName);
			this.Controls.Add(this.planetImage);
			this.Name = "PlanetItem";
			this.Size = new System.Drawing.Size(580, 46);
			((System.ComponentModel.ISupportInitialize)(this.planetImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.populationIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.industryIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox planetImage;
		private System.Windows.Forms.Label planetName;
		private System.Windows.Forms.PictureBox populationIcon;
		private System.Windows.Forms.Label populationText;
		private System.Windows.Forms.Label industryText;
		private System.Windows.Forms.PictureBox industryIcon;
		private System.Windows.Forms.Button buildingButton;
		private System.Windows.Forms.Label buildingInfo;
		private System.Windows.Forms.HScrollBar resourceSlider;
	}
}
