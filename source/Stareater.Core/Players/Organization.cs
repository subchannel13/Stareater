using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.Players
{
	[StateType(true)]
	class Organization : IIdentifiable
	{
		public string IdCode { get; internal set; }
		public string LanguageCode { get; private set; }
		public IEnumerable<string> ResearchAffinities { get; private set; }

		public Organization(string id, string languageCode, IEnumerable<string> affinities)
		{
			this.IdCode = id;
			this.LanguageCode = languageCode;
			this.ResearchAffinities = affinities;
		}
	}
}
