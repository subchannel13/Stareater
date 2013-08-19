using System;
using System.Collections.Generic;
using Stareater.AppData.Expressions;

namespace Stareater.GameData
{

	public class Technology
	{
		private string nameCode;
		private string descriptionCode;
		
		public string IdCode { get; private set; }
		public Formula cost { get; private set; }
		public IEnumerable<Prerequisite> Prerequisites { get; private set; }
		public long MaxLevel { get; private set; }
		public TechnologyCategory Category { get; private set; }
	}
}
