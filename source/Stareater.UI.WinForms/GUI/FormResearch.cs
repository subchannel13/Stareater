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
		private readonly ResearchTopicInfo[] topics;

		private ResearchTopicInfo selectedField;

		public FormResearch()
		{
			InitializeComponent();
		}

		public FormResearch(PlayerController controller) : this()
		{
			this.Font = SettingsWinforms.Get.FormFont;
			this.controller = controller;
			this.selectedField = controller.ResearchFocus;
			this.topics = controller.ResearchTopics();

			updateReserchList();
			this.topicList.SelectedIndex = Array.IndexOf(this.topics, controller.ResearchFocus);
			updateFieldDescription(this.topicList.SelectedItem);

			var context = LocalizationManifest.Get.CurrentLanguage["FormTech"];
			this.focusAction.Text = context["focusButton"].Text();
			this.priorityTitle.Text = context["priorityTitle"].Text();
			this.Text = context["ResearchTitle"].Text();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape)
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void updateReserchList()
		{
			topicList.SuspendLayout();

			while (topicList.Controls.Count < topics.Length)
			{
				var topicControl = new ResearchItem(this.controller);
				topicControl.Click += topicList_SelectedIndexChanged;
				topicList.Controls.Add(topicControl);
			}

			for (int i = 0; i < topics.Length; i++)
				{
					(topicList.Controls[i] as ResearchItem).SetData(topics[i], controller.ResearchFocus == topics[i]);
					topicList.Controls[i].Tag = topics[i];
				}

			topicList.ResumeLayout();
		}

		private void updateFieldDescription(Control topic)
		{
			var selection = topic as ResearchItem;
			this.selectedField = (ResearchTopicInfo)topic.Tag;

			techImage.Image = ImageCache.Get[selection.Data.ImagePath];
			fieldDescription.Text = selection.Data.Description;

			this.updateUnlocks();
		}

		private void updateUnlocks()
		{
			var field = this.selectedField;
			var unlocks = this.controller.ResearchUnlockPriorities(field).ToList();
			this.unlocksList.SuspendLayout();

			while (this.unlocksList.Controls.Count > unlocks.Count)
				this.unlocksList.Controls.RemoveAt(0);
			while (this.unlocksList.Controls.Count < unlocks.Count)
				this.unlocksList.Controls.Add(new DevelopmentItem());

			for (int i = 0; i < unlocks.Count; i++)
				(this.unlocksList.Controls[i] as DevelopmentItem).SetData(unlocks[i]);

			this.unlocksList.ResumeLayout();

			if (unlocks.Any())
				this.unlocksList.SelectedIndex = 0;
			else
				this.techDescription.Text = "";
		}

		private void topicList_SelectedIndexChanged(object sender, EventArgs e)
		{
			updateFieldDescription(sender as Control);
		}

		private void focusAction_Click(object sender, EventArgs e)
		{
			this.controller.ResearchFocus = this.selectedField;

			updateReserchList();
		}

		private void unlocksList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.unlocksList.SelectedItem == null)
				return;

			this.techDescription.Text = (this.unlocksList.SelectedItem as DevelopmentItem).Data.Description;
		}

		private void reorderUpAction_Click(object sender, EventArgs e)
		{
			changeSelectionPriority(-1);
		}

		private void reorderDownAction_Click(object sender, EventArgs e)
		{
			changeSelectionPriority(1);
		}

		private void changeSelectionPriority(int direction)
		{
			if (!this.unlocksList.HasSelection)
				return;

			var unlock = (this.unlocksList.SelectedItem as DevelopmentItem).Data;
			this.controller.ResearchReorderPriority(this.selectedField, unlock, this.unlocksList.SelectedIndex + direction);
			this.updateUnlocks();
			this.unlocksList.SelectedIndex = this.controller.ResearchUnlockPriorities(this.selectedField).ToList().IndexOf(unlock);
		}
	}
}
