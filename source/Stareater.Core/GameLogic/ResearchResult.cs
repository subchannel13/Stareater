using Stareater.GameData;

namespace Stareater.GameLogic 
{
	class ResearchResult 
	{
		public long CompletedCount;
		public double InvestedPoints;
		public ResearchProgress Item;
		public double LeftoverPoints;

		public ResearchResult(long completedCount, double investedPoints, ResearchProgress item, double leftoverPoints) 
		{
			this.CompletedCount = completedCount;
			this.InvestedPoints = investedPoints;
			this.Item = item;
			this.LeftoverPoints = leftoverPoints;
		}
	}
}
