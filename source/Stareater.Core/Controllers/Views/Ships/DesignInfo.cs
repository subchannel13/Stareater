using System;
using System.Collections.Generic;
using Stareater.GameData.Ships;
using Stareater.GameLogic;
using Stareater.Ships;
using Stareater.Utils.Collections;

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
		
		public HullInfo Hull
		{
			get { return new HullInfo(design.Hull.TypeInfo, design.Hull.Level); }
		}

		public IsDriveInfo IsDrive
		{
			get 
			{
				if (this.design.IsDrive != null)
					return new IsDriveInfo(design.IsDrive.TypeInfo, design.IsDrive.Level, this.Hull, stats.GalaxyPower);

				return null;
			}
		}

		public ShieldInfo Shield
		{
			get 
			{
				if (this.design.Shield != null)
					return new ShieldInfo(design.Shield.TypeInfo, design.Shield.Level, this.Hull);

				return null;
			}
		}

		public IEnumerable<KeyValuePair<MissionEquipInfo, int>> Equipment
		{
			get
			{
				for(int i = 0; i < design.MissionEquipment.Count; i++)
					yield return new KeyValuePair<MissionEquipInfo, int>(
						new MissionEquipInfo(design.MissionEquipment[i].TypeInfo, design.MissionEquipment[i].Level),
						design.MissionEquipment[i].Quantity
					);
			}
		}
		
		public IEnumerable<KeyValuePair<SpecialEquipInfo, int>> SpecialEquipment
		{
			get
			{
				var hull = new HullInfo(design.Hull.TypeInfo, design.Hull.Level);
				
				for(int i = 0; i < design.SpecialEquipment.Count; i++)
					yield return new KeyValuePair<SpecialEquipInfo, int>(
						new SpecialEquipInfo(design.SpecialEquipment[i].TypeInfo, design.SpecialEquipment[i].Level, hull),
						design.SpecialEquipment[i].Quantity
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
				return design.Hull.TypeInfo.Size.Evaluate(new Var(AComponentType.LevelKey, design.Hull.Level).Get);
			}
		}
	}
}
