namespace Stareater.Maps.DefaultMap.RybPopulator
{
	class PotentialLevel
	{
		public string LanguageCode { get; private set; }
		public WeightedRange[] Ranges { get; private set; }
		public double HomesystemPotentialScore { get; private set; }

		public PotentialLevel(string languageCode, WeightedRange[] ranges, double homesystemPotentialScore)
		{
			this.HomesystemPotentialScore = homesystemPotentialScore;
			this.LanguageCode = languageCode;
			this.Ranges = ranges;
		}
	}
}
