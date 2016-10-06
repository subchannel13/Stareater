using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Localization;
using Stareater.Utils;

namespace Stareater.GUI
{
	public sealed partial class FormLibrary : Form
	{
		private readonly LibraryController controller;
		private LibraryPage currentPage = null;
		
		public FormLibrary()
		{
			InitializeComponent();
		}
		
		public FormLibrary(LibraryController controller) : this()
		{
			this.controller = controller;
			this.Font = SettingsWinforms.Get.FormFont;
			
			var context = LocalizationManifest.Get.CurrentLanguage["FormLibrary"];
			this.Text = context["FormTitle"].Text();
			this.levelLabel.Text = context["level"].Text() + ":";
			
			researchLink.Text = context["research"].Text();
			developmentLink.Text = context["development"].Text();
			armorLink.Text = context["armor"].Text();
			hullLink.Text = context["hull"].Text();
			isDriveLink.Text = context["isDrive"].Text();
			missionEquipLink.Text = context["missionEquip"].Text();
			reactorLink.Text = context["reactor"].Text();
			sensorLink.Text = context["sensor"].Text();
			specialEquipLink.Text = context["specialEquip"].Text();
			thrusterLink.Text = context["thruster"].Text();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void makeSubtopics(IEnumerable<LibraryPage> subtopics)
		{
			var separatorIndex = topicList.Controls.GetChildIndex(topicSeparator);
			while(topicList.Controls.Count > separatorIndex + 1)
				topicList.Controls.RemoveAt(topicList.Controls.Count - 1);
			
			foreach(var item in subtopics)
			{
				var link = new LinkLabel();
				link.AutoSize = true;
				link.TabStop = true;
				link.Margin = this.developmentLink.Margin;
				link.Text = item.Title(0);
				link.LinkClicked += this.openPage;
				link.Links.Add(0, link.Text.Length, item);
				
				topicList.Controls.Add(link);
			}
		}

		private void openPage(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.currentPage = e.Link.LinkData as LibraryPage;
			var level = Methods.Clamp((int)levelInput.Value, 0, this.currentPage.MaxLevel);
			
			topicName.Text = this.currentPage.Title(level);
			maxLevelInfo.Text = "/ " + this.currentPage.MaxLevel;
			topicText.Text = this.currentPage.Text(level);
			topicThumbnail.Image = ImageCache.Get[this.currentPage.ImagePath];
			levelInput.Maximum = this.currentPage.MaxLevel;
			
			topicName.Visible = true;
			levelLabel.Visible = true;
			levelInput.Visible = true;
			maxLevelInfo.Visible = true;
			topicText.Visible = true;
		}
		
		private void levelInput_ValueChanged(object sender, EventArgs e)
		{
			var level = (int)levelInput.Value;
			topicName.Text = this.currentPage.Title(level);
			topicText.Text = this.currentPage.Text(level);
		}
		
		private void researchLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.ResearchTopics.Select(x => new LibraryPage(x.Name, x.Description, x.ImagePath, x.MaxLevel)));
		}
		
		private void developmentLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.DevelpmentTopics.Select(x => new LibraryPage(x.Name, x.Description, x.ImagePath, x.MaxLevel)));
		}
		
		private void armorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.Armors.Select(x => new LibraryPage(x.Name, x.Description, x.ImagePath, x.MaxLevel)));
		}
		
		private void hullLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.Hulls.Select(x => new LibraryPage(x.Name, x.Description, x.ImagePath, x.MaxLevel)));
		}
		
		private void isDriveLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.IsDrives.Select(x => new LibraryPage(x.Name, x.Description, x.ImagePath, x.MaxLevel)));
		}
		
		private void missionEquipLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.MissionEquipment.Select(x => new LibraryPage(x.Name, x.Description, x.ImagePath, x.MaxLevel)));
		}
		
		private void reactorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.Reactors.Select(x => new LibraryPage(x.Name, x.Description, x.ImagePath, x.MaxLevel)));
		}
		
		private void sensorLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.Sensors.Select(x => new LibraryPage(x.Name, x.Description, x.ImagePath, x.MaxLevel)));
		}
		
		private void specialEquipLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.SpecialEquipment.Select(x => new LibraryPage(x.Name, x.Description, x.ImagePath, x.MaxLevel)));
		}
		
		private void thrusterLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.Thrusters.Select(x => new LibraryPage(x.Name, x.Description, x.ImagePath, x.MaxLevel)));
		}
	}
}
