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
		public const string LanguageContext = "FormTech";
		
		private const string LocalizationLevel = "Level";
		
		public TechnologyTopic Data { get; private set; }
		
		public TechnologyItem()
		{
			InitializeComponent();
		}
		
		public void SetData(TechnologyTopic topicInfo)
		{
			this.Data = topicInfo;
			
			ThousandsFormatter thousandsFormat = new ThousandsFormatter(topicInfo.Cost);
			
			thumbnailImage.Image = ImageCache.Get[topicInfo.ImagePath];
			nameLabel.Text = topicInfo.Name;
			levelLabel.Text = TopicLevelText;
			costLabel.Text = thousandsFormat.Format(topicInfo.InvestedPoints) + " / " +thousandsFormat.Format(topicInfo.Cost);
			
			if (topicInfo.Investment > 0)
				investmentLabel.Text = "+" + thousandsFormat.Format(topicInfo.Investment);
			else
				investmentLabel.Text = "";
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
		
		public string TopicLevelText
		{
			get
			{
				return Settings.Get.Language[LanguageContext][LocalizationLevel].Text(null, new TextVar("lvl", Data.NextLevel.ToString()).Get);
			}
		}
	}
}
