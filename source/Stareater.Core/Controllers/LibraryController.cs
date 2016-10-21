using System;
using System.Collections.Generic;
using System.Linq;
using Stareater.Controllers.Views;
using Stareater.Controllers.Views.Library;
using Stareater.Controllers.Views.Ships;

namespace Stareater.Controllers
{
	public class LibraryController
	{
		private readonly GameController gameController;
		
		internal LibraryController(GameController gameController)
		{
			this.gameController = gameController;
		}
		
		public IEnumerable<TechnologyGeneralInfo> DevelpmentTopics
		{
			get
			{
				foreach(var tech in this.gameController.GameInstance.Statics.DevelopmentTopics.
				        Where(x => x.Category == TechnologyCategory.Development).
				        OrderBy(x => x.IdCode))
					yield return new TechnologyGeneralInfo(tech);
			}
		}
		
		public IEnumerable<TechnologyGeneralInfo> ResearchTopics
		{
			get
			{
				foreach(var tech in this.gameController.GameInstance.Statics.DevelopmentTopics.
				        Where(x => x.Category == TechnologyCategory.Research).
				        OrderBy(x => x.IdCode))
					yield return new TechnologyGeneralInfo(tech);
			}
		}
		
		#region Ship equipment
		public IEnumerable<ShipComponentGeneralInfo> Armors
		{
			get
			{
				foreach(var item in this.gameController.GameInstance.Statics.Armors.Values.OrderBy(x => x.IdCode))
					yield return new ShipComponentGeneralInfo(item, ArmorInfo.LangContext, item.ImagePath);
			}
		}
		
		public IEnumerable<ShipComponentGeneralInfo> Hulls
		{
			get
			{
				foreach(var item in this.gameController.GameInstance.Statics.Hulls.Values.OrderBy(x => x.IdCode))
					yield return new ShipComponentGeneralInfo(item, HullInfo.LangContext, item.ImagePaths[0]);
			}
		}
		
		public IEnumerable<ShipComponentGeneralInfo> IsDrives
		{
			get
			{
				foreach(var item in this.gameController.GameInstance.Statics.IsDrives.Values.OrderBy(x => x.IdCode))
					yield return new ShipComponentGeneralInfo(item, IsDriveInfo.LangContext, item.ImagePath);
			}
		}
		
		public IEnumerable<ShipComponentGeneralInfo> MissionEquipment
		{
			get
			{
				foreach(var item in this.gameController.GameInstance.Statics.MissionEquipment.Values.OrderBy(x => x.IdCode))
					yield return new ShipComponentGeneralInfo(item, MissionEquipInfo.LangContext, item.ImagePath);
			}
		}
		
		public IEnumerable<ShipComponentGeneralInfo> Reactors
		{
			get
			{
				foreach(var item in this.gameController.GameInstance.Statics.Reactors.Values.OrderBy(x => x.IdCode))
					yield return new ShipComponentGeneralInfo(item, ReactorInfo.LangContext, item.ImagePath);
			}
		}
		
		public IEnumerable<ShipComponentGeneralInfo> Sensors
		{
			get
			{
				foreach(var item in this.gameController.GameInstance.Statics.Sensors.Values.OrderBy(x => x.IdCode))
					yield return new ShipComponentGeneralInfo(item, SensorInfo.LangContext, item.ImagePath);
			}
		}
		
		public IEnumerable<ShipComponentGeneralInfo> SpecialEquipment
		{
			get
			{
				foreach(var item in this.gameController.GameInstance.Statics.SpecialEquipment.Values.OrderBy(x => x.IdCode))
					yield return new ShipComponentGeneralInfo(item, SpecialEquipInfo.LangContext, item.ImagePath);
			}
		}
		
		public IEnumerable<ShipComponentGeneralInfo> Thrusters
		{
			get
			{
				foreach(var item in this.gameController.GameInstance.Statics.Thrusters.Values.OrderBy(x => x.IdCode))
					yield return new ShipComponentGeneralInfo(item, ThrusterInfo.LangContext, item.ImagePath);
			}
		}
		#endregion
	}
}
