using System;
using Stareater.Galaxy;

namespace Stareater.Controllers.Data
{
	public class ShipGroupInfo
	{
		private ShipGroup shipGroup;
		
		internal ShipGroupInfo(ShipGroup shipGroup)
		{
			this.shipGroup = shipGroup;
		}
		
		public DesignInfo Design 
		{
			get 
			{
				return new DesignInfo(this.shipGroup.Design);
			}
		}
		
		public long Quantity 
		{
			get
			{
				return this.shipGroup.Quantity;
			}
		}
	}
}
