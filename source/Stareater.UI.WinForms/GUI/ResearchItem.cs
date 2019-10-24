using System;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Localization;
using Stareater.Utils.Collections;
using Stareater.Utils.NumberFormatters;

namespace Stareater.GUI
{
	public partial class ResearchItem : UserControl
	{
		public const string LanguageContext = "FormTech";
		private const string LocalizationLevel = "Level";

		private readonly PlayerController controller;

		public ResearchTopicInfo Data { get; private set; }

		public ResearchItem()
		{
			InitializeComponent();
		}

		public ResearchItem(PlayerController controller) : this()
		{
			this.controller = controller;
		}

		public void SetData(ResearchTopicInfo topicInfo)
		{
			this.Data = topicInfo;
			this.RefreshData();
		}

		public void RefreshData()
		{
			var thousandsFormat = new ThousandsFormatter(this.Data.Cost);
			var focused = this.controller.ResearchFocus == this.Data;

			this.thumbnailImage.Image = ImageCache.Get[this.Data.ImagePath];
			this.nameLabel.Text = this.Data.Name;
			this.levelLabel.Text = this.TopicLevelText;
			this.costLabel.Text = thousandsFormat.Format(this.Data.InvestedPoints) + " / " + thousandsFormat.Format(this.Data.Cost);
			this.focusImage.Image = focused ? Stareater.Properties.Resources.center : null;

			if (this.Data.Investment > 0)
				this.investmentLabel.Text = "+" + thousandsFormat.Format(this.Data.Investment);
			else
				this.investmentLabel.Text = "";

			this.unlocksLabel.Text = LocalizationManifest.Get.CurrentLanguage[LanguageContext]["unlockPriorities"].Text() +
					":" + Environment.NewLine +
					string.Join(Environment.NewLine, this.controller.ResearchUnlockPriorities(this.Data).Select((x, i) => (i + 1) + ") " + x.Name));
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

		private void unlocksLabel_Click(object sender, EventArgs e)
		{
			this.InvokeOnClick(this, e);
		}
		private void focusImage_Click(object sender, EventArgs e)
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

		private void unlocksLabel_Enter(object sender, EventArgs e)
		{
			this.OnMouseEnter(e);
		}

		public string TopicLevelText
		{
			get
			{
				return LocalizationManifest.Get.CurrentLanguage[LanguageContext][LocalizationLevel].Text(null, new TextVar("lvl", Data.NextLevel.ToString()).Get);
			}
		}
	}
}
