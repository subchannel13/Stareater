namespace Stareater.Players.Reports
{
	interface IReportVisitor
	{
		void Visit(ContactReport report);
		void Visit(DevelopmentReport report);
		void Visit(ResearchReport report);
	}
}
