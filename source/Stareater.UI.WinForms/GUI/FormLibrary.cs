using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;
using Stareater.Controllers.Views.Library;
using Stareater.Utils;

namespace Stareater.GUI
{
	public sealed partial class FormLibrary : Form
	{
		private readonly LibraryController controller;
		private TechnologyGeneralInfo currentPage = null;
		
		public FormLibrary()
		{
			InitializeComponent();
		}
		
		public FormLibrary(LibraryController controller) : this()
		{
			this.controller = controller;
			
			var context = SettingsWinforms.Get.Language["FormLibrary"];
			this.Text = context["FormTitle"].Text();
			this.levelLabel.Text = context["level"].Text() + ":";
			
			makeLink(researchLink, context["research"].Text());
			makeLink(developmentLink, context["development"].Text());
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void makeLink(LinkLabel linkLabel, string text)
		{
			linkLabel.Text = text;
			linkLabel.Links.Add(0, text.Length);
		}

		void makeSubtopics(IEnumerable<TechnologyGeneralInfo> subtopics)
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
				link.Text = item.Name(0);
				link.LinkClicked += this.openPage;
				link.Links.Add(0, link.Text.Length, item);
				
				topicList.Controls.Add(link);
			}
		}

		private void openPage(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.currentPage = e.Link.LinkData as TechnologyGeneralInfo;
			var level = Methods.Clamp((int)levelInput.Value, 0, this.currentPage.MaxLevel);
			
			topicName.Text = this.currentPage.Name(level);
			maxLevelInfo.Text = "/ " + this.currentPage.MaxLevel;
			topicText.Text = this.currentPage.Description(level);
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
			topicName.Text = this.currentPage.Name(level);
			topicText.Text = this.currentPage.Description(level);
		}
		
		private void researchLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.ResearchTopics);
		}
		
		private void developmentLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			makeSubtopics(controller.DevelpmentTopics);
		}
		
	}
}
