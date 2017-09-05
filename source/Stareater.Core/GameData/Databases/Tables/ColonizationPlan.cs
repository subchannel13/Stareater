using Stareater.Utils.StateEngine;
using System.Collections.Generic;
using Stareater.Galaxy;

namespace Stareater.GameData.Databases.Tables
{
	class ColonizationPlan 
	{
		[StateProperty]
		public Planet Destination { get; private set; }

		[StateProperty]
		public List<StarData> Sources { get; private set; }

		public ColonizationPlan(Planet destination) 
		{
			this.Destination = destination;
			this.Sources = new List<StarData>();
 		} 

		private ColonizationPlan() 
		{ }
	}
}
