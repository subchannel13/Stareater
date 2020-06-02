using Stareater.AppData.Expressions;
using System;
using System.Collections.Generic;

namespace Stareater.GameData.Databases.Tables
{
	class PlanetForumlaSet
	{
		public IEnumerable<string> ImplicitTraits { get; private set; }
		public IEnumerable<string> BestTraits { get; private set; }
		public IEnumerable<string> WorstTraits { get; private set; }
		public HashSet<string> UnchangeableTraits { get; private set; }
		public Formula DiscoveryDifficulty { get; internal set; }
		public Formula SurveyDifficulty { get; internal set; }

		public Formula StartingScore { get; private set; }
		public Formula PotentialScore { get; private set; }

		public PlanetForumlaSet(IEnumerable<string> impliciteTraits, IEnumerable<string> bestTraits, IEnumerable<string> worstTraits,
			Formula discoveryDifficulty, Formula surveyDifficulty,
			IEnumerable<string> unchangeableTraits, Formula startingScore, Formula potentialScore)
		{
			this.BestTraits = bestTraits;
			this.UnchangeableTraits = new HashSet<string>(unchangeableTraits);
			this.ImplicitTraits = impliciteTraits;
			this.DiscoveryDifficulty = discoveryDifficulty;
			this.SurveyDifficulty = surveyDifficulty;
			this.StartingScore = startingScore;
			this.PotentialScore = potentialScore;
			this.WorstTraits = worstTraits;
		}
	}
}
