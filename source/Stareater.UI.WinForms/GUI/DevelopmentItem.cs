using System;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers.Views;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class DevelopmentItem : UserControl
	{
		public const string LanguageContext = "FormTech";
		
		private const string LocalizationLevel = "Level";
		
		public DevelopmentTopicInfo Data { get; private set; }
		
		public DevelopmentItem()
		{
			InitializeComponent();
		}
		
		public void SetData(DevelopmentTopicInfo topicInfo)
		{
			this.Data = topicInfo;
			
			ThousandsFormatter thousandsFormat = new ThousandsFormatter(topicInfo.Cost);
			
			thumbnailImage.Image = ImageCache.Get[topicInfo.ImagePath];
			nameLabel.Text = topicInfo.Name;
			levelLabel.Text = this.TopicLevelText;
			costLabel.Text = thousandsFormat.Format(topicInfo.InvestedPoints) + " / " + thousandsFormat.Format(topicInfo.Cost);
			
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
		
		void thumbnailImage_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}
		
		void nameLabel_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}
		
		void levelLabel_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}
		
		void costLabel_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}
		
		void investmentLabel_MouseEnter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}
		
		public string TopicLevelText
		{
			get
			{
				var context = LocalizationManifest.Get.CurrentLanguage[LanguageContext];
				return Data.NextLevel == 0 ?
					context["Level0"].Text() :
					context[LocalizationLevel].Text(null, new TextVar("lvl", Data.NextLevel.ToString()).Get);
			}
		}
	}
}
