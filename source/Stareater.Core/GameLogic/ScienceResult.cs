using Stareater.GameData;

namespace Stareater.GameLogic 
{
	class ScienceResult 
	{
		public long CompletedCount;
		public double InvestedPoints;
		public TechnologyProgress Item;
		public double LeftoverPoints;

		public ScienceResult(long completedCount, double investedPoints, TechnologyProgress item, double leftoverPoints) 
		{
			this.CompletedCount = completedCount;
			this.InvestedPoints = investedPoints;
			this.Item = item;
			this.LeftoverPoints = leftoverPoints;
		}
	}
}
