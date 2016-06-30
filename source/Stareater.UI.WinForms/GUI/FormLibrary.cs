using System;
using System.Windows.Forms;
using Stareater.AppData;
using Stareater.Controllers;

namespace Stareater.GUI
{
	public sealed partial class FormLibrary : Form
	{
		private readonly LibraryController controller;
		
		public FormLibrary()
		{
			InitializeComponent();
		}
		
		public FormLibrary(LibraryController controller) : this()
		{
			this.controller = controller;
			
			var context = SettingsWinforms.Get.Language["FormLibrary"];
			this.Text = context["FormTitle"].Text();
			
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
		
		private void researchLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			;
		}
	}
}
