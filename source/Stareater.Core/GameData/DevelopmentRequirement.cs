namespace Stareater.GameData
{
	class DevelopmentRequirement
	{
		public string Code { get; private set; }
		public int Level { get; private set; }
		
		public DevelopmentRequirement(string code, int level)
		{
			this.Code = code;
			this.Level = level;
		}
	}
}
