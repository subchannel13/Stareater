using System;
using Stareater.GameLogic;

namespace Stareater.Controllers.Views.Ships
{
	public class AbilityInfo
	{
		internal Ability Data { get; private set; }
		public double Quantity { get; private set; }
		
		internal AbilityInfo(Ability data, double quantity)
		{
			this.Data = data;
			this.Quantity = quantity;
		}
		
		public string ImagePath 
		{
			get
			{
				return this.Data.Type.ImagePath;
			}
		}
	}
}
