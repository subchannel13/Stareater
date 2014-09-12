using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Stareater.Controllers.Data;

namespace Stareater.GUI
{
	public partial class FormReports : Form
	{
		public FormReports()
		{
			InitializeComponent();
		}
		
		public FormReports(IEnumerable<IReportInfo> reports) : this()
		{
			foreach (var report in reports) {
				listBox1.Items.Add(report.Message);
			}
		}
	}
}
