using Stareater.Utils.StateEngine;

namespace Stareater.Players.Reports
{
	[StateBaseTypeAttribute("Load", typeof(ReportFactory))]
	interface IReport
	{
		Player Owner { get; }
		
		void Accept(IReportVisitor visitor);
	}
}
