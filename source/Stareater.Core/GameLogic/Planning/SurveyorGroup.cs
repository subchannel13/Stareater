namespace Stareater.GameLogic.Planning
{
	class SurveyorGroup
	{
		public int Rounds { get; private set; }
		public double Detection { get; private set; }
		public double Surveys { get; private set; }

		public SurveyorGroup(int rounds, double detection, double surveys)
		{
			this.Rounds = rounds;
			this.Detection = detection;
			this.Surveys = surveys;
		}
	}
}
