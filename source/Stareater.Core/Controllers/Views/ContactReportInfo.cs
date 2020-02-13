using Stareater.Localization;
using Stareater.Players.Reports;
using Stareater.Utils.Collections;
using System;

namespace Stareater.Controllers.Views
{
	public class ContactReportInfo : IReportInfo
	{
		private readonly ContactReport report;

		internal ContactReportInfo(ContactReport report)
		{
			this.report = report;
		}

		public string Message => 
			LocalizationManifest.Get.CurrentLanguage[GameController.ReportContext]["contact"].
			Text(null, new TextVar("contact", report.Contact.Name).Get);

		public void Accept(IReportInfoVisitor visitor)
		{
			if (visitor == null)
				throw new ArgumentNullException(nameof(visitor));

			visitor.Visit(this);
		}
	}
}
