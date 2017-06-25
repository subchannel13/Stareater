/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 28.6.2016.
 * Time: 9:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormLibrary
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label topicText;
		private System.Windows.Forms.FlowLayoutPanel topicList;
		private System.Windows.Forms.LinkLabel researchLink;
		private System.Windows.Forms.LinkLabel developmentLink;
		private System.Windows.Forms.Label topicName;
		private System.Windows.Forms.Label levelLabel;
		private System.Windows.Forms.NumericUpDown levelInput;
		private System.Windows.Forms.Label maxLevelInfo;
		private System.Windows.Forms.Label topicSeparator;
		private System.Windows.Forms.LinkLabel armorLink;
		private System.Windows.Forms.LinkLabel hullLink;
		private System.Windows.Forms.LinkLabel isDriveLink;
		private System.Windows.Forms.LinkLabel missionEquipLink;
		private System.Windows.Forms.LinkLabel reactorLink;
		private System.Windows.Forms.LinkLabel sensorLink;
		private System.Windows.Forms.LinkLabel thrusterLink;
		private System.Windows.Forms.LinkLabel specialEquipLink;
		private System.Windows.Forms.PictureBox topicThumbnail;
		
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
			this.topicText = new System.Windows.Forms.Label();
			this.topicList = new System.Windows.Forms.FlowLayoutPanel();
			this.researchLink = new System.Windows.Forms.LinkLabel();
			this.developmentLink = new System.Windows.Forms.LinkLabel();
			this.armorLink = new System.Windows.Forms.LinkLabel();
			this.hullLink = new System.Windows.Forms.LinkLabel();
			this.isDriveLink = new System.Windows.Forms.LinkLabel();
			this.missionEquipLink = new System.Windows.Forms.LinkLabel();
			this.reactorLink = new System.Windows.Forms.LinkLabel();
			this.sensorLink = new System.Windows.Forms.LinkLabel();
			this.specialEquipLink = new System.Windows.Forms.LinkLabel();
			this.thrusterLink = new System.Windows.Forms.LinkLabel();
			this.topicSeparator = new System.Windows.Forms.Label();
			this.topicName = new System.Windows.Forms.Label();
			this.levelLabel = new System.Windows.Forms.Label();
			this.levelInput = new System.Windows.Forms.NumericUpDown();
			this.maxLevelInfo = new System.Windows.Forms.Label();
			this.topicThumbnail = new System.Windows.Forms.PictureBox();
			this.topicList.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.levelInput)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.topicThumbnail)).BeginInit();
			this.SuspendLayout();
			// 
			// topicText
			// 
			this.topicText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.topicText.Location = new System.Drawing.Point(218, 67);
			this.topicText.Name = "topicText";
			this.topicText.Size = new System.Drawing.Size(373, 414);
			this.topicText.TabIndex = 5;
			this.topicText.Text = "label1";
			this.topicText.Visible = false;
			// 
			// topicList
			// 
			this.topicList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.topicList.AutoScroll = true;
			this.topicList.Controls.Add(this.researchLink);
			this.topicList.Controls.Add(this.developmentLink);
			this.topicList.Controls.Add(this.armorLink);
			this.topicList.Controls.Add(this.hullLink);
			this.topicList.Controls.Add(this.isDriveLink);
			this.topicList.Controls.Add(this.missionEquipLink);
			this.topicList.Controls.Add(this.reactorLink);
			this.topicList.Controls.Add(this.sensorLink);
			this.topicList.Controls.Add(this.specialEquipLink);
			this.topicList.Controls.Add(this.thrusterLink);
			this.topicList.Controls.Add(this.topicSeparator);
			this.topicList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.topicList.Location = new System.Drawing.Point(12, 12);
			this.topicList.Name = "topicList";
			this.topicList.Size = new System.Drawing.Size(200, 466);
			this.topicList.TabIndex = 0;
			this.topicList.WrapContents = false;
			// 
			// researchLink
			// 
			this.researchLink.AutoSize = true;
			this.researchLink.Location = new System.Drawing.Point(3, 0);
			this.researchLink.Name = "researchLink";
			this.researchLink.Size = new System.Drawing.Size(69, 13);
			this.researchLink.TabIndex = 0;
			this.researchLink.TabStop = true;
			this.researchLink.Text = "linkResearch";
			this.researchLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.researchLink_LinkClicked);
			// 
			// developmentLink
			// 
			this.developmentLink.AutoSize = true;
			this.developmentLink.Location = new System.Drawing.Point(3, 16);
			this.developmentLink.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.developmentLink.Name = "developmentLink";
			this.developmentLink.Size = new System.Drawing.Size(86, 13);
			this.developmentLink.TabIndex = 1;
			this.developmentLink.TabStop = true;
			this.developmentLink.Text = "linkDevelopment";
			this.developmentLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.developmentLink_LinkClicked);
			// 
			// armorLink
			// 
			this.armorLink.AutoSize = true;
			this.armorLink.Location = new System.Drawing.Point(3, 39);
			this.armorLink.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this.armorLink.Name = "armorLink";
			this.armorLink.Size = new System.Drawing.Size(50, 13);
			this.armorLink.TabIndex = 2;
			this.armorLink.TabStop = true;
			this.armorLink.Text = "linkArmor";
			this.armorLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.armorLink_LinkClicked);
			// 
			// hullLink
			// 
			this.hullLink.AutoSize = true;
			this.hullLink.Location = new System.Drawing.Point(3, 55);
			this.hullLink.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.hullLink.Name = "hullLink";
			this.hullLink.Size = new System.Drawing.Size(41, 13);
			this.hullLink.TabIndex = 3;
			this.hullLink.TabStop = true;
			this.hullLink.Text = "linkHull";
			this.hullLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.hullLink_LinkClicked);
			// 
			// isDriveLink
			// 
			this.isDriveLink.AutoSize = true;
			this.isDriveLink.Location = new System.Drawing.Point(3, 71);
			this.isDriveLink.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.isDriveLink.Name = "isDriveLink";
			this.isDriveLink.Size = new System.Drawing.Size(61, 13);
			this.isDriveLink.TabIndex = 4;
			this.isDriveLink.TabStop = true;
			this.isDriveLink.Text = "linkIsDrives";
			this.isDriveLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.isDriveLink_LinkClicked);
			// 
			// missionEquipLink
			// 
			this.missionEquipLink.AutoSize = true;
			this.missionEquipLink.Location = new System.Drawing.Point(3, 87);
			this.missionEquipLink.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.missionEquipLink.Name = "missionEquipLink";
			this.missionEquipLink.Size = new System.Drawing.Size(85, 13);
			this.missionEquipLink.TabIndex = 5;
			this.missionEquipLink.TabStop = true;
			this.missionEquipLink.Text = "linkMissionEquip";
			this.missionEquipLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.missionEquipLink_LinkClicked);
			// 
			// reactorLink
			// 
			this.reactorLink.AutoSize = true;
			this.reactorLink.Location = new System.Drawing.Point(3, 103);
			this.reactorLink.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.reactorLink.Name = "reactorLink";
			this.reactorLink.Size = new System.Drawing.Size(61, 13);
			this.reactorLink.TabIndex = 6;
			this.reactorLink.TabStop = true;
			this.reactorLink.Text = "linkReactor";
			this.reactorLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.reactorLink_LinkClicked);
			// 
			// sensorLink
			// 
			this.sensorLink.AutoSize = true;
			this.sensorLink.Location = new System.Drawing.Point(3, 119);
			this.sensorLink.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.sensorLink.Name = "sensorLink";
			this.sensorLink.Size = new System.Drawing.Size(56, 13);
			this.sensorLink.TabIndex = 7;
			this.sensorLink.TabStop = true;
			this.sensorLink.Text = "linkSensor";
			this.sensorLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.sensorLink_LinkClicked);
			// 
			// specialEquipLink
			// 
			this.specialEquipLink.AutoSize = true;
			this.specialEquipLink.Location = new System.Drawing.Point(3, 135);
			this.specialEquipLink.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.specialEquipLink.Name = "specialEquipLink";
			this.specialEquipLink.Size = new System.Drawing.Size(85, 13);
			this.specialEquipLink.TabIndex = 8;
			this.specialEquipLink.TabStop = true;
			this.specialEquipLink.Text = "linkSpecialEquip";
			this.specialEquipLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.specialEquipLink_LinkClicked);
			// 
			// thrusterLink
			// 
			this.thrusterLink.AutoSize = true;
			this.thrusterLink.Location = new System.Drawing.Point(3, 151);
			this.thrusterLink.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.thrusterLink.Name = "thrusterLink";
			this.thrusterLink.Size = new System.Drawing.Size(62, 13);
			this.thrusterLink.TabIndex = 9;
			this.thrusterLink.TabStop = true;
			this.thrusterLink.Text = "linkThruster";
			this.thrusterLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.thrusterLink_LinkClicked);
			// 
			// topicSeparator
			// 
			this.topicSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.topicSeparator.Location = new System.Drawing.Point(3, 174);
			this.topicSeparator.Margin = new System.Windows.Forms.Padding(3, 10, 3, 2);
			this.topicSeparator.Name = "topicSeparator";
			this.topicSeparator.Size = new System.Drawing.Size(150, 3);
			this.topicSeparator.TabIndex = 10;
			// 
			// topicName
			// 
			this.topicName.AutoSize = true;
			this.topicName.Location = new System.Drawing.Point(264, 12);
			this.topicName.Name = "topicName";
			this.topicName.Size = new System.Drawing.Size(58, 13);
			this.topicName.TabIndex = 1;
			this.topicName.Text = "Item Name";
			this.topicName.Visible = false;
			// 
			// levelLabel
			// 
			this.levelLabel.AutoSize = true;
			this.levelLabel.Location = new System.Drawing.Point(264, 30);
			this.levelLabel.Name = "levelLabel";
			this.levelLabel.Size = new System.Drawing.Size(32, 13);
			this.levelLabel.TabIndex = 2;
			this.levelLabel.Text = "level:";
			this.levelLabel.Visible = false;
			// 
			// levelInput
			// 
			this.levelInput.Location = new System.Drawing.Point(334, 28);
			this.levelInput.Name = "levelInput";
			this.levelInput.Size = new System.Drawing.Size(40, 20);
			this.levelInput.TabIndex = 3;
			this.levelInput.Visible = false;
			this.levelInput.ValueChanged += new System.EventHandler(this.levelInput_ValueChanged);
			// 
			// maxLevelInfo
			// 
			this.maxLevelInfo.AutoSize = true;
			this.maxLevelInfo.Location = new System.Drawing.Point(380, 30);
			this.maxLevelInfo.Name = "maxLevelInfo";
			this.maxLevelInfo.Size = new System.Drawing.Size(20, 13);
			this.maxLevelInfo.TabIndex = 4;
			this.maxLevelInfo.Text = "/ x";
			this.maxLevelInfo.Visible = false;
			// 
			// topicThumbnail
			// 
			this.topicThumbnail.Location = new System.Drawing.Point(218, 12);
			this.topicThumbnail.Name = "topicThumbnail";
			this.topicThumbnail.Size = new System.Drawing.Size(40, 40);
			this.topicThumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.topicThumbnail.TabIndex = 6;
			this.topicThumbnail.TabStop = false;
			// 
			// FormLibrary
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(603, 490);
			this.Controls.Add(this.topicThumbnail);
			this.Controls.Add(this.maxLevelInfo);
			this.Controls.Add(this.levelInput);
			this.Controls.Add(this.levelLabel);
			this.Controls.Add(this.topicName);
			this.Controls.Add(this.topicList);
			this.Controls.Add(this.topicText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormLibrary";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormLibrary";
			this.topicList.ResumeLayout(false);
			this.topicList.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.levelInput)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.topicThumbnail)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
