using Stareater.AppData.Expressions;
using System.Collections.Generic;

namespace Stareater.GameData.Databases.Tables
{
	class PlanetForumlaSet
	{
		public IEnumerable<string> ImplicitTraits { get; private set; }

		public Formula StartingScore { get; private set; }
		public Formula PotentialScore { get; private set; }

		public PlanetForumlaSet(IEnumerable<string> impliciteTraits, Formula startingScore, Formula potentialScore)
		{
			this.ImplicitTraits = impliciteTraits;
			this.StartingScore = startingScore;
			this.PotentialScore = potentialScore;
		}
	}
}
