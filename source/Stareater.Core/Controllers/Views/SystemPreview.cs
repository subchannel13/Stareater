using Stareater.Galaxy;
using Stareater.Utils;

namespace Stareater.Controllers.Views
{
	public class SystemPreview
	{
		public Vector2D Position { get; private set; }
		public bool IsHomeSystem { get; private set; }

		public double StartingScore { get; private set; }
		public double PotentialScore { get; private set; }

		internal SystemPreview(StarData star, bool isHomeSystem, double startingScore, double potentialScore)
		{
			this.IsHomeSystem = isHomeSystem;
			this.Position = star.Position;
			this.PotentialScore = potentialScore;
			this.StartingScore = startingScore;
		}
	}
}
