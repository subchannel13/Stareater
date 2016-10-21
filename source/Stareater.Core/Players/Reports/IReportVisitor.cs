namespace Stareater.Players.Reports
{
	interface IReportVisitor
	{
		void Visit(DevelopmentReport report);
		void Visit(ResearchReport report);
	}
}
