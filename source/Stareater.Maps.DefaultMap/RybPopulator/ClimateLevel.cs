namespace Stareater.Maps.DefaultMap.RybPopulator
{
	class ClimateLevel
	{
		public string LanguageCode { get; private set; }
		public WeightedRange[] Ranges { get; private set; }
		public double HomesystemStartScore { get; private set; }

		public ClimateLevel(string languageCode, WeightedRange[] ranges, double homesystemStartScore)
		{
			this.HomesystemStartScore = homesystemStartScore;
			this.LanguageCode = languageCode;
			this.Ranges = ranges;
		}
	}
}
