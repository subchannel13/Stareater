using System;
using Stareater.Controllers.Views.Ships;

namespace Stareater.GUI.ShipDesigns
{
	public class EquipmentActionDispatcher
	{
		public Action<MissionEquipInfo> MissionEquipmentAction { get; set; }
		public Action<SpecialEquipInfo> SpecialEquipmentAction { get; set; }
		
		public void Dispatch(MissionEquipInfo equipmentInfo)
		{
			this.MissionEquipmentAction(equipmentInfo);
		}
		
		public void Dispatch(SpecialEquipInfo equipmentInfo)
		{
			this.SpecialEquipmentAction(equipmentInfo);
		}
	}
}
