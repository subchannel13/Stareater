using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ikadn.Ikon.Types;

namespace Stareater.Galaxy.ProximityLanes
{
	struct DegreeOption
	{
		const string NameKey = "nameKey";
		const string MinKey = "min";
		const string MaxKey = "max";

		public string Name;
		public int Min;
		public int Max;

		public DegreeOption(IkonComposite data)
		{
			Name = data[NameKey].To<string>();
			Min = data[MinKey].To<int>();
			Max = data[MaxKey].To<int>();
		}
	}
}
