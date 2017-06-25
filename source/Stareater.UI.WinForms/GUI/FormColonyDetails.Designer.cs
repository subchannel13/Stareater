namespace Stareater.GUI
{
	partial class FormColonyDetails
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
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.planetImage = new System.Windows.Forms.PictureBox();
			this.popInfoGroup = new System.Windows.Forms.GroupBox();
			this.infrastructureInfo = new System.Windows.Forms.Label();
			this.growthInfo = new System.Windows.Forms.Label();
			this.populationInfo = new System.Windows.Forms.Label();
			this.planetInfoGroup = new System.Windows.Forms.GroupBox();
			this.traitList = new System.Windows.Forms.FlowLayoutPanel();
			this.environmentInfo = new System.Windows.Forms.Label();
			this.sizeInfo = new System.Windows.Forms.Label();
			this.buildingsGroup = new System.Windows.Forms.GroupBox();
			this.buildingsList = new System.Windows.Forms.FlowLayoutPanel();
			this.productivityGroup = new System.Windows.Forms.GroupBox();
			this.developmentTotalInfo = new System.Windows.Forms.Label();
			this.industryTotalInfo = new System.Windows.Forms.Label();
			this.developmentInfo = new System.Windows.Forms.Label();
			this.industryInfo = new System.Windows.Forms.Label();
			this.miningInfo = new System.Windows.Forms.Label();
			this.foodInfo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.planetImage)).BeginInit();
			this.popInfoGroup.SuspendLayout();
			this.planetInfoGroup.SuspendLayout();
			this.buildingsGroup.SuspendLayout();
			this.productivityGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// planetImage
			// 
			this.planetImage.BackColor = System.Drawing.Color.Black;
			this.planetImage.Location = new System.Drawing.Point(12, 12);
			this.planetImage.Name = "planetImage";
			this.planetImage.Size = new System.Drawing.Size(64, 64);
			this.planetImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.planetImage.TabIndex = 0;
			this.planetImage.TabStop = false;
			// 
			// popInfoGroup
			// 
			this.popInfoGroup.Controls.Add(this.infrastructureInfo);
			this.popInfoGroup.Controls.Add(this.growthInfo);
			this.popInfoGroup.Controls.Add(this.populationInfo);
			this.popInfoGroup.Location = new System.Drawing.Point(82, 12);
			this.popInfoGroup.Name = "popInfoGroup";
			this.popInfoGroup.Size = new System.Drawing.Size(116, 64);
			this.popInfoGroup.TabIndex = 0;
			this.popInfoGroup.TabStop = false;
			this.popInfoGroup.Text = "Population";
			// 
			// infrastructureInfo
			// 
			this.infrastructureInfo.AutoSize = true;
			this.infrastructureInfo.Location = new System.Drawing.Point(6, 48);
			this.infrastructureInfo.Name = "infrastructureInfo";
			this.infrastructureInfo.Size = new System.Drawing.Size(104, 13);
			this.infrastructureInfo.TabIndex = 2;
			this.infrastructureInfo.Text = "Infrastructure: xx.x %";
			// 
			// growthInfo
			// 
			this.growthInfo.AutoSize = true;
			this.growthInfo.Location = new System.Drawing.Point(6, 29);
			this.growthInfo.Name = "growthInfo";
			this.growthInfo.Size = new System.Drawing.Size(81, 13);
			this.growthInfo.TabIndex = 1;
			this.growthInfo.Text = "Growth: +x.xx X";
			// 
			// populationInfo
			// 
			this.populationInfo.AutoSize = true;
			this.populationInfo.Location = new System.Drawing.Point(6, 16);
			this.populationInfo.Name = "populationInfo";
			this.populationInfo.Size = new System.Drawing.Size(64, 13);
			this.populationInfo.TabIndex = 0;
			this.populationInfo.Text = "x.xx / x.xx X";
			// 
			// planetInfoGroup
			// 
			this.planetInfoGroup.Controls.Add(this.traitList);
			this.planetInfoGroup.Controls.Add(this.environmentInfo);
			this.planetInfoGroup.Controls.Add(this.sizeInfo);
			this.planetInfoGroup.Location = new System.Drawing.Point(204, 12);
			this.planetInfoGroup.Name = "planetInfoGroup";
			this.planetInfoGroup.Size = new System.Drawing.Size(185, 131);
			this.planetInfoGroup.TabIndex = 1;
			this.planetInfoGroup.TabStop = false;
			this.planetInfoGroup.Text = "Planet";
			// 
			// traitList
			// 
			this.traitList.AutoScroll = true;
			this.traitList.Location = new System.Drawing.Point(6, 45);
			this.traitList.Name = "traitList";
			this.traitList.Size = new System.Drawing.Size(172, 76);
			this.traitList.TabIndex = 2;
			// 
			// environmentInfo
			// 
			this.environmentInfo.AutoSize = true;
			this.environmentInfo.Location = new System.Drawing.Point(6, 29);
			this.environmentInfo.Name = "environmentInfo";
			this.environmentInfo.Size = new System.Drawing.Size(98, 13);
			this.environmentInfo.TabIndex = 1;
			this.environmentInfo.Text = "Environment: xx.x%";
			// 
			// sizeInfo
			// 
			this.sizeInfo.AutoSize = true;
			this.sizeInfo.Location = new System.Drawing.Point(6, 16);
			this.sizeInfo.Name = "sizeInfo";
			this.sizeInfo.Size = new System.Drawing.Size(48, 13);
			this.sizeInfo.TabIndex = 0;
			this.sizeInfo.Text = "Size: xxx";
			// 
			// buildingsGroup
			// 
			this.buildingsGroup.Controls.Add(this.buildingsList);
			this.buildingsGroup.Location = new System.Drawing.Point(12, 82);
			this.buildingsGroup.Name = "buildingsGroup";
			this.buildingsGroup.Size = new System.Drawing.Size(186, 201);
			this.buildingsGroup.TabIndex = 2;
			this.buildingsGroup.TabStop = false;
			this.buildingsGroup.Text = "Buildings";
			// 
			// buildingsList
			// 
			this.buildingsList.AutoScroll = true;
			this.buildingsList.Location = new System.Drawing.Point(6, 19);
			this.buildingsList.Name = "buildingsList";
			this.buildingsList.Size = new System.Drawing.Size(174, 176);
			this.buildingsList.TabIndex = 0;
			// 
			// productivityGroup
			// 
			this.productivityGroup.Controls.Add(this.developmentTotalInfo);
			this.productivityGroup.Controls.Add(this.industryTotalInfo);
			this.productivityGroup.Controls.Add(this.developmentInfo);
			this.productivityGroup.Controls.Add(this.industryInfo);
			this.productivityGroup.Controls.Add(this.miningInfo);
			this.productivityGroup.Controls.Add(this.foodInfo);
			this.productivityGroup.Location = new System.Drawing.Point(204, 149);
			this.productivityGroup.Name = "productivityGroup";
			this.productivityGroup.Size = new System.Drawing.Size(185, 134);
			this.productivityGroup.TabIndex = 3;
			this.productivityGroup.TabStop = false;
			this.productivityGroup.Text = "Productivity";
			// 
			// developmentTotalInfo
			// 
			this.developmentTotalInfo.AutoSize = true;
			this.developmentTotalInfo.Location = new System.Drawing.Point(6, 94);
			this.developmentTotalInfo.Name = "developmentTotalInfo";
			this.developmentTotalInfo.Size = new System.Drawing.Size(104, 13);
			this.developmentTotalInfo.TabIndex = 5;
			this.developmentTotalInfo.Text = "Development: xx.x X";
			// 
			// industryTotalInfo
			// 
			this.industryTotalInfo.AutoSize = true;
			this.industryTotalInfo.Location = new System.Drawing.Point(6, 81);
			this.industryTotalInfo.Name = "industryTotalInfo";
			this.industryTotalInfo.Size = new System.Drawing.Size(78, 13);
			this.industryTotalInfo.TabIndex = 4;
			this.industryTotalInfo.Text = "Industry: x.xx X";
			// 
			// developmentInfo
			// 
			this.developmentInfo.AutoSize = true;
			this.developmentInfo.Location = new System.Drawing.Point(6, 55);
			this.developmentInfo.Name = "developmentInfo";
			this.developmentInfo.Size = new System.Drawing.Size(118, 13);
			this.developmentInfo.TabIndex = 3;
			this.developmentInfo.Text = "Development: x.x / pop";
			// 
			// industryInfo
			// 
			this.industryInfo.AutoSize = true;
			this.industryInfo.Location = new System.Drawing.Point(6, 42);
			this.industryInfo.Name = "industryInfo";
			this.industryInfo.Size = new System.Drawing.Size(92, 13);
			this.industryInfo.TabIndex = 2;
			this.industryInfo.Text = "Industry: x.x / pop";
			// 
			// miningInfo
			// 
			this.miningInfo.AutoSize = true;
			this.miningInfo.Location = new System.Drawing.Point(6, 29);
			this.miningInfo.Name = "miningInfo";
			this.miningInfo.Size = new System.Drawing.Size(86, 13);
			this.miningInfo.TabIndex = 1;
			this.miningInfo.Text = "Mining: x.x / pop";
			// 
			// foodInfo
			// 
			this.foodInfo.AutoSize = true;
			this.foodInfo.Location = new System.Drawing.Point(6, 16);
			this.foodInfo.Name = "foodInfo";
			this.foodInfo.Size = new System.Drawing.Size(79, 13);
			this.foodInfo.TabIndex = 0;
			this.foodInfo.Text = "Food: x.x / pop";
			// 
			// FormColonyDetails
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(398, 295);
			this.Controls.Add(this.productivityGroup);
			this.Controls.Add(this.buildingsGroup);
			this.Controls.Add(this.planetInfoGroup);
			this.Controls.Add(this.popInfoGroup);
			this.Controls.Add(this.planetImage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormColonyDetails";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormColonyDetails";
			((System.ComponentModel.ISupportInitialize)(this.planetImage)).EndInit();
			this.popInfoGroup.ResumeLayout(false);
			this.popInfoGroup.PerformLayout();
			this.planetInfoGroup.ResumeLayout(false);
			this.planetInfoGroup.PerformLayout();
			this.buildingsGroup.ResumeLayout(false);
			this.productivityGroup.ResumeLayout(false);
			this.productivityGroup.PerformLayout();
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.Label miningInfo;
		private System.Windows.Forms.Label industryInfo;
		private System.Windows.Forms.Label developmentInfo;
		private System.Windows.Forms.Label industryTotalInfo;
		private System.Windows.Forms.Label developmentTotalInfo;
		private System.Windows.Forms.Label foodInfo;
		private System.Windows.Forms.GroupBox productivityGroup;
		private System.Windows.Forms.FlowLayoutPanel buildingsList;
		private System.Windows.Forms.GroupBox buildingsGroup;
		private System.Windows.Forms.Label sizeInfo;
		private System.Windows.Forms.Label environmentInfo;
		private System.Windows.Forms.FlowLayoutPanel traitList;
		private System.Windows.Forms.GroupBox planetInfoGroup;
		private System.Windows.Forms.Label populationInfo;
		private System.Windows.Forms.Label growthInfo;
		private System.Windows.Forms.Label infrastructureInfo;
		private System.Windows.Forms.GroupBox popInfoGroup;
		private System.Windows.Forms.PictureBox planetImage;

		#endregion
	}
}