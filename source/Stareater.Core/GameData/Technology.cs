using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{

	public class Technology
	{
		private string nameCode;
		private string descriptionCode;
		
		public string ImagePath { get; private set; }
		public string IdCode { get; private set; }
		public Formula Cost { get; private set; }
		public IEnumerable<Prerequisite> Prerequisites { get; private set; }
		public long MaxLevel { get; private set; }
		public TechnologyCategory Category { get; private set; }
		
		public Technology(string nameCode, string descriptionCode, string imagePath, string code, Formula cost, IEnumerable<Prerequisite> prerequisites, long maxLevel, TechnologyCategory category)
		{
			this.nameCode = nameCode;
			this.descriptionCode = descriptionCode;
			this.ImagePath = imagePath;
			this.IdCode = code;
			this.Cost = cost;
			this.Prerequisites = prerequisites;
			this.MaxLevel = maxLevel;
			this.Category = category;
		}
	}
}
