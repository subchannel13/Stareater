using System;
using Stareater.Ships;

namespace Stareater.Controllers.Data
{
	public class DesignInfo
	{
		private Design design;
		
		internal DesignInfo(Design design)
		{
			this.design = design;
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
	}
}
