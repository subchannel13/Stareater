
namespace Stareater.GUI
{
	partial class FormDevelopment
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
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
			this.topicList = new Zvjezdojedac.GUI.ControlListView();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lable1 = new System.Windows.Forms.Label();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.reorderTopAction = new System.Windows.Forms.Button();
			this.reorderUpAction = new System.Windows.Forms.Button();
			this.reorderDownAction = new System.Windows.Forms.Button();
			this.reorderBottomAction = new System.Windows.Forms.Button();
			this.techImage = new System.Windows.Forms.PictureBox();
			this.techLevel = new System.Windows.Forms.Label();
			this.techName = new System.Windows.Forms.Label();
			this.techDescription = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.techImage)).BeginInit();
			this.SuspendLayout();
			// 
			// topicList
			// 
			this.topicList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.topicList.AutoScroll = true;
			this.topicList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.topicList.Location = new System.Drawing.Point(12, 12);
			this.topicList.Name = "topicList";
			this.topicList.SelectedIndex = -1;
			this.topicList.Size = new System.Drawing.Size(277, 452);
			this.topicList.TabIndex = 1;
			this.topicList.SelectedIndexChanged += new System.EventHandler(this.topicList_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(372, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(135, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "Development points: x.xx X";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(372, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 13);
			this.label2.TabIndex = 13;
			this.label2.Text = "Distribution";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(574, 111);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "Focused";
			// 
			// lable1
			// 
			this.lable1.AutoSize = true;
			this.lable1.Location = new System.Drawing.Point(372, 111);
			this.lable1.Name = "lable1";
			this.lable1.Size = new System.Drawing.Size(32, 13);
			this.lable1.TabIndex = 11;
			this.lable1.Text = "Even";
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(372, 63);
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(250, 45);
			this.trackBar1.TabIndex = 10;
			this.trackBar1.Value = 5;
			// 
			// reorderTopAction
			// 
			this.reorderTopAction.Image = global::Stareater.Properties.Resources.arrowFirst;
			this.reorderTopAction.Location = new System.Drawing.Point(295, 74);
			this.reorderTopAction.Name = "reorderTopAction";
			this.reorderTopAction.Size = new System.Drawing.Size(50, 50);
			this.reorderTopAction.TabIndex = 15;
			this.reorderTopAction.UseVisualStyleBackColor = true;
			this.reorderTopAction.Click += new System.EventHandler(this.reorderTopAction_Click);
			// 
			// reorderUpAction
			// 
			this.reorderUpAction.Image = global::Stareater.Properties.Resources.arrowUp;
			this.reorderUpAction.Location = new System.Drawing.Point(295, 130);
			this.reorderUpAction.Name = "reorderUpAction";
			this.reorderUpAction.Size = new System.Drawing.Size(50, 50);
			this.reorderUpAction.TabIndex = 16;
			this.reorderUpAction.UseVisualStyleBackColor = true;
			this.reorderUpAction.Click += new System.EventHandler(this.reorderUpAction_Click);
			// 
			// reorderDownAction
			// 
			this.reorderDownAction.Image = global::Stareater.Properties.Resources.arrowDown;
			this.reorderDownAction.Location = new System.Drawing.Point(295, 186);
			this.reorderDownAction.Name = "reorderDownAction";
			this.reorderDownAction.Size = new System.Drawing.Size(50, 50);
			this.reorderDownAction.TabIndex = 17;
			this.reorderDownAction.UseVisualStyleBackColor = true;
			this.reorderDownAction.Click += new System.EventHandler(this.reorderDownAction_Click);
			// 
			// reorderBottomAction
			// 
			this.reorderBottomAction.Image = global::Stareater.Properties.Resources.arrowLast;
			this.reorderBottomAction.Location = new System.Drawing.Point(295, 242);
			this.reorderBottomAction.Name = "reorderBottomAction";
			this.reorderBottomAction.Size = new System.Drawing.Size(50, 50);
			this.reorderBottomAction.TabIndex = 18;
			this.reorderBottomAction.UseVisualStyleBackColor = true;
			this.reorderBottomAction.Click += new System.EventHandler(this.reorderBottomAction_Click);
			// 
			// techImage
			// 
			this.techImage.Location = new System.Drawing.Point(372, 186);
			this.techImage.Name = "techImage";
			this.techImage.Size = new System.Drawing.Size(80, 80);
			this.techImage.TabIndex = 20;
			this.techImage.TabStop = false;
			// 
			// techLevel
			// 
			this.techLevel.AutoSize = true;
			this.techLevel.Location = new System.Drawing.Point(458, 200);
			this.techLevel.Name = "techLevel";
			this.techLevel.Size = new System.Drawing.Size(43, 13);
			this.techLevel.TabIndex = 22;
			this.techLevel.Text = "Level X";
			// 
			// techName
			// 
			this.techName.AutoSize = true;
			this.techName.Location = new System.Drawing.Point(458, 186);
			this.techName.Name = "techName";
			this.techName.Size = new System.Drawing.Size(61, 13);
			this.techName.TabIndex = 21;
			this.techName.Text = "Tech name";
			// 
			// techDescription
			// 
			this.techDescription.Location = new System.Drawing.Point(372, 272);
			this.techDescription.Multiline = true;
			this.techDescription.Name = "techDescription";
			this.techDescription.ReadOnly = true;
			this.techDescription.Size = new System.Drawing.Size(250, 192);
			this.techDescription.TabIndex = 23;
			this.techDescription.Text = "Description here";
			// 
			// FormDevelopment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(634, 476);
			this.Controls.Add(this.techDescription);
			this.Controls.Add(this.techLevel);
			this.Controls.Add(this.techName);
			this.Controls.Add(this.techImage);
			this.Controls.Add(this.reorderBottomAction);
			this.Controls.Add(this.reorderDownAction);
			this.Controls.Add(this.reorderUpAction);
			this.Controls.Add(this.reorderTopAction);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lable1);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.topicList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormDevelopment";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Development topics";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formDevelopment_FormClosed);
			this.Load += new System.EventHandler(this.formDevelopment_Load);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.techImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox techDescription;
		private System.Windows.Forms.Label techName;
		private System.Windows.Forms.Label techLevel;
		private System.Windows.Forms.PictureBox techImage;
		private System.Windows.Forms.Button reorderBottomAction;
		private System.Windows.Forms.Button reorderDownAction;
		private System.Windows.Forms.Button reorderUpAction;
		private System.Windows.Forms.Button reorderTopAction;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Label lable1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private Zvjezdojedac.GUI.ControlListView topicList;
	}
}
