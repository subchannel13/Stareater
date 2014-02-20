using System;

namespace Stareater.GameData
{
	public class AComponentType
	{
		public string nameCode { get; private set; }
		public string descCode { get; private set; }

		public IEnumerable<Prerequisite> Prerequisites { get; private set; }
		public int MaxLevel { get; private set; }
	}
}
