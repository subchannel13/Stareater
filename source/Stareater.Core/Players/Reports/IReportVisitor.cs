namespace Stareater.Players.Reports
{
	interface IReportVisitor
	{
		void Visit(TechnologyReport report);
	}
}
