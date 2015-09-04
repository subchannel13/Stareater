/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 3.9.2015.
 * Time: 15:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class EmpyPlanetView
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Button detailsButton;
		private System.Windows.Forms.Label estimationLabel;
		private System.Windows.Forms.Button colonizeButton;
		
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
			this.nameLabel = new System.Windows.Forms.Label();
			this.detailsButton = new System.Windows.Forms.Button();
			this.estimationLabel = new System.Windows.Forms.Label();
			this.colonizeButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nameLabel.Location = new System.Drawing.Point(8, 5);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(112, 13);
			this.nameLabel.TabIndex = 2;
			this.nameLabel.Text = "System body name";
			// 
			// detailsButton
			// 
			this.detailsButton.Location = new System.Drawing.Point(274, 86);
			this.detailsButton.Name = "detailsButton";
			this.detailsButton.Size = new System.Drawing.Size(75, 23);
			this.detailsButton.TabIndex = 7;
			this.detailsButton.Text = "button1";
			this.detailsButton.UseVisualStyleBackColor = true;
			// 
			// estimationLabel
			// 
			this.estimationLabel.AutoSize = true;
			this.estimationLabel.Location = new System.Drawing.Point(102, 21);
			this.estimationLabel.Name = "estimationLabel";
			this.estimationLabel.Size = new System.Drawing.Size(35, 13);
			this.estimationLabel.TabIndex = 6;
			this.estimationLabel.Text = "label1";
			// 
			// colonizeButton
			// 
			this.colonizeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.colonizeButton.Location = new System.Drawing.Point(8, 21);
			this.colonizeButton.Name = "colonizeButton";
			this.colonizeButton.Size = new System.Drawing.Size(88, 88);
			this.colonizeButton.TabIndex = 5;
			this.colonizeButton.Text = "button1";
			this.colonizeButton.UseVisualStyleBackColor = true;
			// 
			// EmpyPlanetView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.Controls.Add(this.detailsButton);
			this.Controls.Add(this.estimationLabel);
			this.Controls.Add(this.colonizeButton);
			this.Controls.Add(this.nameLabel);
			this.Name = "EmpyPlanetView";
			this.Size = new System.Drawing.Size(358, 116);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
