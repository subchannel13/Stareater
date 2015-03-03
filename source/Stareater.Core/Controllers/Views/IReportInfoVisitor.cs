namespace Stareater.Controllers.Views
{
	public interface IReportInfoVisitor
	{
		void Visit(TechnologyReportInfo reportInfo);
	}
}
