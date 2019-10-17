using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stareater.Controllers;
using Stareater.Controllers.Views;
using Stareater.Localization;
using Stareater.AppData;

namespace Stareater.GUI
{
	public sealed partial class FormResearch : Form
	{
		private readonly PlayerController controller;
		private IList<ResearchTopicInfo> topics;

		private Control lastTopic = null;

		public FormResearch()
		{
			InitializeComponent();
		}

		public FormResearch(PlayerController controller) : this()
		{
			this.Font = SettingsWinforms.Get.FormFont;
			this.controller = controller;

			updateList();

			updateDescription(focusedItem);

			Context context = LocalizationManifest.Get.CurrentLanguage["FormTech"];
			this.Text = context["ResearchTitle"].Text();
			this.focusedLabel.Text = context["focusedResearchTitle"].Text();
			this.listTitle.Text = context["otherResearchHeader"].Text();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape)
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void updateList()
		{
			this.topics = controller.ResearchTopics().ToList();
			topicList.SuspendLayout();

			while (topicList.Controls.Count < topics.Count - 1)
			{
				var topicControl = new ResearchItem();
				topicControl.MouseEnter += topic_OnMouseEnter;
				topicControl.Click += topicList_SelectedIndexChanged;
				topicList.Controls.Add(topicControl);
			}

			for (int i = 0; i < topics.Count; i++)
				if (controller.ResearchFocus != i)
				{
					int controlIndex = i + (controller.ResearchFocus < i ? -1 : 0);
					(topicList.Controls[controlIndex] as ResearchItem).SetData(topics[i], true);
					topicList.Controls[controlIndex].Tag = i;
				}

			focusedItem.SetData(topics[controller.ResearchFocus], false);
			topicList.ResumeLayout();
		}

		private void updateDescription(Control topic)
		{
			if (topic == null)
			{
				techImage.Image = null;
				techName.Text = "";
				techDescription.Text = "";
				techLevel.Text = "";
			}
			else if (lastTopic != topic)
			{
				var selection = topic as ResearchItem;
				lastTopic = topic;

				techImage.Image = ImageCache.Get[selection.Data.ImagePath];
				techName.Text = selection.Data.Name;
				techLevel.Text = selection.TopicLevelText;
				techDescription.Text = 
					selection.Data.Description + 
					Environment.NewLine +
					Environment.NewLine +
					LocalizationManifest.Get.CurrentLanguage["FormTech"]["researchUnlock"].Text() +
					Environment.NewLine +
					string.Join(Environment.NewLine, selection.Data.Unlocks.Select(x => x.Name));
			}
		}

		private void topic_OnMouseEnter(object sender, EventArgs e)
		{
			updateDescription(sender as Control);
		}

		private void topicList_MouseLeave(object sender, EventArgs e)
		{
			updateDescription(this.focusedItem);
		}

		private void topicList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.controller.ResearchFocus = (int)(sender as ResearchItem).Tag;

			updateList();
		}
	}
}
