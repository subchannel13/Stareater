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
			this.topicName = new System.Windows.Forms.Label();
			this.levelLabel = new System.Windows.Forms.Label();
			this.levelInput = new System.Windows.Forms.NumericUpDown();
			this.maxLevelInfo = new System.Windows.Forms.Label();
			this.topicList.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.levelInput)).BeginInit();
			this.SuspendLayout();
			// 
			// topicText
			// 
			this.topicText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.topicText.Location = new System.Drawing.Point(218, 54);
			this.topicText.Name = "topicText";
			this.topicText.Size = new System.Drawing.Size(373, 427);
			this.topicText.TabIndex = 0;
			this.topicText.Text = "label1";
			// 
			// topicList
			// 
			this.topicList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
			this.topicList.AutoScroll = true;
			this.topicList.Controls.Add(this.researchLink);
			this.topicList.Controls.Add(this.developmentLink);
			this.topicList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.topicList.Location = new System.Drawing.Point(12, 12);
			this.topicList.Name = "topicList";
			this.topicList.Size = new System.Drawing.Size(200, 466);
			this.topicList.TabIndex = 1;
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
			// 
			// topicName
			// 
			this.topicName.AutoSize = true;
			this.topicName.Location = new System.Drawing.Point(218, 12);
			this.topicName.Name = "topicName";
			this.topicName.Size = new System.Drawing.Size(58, 13);
			this.topicName.TabIndex = 2;
			this.topicName.Text = "Item Name";
			// 
			// levelLabel
			// 
			this.levelLabel.AutoSize = true;
			this.levelLabel.Location = new System.Drawing.Point(218, 30);
			this.levelLabel.Name = "levelLabel";
			this.levelLabel.Size = new System.Drawing.Size(32, 13);
			this.levelLabel.TabIndex = 3;
			this.levelLabel.Text = "level:";
			// 
			// levelInput
			// 
			this.levelInput.Location = new System.Drawing.Point(288, 28);
			this.levelInput.Name = "levelInput";
			this.levelInput.Size = new System.Drawing.Size(49, 20);
			this.levelInput.TabIndex = 4;
			// 
			// maxLevelInfo
			// 
			this.maxLevelInfo.AutoSize = true;
			this.maxLevelInfo.Location = new System.Drawing.Point(343, 30);
			this.maxLevelInfo.Name = "maxLevelInfo";
			this.maxLevelInfo.Size = new System.Drawing.Size(20, 13);
			this.maxLevelInfo.TabIndex = 5;
			this.maxLevelInfo.Text = "/ x";
			// 
			// FormLibrary
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(603, 490);
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
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
