﻿using System.Collections.Generic;
using Stareater.GameData.Databases;
using Stareater.GameData.Ships;
using Stareater.GameLogic;
using Stareater.Ships;
using Stareater.Utils.Collections;
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
				return Data.Name;
			}
		}
		
		public string ImagePath 
		{
			get
			{
				return Data.ImagePath;
			}
		}
		
		public bool Constructable 
		{
			get
			{
				return !Data.IsObsolete;
			}
		}
		
		public HullInfo Hull
		{
			get { return new HullInfo(Data.Hull.TypeInfo, Data.Hull.Level); }
		}

		public IsDriveInfo IsDrive
		{
			get 
			{
				return this.Data.IsDrive != null ? 
					new IsDriveInfo(Data.IsDrive.TypeInfo, Data.IsDrive.Level, this.stats) : 
					null;

			}
		}

		public ShieldInfo Shield
		{
			get 
			{
				return this.Data.Shield != null ? 
					new ShieldInfo(Data.Shield.TypeInfo, Data.Shield.Level, this.stats.ShieldSize) : 
					null;

			}
		}

		public IEnumerable<KeyValuePair<MissionEquipInfo, int>> Equipment
		{
			get
			{
				for(int i = 0; i < Data.MissionEquipment.Count; i++)
					yield return new KeyValuePair<MissionEquipInfo, int>(
						new MissionEquipInfo(Data.MissionEquipment[i].TypeInfo, Data.MissionEquipment[i].Level),
						Data.MissionEquipment[i].Quantity
					);
			}
		}
		
		public IEnumerable<KeyValuePair<SpecialEquipInfo, int>> SpecialEquipment
		{
			get
			{
				var hull = new HullInfo(Data.Hull.TypeInfo, Data.Hull.Level);
				
				for(int i = 0; i < Data.SpecialEquipment.Count; i++)
					yield return new KeyValuePair<SpecialEquipInfo, int>(
						new SpecialEquipInfo(Data.SpecialEquipment[i].TypeInfo, Data.SpecialEquipment[i].Level, hull),
						Data.SpecialEquipment[i].Quantity
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
				return Data.Hull.TypeInfo.Size.Evaluate(new Var(AComponentType.LevelKey, Data.Hull.Level).Get);
			}
		}
	}
}
