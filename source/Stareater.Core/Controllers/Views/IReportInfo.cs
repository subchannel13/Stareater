namespace Stareater.Controllers.Views
{
	public interface IReportInfo
	{
		string Message { get; }
		
		void Accept(IReportInfoVisitor visitor);
	}
}
