using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stareater.AppData;

namespace Stareater.Maps
{
	public class StartingConditions
	{
		public int Colonies { get; private set; }
		public long Population { get; private set; }
		public long Infrastructure { get; private set; }

		private string nameKey;

		public StartingConditions(long population, int colonies, long infrastructure, string nameKey)
		{
			this.Colonies = colonies;
			this.Population = population;
			this.Infrastructure = infrastructure;
			this.nameKey = nameKey;
		}

		public string Name
		{
			get
			{
				return Settings.Get.Language["StartingConditions"][nameKey];
			}
		}
	}
}
