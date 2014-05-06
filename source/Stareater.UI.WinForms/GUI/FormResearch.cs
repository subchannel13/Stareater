using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NGenerics.Extensions;
using Stareater.Controllers;
using Stareater.Controllers.Data;
using Stareater.Localization;
using Stareater.AppData;

namespace Stareater.GUI
{
	public partial class FormResearch : Form
	{
		private GameController controller;
		private IList<TechnologyTopic> topics;
		
		public FormResearch()
		{
			InitializeComponent();
		}
		
		public FormResearch(GameController controller) : this()
		{
			this.controller = controller;
			this.topics = controller.ResearchTopics().ToList();
			
			topicList.SuspendLayout();
			
			while (topicList.Controls.Count < topics.Count)
				topicList.Controls.Add(new TechnologyItem());
			while (topicList.Controls.Count > topics.Count)
				topicList.Controls.RemoveAt(topicList.Controls.Count - 1);

			for (int i = 0; i < topics.Count; i++)
				(topicList.Controls[i] as TechnologyItem).SetData(topics[i]);
			
			topicList.ResumeLayout();
			
			Context context = SettingsWinforms.Get.Language["FormTech"];
			this.Text = context["ResearchTitle"].Text();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
