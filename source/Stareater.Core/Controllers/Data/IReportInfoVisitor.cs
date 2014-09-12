namespace Stareater.Controllers.Data
{
	public interface IReportInfoVisitor
	{
		void Visit(TechnologyReportInfo reportInfo);
	}
}
