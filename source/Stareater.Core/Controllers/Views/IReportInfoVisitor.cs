namespace Stareater.Controllers.Views
{
	public interface IReportInfoVisitor
	{
		void Visit(ContactReportInfo reportInfo);
		void Visit(DevelopmentReportInfo reportInfo);
		void Visit(ResearchReportInfo reportInfo);
	}
}
