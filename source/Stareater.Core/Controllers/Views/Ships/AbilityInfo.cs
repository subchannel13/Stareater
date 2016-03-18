using System;
using Stareater.GameLogic;

namespace Stareater.Controllers.Views.Ships
{
	public class AbilityInfo
	{
		internal Ability Data { get; private set; }
		internal int Index { get; private set; }
		public double Quantity { get; private set; }
		
		internal AbilityInfo(Ability data, int index, double quantity)
		{
			this.Data = data;
			this.Index = index;
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
