using System;
using System.Linq;

namespace Stareater.Players
{
	public class Organization
	{
		public string Name { get; private set; }
		public string Description { get; private set; }

		internal Organization(string name, string description)
		{
			this.Name = name;
			this.Description = description;
		}
	}
}
