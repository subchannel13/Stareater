using Stareater.GameData;

namespace Stareater.GameLogic
{
	class ConstructionResult
	{
		public long DoneCount;
		public double InvestedPoints;
		public Constructable Item;
		public double PartialPoints;

		public ConstructionResult(long donecount, double investedpoints, Constructable item, double partialpoints) {
			this.DoneCount = donecount;
			this.InvestedPoints = investedpoints;
			this.Item = item;
			this.PartialPoints = partialpoints;
		}
	}
}
