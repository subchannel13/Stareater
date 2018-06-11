/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 30.7.2013.
 * Time: 10:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class ConstructionSiteView
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
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
			this.queueButton = new System.Windows.Forms.Button();
			this.nameLabel = new System.Windows.Forms.Label();
			this.industrySlider = new System.Windows.Forms.HScrollBar();
			this.estimationLabel = new System.Windows.Forms.Label();
			this.detailsButton = new System.Windows.Forms.Button();
			this.policyButton = new System.Windows.Forms.Button();
			this.policyName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// queueButton
			// 
			this.queueButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.queueButton.Location = new System.Drawing.Point(8, 21);
			this.queueButton.Name = "queueButton";
			this.queueButton.Size = new System.Drawing.Size(88, 88);
			this.queueButton.TabIndex = 1;
			this.queueButton.Text = "button1";
			this.queueButton.UseVisualStyleBackColor = true;
			this.queueButton.Click += new System.EventHandler(this.queueButton_Click);
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nameLabel.Location = new System.Drawing.Point(8, 5);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(136, 13);
			this.nameLabel.TabIndex = 0;
			this.nameLabel.Text = "Construction site name";
			// 
			// industrySlider
			// 
			this.industrySlider.LargeChange = 1;
			this.industrySlider.Location = new System.Drawing.Point(99, 25);
			this.industrySlider.Name = "industrySlider";
			this.industrySlider.Size = new System.Drawing.Size(250, 17);
			this.industrySlider.TabIndex = 2;
			this.industrySlider.Scroll += new System.Windows.Forms.ScrollEventHandler(this.industrySlider_Scroll);
			// 
			// estimationLabel
			// 
			this.estimationLabel.AutoSize = true;
			this.estimationLabel.Location = new System.Drawing.Point(99, 46);
			this.estimationLabel.Name = "estimationLabel";
			this.estimationLabel.Size = new System.Drawing.Size(35, 13);
			this.estimationLabel.TabIndex = 3;
			this.estimationLabel.Text = "label1";
			// 
			// detailsButton
			// 
			this.detailsButton.Location = new System.Drawing.Point(274, 77);
			this.detailsButton.Name = "detailsButton";
			this.detailsButton.Size = new System.Drawing.Size(75, 23);
			this.detailsButton.TabIndex = 4;
			this.detailsButton.Text = "button1";
			this.detailsButton.UseVisualStyleBackColor = true;
			this.detailsButton.Click += new System.EventHandler(this.detailsButton_Click);
			// 
			// policyButton
			// 
			this.policyButton.Location = new System.Drawing.Point(99, 77);
			this.policyButton.Name = "policyButton";
			this.policyButton.Size = new System.Drawing.Size(32, 32);
			this.policyButton.TabIndex = 5;
			this.policyButton.UseVisualStyleBackColor = true;
			this.policyButton.Click += new System.EventHandler(this.policyButton_Click);
			// 
			// policyName
			// 
			this.policyName.AutoSize = true;
			this.policyName.Location = new System.Drawing.Point(137, 82);
			this.policyName.Name = "policyName";
			this.policyName.Size = new System.Drawing.Size(35, 13);
			this.policyName.TabIndex = 6;
			this.policyName.Text = "label1";
			// 
			// ConstructionSiteView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.Controls.Add(this.policyName);
			this.Controls.Add(this.policyButton);
			this.Controls.Add(this.detailsButton);
			this.Controls.Add(this.estimationLabel);
			this.Controls.Add(this.industrySlider);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.queueButton);
			this.Name = "ConstructionSiteView";
			this.Size = new System.Drawing.Size(358, 116);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Button detailsButton;
		private System.Windows.Forms.Label estimationLabel;
		private System.Windows.Forms.HScrollBar industrySlider;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Button queueButton;
		private System.Windows.Forms.Button policyButton;
		private System.Windows.Forms.Label policyName;
	}
}
