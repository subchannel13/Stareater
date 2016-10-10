using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Ikadn.Ikon.Types;
using Ikadn.Ikon;

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
