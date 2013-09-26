using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{

	class Technology
	{
		public string NameCode { get; private set; }
		public string DescriptionCode{ get; private set; }
		
		public string ImagePath { get; private set; }
		public string IdCode { get; private set; }
		public Formula Cost { get; private set; }
		public IEnumerable<Prerequisite> Prerequisites { get; private set; }
		public int MaxLevel { get; private set; }
		public TechnologyCategory Category { get; private set; }
		
		public Technology(string nameCode, string descriptionCode, string imagePath, string code, Formula cost, IEnumerable<Prerequisite> prerequisites, int maxLevel, TechnologyCategory category)
		{
			this.NameCode = nameCode;
			this.DescriptionCode = descriptionCode;
			this.ImagePath = imagePath;
			this.IdCode = code;
			this.Cost = cost;
			this.Prerequisites = prerequisites;
			this.MaxLevel = maxLevel;
			this.Category = category;
		}
	}
}
