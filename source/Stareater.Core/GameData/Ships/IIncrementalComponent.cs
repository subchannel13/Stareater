using System;

namespace Stareater.GameData.Ships
{
	interface IIncrementalComponent
	{
		double ComparisonValue(int level);
	}
}
