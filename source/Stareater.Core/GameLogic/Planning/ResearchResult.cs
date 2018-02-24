using Stareater.GameData;
using Stareater.Utils.StateEngine;

namespace Stareater.GameLogic.Planning
{
	class ResearchResult 
	{
		[StateProperty]
		public long CompletedCount { get; private set; }
		[StateProperty]
		public double InvestedPoints { get; private set; }
		[StateProperty]
		public ResearchProgress Item { get; private set; }
		[StateProperty]
		public double LeftoverPoints { get; private set; }

		public ResearchResult(long completedCount, double investedPoints, ResearchProgress item, double leftoverPoints) 
		{
			this.CompletedCount = completedCount;
			this.InvestedPoints = investedPoints;
			this.Item = item;
			this.LeftoverPoints = leftoverPoints;
		}

		private ResearchResult()
		{ }
	}
}
