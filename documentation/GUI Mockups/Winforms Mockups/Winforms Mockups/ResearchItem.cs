
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Winforms_Mockups
{
	/// <summary>
	/// Description of ResearchItem.
	/// </summary>
	public partial class ResearchItem : UserControl
	{
		public ResearchItem()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void PictureBox1Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(new Pen(Color.Gray, 2), e.ClipRectangle);
		}
	}
}
