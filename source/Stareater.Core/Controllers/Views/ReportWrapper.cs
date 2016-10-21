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
		
		public void Visit(DevelopmentReport report)
		{
			wrapped = new DevelopmentReportInfo(report);
		}
		
		public void Visit(ResearchReport report)
		{
			wrapped = new ResearchReportInfo(report);
		}
	}
}
