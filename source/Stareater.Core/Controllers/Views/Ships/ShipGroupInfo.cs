using System;
using Stareater.Galaxy;
using Stareater.GameLogic;

namespace Stareater.Controllers.Views.Ships
{
	public class ShipGroupInfo
	{
		internal ShipGroup Data { get; private set; }
		private readonly DesignStats stats;
		
		internal ShipGroupInfo(ShipGroup shipGroup, DesignStats stats)
		{
			this.Data = shipGroup;
			this.stats = stats;
		}
		
		public DesignInfo Design 
		{
			get 
			{
				return new DesignInfo(this.Data.Design, stats);
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
