using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers.Data;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class TechnologyItem : UserControl
	{
		private const string LanguageContext = "FormTech";
		
		public TechnologyItem()
		{
			InitializeComponent();
		}
		
		public void SetData(TechnologyTopic topicInfo)
		{
			Context lang = Settings.Get.Language[LanguageContext];
			
			ThousandsFormatter thousandsFormat = new ThousandsFormatter(topicInfo.Cost);
			
			thumbnailImage.Image = ImageCache.Get[topicInfo.ImagePath];
			nameLabel.Text = topicInfo.Name;
			levelLabel.Text = lang["Level"].Text(new Var("lvl", topicInfo.NextLevel).Get);
			costLabel.Text = thousandsFormat.Format(topicInfo.InvestedPoints) + " / " +thousandsFormat.Format(topicInfo.Cost);
			investmentLabel.Text = thousandsFormat.Format(topicInfo.Investment);
		}
		
		void thumbnailImage_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(new Pen(Color.Gray, 2), e.ClipRectangle);
		}
		
		void thumbnailImage_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void nameLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void levelLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void costLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		
		void investmentLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
	}
}
