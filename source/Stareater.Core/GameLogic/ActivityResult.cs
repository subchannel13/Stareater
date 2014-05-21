using Stareater.GameData;

namespace Stareater.GameLogic 
{
	class ActivityResult<ItemType> 
	{
		public long CompletedCount;
		public double InvestedPoints;
		public ItemType Item;
		public double LeftoverPoints;

		public ActivityResult(long completedCount, double investedPoints, ItemType item, double leftoverPoints) 
		{
			this.CompletedCount = completedCount;
			this.InvestedPoints = investedPoints;
			this.Item = item;
			this.LeftoverPoints = leftoverPoints;
		}
	}
}
