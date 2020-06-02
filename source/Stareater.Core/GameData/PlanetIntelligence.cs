using Stareater.Utils.StateEngine;

namespace Stareater.GameData
{
	class PlanetIntelligence 
	{
		[StatePropertyAttribute]
		public bool Discovered { get; set; }
		[StatePropertyAttribute]
		public double SurveyLevel { get; set; }
	}
}
