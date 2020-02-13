using Stareater.GameData.Construction;
using Stareater.Utils.StateEngine;

namespace Stareater.GameLogic .Planning
{
	[StateTypeAttribute(true)]
	class ConstructionResult 
	{
		public long CompletedCount;
		public double InvestedPoints;
		public double FromStockpile;
		public IConstructionProject Project;
		public double LeftoverPoints;

		public ConstructionResult(long completedCount, double investedPoints, double fromStockpile, IConstructionProject project, double leftoverPoints) 
		{
			this.CompletedCount = completedCount;
			this.InvestedPoints = investedPoints;
			this.FromStockpile = fromStockpile;
			this.Project = project;
			this.LeftoverPoints = leftoverPoints;
		}
	}
}
