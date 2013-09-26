using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{
	class Constructable
	{
		public string NameCode { get; private set; }
		public string DescriptionCode{ get; private set; }
		public bool LiteralText { get; private set; }
		public string ImagePath { get; private set; }
		
		public string IdCode { get; private set; }
		public IEnumerable<Prerequisite> Prerequisites { get; private set; }
		public SiteType ConstructableAt { get; private set; }
		
		public Formula Condition { get; private set; }
		public Formula Cost { get; private set; }
		public Formula TurnLimit { get; private set; }
		
		public IEnumerable<object> Effects { get; private set; } //TODO: make type
		
		public Constructable(string nameCode, string descriptionCode, bool literalText, string imagePath, 
		                     string idCode, IEnumerable<Prerequisite> prerequisites, SiteType constructableAt, 
		                     Formula condition, Formula cost, Formula turnLimit, IEnumerable<object> effects)
		{
			this.NameCode = nameCode;
			this.DescriptionCode = descriptionCode;
			this.LiteralText = literalText;
			this.ImagePath = imagePath;
			this.IdCode = idCode;
			this.Prerequisites = prerequisites;
			this.ConstructableAt = constructableAt;
			this.Condition = condition;
			this.Cost = cost;
			this.TurnLimit = turnLimit;
			this.Effects = effects;
		}
	}
}
