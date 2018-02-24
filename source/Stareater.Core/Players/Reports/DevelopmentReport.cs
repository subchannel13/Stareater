using Stareater.GameLogic.Planning;
using Stareater.Utils.StateEngine;

namespace Stareater.Players.Reports
{
	[StateType(saveTag: SaveTag)]
	class DevelopmentReport : IReport
	{
		[StateProperty]
		public DevelopmentResult TechProgress { get; private set; }
		
		internal DevelopmentReport(DevelopmentResult techProgress)
		{
			this.TechProgress = techProgress;
		}

		private DevelopmentReport()
		{ }
		
		public Player Owner {
			get {
				return this.TechProgress.Item.Owner;
			}
		}
		
		public void Accept(IReportVisitor visitor)
		{
			visitor.Visit(this);
		}
		
		public const string SaveTag = "DevelopmentReport";
	}
}
