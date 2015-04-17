using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;
using Stareater.Ships;

namespace Stareater.GameData.Ships
{
	class ReactorType : AComponentType
	{
		public string ImagePath { get; private set; }
		
		public Formula Power { get; private set; }
		public Formula MinSize { get; private set; }
		
		public ReactorType(string code, string nameCode, string descCode, string imagePath, 
		                   IEnumerable<Prerequisite> prerequisites, int maxLevel, 
		                   Formula power, Formula minSize)
			: base(code, nameCode, descCode, prerequisites, maxLevel)
		{
			this.ImagePath = imagePath;
			this.Power = power;
			this.MinSize = minSize;
			
		}

		public static Component<ReactorType> MakeBest()
		{
			//TODO(v0.5)
			throw new NotImplementedException();
		}
	}
}
