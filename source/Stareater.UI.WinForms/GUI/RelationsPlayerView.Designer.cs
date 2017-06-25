namespace Stareater.GUI
{
	partial class RelationsPlayerView
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.PictureBox playerColor;
		private System.Windows.Forms.Label playerName;
		private System.Windows.Forms.FlowLayoutPanel treatyList;
		
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
			this.playerColor = new System.Windows.Forms.PictureBox();
			this.playerName = new System.Windows.Forms.Label();
			this.treatyList = new System.Windows.Forms.FlowLayoutPanel();
			this.audienceRequest = new Stareater.GUI.TreatyBriefView();
			((System.ComponentModel.ISupportInitialize)(this.playerColor)).BeginInit();
			this.treatyList.SuspendLayout();
			this.SuspendLayout();
			// 
			// playerColor
			// 
			this.playerColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.playerColor.Location = new System.Drawing.Point(3, 3);
			this.playerColor.Name = "playerColor";
			this.playerColor.Size = new System.Drawing.Size(33, 33);
			this.playerColor.TabIndex = 0;
			this.playerColor.TabStop = false;
			this.playerColor.Click += new System.EventHandler(this.playerColor_Click);
			// 
			// playerName
			// 
			this.playerName.Location = new System.Drawing.Point(43, 4);
			this.playerName.Name = "playerName";
			this.playerName.Size = new System.Drawing.Size(114, 32);
			this.playerName.TabIndex = 0;
			this.playerName.Text = "label1";
			this.playerName.Click += new System.EventHandler(this.playerName_Click);
			// 
			// treatyList
			// 
			this.treatyList.AutoScroll = true;
			this.treatyList.Controls.Add(this.audienceRequest);
			this.treatyList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.treatyList.Location = new System.Drawing.Point(3, 42);
			this.treatyList.Name = "treatyList";
			this.treatyList.Size = new System.Drawing.Size(154, 75);
			this.treatyList.TabIndex = 1;
			this.treatyList.Click += new System.EventHandler(this.treatyList_Click);
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
			// RelationsPlayerView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.treatyList);
			this.Controls.Add(this.playerName);
			this.Controls.Add(this.playerColor);
			this.Name = "RelationsPlayerView";
			this.Size = new System.Drawing.Size(160, 120);
			((System.ComponentModel.ISupportInitialize)(this.playerColor)).EndInit();
			this.treatyList.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		private TreatyBriefView audienceRequest;
	}
}
