using Stareater.AppData;
using Stareater.Controllers.Views;
using System;

namespace Stareater.GUI.Reports
{
	class FilterRepotVisitor : IReportInfoVisitor
	{
		private bool show;

		public bool ShowItem(IReportInfo report)
		{
			report.Accept(this);

			return this.show;
		}

		#region IReportInfoVisitor implementation

		public void Visit(ContactReportInfo reportInfo)
		{
			this.show = SettingsWinforms.Get.ReportContact;
		}

		public void Visit(DevelopmentReportInfo reportInfo)
		{
			this.show = SettingsWinforms.Get.ReportTechnology;
		}

		public void Visit(ResearchReportInfo reportInfo)
		{
			this.show = SettingsWinforms.Get.ReportTechnology;
		}
		#endregion
	}
}
