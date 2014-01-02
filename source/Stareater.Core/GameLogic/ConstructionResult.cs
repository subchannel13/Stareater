using Stareater.GameData;

namespace Stareater.GameLogic
{
	class ConstructionResult
	{
		public double DoneCount;
		public double InvestedPoints;
		public Constructable Item;
		public double PartialPoints;

		public ConstructionResult(double donecount, double investedpoints, Constructable item, double partialpoints) {
			this.DoneCount = donecount;
			this.InvestedPoints = investedpoints;
			this.Item = item;
			this.PartialPoints = partialpoints;
		}
	}
}
