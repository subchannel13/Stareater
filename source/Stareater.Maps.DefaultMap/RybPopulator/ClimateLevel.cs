namespace Stareater.Maps.DefaultMap.RybPopulator
{
	class ClimateLevel
	{
		public string LanguageCode { get; private set; }
		public WeightedRange[] Ranges { get; private set; }

		public ClimateLevel(string languageCode, WeightedRange[] ranges)
		{
			this.LanguageCode = languageCode;
			this.Ranges = ranges;
		}
	}
}
