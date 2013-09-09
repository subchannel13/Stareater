using System;
using System.Collections.Generic;
using Stareater.GameData;
using Stareater.Utils.Collections;

namespace Stareater.Controllers.Data
{
	public class TechnologyTopic
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		
		public string ImagePath { get; private set; }
		public double Cost { get; private set; }
		public long MaxLevel { get; private set; }
		
		public TechnologyTopic(Technology tech)
		{
			this.Cost = tech.Cost.Evaluate(new Var("lvl0", 0).Get); //TODO: determine level
		}
	}
}
