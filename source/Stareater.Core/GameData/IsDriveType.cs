using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{
	class IsDriveType : AComponentType
	{
		public string IdCode { get; private set; }
		
		public Formula Cost { get; private set; }
		public string ImagePath { get; private set; }
		
		public Formula Speed { get; private set; }
		public Formula MinSize { get; private set; }
		
		public IsDriveType(string code, string nameCode, string descCode, string imagePath,
		                   IEnumerable<Prerequisite> prerequisites, int maxLevel, Formula cost, 
		                   Formula speed, Formula minSize)
			: base(nameCode, descCode, prerequisites, maxLevel)
		{
			this.IdCode = code;
			this.Cost = cost;
			this.ImagePath = imagePath;
			this.Speed = speed;
			this.MinSize = minSize;
		}
	}
}
