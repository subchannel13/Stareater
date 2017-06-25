namespace Stareater.GUI
{
	partial class FormAudience
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private Stareater.GUI.PlayerView player1View;
		private Stareater.GUI.PlayerView player2View;
		private System.Windows.Forms.FlowLayoutPanel treatyList;
		private Stareater.GUI.TreatyBriefView audienceRequest;
		private System.Windows.Forms.Button endAudienceAction;
		private System.Windows.Forms.Button warAction;
		
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
			this.player1View = new Stareater.GUI.PlayerView();
			this.player2View = new Stareater.GUI.PlayerView();
			this.treatyList = new System.Windows.Forms.FlowLayoutPanel();
			this.audienceRequest = new Stareater.GUI.TreatyBriefView();
			this.endAudienceAction = new System.Windows.Forms.Button();
			this.warAction = new System.Windows.Forms.Button();
			this.treatyList.SuspendLayout();
			this.SuspendLayout();
			// 
			// player1View
			// 
			this.player1View.Location = new System.Drawing.Point(12, 12);
			this.player1View.Name = "player1View";
			this.player1View.Size = new System.Drawing.Size(162, 40);
			this.player1View.TabIndex = 0;
			// 
			// player2View
			// 
			this.player2View.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.player2View.Location = new System.Drawing.Point(260, 12);
			this.player2View.Name = "player2View";
			this.player2View.Size = new System.Drawing.Size(162, 40);
			this.player2View.TabIndex = 1;
			// 
			// treatyList
			// 
			this.treatyList.AutoScroll = true;
			this.treatyList.Controls.Add(this.audienceRequest);
			this.treatyList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.treatyList.Location = new System.Drawing.Point(146, 58);
			this.treatyList.Name = "treatyList";
			this.treatyList.Size = new System.Drawing.Size(154, 75);
			this.treatyList.TabIndex = 2;
			// 
			// audienceRequest
			// 
			this.audienceRequest.Location = new System.Drawing.Point(0, 3);
			this.audienceRequest.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.audienceRequest.Name = "audienceRequest";
			this.audienceRequest.Size = new System.Drawing.Size(135, 20);
			this.audienceRequest.TabIndex = 0;
			this.audienceRequest.Visible = false;
			// 
			// endAudienceAction
			// 
			this.endAudienceAction.Location = new System.Drawing.Point(327, 205);
			this.endAudienceAction.Name = "endAudienceAction";
			this.endAudienceAction.Size = new System.Drawing.Size(95, 45);
			this.endAudienceAction.TabIndex = 4;
			this.endAudienceAction.Text = "button1";
			this.endAudienceAction.UseVisualStyleBackColor = true;
			this.endAudienceAction.Click += new System.EventHandler(this.endAudienceAction_Click);
			// 
			// warAction
			// 
			this.warAction.Location = new System.Drawing.Point(12, 170);
			this.warAction.Name = "warAction";
			this.warAction.Size = new System.Drawing.Size(75, 30);
			this.warAction.TabIndex = 3;
			this.warAction.Text = "button1";
			this.warAction.UseVisualStyleBackColor = true;
			this.warAction.Click += new System.EventHandler(this.warAction_Click);
			// 
			// FormAudience
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(434, 262);
			this.Controls.Add(this.warAction);
			this.Controls.Add(this.endAudienceAction);
			this.Controls.Add(this.treatyList);
			this.Controls.Add(this.player2View);
			this.Controls.Add(this.player1View);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "FormAudience";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormAudience";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formAudience_FormClosed);
			this.treatyList.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
