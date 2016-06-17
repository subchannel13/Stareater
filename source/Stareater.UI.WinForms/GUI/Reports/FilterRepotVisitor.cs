using System;
using Stareater.AppData;
using Stareater.Controllers.Views;

namespace Stareater.GUI.Reports
{
	class FilterRepotVisitor : IReportInfoVisitor
	{
		public bool ShowItem { get; private set; }

		#region IReportInfoVisitor implementation

		public void Visit(TechnologyReportInfo reportInfo)
		{
			this.ShowItem = SettingsWinforms.Get.ReportTechnology;
		}

		#endregion
	}
}
