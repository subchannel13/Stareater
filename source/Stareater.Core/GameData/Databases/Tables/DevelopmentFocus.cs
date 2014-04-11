using System;

namespace Stareater.GameData.Databases.Tables
{
	class DevelopmentFocus
	{
		public double[] Weights { get; private set; }
		
		public DevelopmentFocus(double[] weights)
		{
			this.Weights = weights;
		}
	}
}
