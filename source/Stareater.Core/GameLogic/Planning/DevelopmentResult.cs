using Stareater.GameData;
using Stareater.Utils.StateEngine;

namespace Stareater.GameLogic.Planning
{
	class DevelopmentResult 
	{
		[StatePropertyAttribute]
		public long CompletedCount { get; private set; }
		[StatePropertyAttribute]
		public double InvestedPoints { get; private set; }
		[StatePropertyAttribute]
		public DevelopmentProgress Item { get; private set; }
		[StatePropertyAttribute]
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
