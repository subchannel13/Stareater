using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stareater.Players
{
	public interface IOffscreenPlayerFactory
	{
		string Name { get; }
		IOffscreenPlayer Create();
	}
}
