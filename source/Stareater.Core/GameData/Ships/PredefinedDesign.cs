using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.GameData.Ships
{
	class PredefinedDesign
	{
		public string Name { get; private set; }

		public PredefinedComponent Hull { get; private set; }
		public int HullImageIndex { get; private set; }

		public PredefinedComponent IsDrive { get; private set; }
		public PredefinedComponent Shield { get; private set; }
		public List<PredefinedComponent> MissionEquipment { get; private set; }
		public List<PredefinedComponent> SpecialEquipment { get; private set; }

		public PredefinedComponent Armor { get; private set; }
		public PredefinedComponent Reactor { get; private set; }
		public PredefinedComponent Sensors { get; private set; }
		public PredefinedComponent Thrusters { get; private set; }

		public PredefinedDesign(string name, PredefinedComponent hull, int hullImageIndex, 
			PredefinedComponent isDrive, PredefinedComponent shield, 
			List<PredefinedComponent> missionEquipment, List<PredefinedComponent> specialEquipment, 
			PredefinedComponent armor, PredefinedComponent reactor, PredefinedComponent sensors, PredefinedComponent thrusters)
		{
			this.Name = name;
			this.Hull = hull;
			this.HullImageIndex = hullImageIndex;
			this.IsDrive = isDrive;
			this.Shield = shield;
			this.MissionEquipment = missionEquipment;
			this.SpecialEquipment = specialEquipment;
			this.Armor = armor;
			this.Reactor = reactor;
			this.Sensors = sensors;
			this.Thrusters = thrusters;
		}
	}
}
