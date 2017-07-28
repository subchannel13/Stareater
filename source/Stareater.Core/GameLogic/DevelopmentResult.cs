﻿using Stareater.GameData;
using Stareater.Utils.StateEngine;

namespace Stareater.GameLogic 
{
	[StateType(true)]
	class DevelopmentResult 
	{
		public long CompletedCount;
		public double InvestedPoints;
		public DevelopmentProgress Item;
		public double LeftoverPoints;

		public DevelopmentResult(long completedCount, double investedPoints, DevelopmentProgress item, double leftoverPoints) 
		{
			this.CompletedCount = completedCount;
			this.InvestedPoints = investedPoints;
			this.Item = item;
			this.LeftoverPoints = leftoverPoints;
		}
	}
}
