using System;
using Stareater.Players.Reports;

namespace Stareater.Controllers.Views
{
	class ReportWrapper : IReportVisitor
	{
		IReportInfo wrapped = null;
		
		public IReportInfo Wrap(IReport rawReport)
		{
			rawReport.Accept(this);
			
			return wrapped;
		}
		
		public void Visit(TechnologyReport report)
		{
			wrapped = new TechnologyReportInfo(report);
		}
	}
}
