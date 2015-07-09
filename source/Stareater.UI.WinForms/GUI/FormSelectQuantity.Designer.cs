/*
 * Created by SharpDevelop.
 * User: ekraiva
 * Date: 7.7.2015.
 * Time: 11:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Stareater.GUI
{
	partial class FormSelectQuantity
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TrackBar quantitySlider;
		private System.Windows.Forms.TextBox quantityInput;
		private System.Windows.Forms.Button acceptButton;
		
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
			this.quantitySlider = new System.Windows.Forms.TrackBar();
			this.quantityInput = new System.Windows.Forms.TextBox();
			this.acceptButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.quantitySlider)).BeginInit();
			this.SuspendLayout();
			// 
			// quantitySlider
			// 
			this.quantitySlider.AutoSize = false;
			this.quantitySlider.Location = new System.Drawing.Point(12, 12);
			this.quantitySlider.Maximum = 100;
			this.quantitySlider.Name = "quantitySlider";
			this.quantitySlider.Size = new System.Drawing.Size(120, 23);
			this.quantitySlider.TabIndex = 1;
			this.quantitySlider.TickStyle = System.Windows.Forms.TickStyle.None;
			this.quantitySlider.Scroll += new System.EventHandler(this.quantitySlider_Scroll);
			// 
			// quantityInput
			// 
			this.quantityInput.Location = new System.Drawing.Point(138, 12);
			this.quantityInput.Name = "quantityInput";
			this.quantityInput.Size = new System.Drawing.Size(60, 20);
			this.quantityInput.TabIndex = 2;
			this.quantityInput.TextChanged += new System.EventHandler(this.quantityInput_TextChanged);
			// 
			// acceptButton
			// 
			this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.acceptButton.Location = new System.Drawing.Point(123, 41);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(75, 23);
			this.acceptButton.TabIndex = 3;
			this.acceptButton.Text = "button1";
			this.acceptButton.UseVisualStyleBackColor = true;
			this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
			// 
			// FormSelectQuantity
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.acceptButton;
			this.ClientSize = new System.Drawing.Size(211, 70);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.quantityInput);
			this.Controls.Add(this.quantitySlider);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FormSelectQuantity";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FormSelectQuantity";
			((System.ComponentModel.ISupportInitialize)(this.quantitySlider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
