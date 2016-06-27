namespace Stareater.GUI
{
	partial class FormStellarisDetails
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
			this.starImage = new System.Windows.Forms.PictureBox();
			this.coloniesInfoGroup = new System.Windows.Forms.GroupBox();
			this.infrastructureInfo = new System.Windows.Forms.Label();
			this.populationInfo = new System.Windows.Forms.Label();
			this.buildingsGroup = new System.Windows.Forms.GroupBox();
			this.buildingsList = new System.Windows.Forms.FlowLayoutPanel();
			this.traitList = new System.Windows.Forms.FlowLayoutPanel();
			this.outputInfoGroup = new System.Windows.Forms.GroupBox();
			this.researchInfo = new System.Windows.Forms.Label();
			this.developmentInfo = new System.Windows.Forms.Label();
			this.industryInfo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.starImage)).BeginInit();
			this.coloniesInfoGroup.SuspendLayout();
			this.buildingsGroup.SuspendLayout();
			this.outputInfoGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// starImage
			// 
			this.starImage.BackColor = System.Drawing.Color.Black;
			this.starImage.Location = new System.Drawing.Point(12, 12);
			this.starImage.Name = "starImage";
			this.starImage.Size = new System.Drawing.Size(64, 64);
			this.starImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.starImage.TabIndex = 0;
			this.starImage.TabStop = false;
			// 
			// coloniesInfoGroup
			// 
			this.coloniesInfoGroup.Controls.Add(this.infrastructureInfo);
			this.coloniesInfoGroup.Controls.Add(this.populationInfo);
			this.coloniesInfoGroup.Location = new System.Drawing.Point(82, 12);
			this.coloniesInfoGroup.Name = "coloniesInfoGroup";
			this.coloniesInfoGroup.Size = new System.Drawing.Size(116, 64);
			this.coloniesInfoGroup.TabIndex = 1;
			this.coloniesInfoGroup.TabStop = false;
			this.coloniesInfoGroup.Text = "Colonies";
			// 
			// infrastructureInfo
			// 
			this.infrastructureInfo.AutoSize = true;
			this.infrastructureInfo.Location = new System.Drawing.Point(6, 29);
			this.infrastructureInfo.Name = "infrastructureInfo";
			this.infrastructureInfo.Size = new System.Drawing.Size(104, 13);
			this.infrastructureInfo.TabIndex = 1;
			this.infrastructureInfo.Text = "Infrastructure: xx.x %";
			// 
			// populationInfo
			// 
			this.populationInfo.AutoSize = true;
			this.populationInfo.Location = new System.Drawing.Point(6, 16);
			this.populationInfo.Name = "populationInfo";
			this.populationInfo.Size = new System.Drawing.Size(91, 13);
			this.populationInfo.TabIndex = 0;
			this.populationInfo.Text = "Population: x.xx X";
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
			// traitList
			// 
			this.traitList.AutoScroll = true;
			this.traitList.Location = new System.Drawing.Point(204, 12);
			this.traitList.Name = "traitList";
			this.traitList.Size = new System.Drawing.Size(170, 76);
			this.traitList.TabIndex = 3;
			// 
			// outputInfoGroup
			// 
			this.outputInfoGroup.Controls.Add(this.researchInfo);
			this.outputInfoGroup.Controls.Add(this.developmentInfo);
			this.outputInfoGroup.Controls.Add(this.industryInfo);
			this.outputInfoGroup.Location = new System.Drawing.Point(204, 94);
			this.outputInfoGroup.Name = "outputInfoGroup";
			this.outputInfoGroup.Size = new System.Drawing.Size(170, 70);
			this.outputInfoGroup.TabIndex = 4;
			this.outputInfoGroup.TabStop = false;
			this.outputInfoGroup.Text = "Output";
			// 
			// researchInfo
			// 
			this.researchInfo.AutoSize = true;
			this.researchInfo.Location = new System.Drawing.Point(6, 42);
			this.researchInfo.Name = "researchInfo";
			this.researchInfo.Size = new System.Drawing.Size(74, 13);
			this.researchInfo.TabIndex = 2;
			this.researchInfo.Text = "Research: xxx";
			// 
			// developmentInfo
			// 
			this.developmentInfo.AutoSize = true;
			this.developmentInfo.Location = new System.Drawing.Point(6, 29);
			this.developmentInfo.Name = "developmentInfo";
			this.developmentInfo.Size = new System.Drawing.Size(104, 13);
			this.developmentInfo.TabIndex = 1;
			this.developmentInfo.Text = "Development: x.xx X";
			// 
			// industryInfo
			// 
			this.industryInfo.AutoSize = true;
			this.industryInfo.Location = new System.Drawing.Point(6, 16);
			this.industryInfo.Name = "industryInfo";
			this.industryInfo.Size = new System.Drawing.Size(78, 13);
			this.industryInfo.TabIndex = 0;
			this.industryInfo.Text = "Industry: x.xx X";
			// 
			// FormStellarisDetails
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(383, 295);
			this.Controls.Add(this.outputInfoGroup);
			this.Controls.Add(this.traitList);
			this.Controls.Add(this.buildingsGroup);
			this.Controls.Add(this.coloniesInfoGroup);
			this.Controls.Add(this.starImage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormStellarisDetails";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormStellarisDetails";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formStellarisDetails_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.starImage)).EndInit();
			this.coloniesInfoGroup.ResumeLayout(false);
			this.coloniesInfoGroup.PerformLayout();
			this.buildingsGroup.ResumeLayout(false);
			this.outputInfoGroup.ResumeLayout(false);
			this.outputInfoGroup.PerformLayout();
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.Label industryInfo;
		private System.Windows.Forms.Label developmentInfo;
		private System.Windows.Forms.Label researchInfo;
		private System.Windows.Forms.GroupBox outputInfoGroup;
		private System.Windows.Forms.FlowLayoutPanel traitList;
		private System.Windows.Forms.FlowLayoutPanel buildingsList;
		private System.Windows.Forms.GroupBox buildingsGroup;
		private System.Windows.Forms.Label infrastructureInfo;
		private System.Windows.Forms.Label populationInfo;
		private System.Windows.Forms.GroupBox coloniesInfoGroup;
		private System.Windows.Forms.PictureBox starImage;

		#endregion
	}
}