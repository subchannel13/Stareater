using Stareater.AppData;
using Stareater.Controllers.Views;
using System.Drawing;

namespace Stareater.GUI.Reports
{
	class ReportThumbnailVisitor : IReportInfoVisitor
	{
		public Image Result { get; private set; }
		
		public ReportThumbnailVisitor(IReportInfo report)
		{
			this.Result = null;
			
			report.Accept(this);
		}

		public void Visit(ContactReportInfo reportInfo)
		{
			this.Result = ImageCache.Get["./images/other/diplomacy"];
		}

		public void Visit(DevelopmentReportInfo reportInfo)
		{
			this.Result = ImageCache.Get[reportInfo.ImagePath];
		}

		public void Visit(ResearchReportInfo reportInfo)
		{
			this.Result = ImageCache.Get[reportInfo.ImagePath];
		}
	}
}
