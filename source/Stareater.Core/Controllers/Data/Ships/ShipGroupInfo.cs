using System;
using Stareater.Galaxy;

namespace Stareater.Controllers.Data.Ships
{
	public class ShipGroupInfo
	{
		internal ShipGroup Data { get; private set; }
		
		internal ShipGroupInfo(ShipGroup shipGroup)
		{
			this.Data = shipGroup;
		}
		
		public DesignInfo Design 
		{
			get 
			{
				return new DesignInfo(this.Data.Design);
			}
		}
		
		public long Quantity 
		{
			get
			{
				return this.Data.Quantity;
			}
		}
	}
}
