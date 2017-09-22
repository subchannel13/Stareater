using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.Players
{
	[StateType(true)]
	class Organization
	{
		public string LanguageCode { get; private set; }
		public IEnumerable<string> ResearchAffinities { get; private set; }

		internal Organization(string languageCode, IEnumerable<string> affinities)
		{
			this.LanguageCode = languageCode;
			this.ResearchAffinities = affinities;
		}
	}
}
