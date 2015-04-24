using Stareater.GameData;

namespace Stareater.GameLogic 
{
	class ConstructionResult 
	{
		public long CompletedCount;
		public double InvestedPoints;
		public double FromStockpile;
		public Constructable Type;
		public double LeftoverPoints;

		public ConstructionResult(long completedCount, double investedPoints, double fromStockpile, Constructable type, double leftoverPoints) 
		{
			this.CompletedCount = completedCount;
			this.InvestedPoints = investedPoints;
			this.FromStockpile = fromStockpile;
			this.Type = type;
			this.LeftoverPoints = leftoverPoints;
		}
	}
}
