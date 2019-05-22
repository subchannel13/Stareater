using Stareater.Galaxy;
using Stareater.GameData.Databases;
using Stareater.GameLogic.Combat;
using System;

namespace Stareater.Controllers.Views.Ships
{
	public class ShipGroupInfo
	{
		internal ShipGroup Data { get; private set; }
		private readonly DesignStats stats;
		private readonly StaticsDB statics;
		
		internal ShipGroupInfo(ShipGroup shipGroup, DesignStats stats, StaticsDB statics)
		{
			this.Data = shipGroup;
			this.stats = stats;
			this.statics = statics;
		}
		
		public DesignInfo Design 
		{
			get 
			{
				return new DesignInfo(this.Data.Design, this.stats, this.statics);
			}
		}
		
		public long Quantity 
		{
			get
			{
				return this.Data.Quantity;
			}
		}

		public double PopulationCapacity
		{
			get { return this.stats.ColonizerPopulation * this.Data.Quantity; }
		}

		public double Population
		{
			get { return this.Data.PopulationTransport; }
		}

		public long FullTransporters
		{
			get
			{
				return this.stats.ColonizerPopulation > 0 ? 
					(long)Math.Floor(this.Data.PopulationTransport / this.stats.ColonizerPopulation) :
					0L;
			}
		}
	}
}
