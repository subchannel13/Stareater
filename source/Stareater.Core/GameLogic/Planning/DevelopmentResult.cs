using Stareater.GameData;
using Stareater.Utils.StateEngine;

namespace Stareater.GameLogic.Planning
{
	class DevelopmentResult 
	{
		[StateProperty]
		public long CompletedCount { get; private set; }
		[StateProperty]
		public double InvestedPoints { get; private set; }
		[StateProperty]
		public DevelopmentProgress Item { get; private set; }
		[StateProperty]
		public double LeftoverPoints { get; private set; }

		public DevelopmentResult(long completedCount, double investedPoints, DevelopmentProgress item, double leftoverPoints) 
		{
			this.CompletedCount = completedCount;
			this.InvestedPoints = investedPoints;
			this.Item = item;
			this.LeftoverPoints = leftoverPoints;
		}

		private DevelopmentResult()
		{ }
	}
}
