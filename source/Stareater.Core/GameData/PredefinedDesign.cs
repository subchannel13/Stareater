using System;
using System.Collections.Generic;
using Stareater.GameData.Databases;

namespace Stareater.GameData
{
	class PredefinedDesign
	{
		public string Name { get; private set; }
		
		public string HullCode { get; private set; }
		public int HullImageIndex { get; private set; }
		
		public PredefinedDesign(string name, string hullCode, int hullImageIndex)
		{
			this.Name = name;
			this.HullCode = hullCode;
			this.HullImageIndex = hullImageIndex;
		}
		
		public IEnumerable<Prerequisite> Prerequisites(StaticsDB statics)
		{
			return statics.Hulls[HullCode].Prerequisites;
		}
	}
}
