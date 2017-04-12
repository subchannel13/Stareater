using System;
using System.Drawing;
using System.Windows.Forms;

namespace Stareater.GUI
{
	public partial class TreatyBriefView : UserControl
	{
		public TreatyBriefView()
		{
			InitializeComponent();
		}
		
		public void SetData(Image thumbnail, string text)
		{
			this.thumbnailImage.Image = thumbnail;
			this.nameText.Text = text;
		}
	}
}
