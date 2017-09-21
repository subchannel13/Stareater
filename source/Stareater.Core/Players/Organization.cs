using Stareater.Utils.StateEngine;
using System.Collections.Generic;

namespace Stareater.Players
{
	[StateType(true)]
	public class Organization
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public IEnumerable<string> ResearchAffinities { get; private set; }

		internal Organization(string name, string description, IEnumerable<string> affinities)
		{
			this.Name = name;
			this.Description = description;
			this.ResearchAffinities = affinities;
		}
	}
}
