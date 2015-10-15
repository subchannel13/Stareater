using System;
using System.Collections.Generic;
using Stareater.GameData.Databases;

namespace Stareater.GameData.Ships
{
	class PredefinedDesign
	{
		public string Name { get; private set; }
		public PredefinedDesignType Type { get; private set; }
		
		public string HullCode { get; private set; }
		public int HullImageIndex { get; private set; }
		
		public bool HasIsDrive { get; private set; }
		public Dictionary<string, int> SpecialEquipment { get; private set; }
		
		public PredefinedDesign(string name, PredefinedDesignType type, string hullCode, int hullImageIndex, bool hasIsDrive, Dictionary<string, int> specialEquipment)
		{
			this.Name = name;
			this.Type = type;
			
			this.HasIsDrive = hasIsDrive;
			this.HullCode = hullCode;
			this.HullImageIndex = hullImageIndex;
			this.SpecialEquipment = specialEquipment;
		}
		
		public IEnumerable<Prerequisite> Prerequisites(StaticsDB statics)
		{
			return statics.Hulls[HullCode].Prerequisites;
		}
	}
}
