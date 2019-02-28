namespace Stareater.Maps.DefaultMap.RybPopulator
{
	class PotentialLevel
	{
		public string LanguageCode { get; private set; }
		public WeightedRange[] Ranges { get; private set; }

		public PotentialLevel(string languageCode, WeightedRange[] ranges)
		{
			this.LanguageCode = languageCode;
			this.Ranges = ranges;
		}
	}
}
