
namespace Winforms_Mockups
{
	partial class FormBuildingQueue
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBuildingQueue));
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.controlListView1 = new My_ListView.ControlListView();
			this.constructionItem1 = new Winforms_Mockups.ConstructionItem();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.controlListView2 = new My_ListView.ControlListView();
			this.constructableItem1 = new Winforms_Mockups.ConstructableItem();
			this.constructableItem2 = new Winforms_Mockups.ConstructableItem();
			this.constructableItem3 = new Winforms_Mockups.ConstructableItem();
			this.constructableItem4 = new Winforms_Mockups.ConstructableItem();
			this.constructableItem5 = new Winforms_Mockups.ConstructableItem();
			this.controlListView1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.controlListView2.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "krstarica.png");
			this.imageList1.Images.SetKeyName(1, "hydroponicFarms.png");
			this.imageList1.Images.SetKeyName(2, "industry.png");
			this.imageList1.Images.SetKeyName(3, "spaceport.png");
			// 
			// controlListView1
			// 
			this.controlListView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.controlListView1.Controls.Add(this.constructionItem1);
			this.controlListView1.Location = new System.Drawing.Point(278, 215);
			this.controlListView1.Name = "controlListView1";
			this.controlListView1.Size = new System.Drawing.Size(246, 186);
			this.controlListView1.TabIndex = 1;
			// 
			// constructionItem1
			// 
			this.constructionItem1.Location = new System.Drawing.Point(3, 3);
			this.constructionItem1.Name = "constructionItem1";
			this.constructionItem1.Size = new System.Drawing.Size(200, 38);
			this.constructionItem1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Image = global::Winforms_Mockups.Properties.Resources.arrowRight;
			this.button1.Location = new System.Drawing.Point(232, 259);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(40, 40);
			this.button1.TabIndex = 2;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Image = global::Winforms_Mockups.Properties.Resources.arrowLeft;
			this.button2.Location = new System.Drawing.Point(232, 305);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(40, 40);
			this.button2.TabIndex = 3;
			this.button2.UseVisualStyleBackColor = true;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Winforms_Mockups.Properties.Resources.hydroponic_farms;
			this.pictureBox1.Location = new System.Drawing.Point(278, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(40, 40);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(324, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 198);
			this.label1.TabIndex = 5;
			this.label1.Text = "label1";
			// 
			// controlListView2
			// 
			this.controlListView2.AutoScroll = true;
			this.controlListView2.Controls.Add(this.constructableItem1);
			this.controlListView2.Controls.Add(this.constructableItem2);
			this.controlListView2.Controls.Add(this.constructableItem3);
			this.controlListView2.Controls.Add(this.constructableItem4);
			this.controlListView2.Controls.Add(this.constructableItem5);
			this.controlListView2.Location = new System.Drawing.Point(12, 12);
			this.controlListView2.Name = "controlListView2";
			this.controlListView2.Size = new System.Drawing.Size(214, 389);
			this.controlListView2.TabIndex = 6;
			// 
			// constructableItem1
			// 
			this.constructableItem1.Location = new System.Drawing.Point(3, 3);
			this.constructableItem1.Name = "constructableItem1";
			this.constructableItem1.Size = new System.Drawing.Size(150, 38);
			this.constructableItem1.TabIndex = 0;
			// 
			// constructableItem2
			// 
			this.constructableItem2.Location = new System.Drawing.Point(3, 47);
			this.constructableItem2.Name = "constructableItem2";
			this.constructableItem2.Size = new System.Drawing.Size(150, 38);
			this.constructableItem2.TabIndex = 1;
			// 
			// constructableItem3
			// 
			this.constructableItem3.Location = new System.Drawing.Point(3, 91);
			this.constructableItem3.Name = "constructableItem3";
			this.constructableItem3.Size = new System.Drawing.Size(150, 38);
			this.constructableItem3.TabIndex = 2;
			// 
			// constructableItem4
			// 
			this.constructableItem4.Location = new System.Drawing.Point(3, 135);
			this.constructableItem4.Name = "constructableItem4";
			this.constructableItem4.Size = new System.Drawing.Size(150, 38);
			this.constructableItem4.TabIndex = 3;
			// 
			// constructableItem5
			// 
			this.constructableItem5.Location = new System.Drawing.Point(3, 179);
			this.constructableItem5.Name = "constructableItem5";
			this.constructableItem5.Size = new System.Drawing.Size(150, 38);
			this.constructableItem5.TabIndex = 4;
			// 
			// FormBuildingQueue
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(536, 413);
			this.Controls.Add(this.controlListView2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.controlListView1);
			this.Name = "FormBuildingQueue";
			this.Text = "FormBuildingQueue";
			this.controlListView1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.controlListView2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private Winforms_Mockups.ConstructableItem constructableItem5;
		private Winforms_Mockups.ConstructableItem constructableItem4;
		private Winforms_Mockups.ConstructableItem constructableItem3;
		private Winforms_Mockups.ConstructableItem constructableItem2;
		private Winforms_Mockups.ConstructableItem constructableItem1;
		private My_ListView.ControlListView controlListView2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private Winforms_Mockups.ConstructionItem constructionItem1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private My_ListView.ControlListView controlListView1;
		private System.Windows.Forms.ImageList imageList1;
	}
}
