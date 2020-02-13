using Stareater.GameData;
using Stareater.Utils.StateEngine;

namespace Stareater.GameLogic.Planning
{
	class ResearchResult 
	{
		[StatePropertyAttribute]
		public long CompletedCount { get; private set; }
		[StatePropertyAttribute]
		public double InvestedPoints { get; private set; }
		[StatePropertyAttribute]
		public ResearchProgress Item { get; private set; }
		[StatePropertyAttribute]
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
