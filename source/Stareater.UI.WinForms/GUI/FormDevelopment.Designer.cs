
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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
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
			// button1
			// 
			this.button1.Image = global::Stareater.Properties.Resources.arrowFirst;
			this.button1.Location = new System.Drawing.Point(295, 74);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(50, 50);
			this.button1.TabIndex = 15;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Image = global::Stareater.Properties.Resources.arrowUp;
			this.button2.Location = new System.Drawing.Point(295, 130);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(50, 50);
			this.button2.TabIndex = 16;
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Image = global::Stareater.Properties.Resources.arrowDown;
			this.button3.Location = new System.Drawing.Point(295, 186);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(50, 50);
			this.button3.TabIndex = 17;
			this.button3.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.Image = global::Stareater.Properties.Resources.arrowLast;
			this.button4.Location = new System.Drawing.Point(295, 242);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(50, 50);
			this.button4.TabIndex = 18;
			this.button4.UseVisualStyleBackColor = true;
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
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
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
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Label lable1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private Zvjezdojedac.GUI.ControlListView topicList;
	}
}
