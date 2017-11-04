namespace Stareater.GameData.Ships
{
	class PredefinedComponent
	{
		public string IdCode { get; private set; }
		public int Level { get; private set; }
		public int Amount { get; private set; }

		public PredefinedComponent(string idCode, int level, int amount)
		{
			this.Amount = amount;
			this.IdCode = idCode;
			this.Level = level;
		}
	}
}
