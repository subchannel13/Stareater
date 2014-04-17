using System;
using Stareater.GameData;

namespace Stareater.GameLogic
{
	class DevelopmentResult
	{
		public int NewLevels;
		public double InvestedPoints;
		public TechnologyProgress Item;
		public double LeftoverPoints;

		public DevelopmentResult(int newLevels, double investedpoints, TechnologyProgress item, double leftoverPoints) {
			this.NewLevels = newLevels;
			this.InvestedPoints = investedpoints;
			this.Item = item;
			this.LeftoverPoints = leftoverPoints;
		}
	}
}
