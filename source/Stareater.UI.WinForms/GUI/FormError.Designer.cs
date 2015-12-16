namespace Stareater.GUI
{
	partial class FormError
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label headerText;
		private System.Windows.Forms.LinkLabel issuesLink;
		private System.Windows.Forms.TextBox errorText;
		private System.Windows.Forms.Button closeButton;
		
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
			this.headerText = new System.Windows.Forms.Label();
			this.issuesLink = new System.Windows.Forms.LinkLabel();
			this.errorText = new System.Windows.Forms.TextBox();
			this.closeButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// headerText
			// 
			this.headerText.AutoSize = true;
			this.headerText.Location = new System.Drawing.Point(12, 9);
			this.headerText.Name = "headerText";
			this.headerText.Size = new System.Drawing.Size(289, 26);
			this.headerText.TabIndex = 0;
			this.headerText.Text = "Run time exception occurred, please reoport it to developer.\r\nError message:";
			// 
			// issuesLink
			// 
			this.issuesLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.issuesLink.AutoSize = true;
			this.issuesLink.Location = new System.Drawing.Point(12, 352);
			this.issuesLink.Name = "issuesLink";
			this.issuesLink.Size = new System.Drawing.Size(115, 13);
			this.issuesLink.TabIndex = 1;
			this.issuesLink.TabStop = true;
			this.issuesLink.Text = "Stareater bug reporting";
			this.issuesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.IssuesLinkLinkClicked);
			// 
			// errorText
			// 
			this.errorText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.errorText.Location = new System.Drawing.Point(12, 38);
			this.errorText.Multiline = true;
			this.errorText.Name = "errorText";
			this.errorText.ReadOnly = true;
			this.errorText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.errorText.Size = new System.Drawing.Size(518, 303);
			this.errorText.TabIndex = 2;
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.Location = new System.Drawing.Point(457, 347);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 3;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
			// 
			// FormError
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(544, 382);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.errorText);
			this.Controls.Add(this.issuesLink);
			this.Controls.Add(this.headerText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormError";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Something exlopded...";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
