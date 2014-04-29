using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NGenerics.Extensions;
using Stareater.Controllers;
using Stareater.Controllers.Data;

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
			this.topics = controller.DevelopmentTopics().ToList();
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape) 
				this.Close();
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
