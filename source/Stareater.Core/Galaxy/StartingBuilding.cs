namespace Stareater.Galaxy
{
	public class StartingBuilding
	{
		public string Id { get; private set; }
		public long Amount { get; private set; }

		public StartingBuilding(string id, long amount)
		{
			this.Id = id;
			this.Amount = amount;
		}
	}
}
