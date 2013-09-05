using System;
using System.Collections.Generic;
using Stareater.GameData;

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
			this.Cost = tech.Cost.Evaluate(new Dictionary<string, double>() {{ "lvl0", 0}}); //TODO: determine level
		}
	}
}
