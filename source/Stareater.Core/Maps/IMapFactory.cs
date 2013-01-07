using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Maps
{
	public interface IMapFactory
	{
		string Name { get; }
		IEnumerable<MapFactoryParameterInfo> Parameters();
	}
}
