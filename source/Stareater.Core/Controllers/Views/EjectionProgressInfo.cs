namespace Stareater.Controllers.Views
{
	public class EjectionProgressInfo
	{
		public bool CanProgress { get; private set; }
		public double Eta { get; private set; }
		public double VictoryPoints { get; private set; }

		public EjectionProgressInfo(bool canProgress, double eta, double victoryPoints)
		{
			this.CanProgress = canProgress;
			this.Eta = eta;
			this.VictoryPoints = victoryPoints;
		}
	}
}
