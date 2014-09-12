namespace Stareater.Controllers.Data
{
	public interface IReportInfo
	{
		string Message { get; }
		
		void Accept(IReportInfoVisitor visitor);
	}
}
