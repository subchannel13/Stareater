using System.Collections.Generic;
using Stareater.Ships;
using Stareater.GameLogic.Combat;

namespace Stareater.Controllers.Views.Ships
{
	public class DesignInfo
	{
		private readonly DesignStats stats;

		internal Design Data { get; private set; }
		
		internal DesignInfo(Design design, DesignStats stats)
		{
			this.Data = design;
			this.stats = stats;
		}
		
		public string Name 
		{
			get 
			{
				return this.Data.Name;
			}
		}
		
		public string ImagePath 
		{
			get
			{
				return this.Data.ImagePath;
			}
		}
		
		public bool Constructable 
		{
			get
			{
				return !this.Data.IsObsolete;
			}
		}
		
		public HullInfo Hull
		{
			get { return new HullInfo(this.Data.Hull); }
		}

		public IsDriveInfo IsDrive
		{
			get 
			{
				return this.Data.IsDrive != null ? 
					new IsDriveInfo(this.Data.IsDrive, this.stats.GalaxySpeed) : 
					null;

			}
		}

		public ShieldInfo Shield
		{
			get 
			{
				return this.Data.Shield != null ? 
					new ShieldInfo(this.Data.Shield, this.stats.ShieldSize) : 
					null;

			}
		}

		public IEnumerable<KeyValuePair<MissionEquipInfo, int>> Equipment
		{
			get
			{
				for(int i = 0; i < this.Data.MissionEquipment.Count; i++)
					yield return new KeyValuePair<MissionEquipInfo, int>(
						new MissionEquipInfo(this.Data.MissionEquipment[i].TypeInfo, this.Data.MissionEquipment[i].Level),
						this.Data.MissionEquipment[i].Quantity
					);
			}
		}
		
		public IEnumerable<KeyValuePair<SpecialEquipInfo, int>> SpecialEquipment
		{
			get
			{
				for(int i = 0; i < this.Data.SpecialEquipment.Count; i++)
					yield return new KeyValuePair<SpecialEquipInfo, int>(
						new SpecialEquipInfo(this.Data.SpecialEquipment[i].TypeInfo, this.Data.SpecialEquipment[i].Level, this.Data.Hull.TypeInfo),
						this.Data.SpecialEquipment[i].Quantity
					);
			}
		}
		
		public double ColonizerPopulation
		{
			get 
			{
				return stats.ColonizerPopulation;
			}
		}
		
		public double Size
		{
			get 
			{
				return this.Data.Hull.TypeInfo.Size;
			}
		}
	}
}
