using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Stareater.GUI
{
	public partial class FormError : Form
	{
		public FormError()
		{
			InitializeComponent();
		}
		
		public FormError(string errorMessage) : this()
		{
			this.errorText.Text = errorMessage;
			this.issuesLink.Links.Add(0, this.issuesLink.Text.Length, "http://github.com/subchannel13/zvjezdojedac/issues");
		}
		
		void IssuesLinkLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(e.Link.LinkData.ToString());
		}
		
		void CloseButtonClick(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
