using System;
using Stareater.GameLogic;
using Stareater.Ships;

namespace Stareater.Controllers.Views.Ships
{
	public class DesignInfo
	{
		private readonly Design design;
		private readonly DesignStats stats;
		
		internal DesignInfo(Design design, DesignStats stats)
		{
			this.design = design;
			this.stats = stats;
		}
		
		public string Name 
		{
			get 
			{
				return design.Name;
			}
		}
		
		public string ImagePath 
		{
			get
			{
				return design.ImagePath;
			}
		}
		
		public double ColonizerPopulation
		{
			get 
			{
				return stats.ColonizerPopulation;
			}
		}
	}
}
